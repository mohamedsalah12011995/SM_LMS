
using RM.Jobs.Dtos;
using static RM.Jobs.Dtos.OperationOutput;
using LinqKit;
using RM.Jobs.UnitOfWorks;
using RM.Core.Services;
using RM.Core.Consts;
using static RM.Core.Helpers.Enums;
using RM.Core.Helpers;
using RM.Core.Integrations;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace RM.Jobs.Services
{
    public class JobApplicationService:BaseService,IJobApplicationService
    {
        private string yaqeenPersonInfoUrl;
        private string SMSUrl;
        private string MilitaryJobsUrl;
        private readonly IUnitOfWork _unitOfWork;
        public JobApplicationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            yaqeenPersonInfoUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("YaqeenPersonInfoUrl").Value;
            SMSUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("SMSUrl").Value;
            MilitaryJobsUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("MilitaryJobsUrl").Value;
        }

        public async Task<OperationOutput> GetJobApplicationsLookups(Dtos.JobApplication RequestedData)
        {
            var gender = _unitOfWork.MajorLookupsType.FindAll(c => c.NameEn.Trim() == LookupsType.Gender, m => m.MajorLookups)
                .Select(x => x.MajorLookups.Select(q => new Dtos.JobLookUp{ Id = q.Id,Name = q.NameAr,NameEn = q.NameEn,})
                .ToList()).FirstOrDefault();

            var qualifications = await _unitOfWork.MajorLookupsType.GetAll().Include(m => m.MajorLookups)
                .Where(c => c.NameEn.Trim() == LookupsType.EducationalQualifications)
                .Select(x => x.MajorLookups.ToList().Adapt<List<Dtos.Qualification>>()).FirstOrDefaultAsync();

            var yearsLookup = Enumerable.Range(1995, DateTime.Now.Year - 1994).ToList();

            var specifications = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Specifications && c.ReferenceId==RequestedData.ReferenceId)
                 .ToList().Adapt<List<Dtos.JobLookUp>>();

            var tags = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Tags && c.ReferenceId == RequestedData.ReferenceId)
                 .ToList().Adapt<List<Dtos.JobLookUp>>();

            var skills = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Skills && c.ReferenceId == RequestedData.ReferenceId)
                 .ToList().Adapt<List<Dtos.JobLookUp>>();


            var grades = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Grade && c.ReferenceId == RequestedData.ReferenceId)
                 .ToList().Adapt<List<Dtos.JobLookUp>>();


            var ExamResultStatus = new List<JobApplicationStatuses>
            {
                new JobApplicationStatuses {Id=(int)Enums.ExamResultStatus.Pass,NameAr="ناجح",NameEn="Pass"},
                new JobApplicationStatuses {Id=(int)Enums.ExamResultStatus.Fail,NameAr="راسب",NameEn="Fail"},
                new JobApplicationStatuses {Id=(int)Enums.ExamResultStatus.NotApply,NameAr="لم يتقدم",NameEn="Not Apply"},

                new JobApplicationStatuses {Id=(int)Enums.ExamResultStatus.NotComplete,NameAr="غير مكتمل",NameEn="Not Complete"},
                new JobApplicationStatuses {Id=(int)Enums.ExamResultStatus.ExpireDate,NameAr="تجاوز تاريخ التقديم",NameEn="Expire Date"}
            };

            var JobApplicationStatuses=new List<JobApplicationStatuses>
            {
                new JobApplicationStatuses {Id=(int)JobApplicationStatus.Filteration,NameAr="مرحلة الفرز",NameEn="Filteration"},
                new JobApplicationStatuses {Id=(int)JobApplicationStatus.InitialAcceptance,NameAr="القبول المبدئي",NameEn="Initial Acceptance"},
                new JobApplicationStatuses {Id=(int)JobApplicationStatus.PassTest,NameAr="اجتياز الاختبار",NameEn="Pass Test"},

                new JobApplicationStatuses {Id=(int)JobApplicationStatus.PassInterview,NameAr="اجتياز المقابلة",NameEn="Pass Interview"},
                new JobApplicationStatuses {Id=(int)JobApplicationStatus.Accreditation,NameAr="الاعتماد",NameEn="Accreditation"}
            };

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.Qualifications, qualifications),
                new OutputDictionary(OperationOutputKeys.yearsLookup, yearsLookup),
                new OutputDictionary(OperationOutputKeys.Specifications, specifications),
                new OutputDictionary(OperationOutputKeys.Tags, tags),
                new OutputDictionary(OperationOutputKeys.Skills, skills),
                new OutputDictionary(OperationOutputKeys.Grades, grades),
                new OutputDictionary(OperationOutputKeys.JobApplicationStatuses, JobApplicationStatuses),
                new OutputDictionary(OperationOutputKeys.Gender, gender),
                new OutputDictionary(OperationOutputKeys.ExamResultStatus, ExamResultStatus));
        }

        public async Task< OperationOutput> GetJobApplicationList(Dtos.JobApplication RequestedData)
        {
            List<int> careerIds = new List<int>();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.CurrentState == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.JobCareerId != null)
            {
                careerIds.Add(RequestedData.JobCareerId.Value);
            }
            else if (RequestedData.ListOfTags.Count>0)
            {
                var tags = RequestedData.ListOfTags.Select(x => x.Name).ToList();
                var tagFilter = PredicateBuilder.New<Models.JobCareer>(true);
                foreach (var tag in tags)
                {
                    tagFilter = tagFilter.Or(g => g.Tags.Contains(tag));
                }
                var careersId = _unitOfWork.JobCareers.GetAll().Where(tagFilter.Compile()).Select(x => x.Id).ToList();

                careerIds.AddRange(careersId);
            }

            var skills = RequestedData.ListOfSkills.Select(x => x.Name).ToList();
            var skillFilter = PredicateBuilder.New<Models.JobApplication>(true);
            foreach (var skill in skills)
            {
                skillFilter = skillFilter.And(g => g.Skills.Contains(skill));
            }

            RequestedData.careerIds = careerIds;

            var result = _unitOfWork.JobApplication.GetAll()
                 .Include(x => x.JobCareer)
                 .Include(x => x.UpdatedByNavigation)
                 .Include(x => x.ActivatedByNavigation)
                 .Include(x => x.Specialist)
                 .Include(x => x.Qualification)
                 .Include(x => x.Grade)
                 .Include(x => x.JobApplicationExams)
                 .ThenInclude(x => x.Exam)
                 .Where(RequestedData.Filteration())
                 .Where(skillFilter.Compile())
                 .Where(x => (RequestedData.ExamResultStatusId != null && x.JobApplicationExams != null) ? x.JobApplicationExams.Any() ?
                  RequestedData.ExamResultStatusId == (int)Enums.ExamResultStatus.Pass ? x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == true :
                  RequestedData.ExamResultStatusId == (int)Enums.ExamResultStatus.Fail ? x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == false :
                  RequestedData.ExamResultStatusId == (int)Enums.ExamResultStatus.NotApply ? !x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate > TransactionDate :
                  RequestedData.ExamResultStatusId == (int)Enums.ExamResultStatus.NotComplete ? x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && !x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt.HasValue :
                  RequestedData.ExamResultStatusId == (int)Enums.ExamResultStatus.ExpireDate ? !x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate < TransactionDate :false : false : true)
                .Where(x => (RequestedData.FromResult != null && x.JobApplicationExams != null) ? x.JobApplicationExams.Any() ? x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Result >= RequestedData.FromResult : false : true)
                .Where(x => (RequestedData.ToResult != null && x.JobApplicationExams != null) ? x.JobApplicationExams.Any() ? x.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Result <= RequestedData.ToResult : false : true)
                .OrderByDescending(x => x.Id).TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.JobApplication>>(Dtos.JobApplication.SelectConfig(FilesGetPath));

            var ActionState = new ActionState();

            if (RequestedData.CurrentState == (int)JobApplicationStatus.Filteration)
                ActionState = new ActionState { InitialAcceptance = true, PassInterview = false, PassTest = false, Accreditation = false };
            else if (RequestedData.CurrentState == (int)JobApplicationStatus.InitialAcceptance)
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = false, PassTest = true, Accreditation = false , EditExamDates = true };
            else if (RequestedData.CurrentState == (int)JobApplicationStatus.PassTest)
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = true, PassTest = false, Accreditation = false };
            else if (RequestedData.CurrentState == (int)JobApplicationStatus.PassInterview)
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = false, PassTest = false, Accreditation = true };
            else
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = false, PassTest = false, Accreditation = false };


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.JobApplicationEntity, resultDto),
                    new OutputDictionary(OperationOutputKeys.ActionState, ActionState),
                    new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }

        public async Task< OperationOutput> GetApplicationDetails(JobApplication RequestedData)
        {
            return await GetApplicationDetails(RequestedData.Id);
        }
        public async Task< OperationOutput> GetApplicationDetails(int? Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var jobApplication = _unitOfWork.JobApplication
                .FindAll(x => x.Id == Id, q => q.Qualification, sp => sp.Specialist,
                ca => ca.JobCareer,exm=>exm.JobApplicationExams,u => u.UpdatedByNavigation,c => c.ActivatedByNavigation).FirstOrDefault();

            if (jobApplication == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            var applicationItem = jobApplication.Adapt<Dtos.JobApplication>(Dtos.JobApplication.SelectConfig(FilesGetPath));

            ActionState ActionState = new ActionState();

            if (applicationItem.CurrentState == (int)JobApplicationStatus.Filteration)
                ActionState = new ActionState { InitialAcceptance = true, PassInterview = false, PassTest = false, Accreditation = false };
            else if (applicationItem.CurrentState == (int)JobApplicationStatus.InitialAcceptance)
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = false, PassTest = true, Accreditation = false, EditExamDates = true };
            else if (applicationItem.CurrentState == (int)JobApplicationStatus.PassTest)
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = true, PassTest = false, Accreditation = false };
            else if (applicationItem.CurrentState == (int)JobApplicationStatus.PassInterview)
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = false, PassTest = false, Accreditation = true };
            else
                ActionState = new ActionState { InitialAcceptance = false, PassInterview = false, PassTest = false, Accreditation = false };
          
            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, applicationItem.ReferenceId, (int)Enums.Entities.Careers, applicationItem.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.JobApplicationEntity, applicationItem),
                    new OutputDictionary(OperationOutputKeys.ActionState, ActionState));
        }


        public async Task<OperationOutput> SaveJobApplicationList(List<JobApplication> RequestedData)
        {
            _unitOfWork.JobApplication.ExecuteUpdate(x => x.IsDeleted != true && RequestedData.Select(v => v.Id).Contains(x.Id),
                sett => sett.SetProperty(x => x.CurrentState , RequestedData.FirstOrDefault().NextState)
                            .SetProperty(x => x.UpdatedBy, RequestOwner.Id)
                            .SetProperty(x => x.UpdatedDate, TransactionDate));

            _unitOfWork.Complete();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task< OperationOutput> SendJobApplicationNotification(JobApplicationNotification RequestedData)
        {
            var smsBody = new SmsBody();
            smsBody.messageBody = RequestedData.NotificationMessage;

           var DbItem = _unitOfWork.JobApplication.GetAll().Where(x => x.IsDeleted != true
             && RequestedData.JobApplicationList.Select(v => v.Id).Contains(x.Id)).ToList();
            foreach (var item in DbItem)
            {
               // var msg = RequestedData.NotificationMessage.Replace("[FullName]", item.FullName).Replace("[Code]", item.Code);
               // to add sms api
                if (item.Phone.StartsWith("05"))
                    smsBody.phonesNumber.Add(item.Phone);
            }
            if (smsBody.phonesNumber.Any())
            {
                var smsResult = await InvokeService<SmsIntegrationData>.Invoke(SMSUrl, smsBody);
                if (smsResult.Body != null && smsResult.Body.smsResponse.success)
                { 
                    UpdateJobApplications(DbItem);
                    return GetOperationOutput(header: Enums.ServiceMessages.SentEmailSuccessfully);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        }

        void UpdateJobApplications(List<Models.JobApplication> applications)
        {
            applications.ForEach(application =>
            {
                if (application.Phone.StartsWith("05"))
                    application.IsSent = true;
            });
            _unitOfWork.Complete();
        }
      
        public async Task<OperationOutput> AddJobApplication(JobApplication RequestdData)
        {
            Models.JobApplication DbItem = new Models.JobApplication();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var adv = _unitOfWork.JobCareers.GetAll().Include(x => x.JobAdvertisement).Where(x => x.Id == RequestdData.JobCareerId).FirstOrDefault();

            if (adv != null)
            {
                if(adv.JobAdvertisement.StartDate.Date > TransactionDate.Date)
                    return GetOperationOutput(header: Enums.ServiceMessages.JobClosed);

                if (  adv.JobAdvertisement.EndDate.Date < TransactionDate.Date)
                    return GetOperationOutput(header: Enums.ServiceMessages.ExpireApplyJob);
            }

            var application = _unitOfWork.JobApplication.GetAll().FirstOrDefault(j => j.JobCareerId == RequestdData.JobCareerId && j.IdCardNumber == RequestdData.IdCardNumber);
            if (application != null)
                return GetOperationOutput(header: Enums.ServiceMessages.SubmittedJobApplication);


            var model = new {
              idNumber= RequestdData.IdCardNumber,
              birthOfDate= Dates.ChangeDateFormat(RequestdData.BirthDay).ToString("dd-MM-yyyy"),
               RequestdData.idTypeCode,
               RequestdData.birthOfDateIsHijri
          };
        var personData = await InvokeService<IntegrationData>.Invoke(yaqeenPersonInfoUrl, model);
            if (personData.Body==null || personData.Body.UserInformation == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowedApplyJob);

            if (!string.IsNullOrEmpty(RequestdData.currentLang))
                RequestdData.FullName = RequestdData.currentLang == "ar" ? personData.Body.UserInformation.arabicName : personData.Body.UserInformation.englishName;
          
            if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestdData.FileAttachmentBase64))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            if (Files.GetFileExtension(RequestdData.FileAttachmentBase64) != "pdf")
                return GetOperationOutput(header: Enums.ServiceMessages.FileExtentionError);


            if (Files.GetBase64FileSizeMb(RequestdData.FileAttachmentBase64) > FileSizeMb)
                return GetOperationOutput(header: Enums.ServiceMessages.FileSizeError);


            var careerLimit = _unitOfWork.JobCareers.Find(x => x.Id == RequestdData.JobCareerId).MaxLimit;
            var appCareerCount = _unitOfWork.JobApplication.Count(x=>x.Id== RequestdData.JobCareerId);

            if (careerLimit > appCareerCount)
            {
                DbItem.CreatedDate = TransactionDate;
                DbItem.Code = DateTime.Now.ToString("yyyyMMdd") + Strings.RandomDigits(4).ToString();
                DbItem.IsDeleted = false;
                DbItem.IsActive = true;

                DbItem.ReferenceId = RequestdData.ReferenceId;
                DbItem.FullName = personData.Body.UserInformation.arabicName;

                DbItem.Phone = RequestdData.Phone;
                DbItem.BirthDay = Dates.ChangeDateFormat( RequestdData.BirthDay);
                DbItem.CurrentState = (int)JobApplicationStatus.Filteration;
                DbItem.Email = RequestdData.Email;
                DbItem.Gendar = personData.Body.UserInformation.gender=="M"?1:2;
                DbItem.GradeYear = RequestdData.GradeYear;
                DbItem.GradeId = RequestdData.GradeId;
                DbItem.IdCardNumber = RequestdData.IdCardNumber;
                DbItem.QualificationId = RequestdData.QualificationId;
                DbItem.SpecialistId = RequestdData.SpecialistId;
                DbItem.JobCareerId = RequestdData.JobCareerId;
                DbItem.Skills = Strings.ConvertListToString(RequestdData.ListOfSkills.Select(x => x.Name).ToList());
                DbItem.TrainingCourses= Strings.ConvertListToString(RequestdData.ListOfTrainingCourses.Select(x=>x).ToList());
                DbItem.UpdatedDate = TransactionDate;
                DbItem.GenderIntegration = personData.Body.UserInformation.gender;

                var fileName = Strings.GenerateGUID() + ".pdf";
                Files.SaveBase64FileToServer(fileName, RequestdData.FileAttachmentBase64, DocumentsSavePath);
                DbItem.FileAttachment = fileName;
                

                _unitOfWork.JobApplication.Add(DbItem);
                _unitOfWork.Complete();

                return await GetApplicationDetails(DbItem.Id);
            }
            else
                return GetOperationOutput(header: Enums.ServiceMessages.MaxLimitReached);

        }

        public async Task<OperationOutput> RejectJobAppList(List<JobApplication> RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            _unitOfWork.JobApplication.ExecuteUpdate(x => x.IsDeleted != true && RequestedData.Select(v => v.Id).Contains(x.Id),
                sett => sett.SetProperty(x => x.IsActive, false)
                            .SetProperty(x=>x.ActivatedBy,RequestOwner.Id)
                            .SetProperty(x=>x.ActivatedDate, TransactionDate)
                            .SetProperty(x=>x.UpdatedBy, RequestOwner.Id)
                            .SetProperty(x => x.UpdatedDate, TransactionDate));

            _unitOfWork.Complete();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> ReAgreeJobAppList(List<JobApplication> RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            _unitOfWork.JobApplication.ExecuteUpdate(x => x.IsDeleted != true && RequestedData.Select(v => v.Id).Contains(x.Id),
                sett => sett.SetProperty(x => x.IsActive, true)
                            .SetProperty(x => x.ActivatedBy, RequestOwner.Id)
                            .SetProperty(x => x.ActivatedDate, TransactionDate)
                            .SetProperty(x => x.UpdatedBy, RequestOwner.Id)
                            .SetProperty(x => x.UpdatedDate, TransactionDate));

            _unitOfWork.Complete();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeleteJobAppList(List<JobApplication> RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            _unitOfWork.JobApplication.ExecuteUpdate(x => x.IsDeleted != true && RequestedData.Select(v => v.Id).Contains(x.Id),
                sett => sett.SetProperty(x => x.IsDeleted, true)
                            .SetProperty(x => x.DeletedBy, RequestOwner.Id)
                            .SetProperty(x => x.DeletedDate, TransactionDate));

            _unitOfWork.Complete();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> ModelActions(JobApplication RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.JobApplication.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;
            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
            {
                DbItem.DeletedBy = RequestOwner.Id;
                DbItem.DeletedDate = TransactionDate;
            }
            if (RequestedData.IsActive.HasValue && RequestedData.IsActive.Value == true)
            {
                DbItem.ActivatedBy = RequestOwner.Id;
                DbItem.ActivatedDate = TransactionDate;
            }
            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.JobApplication.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task< OperationOutput> QueryJobApplication(JobApplication RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.Code) || string.IsNullOrEmpty(RequestedData.IdCardNumber))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var jobApplication = _unitOfWork.JobApplication.Find(x => x.Code == RequestedData.Code && x.IdCardNumber==RequestedData.IdCardNumber, q => q.Qualification, sp => sp.Specialist, ca => ca.JobCareer);
            if (jobApplication == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            return await GetApplicationDetails(jobApplication.Id);
        }

        // For Site
        public async Task<OperationOutput> QueryApplication(JobApplication RequestedData)
        {
            JobApplication applicationItem = new JobApplication();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.Code) && string.IsNullOrEmpty(RequestedData.IdCardNumber))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var jobApplication = await _unitOfWork.JobApplication.GetAll().Include(c => c.JobCareer).Include(exm => exm.JobApplicationExams)
                               .Where(x => (!string.IsNullOrEmpty(RequestedData.Code) ? x.Code == RequestedData.Code : true)
                               && (!string.IsNullOrEmpty(RequestedData.IdCardNumber) ? x.IdCardNumber == RequestedData.IdCardNumber : true)).FirstOrDefaultAsync();

            if (jobApplication == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            var jobReference = _unitOfWork.JobAdvertisement.Find(x => x.Id == jobApplication.JobCareer.JobAdvertisementId);

            if (jobReference.ReferenceId == RequestedData.ReferenceId)
            {
                applicationItem.Id = jobApplication.Id;
                applicationItem.Code = jobApplication.Code;
                applicationItem.FullName = jobApplication.FullName;
                applicationItem.CurrentState = jobApplication.CurrentState;
                applicationItem.IdCardNumber = jobApplication.IdCardNumber;
                applicationItem.IsActive = jobApplication.IsActive;
                applicationItem.IsDeleted = jobApplication.IsDeleted;
                applicationItem.CreatedDate = jobApplication.CreatedDate;
                applicationItem.CreatedDateString = jobApplication.CreatedDate.Value.ToString("yyyy-MM-dd");
                applicationItem.StatusString = jobApplication.IsActive == true ? "مقبول" : "مرفوض";
                applicationItem.StatusStringEn = jobApplication.IsActive == true ? "Acceptable" : "Unacceptable";

                applicationItem.CurrentStateString = jobApplication.JobCareer.IsNoticeBeneficiaries == false ? "تحت التنفيذ" :
                jobApplication.CurrentState == (int)JobApplicationStatus.Filteration ? "مرحلة الفرز" : jobApplication.CurrentState == (int)JobApplicationStatus.InitialAcceptance ? "القبول المبدئي" : jobApplication.CurrentState == (int)JobApplicationStatus.PassInterview ? "اجتياز المقابلة" : jobApplication.CurrentState == (int)JobApplicationStatus.PassTest ? "اجتياز الاختبار" : jobApplication.CurrentState == (int)JobApplicationStatus.Accreditation ? "الاعتماد" : "";

                applicationItem.CurrentStateStringEn = jobApplication.JobCareer.IsNoticeBeneficiaries == false ? "Under Process"
                   : jobApplication.CurrentState == (int)JobApplicationStatus.Filteration ? "Filteration Stage" : jobApplication.CurrentState == (int)JobApplicationStatus.InitialAcceptance ? "Initial Acceptance Stage" : jobApplication.CurrentState == (int)JobApplicationStatus.PassInterview ? "Pass Interview Stage" : jobApplication.CurrentState == (int)JobApplicationStatus.PassTest ? "Pass Test Stage" : jobApplication.CurrentState == (int)JobApplicationStatus.Accreditation ? "Accreditation Stage" : "";

                applicationItem.JobCareerNameAr = jobApplication.JobCareer != null ? jobApplication.JobCareer.TitleAr : string.Empty;
                applicationItem.JobCareerNameEn = jobApplication.JobCareer != null ? jobApplication.JobCareer.TitleEn : string.Empty;

                applicationItem.IsSuccess = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess : null;
                applicationItem.IsSuccessAr = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? (jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == true ? "ناجح" : jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == false ? "راسب" : !jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate > TransactionDate ? "لم يتقدم" : jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && !jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt.HasValue ? "غير مكتمل" : !jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate < TransactionDate ? "تجاوز تاريخ التقديم" : "-") : "-";
                applicationItem.IsSuccessEn = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? (jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == true ? "Pass" : jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().IsSuccess == false ? "Fail" : !jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate > TransactionDate ? "Not Apply" : jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && !jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt.HasValue ? "Not Complete" : !jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().StartAt.HasValue && jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate < TransactionDate ? "Expire Date" : "-") : "-";

                applicationItem.Result = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? (jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt != null ? jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Result.ToString() : "-") : "-";

                applicationItem.AppExamId = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().Id : null;
                applicationItem.ExamId = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ExamId : null;

                applicationItem.HasExam = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().FromDate <= TransactionDate && jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().ToDate >= TransactionDate : false;
                applicationItem.ExamDone = (jobApplication.JobApplicationExams != null && jobApplication.JobApplicationExams.Any()) ? jobApplication.JobApplicationExams.OrderByDescending(x => x.Id).FirstOrDefault().EndAt != null : false;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                      new OutputDictionary(OperationOutputKeys.JobApplicationEntity, applicationItem));
        }

        public async Task <OperationOutput> GetMilitaryJobApplicationInfo(JobApplication RequestedData)
        {
            var applicationInfo = await InvokeService<MilitaryJobsIntegration>.Invoke(MilitaryJobsUrl, RequestedData);
            if (applicationInfo.Body == null ||applicationInfo.Body.MillitaryApplicationInfo==null)
                return GetOperationOutput(header: Enums.ServiceMessages.InValidData);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.MilitaryApplicationEntity, applicationInfo));
        }

        public async Task<OperationOutput> SearchApplication(JobApplication RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.Code) && string.IsNullOrEmpty(RequestedData.IdCardNumber))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var jobApplication = _unitOfWork.JobApplication.GetAll().Include(c => c.JobCareer)
                               .Where(x => (!string.IsNullOrEmpty(RequestedData.Code) ? x.Code == RequestedData.Code : true)
                               && (!string.IsNullOrEmpty(RequestedData.IdCardNumber) ? x.IdCardNumber == RequestedData.IdCardNumber : true)).FirstOrDefault();

            if (jobApplication == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);


            var jobReference = _unitOfWork.JobAdvertisement.Find(x => x.Id == jobApplication.JobCareer.JobAdvertisementId);

            if (jobReference.ReferenceId == RequestedData.ReferenceId)
                return await GetApplicationDetails(jobApplication.Id);
            else
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);
        }
    }
}
