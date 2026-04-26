

using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static RM.Core.Helpers.Enums;

namespace RM.Jobs.Dtos
{
    public class JobApplication
    {

        [JsonIgnore]
        public int? Id { get; set; }
       
        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string ID { set { Id = Accessor.Set(value) ; } get { return Accessor.Get<int?>(Id); } }

        public string referenceId { set { ReferenceId = Accessor.Set(value) ; } get { return Accessor.Get<int?>(ReferenceId); } }
      
        public string FullName { get; set; }
        public string BirthDay { get; set; }
        [JsonIgnore]
        public int? Gendar { get; set; }

        public string gendar { set { Gendar = Accessor.Set(value) ; } get { return Accessor.Get<int?>(Gendar); } }

        public string GendarString { get; set; }
        public string Code { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public int? QualificationId { get; set; }

        public string qualificationId { set { QualificationId = Accessor.Set(value); } get { return Accessor.Get<int?>(QualificationId); } }

        [JsonIgnore]
        public int? AppExamId { get; set; }

        public string appExamId { set { AppExamId =  Accessor.Set(value) ; } get { return AppExamId > 0 ? Accessor.Get<int?>(AppExamId):null; } }

        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value) ; } get { return ExamId > 0 ? Accessor.Get<int?>(ExamId) : null; } }

        public string GenderIntegration { get; set; }

        [JsonIgnore]
        public int? GradeId { get; set; }
        public string gradeId { set { GradeId =  Accessor.Set(value) ; } get { return Accessor.Get<int?>(GradeId); } }


        [JsonIgnore]
        public int? SpecialistId { get; set; }
        public string specialistId { set { SpecialistId =  Accessor.Set(value) ; } get { return Accessor.Get<int?>(SpecialistId); } }

        [JsonIgnore]
        public int? JobCareerId { get; set; }
        public string jobCareerId { set { JobCareerId =  Accessor.Set(value) ; } get { return Accessor.Get<int?>(JobCareerId); } }

        public int? GradeYear { get; set; }
        public string Skills { get; set; }
        public string Tags { get; set; }
        public int? CurrentState { get; set; }
        public int? NextState { get; set; }
        public string FileAttachment { get; set; }
        public string TrainingCourses { get; set; }
        public string GendarStringEn { get; set; }

        public string StatusString { get; set; }
        public string CurrentStateString { get; set; }
        public string StatusStringEn { get; set; }
        public string CurrentStateStringEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string BirthDayString { get; set; }
        public string Qualification { get; set; }
        public string Specialist { get; set; }
        public string QualificationEn { get; set; }
        public string SpecialistEn { get; set; }

        public List<int> careerIds { get; set; }
        public List<JobLookUp> ListOfTags { get; set; } = new List<JobLookUp>();
        public List<JobLookUp> ListOfSkills { get; set; } = new List<JobLookUp>();
        public List<string> ListOfTrainingCourses { get; set; } = new List<string>();

        public string FileAttachmentBase64 { get; set; }
        public string JobCareerNameAr { get; set; }
        public string JobCareerNameEn { get; set; }

        public string GradeAr { get; set; }
        public string GradeEn { get; set; }
        public int? Type { get; set; }

        public string idNumber { get; set; }
        public string birthOfDate { get; set; }
        public string hijBirthdate { get; set; }
        public string identityNo { get; set; }
        public string idTypeCode { get; set; }
        public bool? birthOfDateIsHijri { get; set; }
        public bool? IsSent { get; set; }
        public string SentAr { get; set; }
        public string SentEn { get; set; }

        public bool? IsSuccess { get; set; }
        public bool? HasExam { get; set; }

        public bool? ExamDone { get; set; }

        public string IsSuccessAr { get; set; }
        public string IsSuccessEn { get; set; }
        public string Result { get; set; }
        public int? FromResult { get; set; }
        public int? ToResult { get; set; }
        public int? ExamResultStatusId { get; set; }
        public string ExamResultStatus { get; set; }
        public string currentLang { get; set; }

        public string ExamNameAr { get; set; }
        public string ExamNameEn { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public DateTime? ExamFromDate { get; set; }
        public DateTime? ExamToDate { get; set; }

        [JsonIgnore]
        public int? UpdateById { get; set; }
        public string updateById { set { UpdateById = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdateById); } }
        public string UpdateByName { get; set; }

        [JsonIgnore]
        public int? ActiveById { get; set; }
        public string activeById { set { ActiveById = Accessor.Set(value); } get { return Accessor.Get<int?>(ActiveById); } }
        public string ActiveByName { get; set; }

        public List<JobApplicationExams>  JobApplicationExams { get; set; } = new List<JobApplicationExams>();

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.JobApplication> Filteration()
        {
            var filter = PredicateBuilder.New<Models.JobApplication>(true);

            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.CurrentState == CurrentState);

            if (careerIds != null)
            {
                foreach (var id in careerIds)
                {
                    int tempId = id; // To avoid closure issue
                    filter.And(u => u.JobCareerId == tempId);
                }
            }

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (ExamFromDate.HasValue)
                filter.And(u => u.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().FromDate == null || u.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().FromDate.Value.Date == ExamFromDate.Value.Date);

            if (ExamToDate.HasValue)
                filter.And(u => u.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate == null || u.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate.Value.Date == ExamToDate.Value.Date);


            if (!string.IsNullOrEmpty(BirthDay))
                filter.And(u => u.BirthDay != null ? u.BirthDay.Value.Date.ToString() == BirthDay : true);

            if (Gendar.HasValue)
                filter.And(u => u.Gendar == Gendar);

            if (GradeYear.HasValue)
                filter.And(u => u.GradeYear == GradeYear);

            if (QualificationId.HasValue)
                filter.And(u => u.QualificationId == QualificationId);

            if (SpecialistId.HasValue)
                filter.And(u => u.SpecialistId == SpecialistId);

            if (!string.IsNullOrEmpty(FullName))
                filter.And(u => u.FullName.Contains(FullName));

            if (!string.IsNullOrEmpty(Phone))
                filter.And(u => u.Phone.Contains(Phone));

            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));

            if (!string.IsNullOrEmpty(IdCardNumber))
                filter.And(u => u.IdCardNumber.Contains(IdCardNumber));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string fileGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.JobApplication, JobApplication>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.BirthDayString, src => src.BirthDay.HasValue ? src.BirthDay.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.BirthDay, src => src.BirthDay.ToString())
                .Map(dest => dest.NextState, src => src.CurrentState < 5 ? src.CurrentState + 1 : src.CurrentState)

                .Map(dest => dest.GendarString, src => src.Gendar == (int)Enums.Gender.Male ? "ذكر" : "انثى")
                .Map(dest => dest.GendarStringEn, src => src.Gendar == (int)Enums.Gender.Male ? "Male" : "Female")

                .Map(dest => dest.Specialist, src => src.Specialist != null ? src.Specialist.Name : string.Empty)
                .Map(dest => dest.SpecialistEn, src => src.Specialist != null ? src.Specialist.NameEn : string.Empty)
                .Map(dest => dest.Qualification, src => src.Qualification != null ? src.Qualification.NameAr : string.Empty)
                .Map(dest => dest.QualificationEn, src => src.Qualification != null ? src.Qualification.NameEn : string.Empty)
                .Map(dest => dest.GradeAr, src => src.Grade != null ? src.Grade.Name : string.Empty)
                .Map(dest => dest.GradeEn, src => src.Grade != null ? src.Grade.NameEn : string.Empty)

                .Map(dest => dest.JobCareerNameAr, src => src.JobCareer != null ? src.JobCareer.TitleAr : string.Empty)
                .Map(dest => dest.JobCareerNameEn, src => src.JobCareer != null ? src.JobCareer.TitleEn : string.Empty)                
                
                .Map(dest => dest.SentAr, src => src.IsSent == true ? "تم الارسال" : "لم يتم الارسال")
                .Map(dest => dest.SentEn, src => src.IsSent == true ? "Sent" : "Not Sent")

                .Map(dest => dest.FileAttachment, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.FileAttachment) ? fileGetPath + "/" + src.FileAttachment : "")

                .Map(dest => dest.CurrentStateString, src => src.CurrentState == (int)JobApplicationStatus.Filteration ? "مرحلة الفرز" : src.CurrentState == (int)JobApplicationStatus.InitialAcceptance ? "القبول المبدئي" : src.CurrentState == (int)JobApplicationStatus.PassInterview ? "اجتياز المقابلة" : src.CurrentState == (int)JobApplicationStatus.PassTest ? "اجتياز الاختبار" : src.CurrentState == (int)JobApplicationStatus.Accreditation ? "الاعتماد" : "")
                .Map(dest => dest.CurrentStateStringEn, src => src.CurrentState == (int)JobApplicationStatus.Filteration ? "Filteration Stage" : src.CurrentState == (int)JobApplicationStatus.InitialAcceptance ? "Initial Acceptance Stage" : src.CurrentState == (int)JobApplicationStatus.PassInterview ? "Pass Interview Stage" : src.CurrentState == (int)JobApplicationStatus.PassTest ? "Pass Test Stage" : src.CurrentState == (int)JobApplicationStatus.Accreditation ? "Accreditation Stage" : "")

                .Map(dest => dest.StatusString, src => src.IsActive == true ? "مقبول" : "مرفوض")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Acceptable" : "Unacceptable")

                .Map(dest => dest.IsSuccess, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess : null)
                .Map(dest => dest.IsSuccessAr, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? (src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == true ? "ناجح" : src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == false ? "راسب" : !src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate > DateTime.Now ? "لم يتقدم" : src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && !src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt.HasValue ? "غير مكتمل" : !src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate < DateTime.Now ? "تجاوز تاريخ التقديم" : "-") : "-")
                .Map(dest => dest.IsSuccessEn, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? (src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == true ? "Pass" : src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == false ? "Fail" : !src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate > DateTime.Now ? "Not Apply" : src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && !src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt.HasValue ? "Not Complete" : !src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate < DateTime.Now ? "Expire Date" : "-") : "-")

                .Map(dest => dest.Result, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? (src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt != null ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Result.ToString() : "-") : "-")
                
                .Ignore(x=>x.appExamId)
                .Map(dest => dest.AppExamId, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Id : 0)
                .Ignore(x => x.examId)
                .Map(dest => dest.ExamId, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ExamId : 0)

                .Map(dest => dest.HasExam, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? true : false)
                .Map(dest => dest.ExamDone, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt != null : false)

                .Map(dest => dest.ExamNameAr, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Exam != null ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Exam.TitleAr : null : null)
                .Map(dest => dest.ExamNameEn, src => (src.JobApplicationExams != null && src.JobApplicationExams.Any()) ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Exam != null ? src.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Exam.TitleEn : null : null)

                .Map(dest => dest.FromDate, src => src.JobApplicationExams.Where(c => c.JobApplicationId == src.Id).OrderByDescending(x => x.Id).FirstOrDefault() != null ?
                    src.JobApplicationExams.Where(c => c.JobApplicationId == src.Id).OrderByDescending(x => x.Id).FirstOrDefault().FromDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : null)

                .Map(dest => dest.ToDate, src => src.JobApplicationExams.Where(c => c.JobApplicationId == src.Id).OrderByDescending(x => x.Id).FirstOrDefault() != null ?
                    src.JobApplicationExams.Where(c => c.JobApplicationId == src.Id).OrderByDescending(x => x.Id).FirstOrDefault().ToDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : null)
                                
                .Map(dest => dest.UpdateByName, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.ActiveByName, src => src.ActivatedByNavigation != null ? src.ActivatedByNavigation.Name : string.Empty)

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<JobApplication, Models.JobApplication>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<JobApplication, Models.JobApplication>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }

    public class JobApplicationStatuses
    {
        public int? Id { get; set;}
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    public class JobApplicationNotification
    {
        public List<JobApplication> JobApplicationList { get; set; }   
        public string NotificationMessage { get; set; }

    }

    public class ActionState
    {
     //   public bool Filteration { get; set; }
        public bool InitialAcceptance{ get; set; }
        public bool EditExamDates { get; set; } = false;
        public bool PassInterview { get; set; }
        public bool PassTest { get; set; }
        public bool Accreditation { get; set; }
    }



}
