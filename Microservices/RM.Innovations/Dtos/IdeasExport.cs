using Mapster;
using Newtonsoft.Json;
using RM.Core.CommonDtos;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RM.Core.Helpers;

namespace RM.Innovations.Dtos
{
    public class IdeasExport:BaseDto<IdeasExport,Models.Idea>
    {
        [JsonProperty("Idea Address")]
        public string Address { get; set; }
        [JsonProperty("Idea Description")]
        public string Description { get; set; }
        [JsonProperty("Country")]
        public string Country { get; set; }
        [JsonProperty("City")]
        public string City { get; set; }
        [JsonProperty("Attachment Url")]
        public string Attachment { get; set; }
        [JsonProperty("Created By")]
        public string CreatedBy { get; set; }


        [JsonProperty("Priority")]
        public string Priority { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }
        

        [JsonProperty("NeedsBudget")]
        public string NeedsBudget { get; set; }
        [JsonProperty("Capability")]
        public string Capability { get; set; }
        [JsonProperty("Feasibility")]
        public string Feasibility { get; set; }
        [JsonProperty("CurrentAction")]
        public string CurrentAction { get; set; }
        [JsonProperty("Idea Exist")]
        public string Exist { get; set; }
        [JsonProperty("Idea DisAgree Count")]
        public int? AgreeCount { get; set; }
        [JsonProperty("Idea Agree Count")]
        public int? DisAgreeCount { get; set; }

        [JsonProperty("Idea Comments Count")]
        public int? CommentsCount { get; set; }


        public static TypeAdapterConfig ExportConfig(string FilesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Idea, IdeasExport>()
                .Map(dest => dest.Address, src => src.IdeaAddress)
                .Map(dest => dest.CreatedBy, src => src.CreatedByNavigation.Name)
                .Map(dest => dest.Description, src => src.IdeaDescription)
                .Map(dest => dest.Priority, src => src.PriorityNavigation != null ? src.PriorityNavigation.NameAr : null)
                .Map(dest => dest.Status, src => src.StatusNavigation != null ? src.StatusNavigation.NameAr : null)
                .Map(dest => dest.Type, src => src.TypeNavigation != null ? src.TypeNavigation.NameAr : null)
                .Map(dest => dest.Category, src => src.CategoryNavigation != null ? src.CategoryNavigation.NameAr : null)
                .Map(dest => dest.NeedsBudget, src => src.NeedsBudget.HasValue ? (src.NeedsBudget.Value ? "تطلب" : "لا تطلب") : null)
                .Map(dest => dest.Capability, src => src.Capability.HasValue ? (src.Capability.Value ? "قابلة للتطبيق" : "غير قابلة للتطبيق") : null)
                .Map(dest => dest.Feasibility, src => src.Feasibility.HasValue ? (src.Feasibility.Value ? "مجدية" : "غير مجدية") : null)
                .Map(dest => dest.Exist, src => src.IdeaExist.HasValue ? src.IdeaExist.Value ? "نعم" : "لا":null)
                .Map(dest => dest.Attachment, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.Attachment) ? FilesGetPath + "/" + src.Attachment : null)
                .Map(dest => dest.CurrentAction, src => src.IdeaActions != null ? src.IdeaActions.Count > 0 ? src.IdeaActions.OrderByDescending(x => x.Id).FirstOrDefault() != null && src.IdeaActions.OrderByDescending(x => x.Id).FirstOrDefault().TypeNavigation != null ? src.IdeaActions.OrderByDescending(x => x.Id).FirstOrDefault().TypeNavigation.NameAr : "غير متوفر" : null : null)
                .Map(dest => dest.CommentsCount, src => src.IdeaComments != null ? src.IdeaComments.Where(c => c.IsApproved == true).Count() : 0)

                 .Config;
        }
    }
}
