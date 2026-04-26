
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class JobApplicationExam
    {

        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }


        [JsonIgnore]
        public int? JobApplicationId { get; set; }

        public string jobApplicationId { set { JobApplicationId = Accessor.Set(value); } get { return Accessor.Get<int?>(JobApplicationId); } }

        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }

        [JsonIgnore]
        public int? AppExamId { get; set; }
        public string appExamId { set { AppExamId = Accessor.Set(value); } get { return Accessor.Get(AppExamId); } }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }


        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public string StartAtString { get; set; }
        public string EndAtString { get; set; }

        public int? Result { get; set; }
        public bool? IsSuccess { get; set; }
        public bool? IsDeleted { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

    }

    public class JobApplicationExams
    {
        public List<JobApplicationExam> JobApplicationExamsList { get; set; } = new List<JobApplicationExam>();
        public List<JobApplication> JobApplicationList { get; set; } = new List<JobApplication>();
        [JsonIgnore]
        public int? JobCareerId { get; set; }

        public string jobCareerId { set { JobCareerId = Accessor.Set(value); } get { return Accessor.Get<int?>(JobCareerId); } }

        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }






}
