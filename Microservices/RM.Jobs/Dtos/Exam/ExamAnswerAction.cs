
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class ExamAnswerAction
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get(ItemId); } }


        [JsonIgnore]
        public int? AppExamId { get; set; }
        public string appExamId { set { AppExamId = Accessor.Set(value); } get { return Accessor.Get(AppExamId); } }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }

        public List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; } = new List<ExamQuestionAnswer>();

    }
}
