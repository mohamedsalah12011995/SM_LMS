
using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.WorkFlow.Dtos;
using RM.WorkFlow.Services;
using RM.WorkFlow.UnitOfWorks;
using static RM.WorkFlow.Dtos.OperationOutput;


namespace Surveys.Services.Services.WorkFlow
{

    public class WorkFlowService : BaseService, IWorkFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        public WorkFlowService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;

        }



        #region Engine

        public async Task<OperationOutput> GetWorkflowLookups(Engine RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var workFlowActionsList = await _unitOfWork.WorkFlowActions.GetAll(x => x.IsDeleted != true && x.IsActive == true).AsNoTracking().ToListAsync();
            var workFlowActionsListDto = workFlowActionsList.Adapt<List<WorkFlowActions>>(WorkFlowActions.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.WorkFlowActionsList, workFlowActionsListDto));
        }


        public async Task<OperationOutput> GetEnginesList(Engine RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var engines = await _unitOfWork.Engine.FindAllByPagination(RequestedData.Filteration(),
                RequestedData.Pagination, DefaultPaginationCount, c => c.CreatedDate, OrderBy.Descending,
                c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var enginesDto = engines.Data.Adapt<List<Engine>>(Engine.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.WorkFlowEntity, enginesDto),
             new OutputDictionary(OperationOutputKeys.Pagination, engines.Pagination));

        }

        public async Task<OperationOutput> SaveEngine(Engine RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            RM.Models.Engine Engine = null;
            RM.Models.EngineActionJobRole EnginActionJobRole = null;
            bool EngineIsNew = true;
            List<int> engineActionJobRoleRemoved = new List<int>();
            bool IsSaveSuccess = false;

            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                if (RequestOwner == null)
                {
                    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                    return Result;
                }

                if (!RequestedData.Id.HasValue)
                {
                    Engine = new RM.Models.Engine();
                    Engine.CreatedBy = RequestOwner.Id;
                    Engine.CreatedDate = TransactionDate;
                    Engine.IsActive = false;
                    Engine.IsDeleted = false;
                }
                else
                {
                    EngineIsNew = false;
                    Engine = _unitOfWork.Engine.GetAll().Where(x => x.Id == RequestedData.Id).FirstOrDefault();
                    if (Engine == null)
                    {
                        Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoDataReturned);
                        return Result;
                    }
                    Engine.UpdatedBy = RequestOwner.Id;
                    Engine.UpdatedDate = TransactionDate;
                }
                Engine.ReferenceId = RequestedData.ReferenceId;
                Engine.NameAr = RequestedData.NameAr;
                Engine.NameEn = RequestedData.NameEn;

                if (!EngineIsNew)
                {
                    //EnginActionsJobRole to remove
                    engineActionJobRoleRemoved = _unitOfWork.EnginesActionsJobRole.GetAll().Where(x => x.EngineId == RequestedData.Id && x.IsDeleted != true).Select(x => x.Id).Where(x => !RequestedData.EnginesActionsJobRoles.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();
                    foreach (var egnId in engineActionJobRoleRemoved)
                    {
                        var egnAToDelete = _unitOfWork.EnginesActionsJobRole.GetAll().Where(x => x.Id == egnId).FirstOrDefault();

                        egnAToDelete.IsDeleted = true;
                        egnAToDelete.UpdatedBy = RequestOwner.Id;
                        egnAToDelete.UpdatedDate = TransactionDate;
                        _unitOfWork.EnginesActionsJobRole.Update(egnAToDelete);

                        IsSaveSuccess = _unitOfWork.Complete() > 0 ? true : false;
                    }

                }
                if (RequestedData.EnginesActionsJobRoles != null)
                {
                    //add or update EnginActionJobRole
                    foreach (var enginActionJobRole in RequestedData.EnginesActionsJobRoles)
                    {
                        if (!enginActionJobRole.Id.HasValue)
                        {
                            EnginActionJobRole = new RM.Models.EngineActionJobRole();
                            EnginActionJobRole.EngineId = Engine.Id;
                            EnginActionJobRole.CreatedDate = TransactionDate;
                            EnginActionJobRole.CreatedBy = RequestOwner.Id;
                        }
                        else
                        {
                            EnginActionJobRole = _unitOfWork.EnginesActionsJobRole.GetById(enginActionJobRole.Id.Value);
                            EnginActionJobRole.UpdatedDate = TransactionDate;
                            EnginActionJobRole.UpdatedBy = RequestOwner.Id;
                        }


                        EnginActionJobRole.StepNo = enginActionJobRole.StepNo;
                        EnginActionJobRole.NextStep = enginActionJobRole.NextStep;
                        EnginActionJobRole.CloseStep = enginActionJobRole.CloseStep;
                        EnginActionJobRole.ReturnStep = enginActionJobRole.ReturnStep;
                        EnginActionJobRole.RejectStep = enginActionJobRole.RejectStep;
                        EnginActionJobRole.HasNote = enginActionJobRole.HasNote;
                        EnginActionJobRole.NoteIsRequired = enginActionJobRole.NoteIsRequired;
                        EnginActionJobRole.JobRoleId = enginActionJobRole.JobRoleId;
                        EnginActionJobRole.ActionId = enginActionJobRole.ActionId;
                        EnginActionJobRole.IsTransferToReference = enginActionJobRole.IsTransferToReference;
                        EnginActionJobRole.IsSendEmail = enginActionJobRole.IsSendEmail;
                        EnginActionJobRole.EmailBody = enginActionJobRole.EmailBody;

                        if (!enginActionJobRole.Id.HasValue)
                            Engine.EnginesActionsJobRoles.Add(EnginActionJobRole);
                        else
                            _unitOfWork.EnginesActionsJobRole.Update(EnginActionJobRole);

                    }
                }

                if (EngineIsNew) _unitOfWork.Engine.Add(Engine);
                else _unitOfWork.Engine.Update(Engine);
                IsSaveSuccess = _unitOfWork.Complete() > 0 ? true : false;

                if (IsSaveSuccess)
                {
                    dbContextTransaction.Commit();
                    RequestedData.Id = Engine.Id;
                    return await GetEngineDetails(RequestedData);

                }
                else
                {
                    dbContextTransaction.Rollback();
                    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                }

                return Result;
            }
        }

        #region saveEnginewithmapper
        //public async Task<OperationOutput> SaveEngine1(Engine RequestedData)
        //{
        //    if (RequestOwner is null)
        //        return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

        //    RM.EF.Engine engine = null;
        //    RM.EF.EngineActionJobRole enginActionJobRoleModel = null;
        //    bool isSaveSuccess = false;

        //    using (var dbContextTransaction = _unitOfWork.BeginTransaction())
        //    {
        //        //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        //{

        //        if (RequestedData.Id.HasValue)
        //        {
        //            engine = await _unitOfWork.Engine.GetAll().Include(e => e.EnginesActionsJobRoles).AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted != true);

        //            if (engine is null)
        //                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

        //            var engineActionJobRoleRemoved = engine.EnginesActionsJobRoles.Where(x => x.EngineId == RequestedData.Id && x.IsDeleted != true).Select(x => x.Id).Where(x => !RequestedData.EnginesActionsJobRoles.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();

        //            if (engineActionJobRoleRemoved.Any())
        //                RemoveEngionActionsJobRole(engineActionJobRoleRemoved);

        //            _unitOfWork.Engine.Update(RequestedData.Adapt(engine, RequestedData.UpdateConfig(RequestOwner.Id)));
        //        }
        //        else
        //        {
        //            engine = new RM.EF.Engine();
        //            _unitOfWork.Engine.Add(RequestedData.Adapt(engine, RequestedData.AddConfig(RequestOwner.Id)));
        //        }

        //        isSaveSuccess = _unitOfWork.Complete() > 0 ? true : false;

        //        if (RequestedData.EnginesActionsJobRoles.Any())
        //        {
        //            //add or update EnginActionJobRole
        //            foreach (var enginActionJobRole in RequestedData.EnginesActionsJobRoles)
        //            {
        //                if (enginActionJobRole.Id.HasValue)
        //                {
        //                    enginActionJobRoleModel = _unitOfWork.EnginesActionsJobRole.GetById(enginActionJobRole.Id.Value);

        //                    if (enginActionJobRoleModel is null)
        //                        return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

        //                    enginActionJobRole.Adapt(enginActionJobRoleModel, enginActionJobRole.UpdateConfig(RequestOwner.Id));
        //                    enginActionJobRoleModel.EngineId = engine.Id;
        //                    _unitOfWork.EnginesActionsJobRole.Update(enginActionJobRoleModel);
        //                }
        //                else
        //                {
        //                    enginActionJobRoleModel = new RM.EF.EngineActionJobRole();
        //                    enginActionJobRole.Adapt(enginActionJobRoleModel, enginActionJobRole.AddConfig(RequestOwner.Id, engine));
        //                    enginActionJobRoleModel.EngineId = engine.Id;

        //                    _unitOfWork.EnginesActionsJobRole.Add(enginActionJobRoleModel);

        //                }
        //                //  await _unitOfWork.CompleteAsync();

        //                //if (enginActionJobRole.Id.HasValue)
        //                //{
        //                //    enginActionJobRoleModel = engine.EnginesActionsJobRoles.FirstOrDefault(c => c.Id == enginActionJobRole.Id.Value);

        //                //    if (enginActionJobRoleModel is null)
        //                //        return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
        //                //    engine.EnginesActionsJobRoles.Remove(enginActionJobRoleModel);
        //                //    enginActionJobRoleModel = enginActionJobRole.Adapt(enginActionJobRoleModel, enginActionJobRole.UpdateConfig(RequestOwner.Id));
        //                //    engine.EnginesActionsJobRoles.Add(enginActionJobRoleModel);

        //                //    // _unitOfWork.EnginesActionsJobRole.Update(enginActionJobRole.Adapt(enginActionJobRoleModel, enginActionJobRole.UpdateConfig(RequestOwner.Id)));
        //                //}
        //                //else
        //                //{
        //                //    enginActionJobRoleModel = new RM.EF.EngineActionJobRole();
        //                //    enginActionJobRoleModel = enginActionJobRole.Adapt(enginActionJobRoleModel, enginActionJobRole.AddConfig(RequestOwner.Id, engine));
        //                //    engine.EnginesActionsJobRoles.Add(enginActionJobRoleModel);


        //                //    // _unitOfWork.EnginesActionsJobRole.Add(enginActionJobRole.Adapt(enginActionJobRoleModel, enginActionJobRole.AddConfig(RequestOwner.Id, engine)));
        //                //}


        //            }
        //            //  await _unitOfWork.CompleteAsync();
        //        }

        //        if (isSaveSuccess)
        //        {

        //            dbContextTransaction.Commit();
        //            //  scope.Complete();
        //            RequestedData.Id = engine.Id;
        //            return await GetEngineDetails(RequestedData);
        //        }
        //        else
        //        {

        //            await dbContextTransaction.RollbackAsync();
        //            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        //        }


        //    }
        //}
        #endregion

        #region HELPER METHOD SAVE ENGINE

        private void RemoveEngionActionsJobRole(List<int> engineActionJobRoleRemoved)
        {
            var enginesActionsJobRoleModels = _unitOfWork.EnginesActionsJobRole.GetAll().AsNoTracking().Where(x => engineActionJobRoleRemoved.Contains(x.Id)).ToList();
            if (enginesActionsJobRoleModels.Any())
            {
                foreach (var actionJobRole in enginesActionsJobRoleModels)
                {

                    actionJobRole.IsDeleted = true;
                    actionJobRole.UpdatedBy = RequestOwner.Id;
                    actionJobRole.UpdatedDate = TransactionDate;
                    _unitOfWork.EnginesActionsJobRole.Update(actionJobRole);
                }
            }


        }

        #endregion

        public async Task<OperationOutput> GetEngineDetails(Engine RequestedData)
        {
            IEnumerable<object> steps = null;

            var engin = await _unitOfWork.Engine.GetAll(x => x.IsDeleted != true && x.Id == RequestedData.Id.Value)
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.UpdatedByNavigation)
                .Include(j => j.EnginesActionsJobRoles.Where(j => j.IsDeleted != true))
                .FirstOrDefaultAsync();

            var engionDto = engin.Adapt<Engine>(Engine.SelectConfig());

            if (engionDto is not null)
                steps = engionDto.EnginesActionsJobRoles.OrderBy(x => x.StepNo).GroupBy(x => new { x.ActionId, x.StepNo }, (key, group) => new
                {
                    ActionId = Accessor.Get(key.ActionId),
                    StepNo = Accessor.Get(key.StepNo),
                    Steps = group.Select(x => new EnginesActionsJobRole()
                    {
                        ID = x.ID,
                        actionId = x.actionId,
                        engineId = x.engineId,
                        jobRoleId = x.jobRoleId,
                        nextStep = x.nextStep,
                        closeStep = x.closeStep,
                        returnStep = x.returnStep,
                        rejectStep = x.rejectStep,
                        HasNote = x.HasNote,
                        NoteIsRequired = x.NoteIsRequired,
                        StepNo = x.StepNo,
                        IsTransferToReference = x.IsTransferToReference,
                        IsSendEmail = x.IsSendEmail,
                        EmailBody = x.EmailBody

                    }).ToList()
                });

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.WorkFlowEntity, engionDto),
                  new OutputDictionary(OperationOutputKeys.Steps, steps));
        }

        public async Task<OperationOutput> EnginesDeleteList(List<Engine> RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var deletedEnginIds = RequestedData.Select(x => x.Id).ToList();

            var deletedEngin = await _unitOfWork.Engine.GetAll(x => deletedEnginIds.Contains(x.Id)).ToListAsync();
            foreach (var engin in deletedEngin)
            {
                engin.IsDeleted = true;
                engin.UpdatedBy = RequestOwner.Id;
                engin.DeletedDate = TransactionDate;
                _unitOfWork.Engine.Update(engin);
            }

            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> EnginesActivationList(List<Engine> RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var activatedEnginIds = RequestedData.Select(x => x.Id).ToList();
            var activateEngin = await _unitOfWork.Engine.GetAll(x => activatedEnginIds.Contains(x.Id)).ToListAsync();
            foreach (var engin in activateEngin)
            {
                engin.IsActive = RequestedData.Where(x => x.Id == engin.Id).FirstOrDefault().IsActive ?? !engin.IsActive;
                engin.UpdatedBy = RequestOwner.Id;
                engin.ActivatedDate = TransactionDate;
                _unitOfWork.Engine.Update(engin);
            }

            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetAllMajorsWithReferencesTree(ReferencesTreeDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var maxId = await _unitOfWork.References.GetAll().Select(x => x.Id).MaxAsync();
            var majors = await _unitOfWork.ReferencesMajor.GetAll(x => RequestedData._referencesMajorId != null ? x.Id == RequestedData._referencesMajorId : true && x.IsDeleted != true)
                .Select(x => new ReferencesTreeDto()
                {
                    _Id = x.Id + maxId,
                    Label = x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    _referencesMajorId = x.Id

                }).ToListAsync();

            var references = _unitOfWork.References.GetAll()
                .Include(x => x.ReferencesJobRoles)
                .ThenInclude(x => x.JobRole)
                .Where(x => RequestedData._referencesMajorId != null ? x.ReferencesMajorId == RequestedData._referencesMajorId : true && x.IsDeleted != true)

                .Select(x => new ReferencesTreeDto()
                {
                    _Id = x.Id,
                    Label = x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    _parentId = x.ParentId == null ? x.ReferencesMajorId + maxId : x.ParentId,
                    _referencesMajorId = x.ReferencesMajorId,
                    ReferenceJobRole = x.ReferencesJobRoles.Select(z => new ReferenceJobRoleDto()
                    {
                        _id = z.JobRole.Id,
                        NameAr = z.JobRole.NameAr,
                        NameEn = z.JobRole.NameEn,
                        _referenceId = z.ReferenceId

                    }).ToList(),

                }).ToList();
            references.AddRange(majors);
            var root = references.GenerateTree(c => c._Id, c => c._parentId);

            List<ReferencesTreeDto> referencesTree = new List<ReferencesTreeDto>();
            foreach (var reference in root)
            {
                referencesTree.Add(bindNode(reference));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesTree));

        }

        #region HELPER METHOD GetAllMajorsWithReferencesTree
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

        #endregion

        #region WorkFlowActions

        public async Task<OperationOutput> GetWorkFlowActionsList(WorkFlowActions RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var workFlowActions = await _unitOfWork.WorkFlowActions.FindAllByPagination(RequestedData.Filteration(),
               RequestedData.Pagination, DefaultPaginationCount, c => c.Id, OrderBy.Ascending,
               c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var workFlowActionsDto = workFlowActions.Data.Adapt<List<WorkFlowActions>>(WorkFlowActions.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
          new OutputDictionary(OperationOutputKeys.WorkFlowEntity, workFlowActionsDto),
          new OutputDictionary(OperationOutputKeys.Pagination, workFlowActions.Pagination));

        }

        public async Task<OperationOutput> SaveWorkFlowActions(WorkFlowActions RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RM.Models.WorkFlowActions action = null;

            if (RequestedData.Id.HasValue)
            {
                action = await _unitOfWork.WorkFlowActions.GetAll().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);
                if (action is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                _unitOfWork.WorkFlowActions.Update(RequestedData.Adapt(action, RequestedData.UpdateConfig(RequestOwner.Id)));
            }
            else
            {
                action = new RM.Models.WorkFlowActions();
                _unitOfWork.WorkFlowActions.Add(RequestedData.Adapt(action, RequestedData.AddConfig(RequestOwner.Id)));
            }


            var isSaveSuccess = await _unitOfWork.CompleteAsync() > 0 ? true : false;

            if (isSaveSuccess)
            {
                RequestedData.Id = action.Id;
                return await GetWorkFlowActionsDetails(RequestedData);
            }
            else
            {
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
            }

        }

        public async Task<OperationOutput> GetWorkFlowActionsDetails(WorkFlowActions RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var action = await _unitOfWork.WorkFlowActions.GetAll(x => x.IsDeleted != true && x.Id == RequestedData.Id.Value)
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.UpdatedByNavigation)
                .FirstOrDefaultAsync();

            var actionDto = action.Adapt<WorkFlowActions>(WorkFlowActions.SelectConfig());


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.WorkFlowEntity, actionDto));

        }



        public async Task<OperationOutput> WorkFlowActionsDeleteList(List<WorkFlowActions> RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var deletedActionsIds = RequestedData.Select(x => x.Id).ToList();

            var deletedActions = await _unitOfWork.WorkFlowActions.GetAll(x => deletedActionsIds.Contains(x.Id)).ToListAsync();
            foreach (var action in deletedActions)
            {
                action.IsDeleted = true;
                action.UpdatedBy = RequestOwner.Id;
                action.DeletedDate = TransactionDate;
                _unitOfWork.WorkFlowActions.Update(action);
            }

            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> WorkFlowActionsActivationList(List<WorkFlowActions> RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var activatedActionsIds = RequestedData.Select(x => x.Id).ToList();
            var activateActions = await _unitOfWork.WorkFlowActions.GetAll(x => activatedActionsIds.Contains(x.Id)).ToListAsync();
            foreach (var action in activateActions)
            {
                action.IsActive = RequestedData.Where(x => x.Id == action.Id).FirstOrDefault().IsActive ?? !action.IsActive;
                action.UpdatedBy = RequestOwner.Id;
                action.ActivatedDate = TransactionDate;
                _unitOfWork.WorkFlowActions.Update(action);
            }

            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        #endregion
    }
}
