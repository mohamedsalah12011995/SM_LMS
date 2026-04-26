using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Extensions;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.DynamicForms.Const;
using RM.DynamicForms.Dtos;
using RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos;
using RM.DynamicForms.Dtos.DynamicForm.IntegrationEntities;
using RM.DynamicForms.Dtos.DynamicForm.TemplateDtos;
using RM.DynamicForms.DynamicEnum;
using RM.DynamicForms.Services.EntityServices;
using RM.DynamicForms.UnitOfWorks;
using RM.Models;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using static RM.DynamicForms.Dtos.OperationOutput;

namespace RM.DynamicForms.Services
{

    public class DynamicFormService : BaseService, IDynamicFormService
    {

        private readonly IUnitOfWork _unitOfWork;
        private static string imagesGetPath = string.Empty;
        private static string filesGetPath = string.Empty;
        JsonSerializerOptions jsonOptions = null;
        private string yaqeenPersonInfoUrl;
        private string yaqeenIdTypeLookupsUrl;

        private readonly ILogger<DynamicFormService> _logger;


        public DynamicFormService(IUnitOfWork unitOfWork, ILogger<DynamicFormService> logger, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            imagesGetPath = filesGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);

            SetJsonSerializerOptions();
            SetYaqeenLinkConfigurations();

        }

        #region HELPER METHODS >> CONSTRACTOR
        private void SetJsonSerializerOptions()
        {
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            jsonOptions.PropertyNameCaseInsensitive = true;
        }

        private void SetYaqeenLinkConfigurations()
        {
            yaqeenPersonInfoUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("YaqeenPersonInfoUrl");
            yaqeenIdTypeLookupsUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("YaqeenIdTypeLookupsUrl");
        }
        #endregion

        #region CP  Add / Edit / Save Generated  Dynamic Forms 
        public async Task<OperationOutput> GetFormLookups()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var inputTypes = await _unitOfWork.InputsType.GetAll().AsNoTracking().ToListAsync();
            var inputTypesDto = inputTypes.Adapt<List<Dtos.DynamicForm.TemplateDtos.InputsType>>();

            var references = await _unitOfWork.References.GetAll(c => c.IsPortal == true && c.ParentId == null).AsNoTracking().ToListAsync();
            var referencesDto = references.Adapt<List<Lookup>>(Lookup.ReferencLookupConfig());


            var formTypes = await _unitOfWork.FormType.GetAll().AsNoTracking().ToListAsync();
            var formTypesDto = formTypes.Adapt<List<Lookup>>(Lookup.FormTypeLookupConfig());

            var dataSources = await _unitOfWork.FormsDataSource.GetAll()
                .Where(c => c.ParentId == null && c.IsDeleted == false)
                .Include(c => c.InverseParent).AsNoTracking().ToListAsync();

            var dataSourcesDto = dataSources.Adapt<List<FormsDataSourceLookup>>(FormsDataSourceLookup.SelectConfig());


            var themes = await _unitOfWork.Themes.GetAll(c => c.IsActive == true).AsNoTracking().ToListAsync();
            var themesDto = themes.Adapt<List<Dtos.DynamicForm.TemplateDtos.Theme>>();

            var apiDatasources = await _unitOfWork.APIDataSources.GetAll().Include(s => s.APIParamObjectDetail).AsNoTracking().ToListAsync();
            var apiDatasourcesDto = apiDatasources.Adapt<List<Dtos.DynamicForm.TemplateDtos.APIDataSource>>(Dtos.DynamicForm.TemplateDtos.APIDataSource.SelectConfig());


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.InputTypes, inputTypesDto),
                   new OutputDictionary(OperationOutputKeys.References, referencesDto),
                   new OutputDictionary(OperationOutputKeys.FormTypes, formTypesDto),
                   new OutputDictionary(OperationOutputKeys.DataSources, dataSourcesDto),
                   new OutputDictionary(OperationOutputKeys.Themes, themesDto),
                   new OutputDictionary(OperationOutputKeys.APIDatasourcesDto, apiDatasourcesDto));
        }

        public async Task<OperationOutput> GetAllMajorsWithReferencesTree()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var maxId = await _unitOfWork.References.GetAll().Select(x => x.Id).MaxAsync();
            var majors = await _unitOfWork.ReferencesMajor.GetAll(x => x.IsDeleted != true)
                .Select(x => new Dtos.ReferencesTreeDto()
                {
                    _Id = x.Id + maxId,
                    Label = x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    _referencesMajorId = x.Id

                }).ToListAsync();

            var references = _unitOfWork.References.GetAll()
                .Where(x => x.IsDeleted != true)

                .Select(x => new Dtos.ReferencesTreeDto()
                {
                    _Id = x.Id,
                    Label = x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    _parentId = x.ParentId == null ? x.ReferencesMajorId + maxId : x.ParentId,
                    _referencesMajorId = x.ReferencesMajorId,


                }).ToList();

            references.AddRange(majors);

            var root = references.GenerateTree(c => c._Id, c => c._parentId);

            List<ReferencesTreeDto> referencesTree = new List<ReferencesTreeDto>();

            foreach (var reference in root)
            {
                referencesTree.Add(bindNode(reference));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.DataList, referencesTree));

        }

        #region HELPER METHOD GetAllMajorsWithReferencesTree
        private ReferencesTreeDto bindNode(TreeItem<ReferencesTreeDto> root)
        {
            ReferencesTreeDto references = new Dtos.ReferencesTreeDto
            {
                Label = root.Item.Label,
                NameAr = root.Item.NameAr,
                NameEn = root.Item.NameEn,
                _parentId = root.Item._parentId,
                _referencesMajorId = root.Item._referencesMajorId,
                _Id = root.Item._Id,


                Children = new List<ReferencesTreeDto>(),
                ReferenceJobRole = root.Item.ReferenceJobRole,

            };

            if (root.Children != null)
            {
                foreach (var child in root.Children)
                {
                    // child.Item.ReferenceJobRole = root.Item.ReferenceJobRole;
                    references.Children.Add(bindNode(child));
                }
            }


            return references;
        }

        #endregion

        public async Task<OperationOutput> GetAllEntities()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var entities = await _unitOfWork.Entity.GetAll()
                .Select(x => new Dtos.ReferencesTreeDto()
                {
                    _Id = x.Id,
                    Label = x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    _parentId = x.ParentId,
                }).AsNoTracking().ToListAsync();

            var root = entities.GenerateTree(c => c._Id, c => c._parentId);

            List<ReferencesTreeDto> entitiesTree = new List<ReferencesTreeDto>();

            foreach (var entity in root)
            {
                entitiesTree.Add(bindNode(entity));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.DataList, entitiesTree));
        }

        public async Task<OperationOutput> GetDepartmentsWithWorkFlow()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var workFlow = await _unitOfWork.Engine.GetAll(c => c.IsDeleted == false && c.IsActive == true)
                .Include(c => c.EnginesActionsJobRoles.Where(c => c.IsDeleted != true))
                .ThenInclude(c => c.JobRole)
                .Include(c => c.EnginesActionsJobRoles)
                .ThenInclude(c => c.ActionNavigation)
                .AsNoTracking().ToListAsync();

            var workFlowDto = workFlow.Select(c => new WorkFlowLookup
            {
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                ReferenceId = c.ReferenceId,
                Actions = c.EnginesActionsJobRoles.Select(an => new ActionsLookup
                {
                    Id = an.ActionId,
                    NameAr = $" {an.ActionNavigation.NameAr} - {(an.JobRole != null ? an.JobRole.NameAr : string.Empty)}",
                    NameEn = $" {an.ActionNavigation.NameEn} - {(an.JobRole != null ? an.JobRole.NameEn : string.Empty)}",
                    EngineActionJobRoleId = an.Id
                }).ToList()
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.WorkFlow, workFlowDto));
        }

        public async Task<OperationOutput> GetFormsList(Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var thenByList = new Expression<Func<Models.Form, object>>[] { x => x.UpdatedDate ?? x.UpdatedDate };

            var forms = await _unitOfWork.Forms.FindAllByPaginationWithThenBy(RequestedData.Filteration(),
                           RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate, OrderBy.Descending,
                           thenByList, ThenBy.Descending, c => c.FormType, c => c.Reference, c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var formsDto = forms.Data.Adapt<List<Dtos.DynamicForm.TemplateDtos.Form>>(Dtos.DynamicForm.TemplateDtos.Form.SelectConfig(imagesGetPath, jsonOptions));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.DynamicForm, formsDto),
             new OutputDictionary(OperationOutputKeys.Pagination, forms.Pagination));

        }

        public async Task<OperationOutput> SaveForm(Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.Form form = null;
            Models.FormInput formInput = null;
            Models.FormInputDataSource formInputDs = null;
            Models.FormsDataSource formsDataSource = null;
            Models.FormsDataSource subDataSource = null;
            List<int> formInputsRemoved = new List<int>();

            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (!RequestedData.Id.HasValue)
                    {
                        form = new Models.Form();
                        form.CreatedBy = RequestOwner.Id.Value;
                        form.CreatedDate = TransactionDate;
                        form.IsActive = true;
                    }
                    else
                    {
                        form = _unitOfWork.Forms.GetAll().Include(c => c.FormInputs).Where(x => x.Id == RequestedData.Id).FirstOrDefault();
                        if (form == null)
                            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                    }

                    SetFormAttributes(RequestedData, form);

                    if (RequestedData.Id.HasValue)
                    {
                        formInputsRemoved = await _unitOfWork.FormInputs.GetAll().Where(x => x.FormId == RequestedData.Id && x.IsDeleted == false).Select(x => x.Id).Where(x => !RequestedData.FormInputs.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToListAsync();

                        if (formInputsRemoved.Any())
                            DeleteFormInputs(formInputsRemoved);
                    }

                    if (RequestedData.FormInputs.Any())
                    {
                        foreach (var input in RequestedData.FormInputs)
                        {

                            if (!input.Id.HasValue)
                            {
                                formInput = new Models.FormInput();
                                formInput.IsDeleted = false;
                            }
                            else
                            {
                                formInput = _unitOfWork.FormInputs.GetAll().FirstOrDefault(x => x.Id == input.Id.Value);

                                var removeAllformInputDatasource = _unitOfWork.FormInputDataSource.FindAll(c => c.FormInputId == input.Id.Value);

                                if (removeAllformInputDatasource.Any())
                                    _unitOfWork.FormInputDataSource.DeleteRange(removeAllformInputDatasource);

                            }

                            SetFormInputsAttributes(form, formInput, input);

                            if (input.FormsDataSource != null && input.FormsDataSource.SubDataSource.Any())
                            {
                                if (!input.FormsDataSource.Id.HasValue)
                                {
                                    formsDataSource = new Models.FormsDataSource();
                                    formsDataSource.IsDeleted = false;
                                }
                                else
                                {
                                    formsDataSource = _unitOfWork.FormsDataSource.GetAll().Include(c => c.InverseParent)
                                                        .FirstOrDefault(x => x.Id == input.FormsDataSource.Id.Value);
                                }

                                if (!string.IsNullOrEmpty(input.FormsDataSource.TextAr))
                                    formsDataSource.TextAr = input.FormsDataSource.TextAr;

                                if (!string.IsNullOrEmpty(input.FormsDataSource.TextEn))
                                    formsDataSource.TextEn = input.FormsDataSource.TextEn;

                                foreach (var sub in input.FormsDataSource.SubDataSource)
                                {
                                    if (!sub.Id.HasValue)
                                    {
                                        subDataSource = new Models.FormsDataSource();
                                        subDataSource.IsDeleted = false;
                                    }
                                    else
                                    {
                                        subDataSource = formsDataSource.InverseParent.FirstOrDefault(x => x.Id == sub.Id.Value);
                                    }
                                    subDataSource.TextAr = sub.TextAr;
                                    subDataSource.TextEn = sub.TextEn;
                                    subDataSource.ParentId = formsDataSource.Id;
                                    if (!sub.Id.HasValue) formsDataSource.InverseParent.Add(subDataSource);
                                    else _unitOfWork.FormsDataSource.Update(subDataSource);
                                }

                                if (!input.FormsDataSource.Id.HasValue) _unitOfWork.FormsDataSource.Add(formsDataSource);
                                else _unitOfWork.FormsDataSource.Update(formsDataSource);
                                _unitOfWork.Complete(); // mandatory 

                                // in case add new datasource to parent exisit
                                UpdateInputFormDataSource(formsDataSource, input);

                                formInputDs = InsertFormInputDataSource(formInput, formInputDs, formsDataSource, input);
                            }

                            if (!input.Id.HasValue) form.FormInputs.Add(formInput);
                            else _unitOfWork.FormInputs.Update(formInput);

                        }
                    }

                    if (!RequestedData.Id.HasValue) _unitOfWork.Forms.Add(form);
                    else _unitOfWork.Forms.Update(form);

                    _unitOfWork.Complete();

                    dbContextTransaction.Commit();

                    if (RequestedData.FormTypeId == (int)DynamicEnum.FormTypes.Service || RequestedData.FormTypeId == (int)DynamicEnum.FormTypes.Integration)// workflow
                    {
                        AddWorkflowEnginToFormAndFormInputActions(RequestedData, form);
                    }

                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                         new OutputDictionary(OperationOutputKeys.Id, Accessor.Get<int?>(form.Id)));
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    _logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, RequestedData);
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
                }
            }
        }


        #region HELPER METHODS SAVE FORM

        private void SetFormInputsAttributes(Models.Form form, Models.FormInput formInput, Dtos.DynamicForm.TemplateDtos.FormInput input)
        {
            formInput.NameAr = input.NameAr;
            formInput.NameEn = input.NameEn;
            formInput.Type = input.Type;
            formInput.FormId = form.Id;
            formInput.Mandatory = input.Mandatory;
            formInput.VerticalDataSourceDirection = input.VerticalDataSourceDirection;
            formInput.Order = input.Order;
            formInput.ViewInFullRow = input.ViewInFullRow;
            formInput.HasDataSourceFromAPI = input.HasDataSourceFromAPI;
            formInput.DataSourceAPIRouting = input.DataSourceAPIRouting;
            formInput.GroupId = input.GroupId;

            if (input.HasDataSourceFromAPI == true && input.LookupParameter != null && !input.LookupParameter.ArePropertiesIsNull())
                formInput.APIParameters = System.Text.Json.JsonSerializer.Serialize(input.LookupParameter, jsonOptions);

            else formInput.APIParameters = null;

            formInput.ShowInMainListCP = input.ShowInMainListCP;
            formInput.ShowInMainPortalPage = input.ShowInMainPortalPage;
            formInput.ShowInAdvancedSearch = input.ShowInAdvancedSearch;
            formInput.OnChangeAPIMethodName = input.OnChangeAPIMethodName;
            formInput.OnChangeParamName = input.OnChangeParamName;
            formInput.OnChangeRefelectionInputKey = input.OnChangeRefelectionInputKey;

            formInput.Property = input.Property;
            formInput.IsUnique = input.IsUnique;
            formInput.DescriptionAr = input.DescriptionAr;
            formInput.DescriptionEn = input.DescriptionEn;
            formInput.MaxValue = input.MaxValue;
            formInput.MinValue = input.MinValue;
            formInput.InputUseIntegration = input.InputUseIntegration;
            formInput.Length = input.Length;
            formInput.ShowInExport = input.ShowInExport;
        }

        private void SetFormAttributes(Dtos.DynamicForm.TemplateDtos.Form RequestedData, Models.Form form)
        {
            form.NameAr = RequestedData.NameAr;
            form.NameEn = RequestedData.NameEn;
            form.DescriptionAr = RequestedData.DescriptionAr;
            form.DescriptionEn = RequestedData.DescriptionEn;
            form.UpdatedBy = RequestOwner.Id.Value;
            form.UpdatedDate = TransactionDate;
            form.ReferenceId = RequestedData.ReferenceId;
            form.FormTypeId = RequestedData.FormTypeId;
            form.Url = RequestedData.Url;
            form.UseIntegration = RequestedData.UseIntegration;
            form.NonEditableForm = RequestedData.NonEditableForm;
            form.CheckApplicationNo = RequestedData.CheckApplicationNo;
            form.CheckPersonalData = RequestedData.CheckPersonalData;
            form.ThemeId = RequestedData.ThemeId;
            form.IsViewStatistic = RequestedData.IsViewStatistic;
            if (RequestedData.IconBase64 != null)
                form.Icon = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.IconBase64)
                    ? Images.SaveSingleImageOnServer(RequestedData.IconBase64, null, ImagesSavePath, false) : null;
        }

        private void AddWorkflowEnginToFormAndFormInputActions(Dtos.DynamicForm.TemplateDtos.Form RequestedData, Models.Form form)
        {
            int engineFormId = AddWorkFlowEnginForm(form.Id, RequestedData);
            foreach (var input in form.FormInputs)
            {
                var enInput = RequestedData.FormInputs.Find(c => c.NameAr == input.NameAr && c.NameEn == input.NameEn);
                if (enInput != null) { enInput.Id = input.Id; enInput.FormId = form.Id; }
            }
            if (engineFormId > 0)
                AddFormInputActions(form.Id, engineFormId, RequestedData.FormInputs);
        }

        int AddWorkFlowEnginForm(int formId, Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            var engineForm = _unitOfWork.EngineForms.Find(c => c.FormId == formId && c.IsDeleted == false);
            if (engineForm is not null)
                engineForm.WorkFlowEngineId = RequestedData.WorkFlowEngineId;
            else
            {
                engineForm = new Models.EngineForms();
                engineForm.FormId = formId;
                engineForm.WorkFlowEngineId = RequestedData.WorkFlowEngineId;
                engineForm.IsDeleted = false;
                _unitOfWork.EngineForms.Add(engineForm);
            }
            _unitOfWork.Complete();
            return engineForm.Id;
        }

        void AddFormInputActions(int formId, int engineFormId, List<Dtos.DynamicForm.TemplateDtos.FormInput> formInputs)
        {
            var inputActionsList = _unitOfWork.FormInputsActions.FindAll(c => c.EngineFormId == engineFormId && c.IsDeleted == false).ToList();

            foreach (var input in formInputs)
            {
                var inputAction = inputActionsList.Find(c => c.FormInputId == input.Id);

                if (inputAction == null)
                {
                    inputAction = new Models.FormInputsActions();
                    _unitOfWork.FormInputsActions.Add(inputAction);
                }
                inputAction.FormInputId = input.Id;
                inputAction.FormId = formId;
                inputAction.ActionId = input.ActionId;
                inputAction.EngineFormId = engineFormId;
                inputAction.CreatedBy = RequestOwner.Id.Value;
                inputAction.CreatedDate = TransactionDate;
                inputAction.UpdatedBy = RequestOwner.Id.Value;
                inputAction.UpdatedDate = TransactionDate;
                inputAction.ActivatedDate = TransactionDate;
                inputAction.IsDeleted = false;
                inputAction.IsActive = true;
                inputAction.EngineActionJobRoleId = input.EngineActionJobRoleId;
            }
            _unitOfWork.Complete();

        }


        private static Models.FormInputDataSource InsertFormInputDataSource(Models.FormInput formInput, Models.FormInputDataSource formInputDs, Models.FormsDataSource formsDataSource, Dtos.DynamicForm.TemplateDtos.FormInput input)
        {
            var inputDatasource = (from ds in formsDataSource.InverseParent
                                   join sub in input.FormsDataSource.SubDataSource on ds.Id equals sub.Id.Value
                                   select ds).ToList();

            if (inputDatasource.Any())
            {
                foreach (var fds in inputDatasource)
                {
                    formInputDs = new Models.FormInputDataSource();
                    formInputDs.DataSourceId = fds.Id;
                    formInputDs.FormInputId = formInput.Id;
                    formInput.FormInputDataSource.Add(formInputDs);
                }

            }

            return formInputDs;
        }

        private static void UpdateInputFormDataSource(Models.FormsDataSource formsDataSource, Dtos.DynamicForm.TemplateDtos.FormInput input)
        {
            if (input.FormsDataSource.SubDataSource.Any(c => c.Id == null))
            {
                foreach (var item in input.FormsDataSource.SubDataSource)
                {
                    var dsNew = formsDataSource.InverseParent.Find(c => c.TextAr == item.TextAr && c.TextEn == item.TextEn);
                    if (dsNew != null)
                    {
                        item.Id = dsNew.Id;
                    }
                }
            }
        }

        private void DeleteFormInputs(List<int> formInputsRemoved)
        {
            foreach (var input in formInputsRemoved)
            {
                var inputToDelete = _unitOfWork.FormInputs.GetAll().FirstOrDefault(x => x.Id == input);
                if (inputToDelete is not null)
                {
                    inputToDelete.IsDeleted = true;
                    _unitOfWork.FormInputs.Update(inputToDelete);
                }
            }
        }

        #endregion

        public async Task<OperationOutput> GetFormDetails(Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var formModel = await _unitOfWork.Forms.GetAll(x => x.Id == RequestedData.Id)
                   .Include(c => c.Theme)
                   .Include(s => s.FormInputs.Where(c => c.IsDeleted != true))
                   .ThenInclude(v => v.FormInputDataSource)
                   .ThenInclude(c => c.DataSource)
                   .Include(s => s.FormInputs)
                   .ThenInclude(s => s.InputsType).AsNoTracking().FirstOrDefaultAsync();

            var formDto = formModel.Adapt<Dtos.DynamicForm.TemplateDtos.Form>(Dtos.DynamicForm.TemplateDtos.Form.SelectConfig(ImagesGetPath, jsonOptions));

            if (formDto is not null)
            {
                formDto.FormInputs = formDto.FormInputs.Any() ? formDto.FormInputs.Where(n => n.IsDeleted == false).OrderBy(c => c.Order ?? c.Id).ToList() : new List<Dtos.DynamicForm.TemplateDtos.FormInput>();
                if (formDto.FormInputs.Any())
                {
                    foreach (var n in formModel.FormInputs)
                    {

                        formDto.FormInputs.Find(c => c.Id == n.Id).
                        FormsDataSource = n.FormInputDataSource.Any() ? new Dtos.DynamicForm.TemplateDtos.FormsDataSource
                        {
                            Id = n.FormInputDataSource.Any() ? n.FormInputDataSource.Select(ds => ds.DataSource != null ? ds.DataSource.ParentId : null).FirstOrDefault() : null,
                            SubDataSource = n.FormInputDataSource.Any() ? n.FormInputDataSource.Select(ds => new Dtos.DynamicForm.TemplateDtos.FormsDataSource
                            {
                                Id = ds.DataSourceId,
                                FormInputDatasourceId = ds.Id,
                                ParentId = ds.DataSource != null ? ds.DataSource.ParentId.Value : null,
                                TextAr = ds.DataSource != null ? ds.DataSource.TextAr : string.Empty,
                                TextEn = ds.DataSource != null ? ds.DataSource.TextEn : string.Empty

                            }).OrderBy(c => c.Id).ToList() : new List<Dtos.DynamicForm.TemplateDtos.FormsDataSource>(),
                        } : new Dtos.DynamicForm.TemplateDtos.FormsDataSource();
                    }
                }


            }

            if (formDto.FormTypeId == (int)DynamicEnum.FormTypes.Service || formDto.FormTypeId == (int)DynamicEnum.FormTypes.Integration) // Workflow
                SetFormDetailWithWorkflow(formDto);


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.DynamicForm, formDto));

        }



        #region HELPER METHOD WORKFLOW
        private void SetFormDetailWithWorkflow(Dtos.DynamicForm.TemplateDtos.Form form)
        {
            var enginForm = _unitOfWork.EngineForms.Find(c => c.FormId == form.Id && c.IsDeleted == false);
            if (enginForm != null)
                form.WorkFlowEngineId = enginForm.WorkFlowEngineId;

            var inputsActions = _unitOfWork.FormInputsActions.FindAll(c => c.FormId == form.Id);
            if (inputsActions.Any())
            {
                foreach (var input in form.FormInputs)
                {
                    var item = inputsActions.FirstOrDefault(c => c.FormInputId == input.Id);
                    if (item != null)
                    {
                        input.ActionId = item.ActionId;
                        input.EngineActionJobRoleId = item.EngineActionJobRoleId;
                    }

                }
            }
        }

        #endregion

        public async Task<OperationOutput> FormActivation(Dtos.DynamicForm.TemplateDtos.FormActions RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if (!RequestedData.Forms.Any())
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var models = await _unitOfWork.Forms
                .GetAll(c => RequestedData.Forms.Select(c => c.Id).Contains(c.Id)).ToListAsync();

            if (!models.Any())
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);
            else
            {

                foreach (var model in models)
                {
                    var requestModel = RequestedData.Forms.Where(c => c.Id == model.Id).FirstOrDefault();
                    if (requestModel is not null)
                    {
                        model.IsActive = requestModel.IsActive.HasValue ? requestModel.IsActive.Value : model.IsActive;
                        model.UpdatedBy = RequestOwner.Id;
                        model.UpdatedDate = DateTime.Now;
                        _unitOfWork.Forms.Update(model);
                    }
                }
                _unitOfWork.Complete();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
        public async Task<OperationOutput> FormDeletion(Dtos.DynamicForm.TemplateDtos.FormActions RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if (!RequestedData.IsDeleted.HasValue || !RequestedData.Forms.Any())
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            if (RequestedData.IsDeleted.HasValue)
            {
                var models = await _unitOfWork.Forms
                    .GetAll(c => RequestedData.Forms.Select(c => c.Id).Contains(c.Id)).ToListAsync();

                if (!models.Any())
                    return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);
                else
                {
                    foreach (var model in models)
                    {
                        model.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : model.IsDeleted;
                        model.UpdatedBy = RequestOwner.Id;
                        model.UpdatedDate = DateTime.Now;
                        _unitOfWork.Forms.Update(model);
                    }
                    _unitOfWork.Complete();
                }
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        #endregion

        #region CP Preview Dynamic form (Draw form Add (user access to entity ) / view (for super admin) ) Without value 

        // id: form id , view refernce : null , add: refernceid
        public async Task<OperationOutput> GetFormForView(Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            Models.FormsEntity entityForm = null;
            UserPermission userPermission = new UserPermission();
            var accordionFields = new List<FormField>();
            dynamic firstEnginesActionsJobRole = null;


            if (!RequestedData.EntityId.HasValue && !string.IsNullOrEmpty(RequestedData.EntityUrl))
            {
                var entity = _unitOfWork.Entity.GetAll().AsNoTracking().FirstOrDefault(c => c.FrontIdentity.ToLower() == RequestedData.EntityUrl.ToLower());

                if (entity is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

                RequestedData.EntityId = entity.Id;
                userPermission = GetUserEntityPermission(entity.Id, RequestedData.ReferenceId);
            }

            if (RequestedData.EntityId.HasValue)
                entityForm = _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).AsNoTracking().FirstOrDefault(c => c.EntityId == RequestedData.EntityId);

            // from dynamic view
            RequestedData.Id = RequestedData.EntityId.HasValue && entityForm is not null ? entityForm.FormId : RequestedData.Id.GetValueOrDefault();

            var formModel = await _unitOfWork.Forms.GetAll(x => x.Id == RequestedData.Id && (RequestedData.ReferenceId != null ? x.ReferenceId == RequestedData.ReferenceId : true))
                   .Include(c => c.Theme)
                   .Include(s => s.FormInputs)
                   .ThenInclude(v => v.FormInputDataSource)
                   .ThenInclude(c => c.DataSource)
                   .Include(s => s.FormInputs)
                   .ThenInclude(s => s.InputsType).AsNoTracking().FirstOrDefaultAsync();

            var formDto = formModel.Adapt<Dtos.DynamicForm.TemplateDtos.FormView>(Dtos.DynamicForm.TemplateDtos.FormView.SelectConfig(ImagesGetPath));

            // if input fields has datasource from api method 
            GetInputsOptionsFromCallAPIMethods(formDto);

            if (formDto is not null && formDto.FormFields.Any(c => c.Type == (int)InputTypes.Accordion))
            {
                var formValue = _unitOfWork.FormValue.GetAll().FirstOrDefault(c => c.FormId == formDto.Id);
                accordionFields = GetAccordionFields(formDto, accordionFields, formValue);
            }
            if (formDto is not null && IsPortal == true)
            {
                firstEnginesActionsJobRole = await _unitOfWork.EngineForms.GetAll().AsNoTracking()
                   .Include(c => c.WorkFlowEngine)
                   .ThenInclude(n => n.EnginesActionsJobRoles)
                   .Where(c => c.FormId == formDto.Id)
                   .Select(c => c.WorkFlowEngine)
                   .SelectMany(c => c.EnginesActionsJobRoles)
                   .Select(c => new { Id = Accessor.Get<int?>(c.Id), ActionId = Accessor.Get<int?>(c.ActionId), c.IsDeleted })
                   .FirstOrDefaultAsync(c => c.IsDeleted != true);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.DynamicForm, formDto),
                  new OutputDictionary(OperationOutputKeys.EntityObj, entityForm != null && entityForm.Entity != null ? new Dtos.DynamicForm.FormModuleDtos.Entity { Id = entityForm.Entity.Id, FrontIdentity = entityForm.Entity.FrontIdentity, NameAr = entityForm.Entity.NameAr, NameEn = entityForm.Entity.NameEn, CmsIdentity = entityForm.Entity.CmsIdentity } : null),
                  new OutputDictionary(OperationOutputKeys.UserPermission, userPermission),
                  new OutputDictionary(OperationOutputKeys.AccordionFields, accordionFields),
                   new OutputDictionary(OperationOutputKeys.EngineActionsJobRole, firstEnginesActionsJobRole));
        }

        #region  HELPER METHODS >> CP Preview Dynamic
        private void GetInputsOptionsFromCallAPIMethods(Dtos.DynamicForm.TemplateDtos.FormView form)
        {
            if (form != null)
            {
                var inputsDSFromAPI = form.FormFields.Where(c => c.HasDataSourceFromAPI == true).ToList();
                if (inputsDSFromAPI.Any())
                {
                    List<FieldOptions> result = null;
                    for (int i = 0; i < inputsDSFromAPI.Count; i++)
                    {
                        result = new List<FieldOptions>();
                        MethodInfo apiMethod = typeof(DynamicFormService).GetMethod(inputsDSFromAPI[i].DataSourceAPIRouting.Trim());
                        if (apiMethod != null)
                        {
                            if (!string.IsNullOrEmpty(inputsDSFromAPI[i].APIParameters))
                            {
                                LookupParameterModel parameterModel = System.Text.Json.JsonSerializer.Deserialize<LookupParameterModel>(inputsDSFromAPI[i].APIParameters, jsonOptions);
                                result = (List<FieldOptions>)apiMethod.Invoke(this, new object[] { parameterModel });
                            }
                            else result = (List<FieldOptions>)apiMethod.Invoke(this, null);
                            form.FormFields.Find(c => c.Key == inputsDSFromAPI[i].Key).Options = result;
                        }
                    }
                }

            }
        }



        #region Example of creating a function to return a dynamic lookup from API method

        /// <summary>
        /// Must be a public function
        /// The return must be of the List<FieldOptions> type
        /// must be the type of parameter >> " LookupParameterModel "
        /// if use ParamModel in LookupParameterModel must be Deserialize 
        /// by call DeserializeInnerParamModel meyhod from LookupParameterModel 
        /// </summary>
        /// <returns> List<FieldOptions> </returns>

        //  EXAMPLE :How to read and deserialize LookupParameterModel.ParamModel ( inner Model)  

        public List<FieldOptions> GetEntityDataForAllReportType(LookupParameterModel model)
        {
            MokeData paramModel = new MokeData();
            paramModel = model.DeserializeInnerParamModel(paramModel); //MUST 
            return EventService.GetAllEntityForReportType(_unitOfWork, paramModel);
        }

        public List<FieldOptions> GetReferenceDataByParentId(LookupParameterModel model)
        {
            return EventService.GetReferencesByParentId(_unitOfWork, model);
        }

        public List<FieldOptions> GetEntityDataByParentId(LookupParameterModel model)
        {
            return EventService.GetEntityDataByParentId(_unitOfWork, model);
        }
        #endregion

        private List<FormField> GetAccordionFields(Dtos.DynamicForm.TemplateDtos.FormView form, List<FormField> accordionFields, Models.FormValue formvalue = null)
        {
            List<FormField> accordionInput = GetFormAccordionInputs(form);

            if (accordionInput.Count > 0)
            {
                if (formvalue is not null)
                {
                    var accordionValue = _unitOfWork.FormValueDetails.FindAll(c => c.FormValueId == formvalue.Id &&
                    accordionInput.Select(c => c._key).Contains(c.InputKey)).OrderBy(c => c.Order).ToList();
                    if (accordionValue.Any())
                    {
                        accordionFields = SetAccordionValue(accordionValue, accordionInput);
                    }
                }
            }

            return accordionFields;
        }

        private static List<FormField> GetFormAccordionInputs(Dtos.DynamicForm.TemplateDtos.FormView form)
        {
            return (from field in form.FormFields
                    where field.Type == (int)InputTypes.Accordion
                    group field by field.GroupId into grp
                    select new FormField
                    {
                        _key = grp.First()._key,
                        GroupId = grp.Key,
                        DescriptionAr = grp.First().DescriptionAr,
                        DescriptionEn = grp.First().DescriptionEn
                    }).ToList();
        }

        private static List<FormField> SetAccordionValue(List<Models.FormValueDetails> formValueDetails, List<FormField> accordionGroups)
        {
            var result = new List<FormField>();

            foreach (var entry in formValueDetails)
                result.Add(new FormField { _key = entry.InputKey, Value = entry.InputValue, Description = entry.Description, ControlType = InputTypes.Accordion.ToString(), DescriptionAr = accordionGroups.First(c => c._key == entry.InputKey).DescriptionAr, DescriptionEn = accordionGroups.First(c => c._key == entry.InputKey).DescriptionEn, GroupId = accordionGroups.First(c => c._key == entry.InputKey).GroupId });
            return result.OrderBy(c => c._key).ToList();

        }

        #endregion


        #endregion

        #region CP Save Form Value
        public async Task<OperationOutput> SaveFormValue(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData)
        {
            if (RequestOwner is null && IsPortal != true)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.FormValue formValue = null;
            List<FormField> fieldsUrl = new List<FormField>();

            if (!RequestedData._id.HasValue)
            {
                formValue = new Models.FormValue();
                formValue.CreatedBy = RequestOwner.Id.HasValue ? RequestOwner.Id.Value : null;
                formValue.CreatedDate = TransactionDate;
                formValue.IsDeleted = false;
                formValue.IsActive = IsPortal != true ? false : true;
            }
            else
            {
                formValue = await _unitOfWork.FormValue.GetAll().Include(c => c.FormValueDetails).FirstOrDefaultAsync(c => c.Id == RequestedData._id.Value);

                if (formValue is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                if (formValue.FormValueDetails.Any())
                    _unitOfWork.FormValueDetails.DeleteRange(formValue.FormValueDetails);

                var formValueDataSourceRemoved = _unitOfWork.FormValueDataSource.GetAll(x => x.FormValueId == formValue.Id).ToList();
                if (formValueDataSourceRemoved.Any())
                    _unitOfWork.FormValueDataSource.DeleteRange(formValueDataSourceRemoved);

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
            formValue.UpdatedBy = RequestOwner.Id.HasValue ? RequestOwner.Id.Value : null; ;

            if (!RequestedData._id.HasValue) _unitOfWork.FormValue.Add(formValue);
            else _unitOfWork.FormValue.Update(formValue);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.Id, Accessor.Get<int?>(formValue.Id)),
                new OutputDictionary(OperationOutputKeys.FieldsUrl, fieldsUrl));

        }


        #region HELPER METHODS >> SaveFormValue
        private void AddFormValueSingleDataSource(Models.FormValue formValue, FormField field)
        {
            var formValueDs = new Models.FormValueDataSource
            {
                FormValueId = formValue.Id,
                InputKey = field._key.Value,
                FormDataSourceId = Accessor.Set(field.Value.ToString())
            };
            formValue.FormValueDataSource.Add(formValueDs);
        }
        private void AddFormValueMultipleDataSource(Models.FormValue formValue, FormField field)
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

        private static void AddFormValueDetails(Models.FormValue formValue, FormField field)
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

        private bool IsFieldHasSingleDatasource(FormField field)
        {
            return field.ControlType == FormInputTypes.radio || field.ControlType == FormInputTypes.dropdownSingle;
        }
        private bool IsFieldHasMultipleDatasource(FormField field)
        {
            return
                 field.ControlType == FormInputTypes.checkbox
                || field.ControlType == FormInputTypes.dropdownMultiple
                || field.ControlType == FormInputTypes.plistbox;

        }

        private static OperationOutput ReturnMessageForDublicateFormInput(Models.FormInput formInput)
        {
            var result = GetOperationOutput(header: Enums.ServiceMessages.ValueIsExist);
            result.Header.Message = $"{formInput.NameAr}   موجود مسبقا  ";
            result.Header.MessageEn = $"{formInput.NameEn}  already exist ";
            return result;
        }

        private Models.FormInput CheckValueIsExist(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData)
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

        private bool? ConvertImageBase64ToString(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData, List<FormField> fieldsUrl)
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
                        fieldsUrl.Add(new FormField { _key = field._key, Url = imagesGetPath + "/" + imageUrl });
                        return true;
                    }
                }
            }
            return null;
        }

        private bool? ConvertFileBase64ToString(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData, List<FormField> fieldsUrl)
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
                        fieldsUrl.Add(new FormField { _key = field._key, Url = filesGetPath + "/" + FileName });
                        return true;
                    }
                }
            }
            return null;
        }

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
                        integrationData.birthOfDateHijri = Dates.ConvertFromGerogianToHijriDateString(DateTime.Parse(integrationData.birthOfDateGeorogian), DateFormat.DayMonthYear);
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


        #endregion

        #region CP Dynamic Form <<Entity>> List Data
        //formId ,_referenceId, entityId
        public async Task<OperationOutput> GetFormDataList(DynamicFormListDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            DynamicFormListDto ItemResult = new DynamicFormListDto();
            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            UserPermission userPermission = new UserPermission();
            Models.FormsEntity entityForm = null;

            if (!RequestedData.EntityId.HasValue && !string.IsNullOrEmpty(RequestedData.EntityUrl))
            {
                var entity = await _unitOfWork.Entity.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.FrontIdentity.ToLower() == RequestedData.EntityUrl.ToLower());

                if (entity is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.EntityId = entity.Id;
                userPermission = GetUserEntityPermission(entity.Id, RequestedData.ReferenceId);
            }

            if (RequestedData.EntityId.HasValue)
                entityForm = await _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).AsNoTracking().FirstOrDefaultAsync(c => c.EntityId == RequestedData.EntityId);

            if (entityForm is not null)
            {
                RequestedData.FormId = entityForm.FormId;
                ItemResult.FormId = entityForm.FormId;

                bool accessToForm = await _unitOfWork.Forms.GetAll().CountAsync(c => c.Id == entityForm.FormId && c.ReferenceId == RequestedData.ReferenceId) > 0 ? true : false;

                var Headers = GetFormHeader(RequestedData);
                if (accessToForm)
                {
                    formResult = GetFormListValue(RequestedData, Headers);
                }
                ItemResult.FormListHeader = Headers; ItemResult.ListFormValue = formResult.ListFormValue;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.EntityFormValue, ItemResult),
              new OutputDictionary(OperationOutputKeys.Pagination, formResult.Pagination),
              new OutputDictionary(OperationOutputKeys.EntityObj, entityForm != null && entityForm.Entity != null ? new Dtos.DynamicForm.FormModuleDtos.Entity { Id = entityForm.Entity.Id, FrontIdentity = entityForm.Entity.FrontIdentity, NameAr = entityForm.Entity.NameAr, NameEn = entityForm.Entity.NameEn, CmsIdentity = entityForm.Entity.CmsIdentity } : null),
              new OutputDictionary(OperationOutputKeys.UserPermission, userPermission));
        }

        #region HELPER MTHODS >> GetFormDataList
        private List<FormListHeader> GetFormHeader(DynamicFormListDto RequestedData)
        {
            List<FormListHeader> Headers = new List<FormListHeader>();
            var formDynamicHeader = _unitOfWork.FormInputs.FindAll(c => c.FormId == RequestedData.FormId
            && c.ShowInMainListCP == true && c.IsDeleted == false).Select(c => new FormListHeader
            {
                _key = c.Id,
                TitleAr = c.NameAr,
                TitleEn = c.NameEn,
                Property = c.Property,
                Order = c.Order
            }).OrderBy(c => c.Order ?? c._key).ToList();

            var headers = formDynamicHeader.Concat(FormListHeader.GetStaticHeaderAttributes()).ToList();
            Headers.AddRange(headers);
            return Headers;
        }

        private Dtos.DynamicForm.FormModuleDtos.FormListDataResult GetFormListValue(DynamicFormListDto RequestedData, List<FormListHeader> Headers)
        {
            // Work with Traditinal Pagination 

            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();
            int NumberOfRecord;
            int CurrentPageIndex = 1;

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;
            NumberOfRecord = _unitOfWork.FormValue.Count(RequestedData.Filteration());

            if (RequestedData.Pagination != null)
            {
                if (RequestedData.Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = RequestedData.Pagination.CurrentPageIndex.Value;
                if (RequestedData.Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = RequestedData.Pagination.RecordPerPage.Value;

            }
            else DefaultPaginationCount = NumberOfRecord;

            var listFormValueModel = _unitOfWork.FormValue.GetAll(RequestedData.Filteration())

                  .Include(c => c.CreatedByNavigation)
                  .Include(c => c.UpdatedByNavigation)
                  .Include(c => c.FormValueDetails.Where(c => c.FormInput.ShowInMainListCP == true && c.FormInput.IsDeleted == false))
                  .ThenInclude(c => c.FormInput)
                  .ThenInclude(c => c.FormInputDataSource)
                  .ThenInclude(c => c.DataSource)
                  .OrderByDescending(x => x.CreatedDate ?? x.CreatedDate)
                  .Skip((CurrentPageIndex - 1) * DefaultPaginationCount)
                  .Take(DefaultPaginationCount).AsNoTracking().ToList();

            var listFormValue = listFormValueModel.Adapt<List<ListFormValue>>(ListFormValue.SelectConfig());


            if (listFormValue.Any())
            {

                foreach (var item in listFormValue)
                {
                    Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();

                    AddAllPropertyValues(Headers, item, PropertyValues);

                    formResult.ListFormValue.Add(PropertyValues);
                }
            }

            formResult.Pagination = new ApplicationOperation.Pagination
            {
                TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount),
                CurrentPageIndex = CurrentPageIndex,
                TotalItemsCount = NumberOfRecord
            };
            return formResult;
        }

        private static void AddAllPropertyValues(List<FormListHeader> Headers, ListFormValue item, Dictionary<string, dynamic> PropertyValues)
        {
            var except = Headers.Where(c => c._key != null).Select(c => new { c._key, c.Property, c.Order }).Except(item.FormValueDetails.Select(c => new { c._key, c.Property, c.Order })).ToList();
            if (except.Any())
            {
                foreach (var p in except)
                {
                    item.FormValueDetails.Add(new Dtos.DynamicForm.TemplateDtos.FormValueDetails { _key = p._key, Value = null, Property = p.Property, Order = p.Order });
                }
                item.FormValueDetails = item.FormValueDetails.OrderBy(c => c.Order ?? c._key).ToList();
            }

            PropertyValues.Add("id", item.ID);
            foreach (var p in item.FormValueDetails)
            {
                if (p._typeId == (int)InputTypes.HijriDate && p.DateTimeValue.HasValue)
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

            PropertyValues.Add("PersonCreatedBy", item.PersonCreatedBy);
            PropertyValues.Add("PersonUpdatedBy", item.PersonUpdatedBy);
            PropertyValues.Add("CreatedDateString", item.CreatedDateString);
            PropertyValues.Add("UpdatedDateString", item.UpdatedDateString);
            PropertyValues.Add("IsActive", item.IsActive);
            PropertyValues.Add("StatusStringAr", item.StatusStringAr);
            PropertyValues.Add("StatusStringEn", item.StatusStringEn);
        }

        #endregion


        public async Task<OperationOutput> ModelActions(ListFormValue RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.FormValueId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.FormValue.ExecuteUpdateAsync(x => x.Id == RequestedData.FormValueId.Value,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.FormValue.ExecuteUpdateAsync(x => x.Id == RequestedData.FormValueId.Value,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ItemActivation, RequestedData.IsActive));

        }

        #endregion

        #region CP Form Value Detail (Draw form With Value)
        // id:formvalueId , _referenceId
        public async Task<OperationOutput> GetFormValueDetail(ListFormValue RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Dtos.DynamicForm.TemplateDtos.FormView formDto = null;
            UserPermission userPermission = null;
            Dtos.DynamicForm.FormModuleDtos.Entity entityDto = null;
            var accordionFields = new List<FormField>();

            var formValue = _unitOfWork.FormValue.GetAll().Include(c => c.FormValueDetails).AsNoTracking().FirstOrDefault(c => c.Id == RequestedData.FormValueId);

            if (formValue is not null)
            {
                var entity = await _unitOfWork.Entity.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == formValue.EntityId);
                entityDto = entity.Adapt<Dtos.DynamicForm.FormModuleDtos.Entity>();

                if (entityDto is not null)
                    userPermission = GetUserEntityPermission(entity.Id, RequestedData.ReferenceId);


                var formModel = await _unitOfWork.Forms.GetAll(x => x.Id == formValue.FormId
                    && x.ReferenceId == RequestedData.ReferenceId)
                   .Include(s => s.FormInputs.Where(c => c.IsDeleted == false))
                   .ThenInclude(v => v.FormInputDataSource)
                   .ThenInclude(c => c.DataSource)
                   .Include(s => s.FormInputs)
                   .ThenInclude(s => s.InputsType).AsNoTracking().FirstOrDefaultAsync();

                formDto = formModel.Adapt<Dtos.DynamicForm.TemplateDtos.FormView>(Dtos.DynamicForm.TemplateDtos.FormView.SelectConfig(ImagesGetPath));

                // if input fields has datasource from api method 
                GetInputsOptionsFromCallAPIMethods(formDto);

                // set values
                if (formDto is not null)
                {
                    foreach (var field in formDto.FormFields)
                    {
                        SetFormFieldValue(formValue, field);
                    }
                }
                if (formDto is not null && formDto.FormFields.Any(c => c.Type == (int)InputTypes.Accordion))
                {
                    accordionFields = GetAccordionFields(formDto, accordionFields, formValue);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
           new OutputDictionary(OperationOutputKeys.DynamicForm, formDto),
           new OutputDictionary(OperationOutputKeys.EntityObj, entityDto),
           new OutputDictionary(OperationOutputKeys.UserPermission, userPermission),
           new OutputDictionary(OperationOutputKeys.ItemFormValue, formValue != null ? new ListFormValue { FormValueId = formValue.Id, IsActive = formValue.IsActive, IsDeleted = formValue.IsDeleted } : null),
           new OutputDictionary(OperationOutputKeys.AccordionFields, accordionFields));

        }

        #region HELPER METHODS GetFormValueDetail

        private static void SetFormFieldValue(Models.FormValue formValue, FormField field)
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

        private static void SetInputValue(Models.FormValue formValue, FormField field)
        {

            if (formValue.FormValueDetails.Any())
            {
                var fdetail = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                field.Value = fdetail != null ? fdetail.InputValue : null;
                field.Description = fdetail != null ? fdetail.Description : string.Empty;
            }

        }

        private static void SetValueToMultipleOptions(Models.FormValue formValue, FormField field)
        {
            if (formValue.FormValueDetails.Any())
            {
                var fv = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                if (fv != null)
                {
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemsValue>>(fv.InputValue);
                    if (list != null && list.Count > 0)
                    {
                        var inputValues = list.Select(c => new { c.Key, c.ValueAr, c.ValueEn }).ToList();
                        field.Value = inputValues;
                    }
                }
            }
        }

        private static void SetHijriDateValue(Models.FormValue formValue, FormField field)
        {
            if (formValue.FormValueDetails.Any())
            {
                var fdetail = formValue.FormValueDetails.FirstOrDefault(c => c.InputKey == field._key);
                field.Value = fdetail != null ? Dates.ConvertFromGerogianToHijriDateString(DateTime.Parse(Convert.ToString(fdetail.InputValue).Replace("ValueKind = String :", ""))) : null;
            }
        }

        private static void SetFileUrlValue(Models.FormValue formValue, FormField field)
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

        private static void SetUrlImageValue(Models.FormValue formValue, FormField field)
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


        #region Portal APIS

        //_formId , ref,entity,pag
        public async Task<OperationOutput> GetDataList(DynamicFormListDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<object> propertyList = new List<object>();
            Dtos.DynamicForm.TemplateDtos.Form form = null;
            ApplicationOperation.Pagination Pagination = null;
            int NumberOfRecord;
            int CurrentPageIndex = 1;

            var entityForm = await _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).AsNoTracking().FirstOrDefaultAsync(c => c.EntityId == RequestedData.EntityId);
            if (entityForm is not null)
            {
                RequestedData.FormId = entityForm.FormId;

                var formModel = await _unitOfWork.Forms.GetAll().Include(c => c.Theme).AsNoTracking().FirstOrDefaultAsync(c => c.Id == entityForm.FormId);
                form = formModel.Adapt<Dtos.DynamicForm.TemplateDtos.Form>(Dtos.DynamicForm.TemplateDtos.Form.ConfigFormToGetDataList(ImagesGetPath));

                bool accessToForm = _unitOfWork.Forms.GetAll().Count(c => c.Id == RequestedData.FormId && c.ReferenceId == RequestedData.ReferenceId) > 0 ? true : false;
                if (!accessToForm)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

                RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;
                NumberOfRecord = _unitOfWork.FormValue.Count(RequestedData.Filteration());

                if (RequestedData.Pagination != null)
                {
                    if (RequestedData.Pagination.CurrentPageIndex.HasValue)
                        CurrentPageIndex = RequestedData.Pagination.CurrentPageIndex.Value;
                    if (RequestedData.Pagination.RecordPerPage.HasValue)
                        DefaultPaginationCount = RequestedData.Pagination.RecordPerPage.Value;

                }
                else DefaultPaginationCount = NumberOfRecord;

                var dList = _unitOfWork.FormValue.GetAll(RequestedData.Filteration())
                       .OrderByDescending(x => x.CreatedDate ?? x.CreatedDate)
                       .Include(c => c.FormValueDetails.Where(c => c.FormInput.ShowInMainPortalPage == true && c.FormInput.IsDeleted == false))
                       .ThenInclude(c => c.FormInput)
                       .ThenInclude(c => c.InputsType)
                       .Skip((CurrentPageIndex - 1) * DefaultPaginationCount)
                       .Take(DefaultPaginationCount)
                       .Select(x => new ListFormValue
                       {
                           FormValueId = x.Id,
                           FormValueDetails = x.FormValueDetails.Any() ? x.FormValueDetails.Select(c => new Dtos.DynamicForm.TemplateDtos.FormValueDetails
                           {
                               _key = c.InputKey,
                               Property = c.FormInput != null ? c.FormInput.Property.Trim() : null,
                               Value = GetValue(c),
                               Order = c.FormInput != null ? c.FormInput.Order : null,
                           }).OrderBy(c => c.Order ?? c._key).ToList() : new List<Dtos.DynamicForm.TemplateDtos.FormValueDetails>()
                       }).ToList();

                foreach (var item in dList)
                {
                    Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();
                    PropertyValues.Add("id", item.ID);
                    foreach (var p in item.FormValueDetails)
                    {
                        PropertyValues.Add(p.Property, p.Value);
                    }
                    propertyList.Add(PropertyValues);

                }
                Pagination = new ApplicationOperation.Pagination
                {
                    TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount),
                    CurrentPageIndex = CurrentPageIndex,
                    TotalItemsCount = NumberOfRecord
                };
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.EntityFormValue, propertyList),
                 new OutputDictionary(OperationOutputKeys.DynamicForm, form),
                 new OutputDictionary(OperationOutputKeys.Pagination, Pagination));
        }

        #region HELPPER METHOD >> GetDataList

        public static string GetValue(Models.FormValueDetails details)
        {
            var inputValue = details.InputValue;
            if (details.FormInput != null && details.FormInput.Type == (int)DynamicEnum.InputTypes.FileImage)
                return !string.IsNullOrEmpty(inputValue) ? imagesGetPath + "/" + inputValue : imagesGetPath + "/noImage.png";

            if (details.FormInput != null && details.FormInput.Type == (int)DynamicEnum.InputTypes.FileDocument)
                return !string.IsNullOrEmpty(inputValue) ? filesGetPath + "/" + inputValue : filesGetPath + "/noImage.png";

            if (details.FormInput != null && details.FormInput.Type == (int)DynamicEnum.InputTypes.Link)
                return !string.IsNullOrEmpty(inputValue) ? filesGetPath + "/" + inputValue : filesGetPath + "/noImage.png";

            if (details.FormInput != null && details.FormInput.Type == (int)DynamicEnum.InputTypes.DropdownSingle)
                return details.Description;

            return inputValue;

        }

        #endregion


        public async Task<OperationOutput> GetDetail(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData)
        {
            bool? formIsViewStatistic = null;

            var formValue = await _unitOfWork.FormValue.GetAll(c => c.Id == RequestedData._id)
                  .Include(c => c.FormValueDetails)
                  .ThenInclude(c => c.FormInput)
                  .ThenInclude(c => c.InputsType)
               .Select(x => new ListFormValue
               {
                   FormValueId = x.Id,
                   FormId = x.FormId,
                   EntityId = x.EntityId,
                   FormValueDetails = x.FormValueDetails.Any() ? x.FormValueDetails.Where(c => c.FormInput.ShowInMainPortalPage == true && c.FormInput.IsDeleted == false).Select(c => new Dtos.DynamicForm.TemplateDtos.FormValueDetails
                   {
                       _key = c.InputKey,
                       _typeId = c.FormInput != null ? c.FormInput.Type : null,
                       Property = c.FormInput != null ? c.FormInput.Property : null,
                       Value = GetValue(c),
                       DateTimeValue = c.DateTimeValue,
                       BooleanValue = c.BooleanValue,
                       NumericValue = c.NumericValue,

                       Order = c.FormInput != null ? c.FormInput.Order : null,
                   }).OrderBy(c => c.Order ?? c._key).ToList() : new List<Dtos.DynamicForm.TemplateDtos.FormValueDetails>()
               }).FirstOrDefaultAsync();

            if (formValue is not null)

                formIsViewStatistic = await _unitOfWork.Forms.GetAll().AsNoTracking()
                                      .Where(c => c.Id == formValue.FormId)
                                      .Select(c => c.IsViewStatistic).FirstOrDefaultAsync();

            Dictionary<string, dynamic> formValueProperty = new Dictionary<string, dynamic>();

            formValueProperty.Add("id", formValue.ID);

            foreach (var p in formValue.FormValueDetails)
            {
                switch (p._typeId)
                {
                    case (int)InputTypes.HijriDate when p.DateTimeValue.HasValue:
                        try { p.Value = Dates.ConvertFromGerogianToHijriDateString(p.DateTimeValue.Value); }
                        catch { }

                        break;

                    case (int)InputTypes.Date when p.DateTimeValue.HasValue:
                        p.Value = p.DateTimeValue.Value.ToString("yyyy-MM-dd");
                        break;

                    case (int)InputTypes.Time when p.DateTimeValue.HasValue:
                        p.Value = p.DateTimeValue.Value.ToString("hh:mm tt");
                        break;

                    case (int)InputTypes.DateTime when p.DateTimeValue.HasValue:
                        p.Value = p.DateTimeValue.Value.ToString("yyyy-MM-dd hh:mm tt");
                        break;


                    default:
                        if (!string.IsNullOrEmpty(p.Description))
                            p.Value = p.Description;

                        else if (p._typeId == (int)InputTypes.Chips && !string.IsNullOrEmpty(p.Value))
                        {
                            var input = p.Value.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty);
                            var list = input.Split(',');
                            if (list != null && list.Length > 0)
                            {
                                formValueProperty.Add(p.Property, list);
                            }
                        }

                        break;
                }

                if (p._typeId != (int)InputTypes.Chips)
                    formValueProperty.Add(p.Property, p.Value);

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.EntityFormValue, formValueProperty),
                 new OutputDictionary(OperationOutputKeys.FormIsViewStatistic, formIsViewStatistic),
                 new OutputDictionary(OperationOutputKeys.EntityID, formValue.entityId));

        }

        public async Task<OperationOutput> AddFormValueViewStatistic(Records.AddFormValueViewStatistic RequestedData)
        {
            if (RequestedData.EntityId is not null)
            {
                var formValueViewStatistic = new FormValueViewStatistic
                {
                    FormValueId = RequestedData.FormValueId,
                    EntityId = RequestedData.EntityId,
                    TextValue = RequestedData.TextValue,
                    UserId = RequestOwner.Id,
                    UserName = RequestOwner.Name,
                    UserReferenceId = RequestedData.UserReferenceId,
                    UserReferenceNameAr = RequestedData.UserReferenceNameAr,
                    UserReferenceNameEn = RequestedData.UserReferenceNameEn,
                    ViewDate = TransactionDate
                };

                _unitOfWork.FormValueViewStatistic.Add(formValueViewStatistic);
                await _unitOfWork.CompleteAsync();
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }



        #endregion


        #region ADVANCED SEARCH

        public async Task<OperationOutput> GetKeysToAdvancedSearch(Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            if (!RequestedData.Id.HasValue && RequestedData.EntityId.HasValue)
            {
                var entityForm = _unitOfWork.FormsEntity.GetAll().AsNoTracking().FirstOrDefault(c => c.EntityId == RequestedData.EntityId);

                if (entityForm is not null)
                    RequestedData.Id = entityForm.FormId;
            }

            var keys = await _unitOfWork.FormInputs.GetAll(c => c.FormId == RequestedData.Id.Value && c.IsDeleted == false
                   && c.ShowInAdvancedSearch == true)
                .Include(c => c.InputsType)
                .Include(v => v.FormInputDataSource)
                .ThenInclude(c => c.DataSource).AsNoTracking().ToListAsync();

            var keysDto = keys.Adapt<List<KeysResult>>(KeysResult.SelectConfig());

            if (keysDto.Any())
            {
                GetSearchKeysOptionsCallAPIMethods(keysDto);
                fillOptionsToOneCheckBoxInput(keysDto);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.FormId, RequestedData.ID),
               new OutputDictionary(OperationOutputKeys.KeysAdvancedSearch, keysDto));
        }

        // Advanced Search by Procedure
        public OperationOutput AdvancedFilteration1(AdvancedSearchDto RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();

            if (string.IsNullOrEmpty(RequestedData._advancedSearchList))
            {
                var dynamicFormListDto = new DynamicFormListDto
                {
                    FormId = RequestedData._formId,
                    FromDate = RequestedData.FromDate,
                    ToDate = RequestedData.ToDate
                };
                formResult = GetFormListValueForAdvancedSearch(dynamicFormListDto);
            }
            else
            {
                var data = CallAdvanceSearchProcedure(RequestedData, false);
                if (data.Any())
                {
                    List<ListFormValue> listFormValue = GetFormValueFilteredList(data);
                    if (listFormValue.Any())
                    {
                        AddDataToDictionary(formResult, listFormValue);
                    }
                }
            }

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutputKeys.DynamicForm,formResult}

            };
            return Result;

        }

        public async Task<OperationOutput> AdvancedFilteration(AdvancedSearchDto RequestedData)
        {
            var formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();

            var dynamicFormListDto = new DynamicFormListDto { FormId = RequestedData._formId, FromDate = RequestedData.FromDate, ToDate = RequestedData.ToDate, Pagination = RequestedData.Pagination };

            formResult = await SearchFormData(dynamicFormListDto, RequestedData.AdvancedSearchList);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.DynamicForm, formResult),
             new OutputDictionary(OperationOutputKeys.Pagination, formResult.Pagination));

        }


        #region HELPER METHODS >> ADVANCED SEARCH

        private async Task<Dtos.DynamicForm.FormModuleDtos.FormListDataResult> SearchFormData(DynamicFormListDto RequestedData, List<AdvancedSearch> advancedSearchList)
        {
            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();
            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;
            //  List<EF.FormValue> listFormValueModel;
            (ApplicationOperation.Pagination Pagination, IQueryable<Models.FormValue> Data) listFormValueModel;

            if (!advancedSearchList.Any())
            {
                var listFormValueQuery = _unitOfWork.FormValue.GetAll(RequestedData.Filteration())
                  .Include(c => c.CreatedByNavigation)
                  .Include(c => c.UpdatedByNavigation)
                  .Include(c => c.FormValueDetails.Where(c => c.FormInput.ShowInMainListCP == true && c.FormInput.IsDeleted == false))
                  .ThenInclude(c => c.FormInput)
                  .ThenInclude(c => c.FormInputDataSource)
                  .ThenInclude(c => c.DataSource)
                  .OrderByDescending(x => x.CreatedDate ?? x.CreatedDate);

                listFormValueModel = _unitOfWork.FormValue.GetAllWithPaggination(listFormValueQuery, RequestedData.Pagination, DefaultPaginationCount);

            }
            else
            {
                listFormValueModel = await SearchFormDataByInputOperators(RequestedData, advancedSearchList, false);
            }
            var listFormValue = listFormValueModel.Data.Adapt<List<ListFormValue>>(ListFormValue.SelectConfig());
            formResult.Pagination = listFormValueModel.Pagination;

            if (listFormValue.Any())
            {
                AddDataToDictionary(formResult, listFormValue);
            }
            return formResult;
        }

        private async Task<(ApplicationOperation.Pagination Pagination, IQueryable<Models.FormValue> Data)> SearchFormDataByInputOperators(DynamicFormListDto RequestedData, List<AdvancedSearch> advancedSearchList, bool isExport)
        {
            IQueryable<int?> formvalueDetails = null;
            // List<EF.FormValue> listFormValueModel;
            (ApplicationOperation.Pagination Pagination, IQueryable<Models.FormValue> Data) listFormValueModel;

            var datasourceFilterationExists = advancedSearchList.Where(c => DatasourceInputs.GetDatasourceInputs().Exists(t => t.Type == c.ItemType)).ToList();

            var formInputsDatasourceFromAPI = _unitOfWork.FormInputs.GetAll(c => c.FormId == RequestedData.FormId && c.IsDeleted == false
                && c.HasDataSourceFromAPI == true && datasourceFilterationExists.Select(n => n._key).Contains(c.Id))
                .Select(c => c.Id).ToList();

            if (formInputsDatasourceFromAPI.Any())
            {
                datasourceFilterationExists = datasourceFilterationExists.Where(c => !formInputsDatasourceFromAPI.Contains(c._key.Value)).ToList();
            }

            var otherThanDatasourceFilteration = advancedSearchList.Exists(c => !DatasourceInputs.GetDatasourceInputs().Exists(t => t.Type == c.ItemType));

            if (otherThanDatasourceFilteration || formInputsDatasourceFromAPI.Any())
            {
                var formValueDetailsFilteration = AdvancedSearchService.FormValueDetailsFilteration(advancedSearchList, RequestedData.FormId.Value, formInputsDatasourceFromAPI, isExport);


                formvalueDetails = _unitOfWork.FormValueDetails.GetAll(formValueDetailsFilteration)
                   .Include(c => c.FormInput)
                   .OrderBy(c => c.FormValueId).Select(c => c.FormValueId);
            }

            if (datasourceFilterationExists != null && datasourceFilterationExists.Count() > 0)
                formvalueDetails = FilteredFormValueDatasource(datasourceFilterationExists, formvalueDetails, isExport);

            if (advancedSearchList.Count > 1)
            {
                formvalueDetails = formvalueDetails.GroupBy(n => n).Where(g => g.Count() == advancedSearchList.Count()).Select(c => c.Key);
            }

            var listFormValueQuery = _unitOfWork.FormValue.GetAll(RequestedData.Filteration())
              .Include(c => c.CreatedByNavigation)
              .Include(c => c.UpdatedByNavigation)
              .Include(c => c.FormValueDetails)
              .ThenInclude(c => c.FormInput)
              .ThenInclude(c => c.FormInputDataSource)
              .ThenInclude(c => c.DataSource)
              .Where(c => formvalueDetails.Contains(c.Id))
              .OrderByDescending(x => x.CreatedDate ?? x.CreatedDate);

            if (isExport)
            {
                RequestedData.Pagination.RecordPerPage = null; RequestedData.Pagination.CurrentPageIndex = 1;
            }

            listFormValueModel = _unitOfWork.FormValue.GetAllWithPaggination(listFormValueQuery, RequestedData.Pagination, DefaultPaginationCount);

            return listFormValueModel;
        }

        private IQueryable<int?> FilteredFormValueDatasource(List<AdvancedSearch> inputs, IQueryable<int?> formvalueDetails, bool isExport)
        {
            if (inputs.Any())
            {
                var formValueDatasource = _unitOfWork.FormValueDataSource.GetAll(AdvancedSearchService.FormValueDatasourceFilteration(inputs, isExport))
                    .Include(c => c.FormInput)
                    .Where(c => c.FormInput.HasDataSourceFromAPI != true)
                    .Select(c => c.FormValueId);
                if (formValueDatasource.Any())
                {
                    if (formvalueDetails is not null)
                    {
                        var intersectFormValue = formvalueDetails.Intersect(formValueDatasource);
                        formvalueDetails = formvalueDetails.Concat(intersectFormValue);
                    }
                    else formvalueDetails = formValueDatasource;
                }
            }

            return formvalueDetails;
        }


        #region HELPER METHODS ADVANCED SEARCH PROCEDURE
        private Dtos.DynamicForm.FormModuleDtos.FormListDataResult GetFormListValueForAdvancedSearch(DynamicFormListDto RequestedData)
        {
            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();


            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var listFormValueModel = _unitOfWork.FormValue.GetAll(RequestedData.Filteration())
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.UpdatedByNavigation)
                  .Include(c => c.FormValueDetails.Where(c => c.FormInput.ShowInMainListCP == true && c.FormInput.IsDeleted == false))
                  .ThenInclude(c => c.FormInput)
                  .ThenInclude(c => c.FormInputDataSource)
                  .ThenInclude(c => c.DataSource)
                  .OrderByDescending(x => x.CreatedDate ?? x.CreatedDate);


            var listFormValue = listFormValueModel.Adapt<List<ListFormValue>>(ListFormValue.SelectConfig());

            if (listFormValue.Any())
            {
                AddDataToDictionary(formResult, listFormValue);
            }
            return formResult;
        }

        private List<DynamicFormAdvanceSearch> CallAdvanceSearchProcedure(AdvancedSearchDto RequestedData, bool isExport)
        {

            string ProcedureParameters = string.Empty;
            string ProcedureName = "sp_dynamicFormAdvanceSearch";


            ProcedureParameters = "@FormId,@FromDate,@ToDate,@IsExport,@DynamicFormsTableDynamicParams";

            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("FormId", RequestedData._formId.HasValue ? RequestedData._formId : DBNull.Value));
            Params.Add(new Tuple<string, object>("FromDate", RequestedData.FromDate.HasValue ? RequestedData.FromDate : DBNull.Value));
            Params.Add(new Tuple<string, object>("ToDate", RequestedData.ToDate.HasValue ? RequestedData.ToDate : DBNull.Value));
            Params.Add(new Tuple<string, object>("IsExport", isExport));
            Params.Add(new Tuple<string, object>("DynamicFormsTableDynamicParams", !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData._advancedSearchList) ? RequestedData._advancedSearchList : DBNull.Value));

            return _unitOfWork.sp_dynamicFormAdvanceSearch.QueryProcedure(ProcedureName, Params).ToList();

        }


        #endregion
        private void GetSearchKeysOptionsCallAPIMethods(List<KeysResult> keys)
        {
            var inputsDSFromAPI = keys.Where(c => c.HasDataSourceFromAPI == true).ToList();
            if (inputsDSFromAPI.Any())
            {
                List<FieldOptions> result = null;
                for (int i = 0; i < inputsDSFromAPI.Count; i++)
                {
                    result = new List<FieldOptions>();
                    MethodInfo apiMethod = typeof(DynamicFormService).GetMethod(inputsDSFromAPI[i].DataSourceAPIRouting.Trim());
                    if (apiMethod != null)
                    {
                        if (!string.IsNullOrEmpty(inputsDSFromAPI[i].APIParameters))
                        {
                            LookupParameterModel parameterModel = System.Text.Json.JsonSerializer.Deserialize<LookupParameterModel>(inputsDSFromAPI[i].APIParameters, jsonOptions);
                            result = (List<FieldOptions>)apiMethod.Invoke(this, new object[] { parameterModel });
                        }
                        else result = (List<FieldOptions>)apiMethod.Invoke(this, null);
                        keys.Find(c => c.Key == inputsDSFromAPI[i].Key).Options = result;
                    }
                }
            }
        }

        private void fillOptionsToOneCheckBoxInput(List<KeysResult> keys)
        {
            var inputsOneCheckbox = keys.Where(c => c.Type == (int)DynamicEnum.InputTypes.OneChoiceCheckbox).ToList();
            if (inputsOneCheckbox.Any())
            {
                List<FieldOptions> result = null;
                for (int i = 0; i < inputsOneCheckbox.Count; i++)
                {
                    result = new List<FieldOptions>();
                    result.Add(new FieldOptions { _key = 1, ValueAr = "True", ValueEn = "True" });
                    result.Add(new FieldOptions { _key = 0, ValueAr = "False", ValueEn = "False" });
                    keys.Find(c => c.Key == inputsOneCheckbox[i].Key).Options = result;
                }
            }
        }
        private List<ListFormValue> GetFormValueFilteredList(List<Models.DynamicFormAdvanceSearch> data)
        {
            return data.GroupBy(det => det.Id)
            .Select(grp => new ListFormValue
            {
                FormValueId = grp.Key,
                PersonCreatedBy = grp.First().CreatedByUser,
                PersonUpdatedBy = grp.First().UpdatedByUser,
                CreatedDate = grp.First().CreatedDate,
                UpdatedDate = grp.First().UpdatedDate,
                IsActive = grp.First().IsActive,
                StatusStringAr = grp.First().IsActive == true ? "فعال" : "غير فعال",
                StatusStringEn = grp.First().IsActive == true ? "Active" : "Deactive ",
                CreatedDateString = grp.First().CreatedDate.ToString("yyyy-MM-dd"),
                UpdatedDateString = grp.First().UpdatedDate.ToString("yyyy-MM-dd"),
                FormValueDetails = grp.Select(c => new Dtos.DynamicForm.TemplateDtos.FormValueDetails
                {
                    _key = c.InputKey,
                    _typeId = c.Type,
                    Value = c.InputValue,
                    Property = c.Property,
                    Order = c.Order
                }).OrderBy(c => c.Order ?? c._key).ToList()
            }).ToList();
        }

        private void AddDataToDictionary(Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult, List<ListFormValue> listFormValue)
        {
            foreach (var item in listFormValue)
            {
                Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();

                PropertyValues.Add("id", item.ID);
                foreach (var p in item.FormValueDetails)
                {
                    if (p._typeId == (int)InputTypes.HijriDate && p.DateTimeValue.HasValue)
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

                PropertyValues.Add("PersonCreatedBy", item.PersonCreatedBy);
                PropertyValues.Add("PersonUpdatedBy", item.PersonUpdatedBy);
                PropertyValues.Add("CreatedDateString", item.CreatedDateString);
                PropertyValues.Add("UpdatedDateString", item.UpdatedDateString);
                PropertyValues.Add("IsActive", item.IsActive);
                PropertyValues.Add("StatusStringAr", item.StatusStringAr);
                PropertyValues.Add("StatusStringEn", item.StatusStringEn);

                formResult.ListFormValue.Add(PropertyValues);
            }
        }

        #endregion

        #endregion


        #region EXPORT
        public async Task<OperationOutput> GetFormDataForExport(ExportDataDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            DynamicFormListDto ItemResult = new DynamicFormListDto();
            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            Models.FormsEntity entityForm = null;

            if (!RequestedData.DynamicFormListDto.EntityId.HasValue && !string.IsNullOrEmpty(RequestedData.DynamicFormListDto.EntityUrl))
            {
                var entity = await _unitOfWork.Entity.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.FrontIdentity.ToLower() == RequestedData.DynamicFormListDto.EntityUrl.ToLower());

                if (entity is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.DynamicFormListDto.EntityId = entity.Id;
            }

            if (RequestedData.DynamicFormListDto.EntityId.HasValue)
                entityForm = await _unitOfWork.FormsEntity.GetAll().Include(c => c.Entity).AsNoTracking().FirstOrDefaultAsync(c => c.EntityId == RequestedData.DynamicFormListDto.EntityId);

            if (entityForm is not null)
            {
                RequestedData.DynamicFormListDto.FormId = entityForm.FormId;
                ItemResult.FormId = entityForm.FormId;

                bool accessToForm = await _unitOfWork.Forms.GetAll().CountAsync(c => c.Id == entityForm.FormId && c.ReferenceId == RequestedData.DynamicFormListDto.ReferenceId) > 0 ? true : false;

                var Headers = GetFormHeaderForExport(RequestedData.DynamicFormListDto);
                if (accessToForm)
                {
                    if (string.IsNullOrEmpty(RequestedData.AdvancedSearchDto._advancedSearchList))
                        formResult = GetFormListValueForExport(RequestedData.DynamicFormListDto, Headers);
                    else
                        formResult = await ExportAdvancedFilteration(RequestedData, Headers);
                }
                ItemResult.FormListHeader = Headers; ItemResult.ListFormValue = formResult.ListFormValue;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.EntityFormValue, ItemResult),
              new OutputDictionary(OperationOutputKeys.Pagination, formResult.Pagination),
              new OutputDictionary(OperationOutputKeys.EntityObj, entityForm != null && entityForm.Entity != null ? new Dtos.DynamicForm.FormModuleDtos.Entity { Id = entityForm.Entity.Id, FrontIdentity = entityForm.Entity.FrontIdentity, NameAr = entityForm.Entity.NameAr, NameEn = entityForm.Entity.NameEn, CmsIdentity = entityForm.Entity.CmsIdentity } : null));

        }

        #region HELPER METHODS 
        private List<FormListHeader> GetFormHeaderForExport(DynamicFormListDto RequestedData)
        {
            var formDynamicHeader = _unitOfWork.FormInputs.FindAll(c => c.FormId == RequestedData.FormId
            && c.ShowInExport == true && c.IsDeleted == false).Select(c => new FormListHeader
            {
                _key = c.Id,
                TitleAr = c.NameAr,
                TitleEn = c.NameEn,
                Property = c.Property,
                Order = c.Order
            }).OrderBy(c => c.Order ?? c._key).ToList();
            return formDynamicHeader;
        }

        private static void AddAllPropertyValuesForExport(List<FormListHeader> Headers, ListFormValue item, Dictionary<string, dynamic> PropertyValues)
        {
            var except = Headers.Where(c => c._key != null).Select(c => new { c._key, c.Property, c.Order }).Except(item.FormValueDetails.Select(c => new { c._key, c.Property, c.Order })).ToList();
            if (except.Any())
            {
                foreach (var p in except)
                {
                    item.FormValueDetails.Add(new Dtos.DynamicForm.TemplateDtos.FormValueDetails { _key = p._key, Value = null, Property = p.Property, Order = p.Order });
                }
                item.FormValueDetails = item.FormValueDetails.OrderBy(c => c.Order ?? c._key).ToList();
            }

            // PropertyValues.Add("id", item.ID);
            foreach (var p in item.FormValueDetails)
            {
                if (p._typeId == (int)InputTypes.HijriDate && p.DateTimeValue.HasValue)
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
        }

        private Dtos.DynamicForm.FormModuleDtos.FormListDataResult GetFormListValueForExport(DynamicFormListDto RequestedData, List<FormListHeader> Headers)
        {
            Dtos.DynamicForm.FormModuleDtos.FormListDataResult formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();


            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var listFormValueModel = _unitOfWork.FormValue.GetAll(RequestedData.Filteration())

                  .Include(c => c.FormValueDetails
                  .Where(c => c.FormInput.ShowInExport == true && c.FormInput.IsDeleted == false))
                  .ThenInclude(c => c.FormInput)
                  .ThenInclude(c => c.FormInputDataSource)
                  .ThenInclude(c => c.DataSource)
                  .OrderByDescending(x => x.CreatedDate ?? x.CreatedDate);


            var listFormValue = listFormValueModel.Adapt<List<ListFormValue>>(ListFormValue.SelectForExportConfig());


            if (listFormValue.Any())
            {

                foreach (var item in listFormValue)
                {
                    Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();
                    item.FormValueDetails = item.FormValueDetails.OrderBy(c => c.Order ?? c._key).ToList();
                    AddAllPropertyValuesForExport(Headers, item, PropertyValues);

                    formResult.ListFormValue.Add(PropertyValues);
                }
            }


            return formResult;
        }

        private async Task<Dtos.DynamicForm.FormModuleDtos.FormListDataResult> ExportAdvancedFilteration(ExportDataDto RequestedData, List<FormListHeader> Headers)
        {
            var formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();

            var listFormValueModel = await SearchFormDataByInputOperators(RequestedData.DynamicFormListDto, RequestedData.AdvancedSearchDto.AdvancedSearchList, true);

            if (listFormValueModel.Data.Any())
            {
                var listFormValue = listFormValueModel.Data.Adapt<List<ListFormValue>>(ListFormValue.SelectForExportConfig());

                if (listFormValue.Any())
                {

                    listFormValue = listFormValue.OrderByDescending(x => x.CreatedDate ?? x.CreatedDate).ToList();
                    foreach (var item in listFormValue)
                    {
                        item.FormValueDetails = item.FormValueDetails.OrderBy(c => c.Order).ToList();
                        Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();

                        AddAllPropertyValuesForExport(Headers, item, PropertyValues);

                        formResult.ListFormValue.Add(PropertyValues);
                    }
                }
            }

            return formResult;


        }

        // Call in Export By Procedure
        private Dtos.DynamicForm.FormModuleDtos.FormListDataResult ExportAdvancedFilteration1(AdvancedSearchDto RequestedData, List<FormListHeader> Headers)
        {
            var formResult = new Dtos.DynamicForm.FormModuleDtos.FormListDataResult();
            formResult.ListFormValue = new List<object>();

            var data = CallAdvanceSearchProcedure(RequestedData, true);
            if (data.Any())
            {
                List<ListFormValue> listFormValue = GetFormValueFilteredList(data);

                if (listFormValue.Any())
                {

                    listFormValue = listFormValue.OrderByDescending(x => x.CreatedDate ?? x.CreatedDate).ToList();
                    foreach (var item in listFormValue)
                    {
                        item.FormValueDetails = item.FormValueDetails.OrderBy(c => c.Order).ToList();
                        Dictionary<string, dynamic> PropertyValues = new Dictionary<string, dynamic>();

                        AddAllPropertyValuesForExport(Headers, item, PropertyValues);

                        formResult.ListFormValue.Add(PropertyValues);
                    }
                }
            }

            return formResult;


        }

        #endregion

        #endregion

        #region  HELPER METHOD  UserPermissions

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

        #region  SAVE API DATASOURCE

        public async Task<OperationOutput> SaveAPIDataSource(Dtos.DynamicForm.TemplateDtos.APIDataSource RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.APIDataSource apiDatasource = null;

            var isExisit = await _unitOfWork.APIDataSources.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.SourceMethod.ToLower() == RequestedData.SourceMethod.ToLower());


            if (RequestedData.Id.HasValue)
            {
                apiDatasource = await _unitOfWork.APIDataSources.GetAll().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

                if (apiDatasource is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                if (isExisit is not null && isExisit.Id != RequestedData.Id)
                    return GetOperationOutput(header: Enums.ServiceMessages.SourceMethodIsExist);

                if (apiDatasource.ParamTypeIsObject)
                {
                    var objectDetails = _unitOfWork.APIDataSourceParamObjectDetail.GetAll().Where(c => c.APIDataSourceId == RequestedData.Id).ToList();
                    _unitOfWork.APIDataSourceParamObjectDetail.DeleteRange(objectDetails);
                }
                RequestedData.Adapt(apiDatasource, RequestedData.AddAndUpdateConfig());

                _unitOfWork.APIDataSources.Update(apiDatasource);
            }
            else
            {
                if (isExisit is not null)
                    return GetOperationOutput(header: Enums.ServiceMessages.SourceMethodIsExist);
                apiDatasource = RequestedData.Adapt(new Models.APIDataSource(), RequestedData.AddAndUpdateConfig());

                _unitOfWork.APIDataSources.Add(apiDatasource);
            }

            await _unitOfWork.CompleteAsync();

            var apiDatasources = await _unitOfWork.APIDataSources.GetAll().Include(s => s.APIParamObjectDetail).AsNoTracking().ToListAsync();
            var apiDatasourcesDto = apiDatasources.Adapt<List<Dtos.DynamicForm.TemplateDtos.APIDataSource>>(Dtos.DynamicForm.TemplateDtos.APIDataSource.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
          new OutputDictionary(OperationOutputKeys.APIDatasourcesDto, apiDatasourcesDto));

        }


        #endregion

        #region INTEGRATION 
        public List<FieldOptions> GetIDTypesLookups()
        {
            var task = Task.Run(GetIdTypes);
            task.Wait();
            return task.Result;
        }

        public async Task<List<FieldOptions>> GetIdTypes()
        {
            List<FieldOptions> types = new List<FieldOptions>();

            var idTypeLookups = await InvokeService<IdTypeLookups>.Invoke(yaqeenIdTypeLookupsUrl);

            if (idTypeLookups.Body != null && idTypeLookups.Body.IdNumberTypes.Any())
            {
                types = idTypeLookups.Body.IdNumberTypes.Select(c => new FieldOptions
                {
                    Code = c.code,
                    ValueAr = c.nameAr,
                    ValueEn = c.nameEn

                }).ToList();
            }
            return types;
        }


        #endregion

        #region CLONE FORMS TO REFERENCE
        public async Task<OperationOutput> GetPortalReferences()
        {
            var references = await _unitOfWork.References.GetAll(c => c.IsPortal == true && c.ParentId == null).AsNoTracking().ToListAsync();
            var referencesDto = references.Adapt<List<Lookup>>(Lookup.ReferencLookupConfig());
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.References, referencesDto));
        }


        public async Task<OperationOutput> GetFormsForCloneToReference(Dtos.DynamicForm.TemplateDtos.Form RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var thenByList = new Expression<Func<Models.Form, object>>[] { x => x.UpdatedDate ?? x.UpdatedDate };

            var forms = await _unitOfWork.Forms.FindAllByPaginationWithThenBy(RequestedData.FilterationForClone(),
                           RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate, OrderBy.Descending,
                           thenByList, ThenBy.Descending, c => c.FormType, c => c.Reference, c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var formsDto = forms.Data.Adapt<List<Dtos.DynamicForm.TemplateDtos.Form>>(Dtos.DynamicForm.TemplateDtos.Form.SelectConfig(imagesGetPath, jsonOptions));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.DynamicForm, formsDto),
             new OutputDictionary(OperationOutputKeys.Pagination, forms.Pagination));

        }

        public async Task<OperationOutput> CloneFormsByRefernceId(FormCloneDto RequestedData)
        {
            List<Task<string>> listOfTasks = new List<Task<string>>();
            foreach (var form in RequestedData.Forms)
            {
                listOfTasks.Add(CloneForm(form.Id, RequestedData.ReferenceId.Value));
            }
            var formsId = await Task.WhenAll<string>(listOfTasks);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.NewFormsId, formsId));

        }


        #region HELPER METHOD >> CloneFormsByRefernceId
        private async Task<string> CloneForm(int? formId, int referenceId)
        {
            var optionsBuilder = ReadDbContextOptionFromConfiguration();

            using (var context = new ExternalPortal_v2Context(optionsBuilder.Options))
            {
                if (formId is not null)
                {
                    var formData = await context.Form.Where(c => c.Id == formId)
                        .Include(f => f.FormInputs)
                        .ThenInclude(d => d.FormInputDataSource)
                        .AsNoTracking().FirstOrDefaultAsync();

                    if (formData is not null)
                    {
                        var newForm = formData.Clone(RequestOwner.Id);
                        newForm.Id = 0;
                        newForm.ReferenceId = referenceId;
                        newForm.FormInputs = newForm.FormInputs.Select(n => { n.FormId = 0; n.Id = 0; return n; }).ToList();

                        foreach (var input in newForm.FormInputs)
                        {
                            if (input.FormInputDataSource.Any())
                            {
                                input.FormInputDataSource = input.FormInputDataSource.Select(n => { n.FormInputId = input.Id; n.Id = 0; return n; }).ToList();
                            }
                        }
                        await context.Form.AddAsync(newForm);
                        var result = await context.SaveChangesAsync();
                        if (result > 0)
                            return await Task.FromResult(Accessor.Get(newForm.Id));
                        else return string.Empty;
                    }
                }
                return string.Empty;

            }
        }

        private DbContextOptionsBuilder<ExternalPortal_v2Context> ReadDbContextOptionFromConfiguration()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExternalPortal_v2Context>();
            optionsBuilder.UseSqlServer(_unitOfWork.Configuration.GetConnectionString("DbConnectionString"),
            b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName));
            return optionsBuilder;
        }

        #endregion


        #endregion

    }
}
