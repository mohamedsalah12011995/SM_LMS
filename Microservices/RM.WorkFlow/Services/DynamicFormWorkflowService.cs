using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Extensions;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Models;
using RM.WorkFlow.Const;
using RM.WorkFlow.Dtos;
using RM.WorkFlow.Dtos.DyFormEntities;
using RM.WorkFlow.Dtos.IntegrationEntities;
using RM.WorkFlow.DynamicFormEnums;
using RM.WorkFlow.Records;
using RM.WorkFlow.Services.EntityService;
using RM.WorkFlow.UnitOfWorks;
using System.Reflection;
using System.Text.Json;
using static RM.WorkFlow.Dtos.OperationOutput;

namespace RM.WorkFlow.Services
{
    public class DynamicFormWorkflowService : BaseService, IDynamicFormWorkflowService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected static string imagesGetPath = string.Empty;
        protected static string imagesDyFormGetPath = string.Empty;
        protected static string filesGetPath = string.Empty;
        protected static string filesDyFormGetPath = string.Empty;
        JsonSerializerOptions jsonOptions = null;
        private string yaqeenPersonInfoUrl;
        private string yaqeenIdTypeLookupsUrl;
        public DynamicFormWorkflowService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            imagesGetPath = filesGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);
            imagesDyFormGetPath = filesDyFormGetPath = HandleGetResourcesDyFormPath(IsLocal, GetDyFormPath, IntranetGetPath);
            SetJsonSerializerOptions();
            SetYaqeenLinkConfigurations();
        }

        #region HELPER METHOD CONSTRACTOR

        private void SetJsonSerializerOptions()
        {
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            jsonOptions.PropertyNameCaseInsensitive = true;
        }
        public static string HandleGetResourcesDyFormPath(bool IsIntranetRequestType, string GetPath, string IntranetGetPath)
        {
            return IsIntranetRequestType ? IntranetGetPath : GetPath;
        }
        private void SetYaqeenLinkConfigurations()
        {
            yaqeenPersonInfoUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("YaqeenPersonInfoUrl");
            yaqeenIdTypeLookupsUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("YaqeenIdTypeLookupsUrl");
        }
        #endregion


        #region Stepper Dynamic Form View (super Admin) / dy workflow form (for users)
        public async Task<OperationOutput> GetFormAndStepperActions(Dtos.DyFormEntities.FormView RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var form = new Dtos.DyFormEntities.FormView();
            var userActions = new List<UserActionJobRole>();
            var formsValuesActions = new List<Dtos.DyFormEntities.FormValuesActions>();
            var transferRefrences = new List<ReferencesTreeDto>();
            Models.FormsEntity entityForm = null;

            entityForm = await GetEntityForm(RequestedData);

            if (entityForm is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            // formId     // from dynamic view
            RequestedData._id = !RequestedData._id.HasValue ? entityForm.FormId : RequestedData._id.Value;
            RequestedData.EntityId = entityForm.EntityId;

            Models.User userDbItem = null;

            if (RequestOwner is not null && RequestOwner.Id.HasValue)
                userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            var engineForm = await GetEngionForm(RequestedData);

            if (engineForm is not null && userDbItem is not null)
            {
                var formSteps = await GetFormActionStepper(engineForm);

                form = engineForm.Form.Adapt<Dtos.DyFormEntities.FormView>(Dtos.DyFormEntities.FormView.SelectConfig(imagesGetPath, engineForm, formSteps));

            }
            if (form is not null && form.ActionStepper.Any())
            {
                GetActionStepWithInputs(form, engineForm, userDbItem, RequestedData.FormValueId);
            }

            if (engineForm is not null && userDbItem is not null)
            {
                userActions = GetActionsAllowedToCurrentUser(userDbItem, engineForm.WorkFlowEngineId.Value, form);
                if (RequestedData.FormValueId != null)
                {
                    if (form.ActionStepper.Any(c => c.IsTransferToReference == true))
                    {
                        transferRefrences = GetParentReferenceByReportReferenceId(RequestedData);
                    }

                    formsValuesActions = GetFormValueActions(RequestedData.FormValueId);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.DynamicForm, form),
                 new OutputDictionary(OperationOutputKeys.EntityName, entityForm != null && entityForm.Entity != null ? entityForm.Entity.CmsIdentity.Split('/')[1] : string.Empty),
                 new OutputDictionary(OperationOutputKeys.UserActions, userActions),
                 new OutputDictionary(OperationOutputKeys.FormValueActions, formsValuesActions),
                 new OutputDictionary(OperationOutputKeys.TransferRefrences, transferRefrences));


        }



        #region HELPER METHODS 
        private async Task<List<ActionStepper>> GetFormActionStepper(EngineForms engineForm)
        {

            var formStepsData = await _unitOfWork.EnginesActionsJobRole
                                .GetAll(e => e.EngineId == engineForm.WorkFlowEngineId && e.IsDeleted != true)
                                .Include(c => c.JobRole)
                                .Include(e => e.ActionNavigation).OrderBy(c => c.StepNo).AsNoTracking().ToListAsync();



            var formSteps = formStepsData.Select(s => new ActionStepper
            {
                Id = s.ActionId,
                EngineActionJobRoleId = s.Id,
                NameAr = $" {s.ActionNavigation.NameAr} - {(s.JobRole != null ? s.JobRole.NameAr : string.Empty)}",
                NameEn = $" {s.ActionNavigation.NameEn} - {(s.JobRole != null ? s.JobRole.NameEn : string.Empty)}",
                ReturnActionId = s.ReturnStep != null ? SetReturnActionId(formStepsData, s) : null,
                RejectActionId = s.RejectStep != null ? SetRejectActionId(formStepsData, s) : null,
                ReturnEngineActionJobRoleId = s.ReturnStep != null ? s.ReturnStep != s.ActionId ? SetReturnEngineActionJobRoleId(formStepsData, s) : s.Id : null,
                RejectEngineActionJobRoleId = s.RejectStep != null ? s.RejectStep != s.ActionId ? SetRejectEngineActionJobRoleId(formStepsData, s) : s.Id : null,
                ClosedActionId = s.CloseStep,
                HasNote = s.HasNote,
                NoteIsRequired = s.NoteIsRequired,
                JobRoleId = s.JobRoleId,
                StepNo = s.StepNo,
                IsTransferToReference = s.IsTransferToReference
            }).OrderBy(c => c.StepNo).ToList();

            return formSteps;
        }

        private async Task<List<ActionStepper>> GetFormActionStepperForPortal(EngineForms engineForm)
        {

            var formStepsData = await _unitOfWork.EnginesActionsJobRole
                                .GetAll(e => e.EngineId == engineForm.WorkFlowEngineId && e.IsDeleted != true)
                                .Include(c => c.JobRole)
                                .Include(e => e.ActionNavigation).OrderBy(c => c.StepNo).AsNoTracking().ToListAsync();



            var formSteps = formStepsData.Select(s => new ActionStepper
            {
                Id = s.ActionId,
                EngineActionJobRoleId = s.Id,
                NameAr = $" {s.ActionNavigation.NameAr} ",
                NameEn = $" {s.ActionNavigation.NameEn} ",
                ReturnActionId = s.ReturnStep != null ? SetReturnActionId(formStepsData, s) : null,
                RejectActionId = s.RejectStep != null ? SetRejectActionId(formStepsData, s) : null,
                ReturnEngineActionJobRoleId = s.ReturnStep != null ? s.ReturnStep != s.ActionId ? SetReturnEngineActionJobRoleId(formStepsData, s) : s.Id : null,
                RejectEngineActionJobRoleId = s.RejectStep != null ? s.RejectStep != s.ActionId ? SetRejectEngineActionJobRoleId(formStepsData, s) : s.Id : null,
                ClosedActionId = s.CloseStep,
                HasNote = s.HasNote,
                NoteIsRequired = s.NoteIsRequired,
                JobRoleId = s.JobRoleId,
                StepNo = s.StepNo,
                IsTransferToReference = s.IsTransferToReference
            }).OrderBy(c => c.StepNo).ToList();

            return formSteps;
        }

        private async Task<EngineForms> GetEngionForm(Dtos.DyFormEntities.FormView RequestedData)
        {
            return await _unitOfWork.EngineForms.GetAll().Include(f => f.Form).AsNoTracking()
                .FirstOrDefaultAsync(f => f.FormId == RequestedData._id && f.IsDeleted == false
                 && (RequestedData._referenceId != null ? f.Form.ReferenceId == RequestedData._referenceId : true));
        }

        private async Task<EngineForms> GetEngionFormForPortal(Dtos.DyFormEntities.FormView RequestedData)
        {
            return await _unitOfWork.EngineForms.GetAll().Include(f => f.Form).AsNoTracking()
                .FirstOrDefaultAsync(f => f.FormId == RequestedData._id && f.IsDeleted == false);
        }

        private async Task<FormsEntity> GetEntityForm(Dtos.DyFormEntities.FormView RequestedData)
        {
            if (!RequestedData._id.HasValue)
                return await _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).AsNoTracking().FirstOrDefaultAsync(c => c.Entity.FrontIdentity.ToLower() == RequestedData.EntityUrl.ToLower());

            else
                return await _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).AsNoTracking().FirstOrDefaultAsync(c => c.FormId == RequestedData._id);

        }

        private List<ReferencesTreeDto> GetParentReferenceByReportReferenceId(Dtos.DyFormEntities.FormView RequestedData)
        {
            List<ReferencesTreeDto> referencesTree = new List<ReferencesTreeDto>();

            var references = _unitOfWork.References.GetAll().Where(c => c.IsDeleted != true).ToList();
            if (references.Any())
            {
                var formValueDetails = _unitOfWork.FormValueDetails.GetAll().Where(c => c.FormValueId == RequestedData.FormValueId).ToList();
                if (formValueDetails.Any())
                {
                    int? referenceId = GetReportReferenceId(formValueDetails);
                    var child = references.Find(c => c.Id == referenceId);
                    if (child != null)
                    {
                        var parent = FindAllParents(references, child).Where(c => c.ParentId != null)
                         .Select(c => c.Parent).FirstOrDefault();

                        if (parent != null)
                        {
                            var referencesData = references.Where(c => c.ParentId == parent.Id).Select(x => new ReferencesTreeDto()
                            {
                                _Id = x.Id,
                                Label = x.NameAr,
                                NameAr = x.NameAr,
                                NameEn = x.NameEn,
                                _parentId = x.ParentId,
                            }).ToList();

                            referencesData.Add(new ReferencesTreeDto { _Id = parent.Id, Label = parent.NameAr, NameAr = parent.NameAr, NameEn = parent.NameEn });
                            var root = referencesData.GenerateTree(c => c._Id, c => c._parentId);

                            foreach (var reference in root)
                            {
                                referencesTree.Add(bindNode(reference));
                            }
                        }
                    }
                }
            }
            return referencesTree;
        }

        private static int? SetReturnActionId(List<EngineActionJobRole> formStepsData, EngineActionJobRole model)
        {
            // from return action id get first action for user role 
            var firstActionUser = formStepsData.First(g => g.JobRoleId == formStepsData.Find(c => c.ActionId == model.ReturnStep).JobRoleId).ActionId;
            if (firstActionUser != formStepsData.First().ActionId)
                return formStepsData.Find(c => c.NextStep == firstActionUser).ActionId;

            else return formStepsData.First().ActionId;
        }
        private static int? SetReturnEngineActionJobRoleId(List<EngineActionJobRole> formStepsData, EngineActionJobRole model)
        {
            var firstActionUser = formStepsData.First(g => g.JobRoleId == formStepsData.Find(c => c.ActionId == model.ReturnStep).JobRoleId).ActionId;
            if (firstActionUser != formStepsData.First().ActionId)
                return formStepsData.Find(c => c.NextStep == firstActionUser).Id;

            else return formStepsData.First().Id;
        }
        private static int? SetRejectActionId(List<EngineActionJobRole> formStepsData, EngineActionJobRole model)
        {
            var firstActionUser = formStepsData.First(g => g.JobRoleId == formStepsData.Find(c => c.ActionId == model.RejectStep).JobRoleId).ActionId;
            if (firstActionUser != formStepsData.First().ActionId)
                return formStepsData.Find(c => c.NextStep == firstActionUser).ActionId;

            else return formStepsData.First().ActionId;
        }
        private static int? SetRejectEngineActionJobRoleId(List<EngineActionJobRole> formStepsData, EngineActionJobRole model)
        {
            var firstActionUser = formStepsData.First(g => g.JobRoleId == formStepsData.Find(c => c.ActionId == model.RejectStep).JobRoleId).ActionId;
            if (firstActionUser != formStepsData.First().ActionId)
                return formStepsData.Find(c => c.NextStep == firstActionUser).Id;

            else return formStepsData.First().Id;
        }

        private void GetActionStepWithInputs(Dtos.DyFormEntities.FormView form, EngineForms engineForm, Models.User userDbItem, int? formValueId)
        {
            Models.FormValue formValue = null;
            List<Models.FormValuesActions> FormValueActions = new List<Models.FormValuesActions>();

            if (formValueId != null)
            {
                formValue = _unitOfWork.FormValue.GetAll().Include(c => c.FormValueDetails).FirstOrDefault(c => c.Id == formValueId.Value);
                FormValueActions = _unitOfWork.FormValuesActions.GetAll().Where(c => c.FormValueId == formValue.Id).Select(c => new Models.FormValuesActions { ActionId = c.ActionId, IsRejected = c.IsRejected, IsReturned = c.IsReturned }).ToList();

            }
            var inputs = _unitOfWork.FormInputsActions.GetAll()
                .Include(c => c.FormInput)
                .ThenInclude(c => c.InputsType)
                .Include(c => c.FormInput)
                .ThenInclude(c => c.FormInputDataSource)
                .ThenInclude(c => c.DataSource)
                .Where(c => c.EngineFormId == engineForm.Id && c.FormInput.IsDeleted == false).ToList();

            if (inputs.Any())
            {
                foreach (var action in form.ActionStepper)
                {
                    AssignActionFields(userDbItem, FormValueActions, inputs, action);

                    if (formValue is not null)
                    {
                        foreach (var field in action.FormFields)
                        {
                            SetFormFieldValue(formValue, field);
                        }
                    }
                }

                // if input fields has datasource from api method 
                GetInputsOptionsFromCallAPIMethods(form);
            }
        }

        private static void AssignActionFields(Models.User userDbItem, List<Models.FormValuesActions> FormValueActions, List<Models.FormInputsActions> inputs, ActionStepper action)
        {
            action.FormFields = inputs.Where(c => c.ActionId == action.Id && c.FormInput.IsDeleted == false).Select(n => new FormFieldDto
            {
                _key = n.FormInput.Id,
                LabelAr = n.FormInput.NameAr,
                LabelEn = n.FormInput.NameEn,
                ControlType = n.FormInput.InputsType != null ? n.FormInput.InputsType.Type : string.Empty,
                Type = n.FormInput.Type,
                Required = n.FormInput.Mandatory,
                VerticalDataSourceDirection = n.FormInput.VerticalDataSourceDirection,
                Order = n.FormInput.Order,
                Disable = ((FormValueActions.Count > 1 && FormValueActions.Any(c => c.Id == action.Id && c.IsReturned == false && c.IsRejected == false)) || (userDbItem != null && action.JobRoleId != userDbItem.JobRole)) ? true : false,
                ViewInFullRow = n.FormInput.ViewInFullRow == null ? false : n.FormInput.ViewInFullRow,
                HasDataSourceFromAPI = n.FormInput.HasDataSourceFromAPI,
                DataSourceAPIRouting = n.FormInput.DataSourceAPIRouting,
                APIParameters = n.FormInput.APIParameters,
                OnChangeAPIMethodName = n.FormInput.OnChangeAPIMethodName,
                OnChangeParamName = n.FormInput.OnChangeParamName,
                OnChangeRefelectionInputKey = n.FormInput.OnChangeRefelectionInputKey,
                InputUseIntegration = n.FormInput.InputUseIntegration,
                IsUnique = n.FormInput.IsUnique,
                ShowInExport = n.FormInput.ShowInExport,
                Length = n.FormInput.Length,
                Options = n.FormInput.FormInputDataSource.Any() ? n.FormInput.FormInputDataSource.Select(ds => new FieldOptionsDto
                {
                    _key = ds.DataSourceId,
                    ValueAr = ds.DataSource != null ? ds.DataSource.TextAr : string.Empty,
                    ValueEn = ds.DataSource != null ? ds.DataSource.TextEn : string.Empty
                }).OrderBy(c => c._key).ToList() : new List<FieldOptionsDto>()

            }).OrderBy(c => c.Order ?? c._key).ToList();
        }

        private void GetInputsOptionsFromCallAPIMethods(Dtos.DyFormEntities.FormView form)
        {
            if (form != null)
            {
                foreach (var action in form.ActionStepper)
                {


                    var inputsDSFromAPI = action.FormFields.Where(c => c.HasDataSourceFromAPI == true).ToList();
                    if (inputsDSFromAPI.Any())
                    {
                        List<FieldOptionsDto> result = null;
                        for (int i = 0; i < inputsDSFromAPI.Count; i++)
                        {
                            result = new List<FieldOptionsDto>();
                            MethodInfo apiMethod = typeof(DynamicFormWorkflowService).GetMethod(inputsDSFromAPI[i].DataSourceAPIRouting.Trim());
                            if (apiMethod != null)
                            {
                                if (!string.IsNullOrEmpty(inputsDSFromAPI[i].APIParameters))
                                {
                                    LookupParameterModel parameterModel = System.Text.Json.JsonSerializer.Deserialize<LookupParameterModel>(inputsDSFromAPI[i].APIParameters, jsonOptions);
                                    result = (List<FieldOptionsDto>)apiMethod.Invoke(this, new object[] { parameterModel });
                                }
                                else result = (List<FieldOptionsDto>)apiMethod.Invoke(this, null);
                                action.FormFields.Find(c => c.Key == inputsDSFromAPI[i].Key).Options = result;
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<Models.Reference> FindAllParents(List<Models.Reference> all_data, Models.Reference child)
        {
            var parent = all_data.FirstOrDefault(x => x.Id == child.ParentId);

            if (parent == null)
                return Enumerable.Empty<Models.Reference>();

            return new[] { parent }.Concat(FindAllParents(all_data, parent));
        }

        private ReferencesTreeDto bindNode(TreeItem<ReferencesTreeDto> root)
        {
            ReferencesTreeDto references = new ReferencesTreeDto
            {
                Label = root.Item.Label,
                NameAr = root.Item.NameAr,
                NameEn = root.Item.NameEn,
                _parentId = root.Item._parentId,
                _referencesMajorId = root.Item._referencesMajorId,
                _Id = root.Item._Id,
                Children = new List<ReferencesTreeDto>()
            };

            if (root.Children != null)
            {
                foreach (var child in root.Children)
                {
                    references.Children.Add(bindNode(child));
                }
            }


            return references;
        }



        #region HELPER METHOD MaintenanceReports

        public List<FieldOptionsDto> GetReferenceDataByParentId(LookupParameterModel model)
        {
            return MaintenanceReports.GetReferencesByParentId(_unitOfWork, model);
        }



        #endregion

        private List<UserActionJobRole> GetActionsAllowedToCurrentUser(Models.User userDbItem, int engineId, Dtos.DyFormEntities.FormView form)
        {
            List<UserActionJobRole> userActions = new List<UserActionJobRole>();
            if (userDbItem != null && userDbItem.JobRole != null)
            {

                userActions = _unitOfWork.EnginesActionsJobRole.GetAll(c => c.EngineId == engineId && c.JobRoleId == userDbItem.JobRole
                              && c.IsDeleted != true).Include(c => c.ActionNavigation)
                   .AsNoTracking().Select(c => new UserActionJobRole
                   {
                       ActionId = c.ActionId,
                       NextActionId = c.NextStep,
                       EngineActionJobRoleId = c.Id,
                       JobRoleId = c.JobRoleId,
                       ReturnActionId = c.ReturnStep,
                       RejectActionId = c.RejectStep,
                       ClosedActionId = c.CloseStep,
                       ActionNameAr = c.ActionNavigation != null ? c.ActionNavigation.NameAr : string.Empty,
                       ActionNameEn = c.ActionNavigation != null ? c.ActionNavigation.NameEn : string.Empty,
                       StepNo = c.StepNo
                   }).OrderBy(c => c.StepNo).ToList();

                if (userActions.Count >= 1)
                {
                    foreach (var step in userActions)
                    {
                        form.JumpSteps.Add(step.StepNo.Value - 1);
                        // form.JumpSteps.Add(step.StepNo.Value );
                    }
                }

            }
            return userActions;
        }


        #endregion

        public async Task<OperationOutput> GetReferencesByParentId(Dtos.DyFormEntities.FormView RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var references = await _unitOfWork.References.GetAll(c => c.ParentId == RequestedData._referenceId).AsNoTracking().Select(c => new FieldOptionsDto
            {
                _key = c.Id,
                ValueAr = c.NameAr,
                ValueEn = c.NameEn

            }).ToListAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.References, references));

        }

        #endregion

        #region CP Save Form Value
        public async Task<OperationOutput> SaveFormValue(FormValueDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            Models.FormValue formValue = null;
            List<FormFieldDto> fieldsUrl = new List<FormFieldDto>();
            Models.FormValuesActions currentAction = null;
            List<Dtos.DyFormEntities.FormValuesActions> actions = new List<Dtos.DyFormEntities.FormValuesActions>();
            List<Models.FormValueDetails> formValueDetails = new List<Models.FormValueDetails>();
            EngineActionJobRoleEmailDataRecord engineActionEmailData = new();


            if (!RequestedData._id.HasValue)
            {
                formValue = new Models.FormValue();
                formValue.CreatedBy = RequestOwner.Id.HasValue ? RequestOwner.Id.Value : null;
                formValue.CreatedDate = TransactionDate;
                formValue.IsDeleted = false;
                formValue.IsActive = false;

            }
            else
            {
                formValue = await _unitOfWork.FormValue.GetAll().Include(c => c.FormValueDetails).ThenInclude(c => c.FormInput).FirstOrDefaultAsync(c => c.Id == RequestedData._id.Value);
                if (formValue is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                if (formValue.FormValueDetails.Any())
                {
                    formValueDetails = formValue.FormValueDetails.ToList();

                    var removedInputsValue = new List<Models.FormValueDetails>();
                    foreach (var item in formValue.FormValueDetails)
                    {
                        if (RequestedData.FormFields.Find(c => c._key == item.InputKey) != null)
                            removedInputsValue.Add(item);
                    }
                    if (removedInputsValue.Any())
                        _unitOfWork.FormValueDetails.DeleteRange(removedInputsValue);
                }

                var formValueDataSource = _unitOfWork.FormValueDataSource.GetAll(x => x.FormValueId == formValue.Id).ToList();
                var formValueDataSourceRemoved = new List<Models.FormValueDataSource>();

                if (formValueDataSource.Any())
                {
                    foreach (var item in formValueDataSource)
                    {
                        if (RequestedData.FormFields.Find(c => c._key == item.InputKey) != null)
                            formValueDataSourceRemoved.Add(item);
                    }
                    if (formValueDataSourceRemoved.Any())
                        _unitOfWork.FormValueDataSource.DeleteRange(formValueDataSourceRemoved);
                }
            }
            if (RequestedData.FormFields.Any(c => c.IsUnique == true))
            {
                var formInput = CheckValueIsExist(RequestedData);

                if (formInput is not null)
                    return ReturnMessageForDublicateFormInput(formInput);
            }
            if (RequestedData.FormFields.Any(c => c.ControlType == FormInputTypes.fileImage && !string.IsNullOrEmpty(c.ValueBase64)))
            {
                var resultImage = ConvertImageBase64ToString(RequestedData, fieldsUrl);

                if (resultImage == false)
                    return GetOperationOutput(header: Enums.ServiceMessages.FileSizeError);
            }

            if (RequestedData.FormFields.Any(c => c.ControlType == FormInputTypes.fileDoc && !string.IsNullOrEmpty(c.ValueBase64)))
            {
                var resultfile = ConvertFileBase64ToString(RequestedData, fieldsUrl);

                if (resultfile == false)
                    return GetOperationOutput(header: Enums.ServiceMessages.FileSizeError);

            }

            formValue.FormId = RequestedData._formId;

            foreach (var field in RequestedData.FormFields)
            {
                field.Value = field.Value != null ? field.Value.ToString().Replace("ValueKind = String :", string.Empty) : null;

                AddFormValueDetails(formValue, field);

                if (IsFieldHasSingleDatasource(field) && field.Value != null && !string.IsNullOrEmpty(field.Value.ToString()) && field.HasDataSourceFromAPI != true)
                {
                    AddFormValueSingleDataSource(formValue, field);
                }
                else if (IsFieldHasMultipleDatasource(field) && field.Value != null && !string.IsNullOrEmpty(field.Value.ToString()) && field.HasDataSourceFromAPI != true)
                {
                    AddFormValueMultipleDataSource(formValue, field);
                }
            }

            formValue.EntityId = RequestedData._entityId;
            formValue.UpdatedDate = TransactionDate;
            formValue.UpdatedBy = RequestOwner.Id;


            if (!RequestedData._id.HasValue)
            {
                _unitOfWork.FormValue.Add(formValue);
            }
            else _unitOfWork.FormValue.Update(formValue);
            _unitOfWork.Complete();



            if (!RequestedData._id.HasValue)
            {
                formValueDetails = await _unitOfWork.FormValueDetails.GetAll().Include(c => c.FormInput)
                    .Where(c => c.FormValueId == formValue.Id).ToListAsync();
            }
            if (RequestedData.FormValuesAction._actionId != null)
            {
                RequestedData.FormValuesAction._formValueId = formValue.Id;

                int? referenceId = GetReportReferenceId(formValueDetails);
                currentAction = AddAction(RequestedData.FormValuesAction, referenceId);
                actions = GetFormValueActions(formValue.Id);

                await GetEngineActionDetailForEmail(RequestedData.FormValuesAction, engineActionEmailData, formValueDetails);
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.Id, Accessor.Get<int?>(formValue.Id)),
            new OutputDictionary(OperationOutputKeys.FieldsUrl, fieldsUrl),
            new OutputDictionary(OperationOutputKeys.FormValueActions, actions),
            new OutputDictionary(OperationOutputKeys.ActionId, currentAction != null ? Accessor.Get<int?>(currentAction.ActionId) : string.Empty),
            new OutputDictionary(OperationOutputKeys.EngineActionEmailData, engineActionEmailData));

        }

        #region HELPER METHODS >> SaveFormValue
        private static OperationOutput ReturnMessageForDublicateFormInput(Models.FormInput formInput)
        {
            var result = GetOperationOutput(header: Enums.ServiceMessages.ValueIsExist);
            result.Header.Message = $"{formInput.NameAr}   موجود مسبقا  ";
            result.Header.MessageEn = $"{formInput.NameEn}  already exist ";
            return result;
        }

        private Models.FormInput CheckValueIsExist(FormValueDto RequestedData)
        {
            bool isDublicated = false;
            Models.FormInput formInput = null;
            var uniqueFormInputs = _unitOfWork.FormValue.GetAll()
                .Include(c => c.FormValueDetails)
                .ThenInclude(c => c.FormInput)
                .Where(c => c.FormId == RequestedData._formId && c.IsDeleted == false)
                .SelectMany(c => c.FormValueDetails
                .Where(c => c.FormInput.IsUnique == true)
                ).ToList();

            var FormField = RequestedData.FormFields.Where(c => c.IsUnique == true).ToList();
            if (FormField.Any() && uniqueFormInputs.Any())
            {
                foreach (var item in uniqueFormInputs)
                {
                    var input = FormField.Find(c => c._key == item.InputKey);
                    if (input != null)
                    {
                        isDublicated = input.Value.ToString().Replace("ValueKind = String :", string.Empty).Equals(item.InputValue);
                        if (isDublicated)
                        {
                            formInput = item.FormInput;
                            break;
                        }
                    }
                }
            }
            return formInput;
        }

        private void AddFormValueSingleDataSource(Models.FormValue formValue, FormFieldDto field)
        {
            var formValueDs = new Models.FormValueDataSource
            {
                FormValueId = formValue.Id,
                InputKey = field._key.Value,
                FormDataSourceId = Accessor.Set(field.Value.ToString())
            };
            formValue.FormValueDataSource.Add(formValueDs);
        }

        private void AddFormValueMultipleDataSource(Models.FormValue formValue, FormFieldDto field)
        {
            var datasources = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemsValue>>(field.Value.ToString());

            foreach (var item in datasources)
            {
                formValue.FormValueDataSource.Add(new Models.FormValueDataSource
                {
                    FormValueId = formValue.Id,
                    InputKey = field._key.Value,
                    FormDataSourceId = Accessor.Set(item.Key)
                });

            }
        }

        private bool? ConvertImageBase64ToString(FormValueDto RequestedData, List<FormFieldDto> fieldsUrl)
        {
            var FormFieldHasImages = RequestedData.FormFields.Where(c => c.ControlType == FormInputTypes.fileImage).ToList();
            if (FormFieldHasImages.Any())
            {
                for (int i = 0; i < FormFieldHasImages.Count; i++)
                {

                    var field = RequestedData.FormFields.Find(c => c.Key == FormFieldHasImages[i].Key);
                    var fieldValue = field.ValueBase64.ToString().Replace("ValueKind = String :", string.Empty);
                    if (!string.IsNullOrEmpty(fieldValue) && Files.GetBase64FileSizeMb(fieldValue) > FileSizeMb)
                        return false;
                    else if (!string.IsNullOrEmpty(fieldValue))
                    {
                        var imageUrl = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(fieldValue)
                            ? Images.SaveSingleImageOnServer(fieldValue, null, ImagesSavePath, false) : null;
                        RequestedData.FormFields.Find(c => c.Key == FormFieldHasImages[i].Key).Url = null;
                        RequestedData.FormFields.Find(c => c.Key == FormFieldHasImages[i].Key).Value = imageUrl;
                        fieldsUrl.Add(new FormFieldDto { _key = field._key, Url = imagesGetPath + "/" + imageUrl });
                        return true;
                    }
                }
            }
            return null;
        }

        private bool? ConvertFileBase64ToString(FormValueDto RequestedData, List<FormFieldDto> fieldsUrl)
        {
            var FormFieldHasFiles = RequestedData.FormFields.Where(c => c.ControlType == FormInputTypes.fileDoc).ToList();
            if (FormFieldHasFiles.Any())
            {
                for (int i = 0; i < FormFieldHasFiles.Count; i++)
                {
                    var field = RequestedData.FormFields.Find(c => c.Key == FormFieldHasFiles[i].Key);
                    var fieldValue = field.ValueBase64.ToString().Replace("ValueKind = String :", string.Empty);
                    if (!string.IsNullOrEmpty(fieldValue) && Files.GetBase64FileSizeMb(fieldValue) > FileSizeMb)
                        return false;
                    if (!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(fieldValue))
                    {
                        var FileName = Strings.GenerateGUID() + ".pdf";
                        var fileUrl = Files.SaveBase64FileToServer(FileName, fieldValue, FilesSavePath);
                        RequestedData.FormFields.Find(c => c.Key == FormFieldHasFiles[i].Key).Url = null;
                        RequestedData.FormFields.Find(c => c.Key == FormFieldHasFiles[i].Key).Value = FileName;
                        fieldsUrl.Add(new FormFieldDto { _key = field._key, Url = filesGetPath + "/" + FileName });
                        return true;
                    }
                }
            }
            return null;
        }

        private static int? GetReportReferenceId(List<Models.FormValueDetails> formValueDetails)
        {
            if (formValueDetails.Any())
            {
                var reference = formValueDetails.FirstOrDefault(c => c.FormInput.Property == "referenceId");
                if (reference != null)
                    return Accessor.Set(reference.InputValue);
                else return null;

            }
            else return null;
        }


        private static void AddFormValueDetails(Models.FormValue formValue, FormFieldDto field)
        {
            var valueDetail = new Models.FormValueDetails();

            valueDetail.FormValueId = formValue.Id;
            valueDetail.InputKey = field._key.Value;
            if (field.ControlType == FormInputTypes.fileImage || field.ControlType == FormInputTypes.fileDoc)
            {
                if (!string.IsNullOrEmpty(field.Url) && field.Url.Contains("Resource"))
                {
                    string[] urlSegments = field.Url.Split(new string[] { "/Resource/" }, StringSplitOptions.None);
                    if (urlSegments.Length > 0) field.Value = urlSegments[1];
                }
            }
            valueDetail.InputValue = field.Value != null ? field.Value.ToString() : string.Empty;
            valueDetail.Description = field.Description;
            valueDetail.Order = field.Order;
            valueDetail.InputTypeId = field.Type;

            if (field.Value is not null && !string.IsNullOrEmpty(field.Value.ToString()))
            {
                switch (field.ControlType)
                {
                    case FormInputTypes.number:
                        {
                            valueDetail.NumericValue = decimal.Parse(field.Value.ToString());
                        }
                        break;

                    case FormInputTypes.date:
                    case FormInputTypes.time:
                    case FormInputTypes.datetime:
                    case FormInputTypes.hijriDate:
                        {
                            valueDetail.DateTimeValue = DateTime.Parse(field.Value.ToString());
                        }
                        break;

                    case FormInputTypes.onecheckbox:
                    case FormInputTypes.inputSwitch:
                        {
                            valueDetail.BooleanValue = bool.Parse(field.Value.ToString());
                        }
                        break;
                }
            }
            formValue.FormValueDetails.Add(valueDetail);
        }

        private bool IsFieldHasSingleDatasource(FormFieldDto field)
        {
            return field.ControlType == FormInputTypes.radio || field.ControlType == FormInputTypes.dropdownSingle;
        }
        private bool IsFieldHasMultipleDatasource(FormFieldDto field)
        {
            return
                 field.ControlType == FormInputTypes.checkbox
                || field.ControlType == FormInputTypes.dropdownMultiple
                || field.ControlType == FormInputTypes.plistbox;

        }

        private Models.FormValuesActions AddAction(Dtos.DyFormEntities.FormValuesActions model, int? toReferenceId)
        {

            var action = new Models.FormValuesActions();
            action.FormValueId = model._formValueId;
            action.EngineActionJobRoleId = model._engineActionJobRoleId;
            action.ActionId = model._actionId;
            action.CreatedBy = RequestOwner.Id;
            action.CreatedDate = TransactionDate;
            action.FromUserId = model._fromUserId;
            action.ToReferenceId = toReferenceId;
            action.Notes = model.Notes;
            action.IsReturned = model.IsReturned;
            action.IsRejected = model.IsRejected;
            action.TransferToReferenceId = model._transferToReferenceId;

            _unitOfWork.FormValuesActions.Add(action);
            _unitOfWork.Complete();
            return action;
        }

        private Dtos.DyFormEntities.FormValuesActions MapAction(Models.FormValuesActions model)
        {
            var action = _unitOfWork.FormValuesActions.GetAll().Include(c => c.Action)
                .Include(c => c.EngineActionJobRole)
               .Include(c => c.CreatedByNavigation)
               .Where(c => c.Id == model.Id).FirstOrDefault();

            return new Dtos.DyFormEntities.FormValuesActions
            {
                _id = model.Id,
                _formValueId = model.FormValueId,
                _engineActionJobRoleId = model.EngineActionJobRoleId,
                _actionId = model.ActionId,
                _fromUserId = model.FromUserId,
                _toReferenceId = model.ToReferenceId,
                _createdBy = model.CreatedBy,
                IsRejected = model.IsRejected,
                IsReturned = model.IsReturned,
                PersonCreatedBy = action != null && action.CreatedByNavigation != null ? action.CreatedByNavigation.Name : string.Empty,
                Notes = model.Notes,
                AllowEditNote = model.CreatedBy == RequestOwner.Id && action.EngineActionJobRole.HasNote.HasValue == true,
                ActionNameAr = SetActionNameAr(action), //action != null && action.Action!=null ? action.Action.NameAr : string.Empty,
                ActionNameEn = SetActionNameEn(action),// action != null && action.Action !=null? action.Action.NameEn : string.Empty,
                CreatedDateString = model.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd")
            };
        }

        private List<Dtos.DyFormEntities.FormValuesActions> GetFormValueActions(int? formValueId)
        {

            var actions = _unitOfWork.FormValuesActions.GetAll().Include(c => c.Action)
                .Include(c => c.EngineActionJobRole)
                .Include(c => c.CreatedByNavigation)
                .Where(c => c.FormValueId == formValueId)
                .Select(c => new Dtos.DyFormEntities.FormValuesActions
                {
                    _id = c.Id,
                    _formValueId = c.FormValueId,
                    _engineActionJobRoleId = c.EngineActionJobRoleId,
                    _actionId = c.EngineActionJobRole.ActionId,
                    _jobRoleId = c.EngineActionJobRole.JobRoleId,
                    ActionNameAr = SetActionNameAr(c),
                    ActionNameEn = SetActionNameEn(c),
                    PersonCreatedBy = c.CreatedByNavigation != null ? c.CreatedByNavigation.Name : string.Empty,
                    CreatedDateString = c.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    Notes = c.Notes,
                    IsReturned = c.IsReturned,
                    IsRejected = c.IsRejected,
                    _transferToReferenceId = c.TransferToReferenceId,
                    AllowEditNote = c.CreatedBy == RequestOwner.Id && c.EngineActionJobRole.HasNote.Value == true,
                    IsClosed = c.EngineActionJobRole.CloseStep != null && c.ActionId == c.EngineActionJobRole.CloseStep ? true : null

                }).OrderBy(c => c._id).ToList();

            return actions;

        }

        private async Task GetEngineActionDetailForEmail(Dtos.DyFormEntities.FormValuesActions model, EngineActionJobRoleEmailDataRecord engineActionEmailData, List<FormValueDetails> formValueDetails)
        {
            var engineAction = await _unitOfWork.EnginesActionsJobRole.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == model._engineActionJobRoleId && c.IsDeleted != true);
            if (engineAction is not null && engineAction.IsSendEmail == true)
            {
                engineActionEmailData._engineActionJobRoleId = model._engineActionJobRoleId;
                engineActionEmailData.IsSendEmail = engineAction.IsSendEmail;
                engineActionEmailData.EmailBody = engineAction.EmailBody;
                engineActionEmailData.SendTo = formValueDetails.FirstOrDefault(c => c.FormInput.Type == (int)DynamicFormEnums.InputTypes.Email).InputValue;
            }
        }



        #region Helper Methods  GetFormValueActions API
        static string SetActionNameAr(Models.FormValuesActions model)
        {
            if (model.IsReturned == true)
                return $"ارجاع";

            else if (model.IsRejected == true)
                return "رفض";

            else if (model.EngineActionJobRole.CloseStep == model.ActionId)
                return "موافقة واغلاق";
            else return model.Action.NameAr;
        }

        static string SetActionNameEn(Models.FormValuesActions model)
        {
            if (model.IsReturned == true)
                return $" Return ";

            else if (model.IsRejected == true)
                return "Reject";

            else if (model.EngineActionJobRole.CloseStep == model.ActionId)
                return "Agree And Closed";
            else return model.Action.NameEn;
        }

        #endregion

        #region HELPER METHODS GetFormValueDetail

        private static void SetFormFieldValue(Models.FormValue formValue, FormFieldDto field)
        {

            switch (field.ControlType)
            {
                case FormInputTypes.fileImage:
                    {
                        SetUrlImageValue(formValue, field);
                    }
                    break;
                case FormInputTypes.fileDoc:
                    {
                        SetFileUrlValue(formValue, field);
                    }
                    break;
                case FormInputTypes.hijriDate:
                    {
                        SetHijriDateValue(formValue, field);
                    }
                    break;

                case FormInputTypes.plistbox:
                case FormInputTypes.dropdownMultiple:
                case FormInputTypes.checkbox:

                    {
                        SetValueToMultipleOptions(formValue, field);
                    }
                    break;


                default:
                    SetInputValue(formValue, field);
                    break;

            };



        }

        private static void SetInputValue(Models.FormValue formValue, FormFieldDto field)
        {

            if (formValue.FormValueDetails.Any())
            {
                var fdetail = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                field.Value = fdetail != null ? fdetail.InputValue : null;
                field.Description = fdetail != null ? fdetail.Description : string.Empty;
            }

        }

        private static void SetValueToMultipleOptions(Models.FormValue formValue, FormFieldDto field)
        {
            if (formValue.FormValueDetails.Any())
            {
                var fv = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                if (fv != null)
                {
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemsValue>>(fv.InputValue);
                    if (list.Count > 0)
                    {
                        var inputValues = list.Select(c => new { c.Key, c.ValueAr, c.ValueEn }).ToList();
                        field.Value = inputValues;
                    }
                }
            }
        }

        private static void SetHijriDateValue(Models.FormValue formValue, FormFieldDto field)
        {
            if (formValue.FormValueDetails.Any())
            {
                var fdetail = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                field.Value = fdetail != null ? Dates.ConvertFromGerogianToHijriDateString(DateTime.Parse(Convert.ToString(fdetail.InputValue).Replace("ValueKind = String :", ""))) : null;
            }
        }

        private static void SetFileUrlValue(Models.FormValue formValue, FormFieldDto field)
        {
            if (formValue.FormValueDetails.Any())
            {
                var fv = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                if (fv != null)
                {
                    field.Url = SetDocumentFilePath(fv.InputValue);
                }
            }
        }

        private static void SetUrlImageValue(Models.FormValue formValue, FormFieldDto field)
        {
            if (formValue.FormValueDetails.Any())
            {
                var fv = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                if (fv != null)
                {
                    field.Url = SetImageFilePath(fv.InputValue);
                }
            }
        }

        private record ItemsValue(string Key, string ValueAr, string ValueEn);
        private static string SetImageFilePath(string val)
        {
            return !string.IsNullOrEmpty(val) ? imagesGetPath + "/" + val : imagesGetPath + "/noImage.png";
        }
        private static string SetDocumentFilePath(string val)
        {
            return !string.IsNullOrEmpty(val) ? filesGetPath + "/" + val : filesGetPath + "/noImage.png";
        }

        #endregion

        #endregion


        #endregion

        public async Task<OperationOutput> CheckPersonIntegrationData(IntegrationIInputDto integrationData)
        {
            OperationOutput Result = new OperationOutput();
            if (RequestOwner == null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                return Result;
            }
            bool? isPersonExisit = null;
            string routingUrl = string.Empty;

            if (integrationData is not null && !String.IsNullOrEmpty(integrationData.idNumber) && integrationData.CheckPersonalData)
            {
                if (integrationData.birthOfDateIsHijri)
                {
                    isPersonExisit = await InvokeIntegratePersonalDataByHijriDate(integrationData, isPersonExisit);
                    if (isPersonExisit == false)
                    {
                        isPersonExisit = await InvokeIntegratePersonalDataByGeorogianDate(integrationData, isPersonExisit);
                        if (isPersonExisit == false)
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.IncorrectIdentityData);
                            return Result;
                        }
                    }

                }
                if (!integrationData.birthOfDateIsHijri)
                {
                    isPersonExisit = await InvokeIntegratePersonalDataByGeorogianDate(integrationData, isPersonExisit);
                    if (isPersonExisit == false)
                    {
                        integrationData.birthOfDateHijri = Dates.ConvertFromGerogianToHijriDateString(DateTime.Parse(integrationData.birthOfDateGeorogian), Core.Consts.DateFormat.DayMonthYear);
                        isPersonExisit = await InvokeIntegratePersonalDataByHijriDate(integrationData, isPersonExisit);
                        if (isPersonExisit == false)
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.IncorrectIdentityData);
                            return Result;
                        }
                    }
                }
            }

            if (integrationData.CheckApplicationNo && !String.IsNullOrEmpty(integrationData.ApplicationNo))
            {
                var jobApplication = await _unitOfWork.JobApplication.GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Code == integrationData.ApplicationNo);
                if (jobApplication is null)
                {
                    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.ApplicationNoNotFound);
                    return Result;
                }

            }

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;


        }

        #region HELPER METHODS INTEGRATIONS
        private async Task<bool?> InvokeIntegratePersonalDataByGeorogianDate(IntegrationIInputDto RequestedData, bool? isPersonExisit)
        {
            RequestedData.birthOfDate = RequestedData.birthOfDateGeorogian;
            isPersonExisit = await GetIntegrationData(RequestedData);
            return isPersonExisit;
        }

        private async Task<bool?> InvokeIntegratePersonalDataByHijriDate(IntegrationIInputDto RequestedData, bool? isPersonExisit)
        {
            RequestedData.birthOfDate = RequestedData.birthOfDateHijri;
            isPersonExisit = await GetIntegrationData(RequestedData);
            return isPersonExisit;
        }


        private async Task<bool> GetIntegrationData(IntegrationIInputDto userInfo)
        {
            var personData = await InvokeService<IntegrationData>.Invoke(yaqeenPersonInfoUrl, userInfo);

            if (personData is not null && personData.Body is not null && personData.Body.UserInformation is not null)
                return true;

            return false;
        }

        #endregion

        public async Task<OperationOutput> SaveAction(Dtos.DyFormEntities.FormValuesActions model)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Dtos.DyFormEntities.FormValuesActions actionResult = null;
            EngineActionJobRoleEmailDataRecord engineActionEmailData = new();

            var formValueDetails = await _unitOfWork.FormValueDetails.GetAll(c => c.FormValueId == model._formValueId)
                                 .Include(c => c.FormInput).AsNoTracking().ToListAsync();

            if (formValueDetails.Count > 0)
            {
                int? referenceId = GetReportReferenceId(formValueDetails);

                var action = AddAction(model, referenceId);
                if (action != null)
                {
                    actionResult = MapAction(action);
                }
                await GetEngineActionDetailForEmail(model, engineActionEmailData, formValueDetails);

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.Action, actionResult),
                     new OutputDictionary(OperationOutputKeys.EngineActionEmailData, engineActionEmailData));
            }
            else return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        }



        public async Task<OperationOutput> EditActionNoteByUserCreated(Dtos.DyFormEntities.FormValuesActions model)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var action = await _unitOfWork.FormValuesActions.GetAll()
                .OrderByDescending(c => c.Id).AsNoTracking()
                .FirstOrDefaultAsync(c => c.CreatedBy == RequestOwner.Id
                       && c.FormValueId == model._formValueId
                       && (c.IsReturned != true && c.IsRejected != true ? c.EngineActionJobRoleId == model._engineActionJobRoleId : true));

            if (action is not null)
            {
                action.Notes = model.Notes;
                _unitOfWork.Complete();

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.Id, Accessor.Get<int?>(action.Id)));
            }
            else return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        }

        #region Draw Data For List
        public async Task<OperationOutput> GetFormDataList(DynamicFormWorkflowListDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            DynamicFormWorkflowListDto ItemResult = new Dtos.DyFormEntities.DynamicFormWorkflowListDto();
            Dtos.DyFormEntities.FormListDataResult formResult = new Dtos.DyFormEntities.FormListDataResult();
            UserPermission userPermission = new UserPermission();
            Models.FormsEntity entityForm = null;

            if (!RequestedData._entityId.HasValue && !string.IsNullOrEmpty(RequestedData.EntityUrl))
            {
                var entity = await _unitOfWork.Entity.GetAll().FirstOrDefaultAsync(c => c.FrontIdentity.ToLower() == RequestedData.EntityUrl.ToLower());

                if (entity is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData._entityId = entity.Id;
                userPermission = GetUserEntityPermission(entity.Id, RequestedData._referenceId);
            }

            if (RequestedData._entityId.HasValue)
                entityForm = _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).FirstOrDefault(c => c.EntityId == RequestedData._entityId);

            if (entityForm != null)
            {
                RequestedData._formId = entityForm.FormId;
                ItemResult._formId = entityForm.FormId;
                bool accessToForm = await _unitOfWork.Forms.GetAll().CountAsync(c => c.Id == entityForm.FormId && c.ReferenceId == RequestedData._referenceId) > 0 ? true : false;

                var Headers = GetFormHeader(RequestedData);
                if (accessToForm)
                {
                    formResult = GetFormListValue(RequestedData, Headers);
                }
                ItemResult.FormListHeader = Headers;
                ItemResult.ListFormValue = formResult.ListFormValue;
                ItemResult.HeaderUserActions = formResult.HeaderUserActions;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.EntityFormValue, ItemResult),
                new OutputDictionary(OperationOutputKeys.Pagination, formResult.Pagination),
                new OutputDictionary(OperationOutputKeys.EntityObj, entityForm != null && entityForm.Entity != null ? new Dtos.DyFormEntities.Entity { _id = entityForm.Entity.Id, FrontIdentity = entityForm.Entity.FrontIdentity, NameAr = entityForm.Entity.NameAr, NameEn = entityForm.Entity.NameEn, CmsIdentity = entityForm.Entity.CmsIdentity } : null),
                new OutputDictionary(OperationOutputKeys.UserPermission, userPermission));
        }



        #region HELPER MTHODS
        private List<FormListHeaderDto> GetFormHeader(DynamicFormWorkflowListDto RequestedData)
        {
            List<FormListHeaderDto> Headers = new List<FormListHeaderDto>();
            var formDynamicHeader = _unitOfWork.FormInputs.FindAll(c => c.FormId == RequestedData._formId
            && c.ShowInMainListCP == true && c.IsDeleted == false).Select(c => new Dtos.DyFormEntities.FormListHeaderDto
            {
                _key = c.Id,
                TitleAr = c.NameAr,
                TitleEn = c.NameEn,
                Property = c.Property,
                Order = c.Order
            }).OrderBy(c => c.Order ?? c._key).ToList();

            var headers = formDynamicHeader.Concat(FormListHeaderDto.GetStaticHeaderAttributes()).ToList();
            Headers.AddRange(headers);
            return Headers;
        }

        private Dtos.DyFormEntities.FormListDataResult GetFormListValue(DynamicFormWorkflowListDto RequestedData, List<FormListHeaderDto> Headers)
        {
            Dtos.DyFormEntities.FormListDataResult formResult = new Dtos.DyFormEntities.FormListDataResult();
            formResult.ListFormValue = new List<object>();
            int NumberOfRecord;
            int CurrentPageIndex = 1;
            List<Models.FormValuesActions> userValueActions = new List<Models.FormValuesActions>();
            List<FormValueActionList> userReports = null;
            List<int?> userActions = new List<int?>();
            Models.User userDbItem = null;

            if (RequestOwner != null && RequestOwner.Id != null)
                userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            var engineForm = _unitOfWork.EngineForms.GetAll()
               .Where(f => f.FormId == RequestedData._formId && f.IsDeleted == false)
               .FirstOrDefault();

            if (userDbItem != null && engineForm != null)
            {
                var workflowEnginesActionsJobRole = _unitOfWork.EnginesActionsJobRole.GetAll()
                    .Include(c => c.ActionNavigation)
                    .Where(c => c.EngineId == engineForm.WorkFlowEngineId
                    && c.IsDeleted != true).OrderBy(c => c.StepNo).Select(c => c).ToList();

                if (workflowEnginesActionsJobRole.Count > 0)
                {

                    userActions = workflowEnginesActionsJobRole.Where(c => c.JobRoleId == userDbItem.JobRole)
                      .Select(c => c.ActionId).ToList();

                    // check if user role = first user (( register report ))
                    if (workflowEnginesActionsJobRole.First().JobRoleId == userDbItem.JobRole)
                    {
                        userValueActions = GetFormValueActionsForRegistrationUser(engineForm, userActions);
                    }

                    else // Other uer role workflow
                    {
                        userValueActions = GetFormValueActionsForWorkflowUsers(userActions, engineForm, userDbItem);
                    }

                    if (userValueActions.Any())
                    {

                        NumberOfRecord = userValueActions.Count();

                        if (RequestedData.Pagination != null)
                        {
                            if (RequestedData.Pagination.CurrentPageIndex.HasValue)
                                CurrentPageIndex = RequestedData.Pagination.CurrentPageIndex.Value;
                            if (RequestedData.Pagination.RecordPerPage.HasValue)
                                DefaultPaginationCount = RequestedData.Pagination.RecordPerPage.Value;

                        }
                        else DefaultPaginationCount = NumberOfRecord;


                        userReports = userValueActions.Select(c => new Dtos.DyFormEntities.FormValueActionList
                        {
                            _id = c.FormValueId,
                            PersonCreatedBy = c.CreatedByNavigation != null ? c.CreatedByNavigation.Name : string.Empty,
                            Notes = c.Notes,
                            IsActive = c.FormValue.IsActive,
                            StatusStringAr = c.FormValue.IsActive == true ? "فعال" : "غير فعال",
                            StatusStringEn = c.FormValue.IsActive == true ? "Active" : "Deactive ",
                            ActionNameAr = SetActionNameAr(c),
                            ActionNameEn = SetActionNameEn(c),
                            FormValueDetails = c.FormValue.FormValueDetails.Any() ? c.FormValue.FormValueDetails
                                               .Where(d => d.FormInput.ShowInMainListCP == true && d.FormInput.IsDeleted == false)
                                               .Where(d => workflowEnginesActionsJobRole.First().JobRoleId != userDbItem.JobRole && c.ToReferenceId != null && c.TransferToReferenceId == null ? c.ToReferenceId == userDbItem.ReferenceId : true)
                                               .Where(d => workflowEnginesActionsJobRole.First().JobRoleId != userDbItem.JobRole && c.TransferToReferenceId != null ? c.TransferToReferenceId == userDbItem.ReferenceId : true)

                       .Select(c => new Dtos.DyFormEntities.FormValueDetailsDto
                       {
                           _key = c.InputKey,
                           Value = GetValueForInputList(c), //c.FormInput != null && c.FormInput.Type != (int)DynamicFormEnums.InputTypes.DropdownSingle ? c.InputValue : c.Description,
                           Property = c.FormInput != null ? c.FormInput.Property.Trim() : null,
                           Order = c.FormInput != null ? c.FormInput.Order : null
                       }).OrderBy(c => c.Order ?? c._key).ToList() : new List<FormValueDetailsDto>()

                        }).Skip((CurrentPageIndex - 1) * DefaultPaginationCount)
                          .Take(DefaultPaginationCount).ToList();


                        if (userReports.Any())
                        {

                            foreach (var item in userReports)
                            {
                                Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();
                                var except = Headers.Where(c => c._key != null).Select(c => new { c._key, c.Property, c.Order }).Except(item.FormValueDetails.Select(c => new { c._key, c.Property, c.Order })).ToList();
                                if (except.Any())
                                {
                                    foreach (var p in except)
                                    {
                                        item.FormValueDetails.Add(new Dtos.DyFormEntities.FormValueDetailsDto { _key = p._key, Value = null, Property = p.Property, Order = p.Order });
                                    }
                                    item.FormValueDetails = item.FormValueDetails.OrderBy(c => c.Order ?? c._key).ToList();
                                }

                                PropertyValues.Add("id", item.Id);
                                foreach (var p in item.FormValueDetails)
                                {
                                    PropertyValues.Add(p.Property, p.Value);
                                }

                                PropertyValues.Add("PersonCreatedBy", item.PersonCreatedBy);
                                PropertyValues.Add("Notes", !string.IsNullOrEmpty(item.Notes) ? item.Notes.Substring(0, item.Notes.Length > 100 ? 100 : item.Notes.Length) : string.Empty);
                                PropertyValues.Add("IsActive", item.IsActive);
                                PropertyValues.Add("ActionNameAr", item.ActionNameAr);
                                PropertyValues.Add("ActionNameEn", item.ActionNameEn);
                                PropertyValues.Add("StatusStringAr", item.StatusStringAr);
                                PropertyValues.Add("StatusStringEn", item.StatusStringEn);

                                formResult.ListFormValue.Add(PropertyValues);
                            }
                        }

                        formResult.Pagination = new ApplicationOperation.Pagination
                        {
                            TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount),
                            CurrentPageIndex = CurrentPageIndex,
                            TotalItemsCount = NumberOfRecord
                        };

                    }

                    var headerUserActions = new HeaderUserActions();
                    headerUserActions.AllowAddNew =
                    headerUserActions.AllowActivate =
                    headerUserActions.AllowDelete = workflowEnginesActionsJobRole.First().JobRoleId == userDbItem.JobRole ? true : false;// get if user jobrole register report

                    formResult.HeaderUserActions = headerUserActions;
                }
            }

            return formResult;
        }

        private List<Models.FormValuesActions> GetFormValueActionsForWorkflowUsers(List<int?> userActions, EngineForms engineForm, Models.User userDbItem)
        {
            List<Models.FormValuesActions> userValueActions;

            var formValueGroupQuery = _unitOfWork.FormValuesActions.GetAll()
                                    .Where(c => c.EngineActionJobRole.EngineId == engineForm.WorkFlowEngineId)
                                    .GroupBy(c => c.FormValueId).Select(c => c.Max(c => c.Id));


            userValueActions = _unitOfWork.FormValuesActions.GetAll()
          .Include(c => c.CreatedByNavigation)
          .Include(c => c.EngineActionJobRole)
          .Include(c => c.FormValue)
          .ThenInclude(c => c.FormValueDetails)
          .ThenInclude(c => c.FormInput)
          .ThenInclude(c => c.FormInputDataSource)
          .ThenInclude(c => c.DataSource)
          .Where(c => c.EngineActionJobRole.EngineId == engineForm.WorkFlowEngineId
           && c.FormValue.IsActive == true && c.FormValue.IsDeleted == false
           && formValueGroupQuery.Contains(c.Id)
           && userActions.Contains(c.EngineActionJobRole.NextStep))
          .OrderByDescending(c => c.CreatedDate).ToList();

            var _userValueActionsTransfer = userValueActions.Where(c => c.TransferToReferenceId == userDbItem.ReferenceId).ToList();

            var _userValueActions = userValueActions.Where(c => c.ToReferenceId == userDbItem.ReferenceId).ToList();

            if (_userValueActions.Any())
            {
                _userValueActionsTransfer.AddRange(_userValueActions);
                return _userValueActionsTransfer;
            }
            else return userValueActions;
        }

        private List<Models.FormValuesActions> GetFormValueActionsForRegistrationUser(EngineForms engineForm, List<int?> userActions)
        {
            List<Models.FormValuesActions> userValueActions;
            var formValueGroupQuery = _unitOfWork.FormValuesActions.GetAll()
                                        .Where(c => c.EngineActionJobRole.EngineId == engineForm.WorkFlowEngineId)
                                        .GroupBy(c => c.FormValueId).Where(g => userActions.Count() == 1 ? g.Count() == 1 : true).Select(c => c.Max(c => c.Id));

            userValueActions = _unitOfWork.FormValuesActions.GetAll()
            .Include(c => c.CreatedByNavigation)
            .Include(c => c.EngineActionJobRole)
            .Include(c => c.FormValue)
            .ThenInclude(c => c.FormValueDetails)
            .ThenInclude(c => c.FormInput)
            .ThenInclude(c => c.FormInputDataSource)
            .ThenInclude(c => c.DataSource)
            .Where(c => c.EngineActionJobRole.EngineId == engineForm.WorkFlowEngineId && c.FormValue.IsDeleted == false
             && formValueGroupQuery.Contains(c.Id)).OrderByDescending(c => c.CreatedDate).ToList();
            return userValueActions;
        }

        private static string GetValueForInputList(Models.FormValueDetails model)
        {
            if (model != null)
            {
                switch (model.FormInput.Type)
                {
                    case (int)InputTypes.Date:
                        return !string.IsNullOrEmpty(model.InputValue) ? DateTime.Parse(model.InputValue).ToString("yyyy-MM-dd") : string.Empty;

                    case (int)InputTypes.DropdownSingle:
                        return model.Description;

                    case (int)InputTypes.OneChoiceRadio:
                        var dSource = model.FormInput.FormInputDataSource.Where(c => c.DataSourceId == Accessor.Set(model.InputValue)).FirstOrDefault();
                        if (dSource != null)
                            return dSource.DataSource.TextAr;
                        else return string.Empty;

                    default:
                        return model.InputValue;
                }
            }
            return string.Empty;
        }

        #endregion


        #endregion
        #region UserPermissions

        public UserPermission GetUserEntityPermission(int? EntityId, int? ReferenceId)
        {
            UserPermission userPermission = new UserPermission();
            var user = _unitOfWork.User.GetAll().Include(x => x.UsersEntities).Include(x => x.UsersEntitiesReferences).FirstOrDefault(x => x.Id == RequestOwner.Id);

            if (user != null)
            {
                if (user.ReferenceId == ReferenceId)
                {
                    var userEntity = user.UsersEntities.FirstOrDefault(x => x.EntityId == EntityId);
                    if (userEntity == null)
                        return userPermission;

                    userPermission.Add = userEntity.Add;
                    userPermission.Edit = userEntity.Edit;
                    userPermission.Delete = userEntity.Delete;
                    userPermission.Activate = userEntity.Activate;
                    userPermission.List = userEntity.List;
                    userPermission.View = userEntity.View;
                    userPermission.Reports = userEntity.Reports;
                }
                else
                {
                    var userEntity = user.UsersEntitiesReferences.FirstOrDefault(x => x.EntityId == EntityId && x.ReferenceId == ReferenceId);
                    if (userEntity == null)
                        return userPermission;

                    userPermission.Add = userEntity.Add;
                    userPermission.Edit = userEntity.Edit;
                    userPermission.Delete = userEntity.Delete;
                    userPermission.Activate = userEntity.Activate;
                    userPermission.List = userEntity.List;
                    userPermission.View = userEntity.View;
                    userPermission.Reports = userEntity.Reports;
                }
            }
            return userPermission;
        }
        #endregion


        #region registration user role apis

        public async Task<OperationOutput> ModelActions(FormValueDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData._id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.FormValue.ExecuteUpdateAsync(x => x.Id == RequestedData._id.Value,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.FormValue.ExecuteUpdateAsync(x => x.Id == RequestedData._id.Value,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }





            //var formValue = await _unitOfWork.FormValue.GetByIdAsync(RequestedData._id.Value);

            //if (formValue is null)
            //    return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            //formValue.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : formValue.IsActive;
            //formValue.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : formValue.IsDeleted;

            //formValue.UpdatedBy = RequestOwner.Id;
            //formValue.UpdatedDate = DateTime.Now;

            //_unitOfWork.FormValue.Update(formValue);
            //await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
           new OutputDictionary(OperationOutputKeys.ItemActivation, RequestedData.IsActive));

        }



        #endregion


        #region Portal

        public async Task<OperationOutput> GetDetailAndActionStepper(Dtos.DyFormEntities.FormView RequestedData)
        {
            if (!RequestedData.FormValueId.HasValue)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
            }
            var form = new Dtos.DyFormEntities.FormView();
            var userActions = new List<UserActionJobRole>();
            var formsValuesActions = new List<Dtos.DyFormEntities.FormValuesActions>();
            var transferRefrences = new List<ReferencesTreeDto>();
            Models.FormsEntity entityForm = null;

            entityForm = await GetEntityForm(RequestedData);

            if (entityForm is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            // formId     // from dynamic view
            RequestedData._id = !RequestedData._id.HasValue ? entityForm.FormId : RequestedData._id.Value;
            RequestedData.EntityId = entityForm.EntityId;

            Models.User userDbItem = null;

            if (RequestOwner is not null && RequestOwner.Id.HasValue)
                userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            var engineForm = await GetEngionFormForPortal(RequestedData);

            if (engineForm is not null)
            {
                var formSteps = await GetFormActionStepperForPortal(engineForm);

                form = engineForm.Form.Adapt<Dtos.DyFormEntities.FormView>(Dtos.DyFormEntities.FormView.SelectConfig(imagesGetPath, engineForm, formSteps));

            }
            if (form is not null && form.ActionStepper.Any() && RequestOwner.Id.HasValue)
            {
                GetActionStepWithInputs(form, engineForm, userDbItem, RequestedData.FormValueId);
            }

            if (engineForm is not null)
            {
                userActions = GetActionsAllowedToCurrentUser(userDbItem, engineForm.WorkFlowEngineId.Value, form);
                if (RequestedData.FormValueId != null)
                {
                    if (form.ActionStepper.Any(c => c.IsTransferToReference == true))
                    {
                        transferRefrences = GetParentReferenceByReportReferenceId(RequestedData);
                    }

                    formsValuesActions = GetFormValueActions(RequestedData.FormValueId);
                }
            }


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.DynamicForm, form),
                 new OutputDictionary(OperationOutputKeys.EntityName, entityForm != null && entityForm.Entity != null ? entityForm.Entity.CmsIdentity.Split('/')[1] : string.Empty),
                 new OutputDictionary(OperationOutputKeys.UserActions, userActions),
                 new OutputDictionary(OperationOutputKeys.FormValueActions, formsValuesActions),
                 new OutputDictionary(OperationOutputKeys.TransferRefrences, transferRefrences));


        }

        #region HELPER METHOD



        private Dtos.DyFormEntities.FormListDataResult GetFormListValueForPortal(DynamicFormWorkflowListDto RequestedData)
        {
            Dtos.DyFormEntities.FormListDataResult formResult = new Dtos.DyFormEntities.FormListDataResult();
            formResult.ListFormValue = new List<object>();
            int NumberOfRecord;
            int CurrentPageIndex = 1;
            List<Models.FormValuesActions> userValueActions = new List<Models.FormValuesActions>();
            List<FormValueActionList> userReports = null;
            List<int?> userActions = new List<int?>();
            Models.User userDbItem = null;

            if (RequestOwner != null && RequestOwner.Id != null)
                userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            var engineForm = _unitOfWork.EngineForms.GetAll()
               .Where(f => f.FormId == RequestedData._formId && f.IsDeleted == false)
               .FirstOrDefault();

            if (userDbItem != null && engineForm != null)
            {
                var workflowEnginesActionsJobRole = _unitOfWork.EnginesActionsJobRole.GetAll()
                    .Include(c => c.ActionNavigation)
                    .Where(c => c.EngineId == engineForm.WorkFlowEngineId
                    && c.IsDeleted != true).OrderBy(c => c.StepNo).Select(c => c).ToList();

                if (workflowEnginesActionsJobRole.Count > 0)
                {

                    userActions = workflowEnginesActionsJobRole.Where(c => c.JobRoleId == userDbItem.JobRole)
                      .Select(c => c.ActionId).ToList();

                    // check if user role = first user (( register report ))
                    if (workflowEnginesActionsJobRole.First().JobRoleId == userDbItem.JobRole)
                    {
                        userValueActions = GetFormValueActionsForRegistrationUser(engineForm, userActions);
                    }

                    else // Other uer role workflow
                    {
                        userValueActions = GetFormValueActionsForWorkflowUsers(userActions, engineForm, userDbItem);
                    }

                    if (userValueActions.Any())
                    {

                        NumberOfRecord = userValueActions.Count();

                        if (RequestedData.Pagination != null)
                        {
                            if (RequestedData.Pagination.CurrentPageIndex.HasValue)
                                CurrentPageIndex = RequestedData.Pagination.CurrentPageIndex.Value;
                            if (RequestedData.Pagination.RecordPerPage.HasValue)
                                DefaultPaginationCount = RequestedData.Pagination.RecordPerPage.Value;

                        }
                        else DefaultPaginationCount = NumberOfRecord;


                        userReports = userValueActions.Select(c => new Dtos.DyFormEntities.FormValueActionList
                        {
                            _id = c.FormValueId,
                            ActionNameAr = SetActionNameAr(c),
                            ActionNameEn = SetActionNameEn(c),
                            FormValueDetails = c.FormValue.FormValueDetails.Any() ? c.FormValue.FormValueDetails
                                               .Where(d => d.FormInput.ShowInMainPortalPage == true && d.FormInput.IsDeleted == false)
                                               .Where(d => workflowEnginesActionsJobRole.First().JobRoleId != userDbItem.JobRole && c.ToReferenceId != null && c.TransferToReferenceId == null ? c.ToReferenceId == userDbItem.ReferenceId : true)
                                               .Where(d => workflowEnginesActionsJobRole.First().JobRoleId != userDbItem.JobRole && c.TransferToReferenceId != null ? c.TransferToReferenceId == userDbItem.ReferenceId : true)

                       .Select(c => new Dtos.DyFormEntities.FormValueDetailsDto
                       {
                           _key = c.InputKey,
                           _typeId = c.FormInput.Type,
                           Value = GetValueForInputList(c),
                           DateTimeValue = c.DateTimeValue,
                           NumericValue = c.NumericValue,
                           BooleanValue = c.BooleanValue,
                           Property = c.FormInput != null ? c.FormInput.Property.Trim() : null,
                           Order = c.FormInput != null ? c.FormInput.Order : null
                       }).OrderBy(c => c.Order ?? c._key).ToList() : new List<FormValueDetailsDto>()

                        }).Skip((CurrentPageIndex - 1) * DefaultPaginationCount)
                          .Take(DefaultPaginationCount).ToList();


                        if (userReports.Any())
                        {

                            foreach (var item in userReports)
                            {
                                Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();
                                PropertyValues.Add("id", item.Id);
                                foreach (var p in item.FormValueDetails)
                                {
                                    if (p._typeId == (int)InputTypes.FileImage)
                                        p.Value = $"{imagesDyFormGetPath}/{p.Value}";

                                    else if (p._typeId == (int)InputTypes.HijriDate && p.DateTimeValue.HasValue)
                                        try { p.Value = Dates.ConvertFromGerogianToHijriDateString(p.DateTimeValue.Value); }
                                        catch { }

                                    else if (p._typeId == (int)InputTypes.Date && p.DateTimeValue.HasValue)
                                        p.Value = p.DateTimeValue.Value.ToString("yyyy-MM-dd");

                                    else if (p._typeId == (int)InputTypes.Time && p.DateTimeValue.HasValue)
                                        p.Value = p.DateTimeValue.Value.ToString("hh:mm tt");

                                    else if (p._typeId == (int)InputTypes.DateTime && p.DateTimeValue.HasValue)
                                        p.Value = p.DateTimeValue.Value.ToString("yyyy-MM-dd hh:mm tt");

                                    else if (!string.IsNullOrEmpty(p.Description))
                                        p.Value = p.Description;


                                    PropertyValues.Add(p.Property, p.Value);
                                }
                                formResult.ListFormValue.Add(PropertyValues);
                            }
                        }

                        formResult.Pagination = new ApplicationOperation.Pagination
                        {
                            TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount),
                            CurrentPageIndex = CurrentPageIndex,
                            TotalItemsCount = NumberOfRecord
                        };
                    }


                    var headerUserActions = new HeaderUserActions();
                    headerUserActions.AllowAddNew =
                    headerUserActions.AllowActivate =
                    headerUserActions.AllowDelete = workflowEnginesActionsJobRole.First().JobRoleId == userDbItem.JobRole ? true : false;// get if user jobrole register report

                    formResult.HeaderUserActions = headerUserActions;
                }
            }

            return formResult;
        }




        #endregion


        public async Task<OperationOutput> GetFormUserDataList(DynamicFormWorkflowListDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            DynamicFormWorkflowListDto ItemResult = new Dtos.DyFormEntities.DynamicFormWorkflowListDto();
            Dtos.DyFormEntities.FormListDataResult formResult = new Dtos.DyFormEntities.FormListDataResult();
            UserPermission userPermission = new UserPermission();
            Models.FormsEntity entityForm = null;

            if (!RequestedData._entityId.HasValue && !string.IsNullOrEmpty(RequestedData.EntityUrl))
            {
                var entity = await _unitOfWork.Entity.GetAll().FirstOrDefaultAsync(c => c.FrontIdentity.ToLower() == RequestedData.EntityUrl.ToLower());

                if (entity is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData._entityId = entity.Id;
                userPermission = GetUserEntityPermission(entity.Id, RequestedData._referenceId);
            }

            if (RequestedData._entityId.HasValue)
                entityForm = _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).FirstOrDefault(c => c.EntityId == RequestedData._entityId);

            if (entityForm != null)
            {
                RequestedData._formId = entityForm.FormId;
                ItemResult._formId = entityForm.FormId;
                bool accessToForm = await _unitOfWork.Forms.GetAll().CountAsync(c => c.Id == entityForm.FormId && c.ReferenceId == RequestedData._referenceId) > 0 ? true : false;


                if (accessToForm)
                {
                    formResult = GetFormListValueForPortal(RequestedData);
                }

                ItemResult.ListFormValue = formResult.ListFormValue;
                ItemResult.HeaderUserActions = formResult.HeaderUserActions;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.EntityFormValue, ItemResult),
                new OutputDictionary(OperationOutputKeys.Pagination, formResult.Pagination),
                new OutputDictionary(OperationOutputKeys.EntityObj, entityForm != null && entityForm.Entity != null ? new Dtos.DyFormEntities.Entity { _id = entityForm.Entity.Id, FrontIdentity = entityForm.Entity.FrontIdentity, NameAr = entityForm.Entity.NameAr, NameEn = entityForm.Entity.NameEn, CmsIdentity = entityForm.Entity.CmsIdentity } : null),
                new OutputDictionary(OperationOutputKeys.UserPermission, userPermission));
        }


        #endregion

        #region SendEmail
        public async Task<OperationOutput> SendEmail(EngineActionJobRoleEmailDataRecord RequestedData)
        {
            if (!string.IsNullOrEmpty(RequestedData.SendTo))
            {
                await Email.SendEmailAsync(RequestedData.SendTo, RequestedData.Subject, RequestedData.EmailBody, EmailServiceUrl, Token, null, null);
                Thread.Sleep(500);
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        }


        #endregion

    }



}
