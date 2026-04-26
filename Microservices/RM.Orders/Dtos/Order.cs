
using System.Collections.Generic;
using System;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System.Text.Json.Serialization;
using RM.Core.Helpers;
using RM.Core.CommonDtos;
using LinqKit;
using Mapster;

namespace RM.Orders.Dtos
{
    public class Order:BaseDto<Order,Models.Order>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public string Subject { get; set; }
        public string Details { get; set; }
        public string Code { get; set; }

        public string TypeString { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string PersonCreatedBy { get; set; }

        [JsonIgnore]
        public int? ActionId { get; set; }
        
        public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(ActionId); } }

        [JsonIgnore]
        public int? RejectedActionId { get; set; }
        
        public string rejectedActionId { set { RejectedActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(RejectedActionId); } }

        [JsonIgnore]
        public int? CloseActionId { get; set; }
        
        public string closeActionId { set { CloseActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(CloseActionId); } }

        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string StatusAr { get; set; }
        public string StatusEn { get; set; }


        public bool? IsDeleted { get; set; }
        public List<OrderActions> Actions { get; set; } = new List<OrderActions>();
        public OrderActions LastAction { get; set; }

        public User User { get; set; }

        public ActionPermission ActionPermission { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Order> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Order>(true);

                filter.And(u => u.ReferenceId == ReferenceId);
                filter.And(u => u.EntityId == EntityId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (!string.IsNullOrEmpty(Code))
                filter.And(u => u.Code.Contains(Code));

            if (!string.IsNullOrEmpty(Subject))
                filter.And(u => u.Subject.Contains(Subject));

            if (!string.IsNullOrEmpty(Details))
                filter.And(u => u.Details.Contains(Details));

            if (CreatedBy.HasValue)
                filter.And(u => u.CreatedBy == CreatedBy);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Order, Order>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.TypeString, src => src.Entity != null ? src.Entity.NameAr : null)
                .Map(dest => dest.ActionId, src => src.Actions.OrderByDescending(v => v.Id).Count() > 0 ? src.Actions.OrderByDescending(v => v.Id).FirstOrDefault().TypeId : null)
                .Map(dest => dest.StatusAr, src => src.Actions.OrderByDescending(v => v.Id).Count() > 0 ? src.Actions.OrderByDescending(v => v.Id).FirstOrDefault().Type.NameAr : null)
                .Map(dest => dest.StatusEn, src => src.Actions.OrderByDescending(v => v.Id).Count() > 0 ? src.Actions.OrderByDescending(v => v.Id).FirstOrDefault().Type.NameEn : null)
                .Ignore(x => x.rejectedActionId)
                .Map(dest => dest.RejectedActionId, src => (int)Enums.OrderActionType.Rejected)
                .Ignore(x => x.closeActionId)
                .Map(dest => dest.CloseActionId, src => (int)Enums.OrderActionType.Closed)
                .Map(dest => dest.Actions, src => src.Actions != null ? src.Actions.Adapt<List<Dtos.OrderActions>>(Dtos.OrderActions.SelectConfig()) :new List<OrderActions>())
                .Map(dest => dest.User, src => src.CreatedByNavigation != null ? src.Adapt<Dtos.User>(Dtos.User.SelectConfig()) : null)

                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Order, Models.Order>().IgnoreNullValues(true)

                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Order, Models.Order>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.Code, src => DateTime.Now.ToString("yyMMddhhmm") + Strings.RandomDigits(4).ToString())
                .Config;

        }
    }

    public class ActionPermission
    {
        public bool AllowAgree { get; set; }
        public bool AllowRejected { get; set; }
        public bool AllowReceived { get; set; }
        public bool AllowReturned { get; set; }
        public bool AllowClosed { get; set; }
        public bool IsJobRoleLibarian { get; set; }
        public bool IsJobRoleFatwaManager{ get; set; }

    }
}
