
using Microsoft.Extensions.FileProviders;
using RM.Orders.Dtos;
using RM.Orders.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using RM.Core.Services;
using RM.Core.Helpers;
using Mapster;
using static RM.Orders.Dtos.OperationOutput;
using System.Linq;
using static RM.Core.Helpers.Enums;

namespace RM.Orders.Services
{
    public class OrderService:BaseService, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetOrderLookups(Dtos.Order RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<Dtos.User> Users = new List<Dtos.User>();
            List<Dtos.Type> FatwaActions = new List<Dtos.Type>();
            List<Dtos.Type> BookActions = new List<Dtos.Type>();

            if (RequestedData.EntityId != null)
            {
                var _Users = _unitOfWork.Orders.GetAll().Where(x => x.ReferenceId == RequestedData.ReferenceId && x.EntityId == RequestedData.EntityId)
                .Include(x => x.CreatedByNavigation).AsNoTracking().ToList().Adapt<List<Dtos.User>>(Dtos.User.SelectConfig());

                foreach (var u in _Users)
                    if (Users.Find(c=>c.Id == u.Id) == null)
                        Users.Add(u);
            }

            var OrderActionTypes = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.OrderActionType)
                .AsNoTracking().ToList().Adapt<List<Dtos.Type>>();


            foreach(var type in OrderActionTypes)
            {
                if(type.Id == (int)Enums.OrderActionType.New || type.Id == (int)Enums.OrderActionType.Closed)
                    FatwaActions.Add(type);

                if(type.Id != (int)Enums.OrderActionType.Closed)
                    BookActions.Add(type);
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OrderActionTypes, OrderActionTypes),
                   new OutputDictionary(OperationOutputKeys.FatwaActions, FatwaActions),
                   new OutputDictionary(OperationOutputKeys.BookActions, BookActions),
                   new OutputDictionary(OperationOutputKeys.Users, Users));
        }

        public async Task<OperationOutput> AddOrder(Dtos.Order RequestedData)
        {
            Models.Order DbItem = new Models.Order();

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            var userDbItem = _unitOfWork.Users.GetById(RequestOwner.Id.Value);
            if (userDbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id.Value));    

            var action = new Models.OrderActions();
            action.OrderId = DbItem.Id;
            action.CreatedBy = userDbItem.Id;
            action.CreatedDate = TransactionDate;
            action.TypeId = (int) Enums.OrderActionType.New;
            DbItem.Actions.Add(action);

            _unitOfWork.Orders.Add(DbItem);
            await _unitOfWork.CompleteAsync();

            return await GetOrderDetails(DbItem.Id);

        }

        public async Task<OperationOutput> GetOrderList(Dtos.Order RequestedData)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            var result = _unitOfWork.Orders.GetAll()
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.Entity)
                .Include(x => x.Actions)
                .ThenInclude(t => t.Type)
                .Where(x => RequestedData.ActionId.HasValue ? x.Actions.Count > 0 && x.Actions.OrderByDescending(v => v.Id).FirstOrDefault().TypeId == RequestedData.ActionId : true)
                .Where(RequestedData.Filteration())
                .OrderByDescending(x => x.CreatedDate)
                .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.Order>>(Dtos.Order.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OrderEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }


        public async Task<OperationOutput> GetOrderDetails(Dtos.Order RequestedData)
        {
            return await GetOrderDetails(RequestedData.Id);
        }

        public async Task<OperationOutput> GetOrderDetails(int? Id)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            var UserDbItem = _unitOfWork.Users.GetAll().Where(x => x.Id == RequestOwner.Id).FirstOrDefault();
            var Item = _unitOfWork.Orders.GetAll()
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.Entity)
                .Include(x => x.Actions)
                .ThenInclude(x => x.CreatedByNavigation)
                .Include(x => x.Actions)
                .ThenInclude(x => x.Type)
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();

            if(Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Order>(Dtos.Order.SelectConfig());

            if (UserDbItem != null && UserDbItem.JobRole!=null)
            {
                Enums.JobRole UserJobRole = (Enums.JobRole)UserDbItem.JobRole;
                ActionPermission ActionPermission = new ActionPermission();
                ActionPermission.AllowAgree = (UserJobRole == Enums.JobRole.Libarian && ItemDto.ActionId == (int) Enums.OrderActionType.New) ? true : false;
                ActionPermission.AllowRejected = (UserJobRole == Enums.JobRole.Libarian && ItemDto.ActionId == (int) Enums.OrderActionType.New) ? true : false;
                ActionPermission.AllowReceived = (UserJobRole == Enums.JobRole.Libarian && ItemDto.ActionId == (int) Enums.OrderActionType.Agree) ? true : false;
                ActionPermission.AllowReturned = (UserJobRole == Enums.JobRole.Libarian && ItemDto.ActionId == (int) Enums.OrderActionType.Received) ? true : false;
                ActionPermission.AllowClosed = (UserJobRole == Enums.JobRole.FatwaManager && ItemDto.ActionId == (int) Enums.OrderActionType.New) ? true : false;
                ActionPermission.IsJobRoleLibarian = (UserJobRole == Enums.JobRole.Libarian) ? true : false;
                ActionPermission.IsJobRoleFatwaManager = (UserJobRole == Enums.JobRole.FatwaManager) ? true : false;
                ItemDto.ActionPermission = ActionPermission;
            }

            switch (ItemDto.ActionId)
            {
                case (int)Enums.OrderActionType.New:{ItemDto.ActionId = (int) Enums.OrderActionType.Agree;break;}
                case (int) Enums.OrderActionType.Agree:{ItemDto.ActionId = (int) Enums.OrderActionType.Received; break;}
                case (int) Enums.OrderActionType.Received:{ItemDto.ActionId = (int) Enums.OrderActionType.Returned;break;}
                default:{ItemDto.ActionId = null;break;}
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.OrderEntity, ItemDto));
        }

        public async Task<OperationOutput> AddOrderAction(Dtos.OrderActions RequestedData)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            var order = _unitOfWork.Orders.GetAll().Include(x=>x.Actions).Where(x => x.Id == RequestedData.OrderId).FirstOrDefault();
            var userDbItem = _unitOfWork.Users.GetById(RequestOwner.Id.Value);
            if (userDbItem == null || order == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);


            var lastActionId = order.Actions.OrderByDescending(a => a.Id).FirstOrDefault().TypeId.Value;
            if (lastActionId == (int) Enums.OrderActionType.Returned || lastActionId == (int) Enums.OrderActionType.Closed)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var action = new Models.OrderActions();
            action.OrderId = RequestedData.OrderId.Value;
            action.CreatedBy = userDbItem.Id;
            action.CreatedDate = TransactionDate;
            action.TypeId = RequestedData.TypeId;
            action.Note = RequestedData.Note;

            _unitOfWork.OrderActions.Add(action);
            await _unitOfWork.CompleteAsync();

            return await GetOrderDetails(RequestedData.OrderId);

        }

        public async Task<OperationOutput> DeleteOrder(Dtos.Order RequestedData)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            var Item = _unitOfWork.Orders.GetAll().Where(x => x.Id == RequestedData.Id).FirstOrDefault();
            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.DeletedBy=RequestOwner.Id.Value;
            Item.DeletedDate = TransactionDate;
            Item.IsDeleted = true;

            _unitOfWork.Orders.Update(Item);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }


        #region Query Part from Portal

        public async Task<OperationOutput> GetOrder(Dtos.Order RequestedData)
        {
            var Item = _unitOfWork.Orders.GetAll()
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.Entity)
                .Include(x => x.Actions)
                .ThenInclude(x => x.CreatedByNavigation)
                .Include(x => x.Actions)
                .ThenInclude(x => x.Type)
                .Where(x => x.Code == RequestedData.Code && x.EntityId== RequestedData.EntityId && x.ReferenceId == RequestedData.ReferenceId)
                .Where(x => x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Order>(Dtos.Order.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OrderEntity, ItemDto));
        }


        #endregion



    }

}

