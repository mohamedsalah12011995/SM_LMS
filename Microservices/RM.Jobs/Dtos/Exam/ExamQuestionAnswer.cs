
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class ExamQuestionAnswer
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        [JsonIgnore]
        public int? QuestionId { get; set; }
        public string questionId { set { QuestionId = Accessor.Set(value); } get { return Accessor.Get(QuestionId); } }
        [JsonIgnore]
        public int? DataSourceId { get; set; }
        public string dataSourceId { set { DataSourceId = Accessor.Set(value); } get { return Accessor.Get(DataSourceId); } }
        [JsonIgnore]
        public int? ExamAnswerActionId { get; set; }
        public string examAnswerActionId { set { ExamAnswerActionId = Accessor.Set(value); } get { return Accessor.Get(ExamAnswerActionId); } }


        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }


        public string Text { get; set; }
        public int? Value { get; set; }



    }
}
