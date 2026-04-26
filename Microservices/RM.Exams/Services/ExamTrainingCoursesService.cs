using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Core.Dtos;
using RabbitMQ.Core.Services;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Exams.Dtos;
using RM.Exams.Dtos.Exams.Certifications;
using RM.Exams.Dtos.ExamTrainingCourses.AssignToExam;
using RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee;
using RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment;
using RM.Exams.Dtos.ExamTrainingCourses.CP_Reports;
using RM.Exams.Dtos.ExamTrainingCourses.PortalDtos;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule;
using RM.Exams.Records.Emails;
using RM.Exams.Records.Exam.Certifications;
using RM.Exams.Records.ExamTrainingCourses;
using RM.Exams.Records.ExamTrainingCourses.Portal;
using RM.Exams.Records.ExamTrainingCourses.Reports;
using RM.Exams.UnitOfWorks;
using RM.Models;
using static RM.Exams.Dtos.OperationOutput;

namespace RM.Exams.Services
{
    public class ExamTrainingCoursesService : BaseService, IExamTrainingCoursesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private int DepartmentRefernceId = 0;
        private int GradeTypeId = 0;
        public IRabbitMqService _rabbitMqService { get; }
        private readonly RabbitMQConfiguration _configuration;

        public ExamTrainingCoursesService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
              IOptions<RabbitMQConfiguration> options
            , IRabbitMqService rabbitMqService)
        : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            DepartmentRefernceId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("Departments").Value!);
            GradeTypeId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("GradeTypeId").Value!);
            // TransactionDate = Dates.GetCurrentDateTimeInCulture();
            _rabbitMqService = rabbitMqService;
            _configuration = options.Value;
        }


        #region TrainigCourses

        public async Task<OperationOutput> GetTrainigCourseLookups()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var types = await _unitOfWork.TrainingCourseType.GetAll().AsNoTracking().ToListAsync();

            var typesDto = types.Adapt<List<TrainingCourseTypeDto>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                      new OutputDictionary(OperationOutputKeys.CourseTypes, typesDto));

        }

        public async Task<OperationOutput> GetTrainigCourseList(TrainigCourseListDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courses = await _unitOfWork.TrainingCourses.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending,
                 c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var coursesDto = courses.Data.Adapt<List<TrainigCourseOutputListDto>>(TrainigCourseOutputListDto.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Courses, coursesDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, courses.Pagination));
        }

        public async Task<OperationOutput> SaveTrainigCourse(TrainigCourseInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.TrainingCourse course;

            if (!RequestedData.Id.HasValue)
            {
                course = new();
                RequestedData.Adapt(course, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.TrainingCourses.Add(course);
            }
            else
            {
                course = await _unitOfWork.TrainingCourses.GetAll().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);
                if (course == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(course, RequestedData.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.TrainingCourses.Update(course);
            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = course.Id;
            return await GetTrainigCourseDetail(new TrainigCourseById { Id = RequestedData.Id });
        }

        public async Task<OperationOutput> GetTrainigCourseDetail(TrainigCourseById RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var course = await _unitOfWork.TrainingCourses.GetAll().AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted == false);

            if (course == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            TrainigCourseOutputDetailDto courseDto = new();

            courseDto = course.Adapt(courseDto, TrainigCourseOutputDetailDto.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CourseEntity, courseDto));

        }

        public async Task<OperationOutput> ActivationTrainigCourse(TrainigCourseActivation RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            await _unitOfWork.TrainingCourses.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                    sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive)
                    .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                    .SetProperty(y => y.UpdatedDate, TransactionDate));


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeletionTrainigCourse(TrainigCourseById RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == RequestedData.Id && c.EndDate.Date >= TransactionDate.Date
                 && c.IsDeleted == false && c.IsActive == true);


            if (courseSchedule is null)
            {
                await _unitOfWork.TrainingCourses.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, true)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

            }

            return GetOperationOutput(header: Enums.ServiceMessages.NotAllowDeletionCourse);

        }

        #endregion

        #region TrainingCourseSchedule

        public async Task<OperationOutput> GetCourseScheduleLookups(CourseScheduleLookup RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courses = await _unitOfWork.TrainingCourses
                .GetAll(c => c.ReferenceId == RequestedData.ReferenceId
                     && c.IsDeleted == false && c.IsActive == true).AsNoTracking()
                     .Select(c => new CoursesLookup
                     {
                         Id = c.Id,
                         TitleAr = c.TitleAr,
                         TitleEn = c.TitleEn,
                         HasCertificate = c.HasCertificate
                     }).ToListAsync();


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                      new OutputDictionary(OperationOutputKeys.Courses, courses));
        }

        public async Task<OperationOutput> GetTrainingCourseScheduleList(CourseScheduleListInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.FindAllByPaginationAsync(RequestedData.Filteration(TransactionDate), RequestedData.Pagination, DefaultPaginationCount, x => x.StartDate, OrderBy.Ascending,
                 c => c.CreatedByNavigation, u => u.UpdatedByNavigation, r => r.DepartmentReference, e => e.Exam);

            var courseSchedulesDto = courseSchedule.Data.Adapt<List<CourseScheduelListOutput>>(CourseScheduelListOutput.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Courses, courseSchedulesDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, courseSchedule.Pagination));
        }


        public async Task<OperationOutput> GetLookupsForAddCourseSchedule(LookupCourseScheduleForAdd RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<ExamLookup> exams = await GetExamLookup(RequestedData);
            List<ThemesLookup> certificationThemes = await GetCertificationThemeLookup();
            List<ReferencesTree> departmentReferencesTree = await GetDepartmentReferences();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.ExamsList, exams),
                     new OutputDictionary(OperationOutputKeys.DepartmentReferences, departmentReferencesTree),
                     new OutputDictionary(OperationOutputKeys.CertificationThemes, certificationThemes));
        }

        #region HELPER METHOD >> GetLookupsForAddCourseSchedule

        private async Task<List<ReferencesTree>> GetDepartmentReferences()
        {
            var departmentReferences = await _unitOfWork.References
                .GetAll(x => x.IsDeleted != true && x.ParentId != null).AsNoTracking().ToListAsync();

            var departmentReferencesTree = GenerateReferenceTree(departmentReferences, DepartmentRefernceId).Adapt<List<ReferencesTree>>(ReferencesTree.SelectConfig());
            return departmentReferencesTree;
        }

        public IEnumerable<ReferencesTree> GenerateReferenceTree(List<Models.Reference> references, int? parentId)
        {
            foreach (var r in references.Where(r => r.ParentId == parentId))
            {
                var t = r.Adapt<ReferencesTree>(ReferencesTree.SelectConfig());
                t.Children = GenerateReferenceTree(references, r.Id);
                yield return t;
            }
        }
        private async Task<List<ExamLookup>> GetExamLookup(LookupCourseScheduleForAdd RequestedData)
        {
            return await _unitOfWork.Exams.GetAll(c => c.IsDeleted == false && c.IsActive == true
                     && c.ReferenceId == RequestedData.ReferenceId
                     && c.EntityId == RequestedData.ExamEntityId)
                  .AsNoTrackingWithIdentityResolution()
                  .Include(c => c.Certificate)
                  .Select(c => new ExamLookup
                  {
                      Id = c.Id,
                      TitleAr = c.TitleAr,
                      TitleEn = c.TitleEn,
                      CertificateId = c.CertificateId,
                      CertificateTitleAr = c.Certificate != null ? c.Certificate.TitleAr : string.Empty,
                      CertificateTitleEn = c.Certificate != null ? c.Certificate.TitleEn : string.Empty
                  }).ToListAsync();
        }
        private async Task<List<ThemesLookup>> GetCertificationThemeLookup()
        {
            return await _unitOfWork.CertificateThemes.GetAll(c => c.IsActive)
                  .AsNoTracking().Select(c => new ThemesLookup
                  {
                      Id = c.Id,
                      TitleAr = c.TitleAr,
                      TitleEn = c.TitleEn,
                      ThumbnailPicture = $"{ImagesGetPath}/{c.ThumbnailPicture}"
                  }).ToListAsync();
        }

        #endregion


        public async Task<OperationOutput> SaveTrainingCourseSchedule(TrainingCourseScheduleInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var isScheduleOverlapping = await IsScheduleOverlapping(RequestedData);

            if (isScheduleOverlapping)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.ScheduleOverlapping);
            }

            var examData = await _unitOfWork.Exams.GetByIdAsync(RequestedData.ExamId.Value);

            if (examData != null && examData.CertificateId.HasValue && !RequestedData.CertificateThemeId.HasValue)
            {
                var theme = await _unitOfWork.CertificateThemes.GetAll().AsNoTracking().FirstOrDefaultAsync();
                if (theme != null)
                    RequestedData.CertificateThemeId = theme.Id;
            }

            Models.TrainingCourseSchedule courseSchedule;

            if (!RequestedData.Id.HasValue)
            {

                courseSchedule = new();
                RequestedData.Adapt(courseSchedule, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.TrainingCourseSchedule.Add(courseSchedule);
            }
            else
            {
                courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

                if (courseSchedule == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(courseSchedule, RequestedData.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.TrainingCourseSchedule.Update(courseSchedule);
            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = courseSchedule.Id;
            return await GetTrainigCourseScheduleDetail(new TrainigCourseScheduleById { Id = RequestedData.Id });


        }


        #region HELPER METHOD SaveTrainingCourseSchedule

        public async Task<bool> IsScheduleOverlapping(TrainingCourseScheduleInputDto RequestedData)
        {
            var overlappingSchedules = await _unitOfWork.TrainingCourseSchedule
                .GetAll(c => c.Id != RequestedData.Id && c.CourseId == RequestedData.CourseId
                     && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId &&
                              ((c.StartDate <= RequestedData.StartDate && c.EndDate >= RequestedData.StartDate) ||
                              (c.StartDate <= RequestedData.EndDate && c.EndDate >= RequestedData.EndDate) ||
                              (c.StartDate >= RequestedData.StartDate && c.EndDate <= RequestedData.EndDate)))
                .AsNoTracking()
                .ToListAsync();

            return overlappingSchedules.Any();
        }

        #endregion


        public async Task<OperationOutput> GetTrainigCourseScheduleDetail(TrainigCourseScheduleById RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.Certificate)
                .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted == false);

            if (courseSchedule == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            TrainingCourseScheduleOutputDto courseScheduleDto = new();

            courseScheduleDto = courseSchedule.Adapt(courseScheduleDto, TrainingCourseScheduleOutputDto.SelectConfig(TransactionDate));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CourseScheduleEntity, courseScheduleDto));

        }

        public async Task<OperationOutput> ActivationTrainigCourseSchedule(TrainigCourseScheduleActivation RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            await _unitOfWork.TrainingCourseSchedule.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                    sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive)
                    .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                    .SetProperty(y => y.UpdatedDate, TransactionDate));


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeletionTrainigCourseSchedule(TrainigCourseScheduleById RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == RequestedData.Id);

            if (courseSchedule.StartDate.Date <= TransactionDate.Date
             && courseSchedule.EndDate.Date >= TransactionDate.Date
             && courseSchedule.IsDeleted == false && courseSchedule.IsActive == true && courseSchedule.IsClosed == false)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.NotAlloweDeletionScheduleRecord);
            }

            if (courseSchedule != null)
            {
                await _unitOfWork.TrainingCourseSchedule.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, true)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        #endregion

        #region Assign trainees for training courses

        public async Task<OperationOutput> GetLookupsForAssignTrainees()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<ReferencesTree> departmentReferencesTree = await GetDepartmentReferences();

            var types = await _unitOfWork.TrainingCourseType.GetAll().AsNoTracking().ToListAsync();

            var typesDto = types.Adapt<List<TrainingCourseTypeDto>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.CourseTypes, typesDto),
                     new OutputDictionary(OperationOutputKeys.DepartmentReferences, departmentReferencesTree));
        }

        public async Task<OperationOutput> GetActivationCourseList(ActivationCorsesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courses = await _unitOfWork.TrainingCourseSchedule
                .GetAll(c => c.ReferenceId == RequestedData.ReferenceId && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                    && c.Course.Type == RequestedData.Type
                    && c.IsDeleted == false && c.IsActive == true && c.IsClosed == false
                    && c.EndDate.Date > TransactionDate.Date).AsNoTrackingWithIdentityResolution()
                .Include(c => c.Course)
                .GroupBy(c => c.CourseId)
                .Select(c => new CoursesLookup
                {
                    Id = c.Key,
                    TitleAr = c.Select(c => c.Course.TitleAr).First(),
                    TitleEn = c.Select(c => c.Course.TitleEn).First(),
                })
                .ToListAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.Courses, courses));
        }

        public async Task<OperationOutput> GetInternalCourseScheduleList(CorsesScheduleByCourseId RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var scheduleList = await GetScheduleList(RequestedData);

            if (scheduleList.Count > 0)
            {
                foreach (var item in scheduleList)
                {
                    item.Trainees = await GetInternTranieeUsers(item.Id.Value, RequestedData.DepartmentReferenceId.Value);
                }

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.CourseScheduleEntity, scheduleList));

        }


        public async Task<List<InternTraineeUsers>> PaginateCourseSchedulInternUsers(PaginateInternalUsers RequestedData)
        {
            var numberOfRecord = await _unitOfWork.User.GetAll().AsNoTracking().CountAsync(u => u.ReferenceId == RequestedData.DepartmentReferenceId && !u.IsDeleted && !u.IsBlocked);
            var internTranieeUsers = await (from user in _unitOfWork.User.GetAll()
                                            join trainee in _unitOfWork.InternalCourseTrainees.GetAll(c => c.IsDeleted == false)
                                            on user.Id equals trainee.TraineeId into userTrainees
                                            where user.ReferenceId == RequestedData.DepartmentReferenceId && !user.IsDeleted && !user.IsBlocked
                                            from trainee in userTrainees.DefaultIfEmpty()
                                            select new InternTraineeUsers
                                            {
                                                UserId = user.Id,
                                                Name = user.Name,
                                                Phone = user.Phone,
                                                IdCardNumber = user.IdCardNumber,
                                                EmployeeID = user.EmployeeId,
                                                Email = user.Email,
                                                IsSelected = trainee != null && trainee.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId
                                            })
                                        .Skip((RequestedData.Pagination.CurrentPageIndex.Value - 1) * (RequestedData.Pagination.RecordPerPage.HasValue ? RequestedData.Pagination.RecordPerPage.Value : DefaultPaginationCount))
                                        .Take((RequestedData.Pagination.RecordPerPage.HasValue ? RequestedData.Pagination.RecordPerPage.Value : DefaultPaginationCount))
                                        .ToListAsync();

            RequestedData.Pagination.TotalPagesCount = Math.Ceiling((float)numberOfRecord / (RequestedData.Pagination.RecordPerPage.HasValue ? (float)RequestedData.Pagination.RecordPerPage.Value : DefaultPaginationCount));
            RequestedData.Pagination.TotalItemsCount = numberOfRecord;

            return internTranieeUsers;
        }

        #region  HELPER METHODS GetInternalCourseScheduleList

        private async Task<List<InternCorsesScheduleOutput>> GetScheduleList(CorsesScheduleByCourseId RequestedData)
        {
            return await _unitOfWork.TrainingCourseSchedule
                  .GetAll(c => c.CourseId == RequestedData.CourseId
                  && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                  && c.ReferenceId == RequestedData.ReferenceId
                  && c.IsDeleted == false
                  && c.IsActive == true && c.IsClosed == false && c.EndDate.Date > TransactionDate.Date)
                  .AsNoTracking()
                  .OrderBy(c => c.StartDate)
                  .Select(c => new InternCorsesScheduleOutput
                  {
                      Id = c.Id,
                      StartDateString = c.StartDate.ToString("yyyy-MM-dd"),
                      EndDateString = c.EndDate.ToString("yyyy-MM-dd"),
                  })
                  .ToListAsync();
        }

        private async Task<List<InternTraineeUsers>> GetInternTranieeUsers(int courseScheduleId, int departmentReferenceId)
        {
            var internTranieeUsers = await (from user in _unitOfWork.User.GetAll()
                                            join trainee in _unitOfWork.InternalCourseTrainees.GetAll(c => c.IsDeleted == false && c.TrainingCourseScheduleId == courseScheduleId)
                                            on user.Id equals trainee.TraineeId into userTrainees
                                            where user.ReferenceId == departmentReferenceId && !user.IsDeleted && !user.IsBlocked

                                            from trainee in userTrainees.DefaultIfEmpty()
                                            select new InternTraineeUsers
                                            {
                                                UserId = user.Id,
                                                Name = user.Name,
                                                Phone = user.Phone,
                                                IdCardNumber = user.IdCardNumber,
                                                EmployeeID = user.EmployeeId,
                                                Email = user.Email,
                                                IsSelected = trainee != null && trainee.TrainingCourseScheduleId == courseScheduleId
                                            })

                                        .ToListAsync();
            return internTranieeUsers;
        }

        #endregion

        public async Task<OperationOutput> GetExternalCourseScheduleList(CorsesScheduleByCourseId RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var scheduleList = await GetExternalCourseSchedule(RequestedData);

            if (scheduleList.Count > 0)
            {
                foreach (var item in scheduleList)
                {
                    item.Trainees = await GetExternalTraniee(item.Id.Value);
                }

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.CourseScheduleEntity, scheduleList));

        }

        public async Task<List<ExternalTraineeOutput>> PaginateCourseSchedulExtenalTraniee(PaginateInternalUsers RequestedData)
        {
            var numberOfRecord = await _unitOfWork.ExternalCourseTraniees.GetAll().AsNoTracking().CountAsync(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId && c.IsDeleted != true);

            var externalTrainee = await _unitOfWork.ExternalCourseTraniees
                .GetAll(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId && c.IsDeleted != true).AsNoTrackingWithIdentityResolution()
                .Include(c => c.Grade)
                .Skip((RequestedData.Pagination.CurrentPageIndex.Value - 1) * (RequestedData.Pagination.RecordPerPage.HasValue ? RequestedData.Pagination.RecordPerPage.Value : DefaultPaginationCount))
                .Take((RequestedData.Pagination.RecordPerPage.HasValue ? RequestedData.Pagination.RecordPerPage.Value : DefaultPaginationCount))
                .ToListAsync();

            var externalTraineeDto = externalTrainee.Adapt<List<ExternalTraineeOutput>>(ExternalTraineeOutput.SelectConfig());

            RequestedData.Pagination.TotalPagesCount = Math.Ceiling((float)numberOfRecord / (RequestedData.Pagination.RecordPerPage.HasValue ? (float)RequestedData.Pagination.RecordPerPage.Value : DefaultPaginationCount));
            RequestedData.Pagination.TotalItemsCount = numberOfRecord;


            return externalTraineeDto;
        }


        #region  HELPER METHODS GetExternalCourseScheduleList

        private async Task<List<ExternalCorsesScheduleOutput>> GetExternalCourseSchedule(CorsesScheduleByCourseId RequestedData)
        {
            return await _unitOfWork.TrainingCourseSchedule
                  .GetAll(c => c.CourseId == RequestedData.CourseId
                  && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                  && c.ReferenceId == RequestedData.ReferenceId
                  && c.IsDeleted == false
                  && c.IsActive == true && c.IsClosed == false && c.EndDate.Date > TransactionDate.Date)
                  .AsNoTracking()
                  .OrderBy(c => c.StartDate)
                  .Select(c => new ExternalCorsesScheduleOutput
                  {
                      Id = c.Id,
                      StartDateString = c.StartDate.ToString("yyyy-MM-dd"),
                      EndDateString = c.EndDate.ToString("yyyy-MM-dd"),
                  })
                  .ToListAsync();
        }

        private async Task<List<ExternalTraineeOutput>> GetExternalTraniee(int courseScheduleId)
        {
            var externalTrainee = await _unitOfWork.ExternalCourseTraniees
                .GetAll(c => c.TrainingCourseScheduleId == courseScheduleId && c.IsDeleted != true).AsNoTrackingWithIdentityResolution()
                .Include(c => c.Grade)
                .ToListAsync();

            var externalTraineeDto = externalTrainee.Adapt<List<ExternalTraineeOutput>>(ExternalTraineeOutput.SelectConfig());
            return externalTraineeDto;
        }


        #endregion

        public async Task<OperationOutput> AssignInternTrainees(AssignInternalTraineesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Trainees.Count == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var scheduleTrainees = await _unitOfWork.InternalCourseTrainees
                     .GetAll(c => c.IsDeleted == false && c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId).ToListAsync();

            if (scheduleTrainees.Count > 0)
            {
                var removedTrainees = scheduleTrainees.Where(x => !RequestedData.Trainees.Select(v => v.TraineeId ?? 0).Contains(x.TraineeId.Value)).ToList();
                if (removedTrainees.Count > 0)
                {
                    foreach (var user in removedTrainees)
                    {
                        var tranieeExam = await _unitOfWork.InternalCourseExams.GetAll()
                            .FirstOrDefaultAsync(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId && c.InternalCourseTraineeId == user.TraineeId);
                        if (tranieeExam != null)
                        {
                            _unitOfWork.InternalCourseExams.Delete(tranieeExam);
                        }
                        user.IsDeleted = true;
                        _unitOfWork.InternalCourseTrainees.Update(user);
                    }
                }
            }
            var newTrainees = RequestedData.Trainees.Where(x => !scheduleTrainees.Select(v => v.TraineeId).Contains(x.TraineeId ?? 0)).ToList();

            var internalCourseTraineesList = new List<Models.InternalCourseTrainees>();

            foreach (var user in newTrainees)
            {
                Models.InternalCourseTrainees internalCourseTrainees = new();
                RequestedData.Adapt(internalCourseTrainees, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));

                MapTranieeData(internalCourseTrainees, user);
                internalCourseTraineesList.Add(internalCourseTrainees);
            }

            await _unitOfWork.InternalCourseTrainees.BulkInsertAsync(internalCourseTraineesList);

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }


        #region HELPER METHOD AssignInternTrainees
        private static void MapTranieeData(InternalCourseTrainees internalCourseTrainees, InternTrainees user)
        {
            internalCourseTrainees.TraineeId = user.TraineeId;
            internalCourseTrainees.IdCardNumber = user.IdCardNumber;
            internalCourseTrainees.TraineeName = user.Name;
            internalCourseTrainees.Phone = user.Phone;
            internalCourseTrainees.Email = user.Email;
            internalCourseTrainees.EmployeeID = user.EmployeeID;
        }


        #endregion

        public async Task<OperationOutput> AssignExternalTrainees(AssignExternalTraineesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Trainees.Count == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            var scheduleTrainees = await _unitOfWork.ExternalCourseTraniees
                     .GetAll(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId && c.IsDeleted != true).ToListAsync();

            if (scheduleTrainees.Count > 0)
            {
                var removedTrainees = scheduleTrainees.Where(x => !RequestedData.Trainees.Select(v => v.Id ?? 0).Contains(x.Id)).ToList();
                if (removedTrainees.Count > 0)
                {
                    foreach (var trainee in removedTrainees)
                    {
                        var tranieeExam = await _unitOfWork.ExternalCourseExams.GetAll()
                           .FirstOrDefaultAsync(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId && c.ExternalCourseTranieeId == trainee.Id);
                        if (tranieeExam != null)
                        {
                            _unitOfWork.ExternalCourseExams.Delete(tranieeExam);
                        }
                        trainee.Status = (int)ExamEnums.TraineeStatus.Waiting;
                        _unitOfWork.ExternalCourseTraniees.Update(trainee);
                    }
                }
            }
            var editedTrainees = RequestedData.Trainees.Where(x => scheduleTrainees.Select(v => v.Id).Contains(x.Id ?? 0)).ToList();

            foreach (var trainee in editedTrainees)
            {
                var updatedtrainee = scheduleTrainees.FirstOrDefault(c => c.Id == trainee.Id);
                if (updatedtrainee is not null)
                {
                    updatedtrainee.Status = (int)ExamEnums.TraineeStatus.Enrolled;
                    _unitOfWork.ExternalCourseTraniees.Update(updatedtrainee);
                }
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> TransferExternalTrainees(TransferExternalTraineesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Trainees.Count == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var currentSchedule = await _unitOfWork.TrainingCourseSchedule
               .GetAll(c => c.Id == RequestedData.TrainingCourseScheduleId)
               .AsNoTracking().FirstOrDefaultAsync();

            if (currentSchedule is null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            int nextSchedule = await GetNextCourseSchedule(RequestedData, currentSchedule);

            if (nextSchedule == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

            var scheduleTrainees = await _unitOfWork.ExternalCourseTraniees
                     .GetAll(c => RequestedData.Trainees.Select(t => t.Id).Contains(c.Id)).ToListAsync();

            if (scheduleTrainees.Count > 0)
            {
                foreach (var trainee in scheduleTrainees)
                {

                    trainee.Status = (int)ExamEnums.TraineeStatus.Waiting;
                    trainee.TrainingCourseScheduleId = nextSchedule;
                    _unitOfWork.ExternalCourseTraniees.Update(trainee);

                }

                await _unitOfWork.CompleteAsync();

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        }

        #region HELPER METHOD  TransferExternalTrainees
        private async Task<int> GetNextCourseSchedule(TransferExternalTraineesInputDto RequestedData, Models.TrainingCourseSchedule currentSchedule)
        {
            return await _unitOfWork.TrainingCourseSchedule
                    .GetAll(c => c.CourseId == RequestedData.CourseId
                    && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                    && c.ReferenceId == RequestedData.ReferenceId
                    && c.IsDeleted == false
                    && c.IsActive == true && c.IsClosed == false
                    && c.Id != RequestedData.TrainingCourseScheduleId
                    && c.EndDate.Date > TransactionDate.Date
                    && c.StartDate > currentSchedule.StartDate)
                    .AsNoTracking()
                    .OrderBy(c => c.StartDate).Select(c => c.Id).FirstOrDefaultAsync();
        }

        #endregion

        public async Task<OperationOutput> DeleteExternalTrainees(DeleteExternalTraineesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Trainees.Count == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            var result = await _unitOfWork.ExternalCourseTraniees.ExecuteUpdateAsync(c => RequestedData.Trainees.Select(t => t.Id).Contains(c.Id),
                           sett => sett.SetProperty(c => c.Status, (int)ExamEnums.TraineeStatus.Removed)
                          .SetProperty(c => c.IsDeleted, true));

            if (result > 0)
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
        }

        public async Task<OperationOutput> SendEmailToInternTrainees(SendEmailToTrainees RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
               .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);

            var internalTrainees = await _unitOfWork.InternalCourseTrainees.GetAll(c => RequestedData.Trainees.Select(u => u.Id).Contains(c.TraineeId))
                .AsNoTracking().Select(c => c.Email).ToListAsync();

            if (courseSchedule is null || internalTrainees.Count <= 0)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
            }
            await SendEmailToTraniees(courseSchedule, internalTrainees);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        public async Task<OperationOutput> SendEmailToExternalTrainees(SendEmailToTrainees RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
               .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);

            var emailAddress = await _unitOfWork.ExternalCourseTraniees.GetAll(c => RequestedData.Trainees.Select(u => u.Id).Contains(c.Id))
                .AsNoTracking().Select(c => c.Email).ToListAsync();

            if (courseSchedule is null || emailAddress.Count <= 0)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
            }
            await SendEmailToTraniees(courseSchedule, emailAddress);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);


        }

        #region HELPER METHOD SendEmailToTraniees
        private async Task<OperationOutput> SendEmailToTraniees(TrainingCourseSchedule courseSchedule, List<string> emailAddress)
        {
            string Body = string.Empty;
            string Subject = $" {TransactionDate.ToString("yyy-MM-dd hh:mm ")}  -  ادارة الخدمات التدريبية";


            string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            using (StreamReader reader = new StreamReader(Path.Combine(path, "ApproveTraineesTemplateEmail.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("@currentDateHijri", Dates.ConvertFromGerogianToHijriDateString(TransactionDate));
            Body = Body.Replace("@currentDate", TransactionDate.ToString("yyyy-MM-dd"));
            Body = Body.Replace("@courseName", courseSchedule.Course.TitleAr);
            Body = Body.Replace("@startDate", courseSchedule.StartDate.ToString("yyyy-MM-dd hh:mm"));

            var result = SendEmailMessageToTranieesByRabbitMQ(emailAddress, Body, Subject);

            if (result.IsValid)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);


            // await Email.SendEmailAsync(emailAddress.First(), Subject, Body, EmailServiceUrl, Token, null, null, emailAddress);
        }

        private ResultDto SendEmailMessageToTranieesByRabbitMQ(List<string> emailAddress, string Body, string Subject)
        {
            var emailInfo = new EmailInfoRecord
            {
                ToEmail = emailAddress.First(),
                Subject = Subject,
                Body = Body,
                BCCEmailAddresses = emailAddress
            };

            (_, var model) = _rabbitMqService.CreateConnection();
            _rabbitMqService.DeclareQueue_Quorum(model, _configuration.Queue, _configuration.Exchange, string.Empty, ExchangeType.Fanout);
            return _rabbitMqService.SendMessage(model, _configuration.Exchange, emailInfo);
        }

        #endregion

        #endregion

        #region Assign trainees for Exams

        //Notes :: Call GetLookupsForAssignTrainees  

        public async Task<OperationOutput> GetActivationCourseListForExam(ActivationCorsesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courses = await _unitOfWork.TrainingCourseSchedule
                .GetAll(c => c.ReferenceId == RequestedData.ReferenceId && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                    && c.Course.Type == RequestedData.Type
                    && c.IsDeleted == false
                    && c.StartDate.Date <= TransactionDate.Date).AsNoTrackingWithIdentityResolution()
                .Include(c => c.Course)
                .Include(c => c.Exam)
                .GroupBy(c => c.CourseId)
                .Select(c => new CoursesForExamLookup
                {
                    Id = c.Key,
                    TitleAr = c.Select(c => c.Course.TitleAr).First(),
                    TitleEn = c.Select(c => c.Course.TitleEn).First(),
                    ExamTitleAr = c.Select(c => c.Exam.TitleAr).First(),
                    ExamTitleEn = c.Select(c => c.Exam.TitleEn).First(),
                })
                .ToListAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.Courses, courses));
        }

        public async Task<OperationOutput> GetInternalCourseScheduleListForExam(CorsesScheduleByCourseId RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var scheduleList = await GetScheduleListForExam(RequestedData);

            if (scheduleList.Count > 0)
            {
                foreach (var item in scheduleList)
                {
                    item.Trainees = await GetInternTranieeUsersForExam(item.Id.Value);
                }

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.CourseScheduleEntity, scheduleList));

        }

        #region HELPER METHOD GetInternalCourseScheduleListForExam


        private async Task<List<InternCorsesScheduleForExamOutput>> GetScheduleListForExam(CorsesScheduleByCourseId RequestedData)
        {
            var courseSchedule = await _unitOfWork.TrainingCourseSchedule
                  .GetAll(c => c.CourseId == RequestedData.CourseId
                  && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                  && c.ReferenceId == RequestedData.ReferenceId
                  && c.IsDeleted == false
                  && c.StartDate.Date <= TransactionDate.Date)
                  .AsNoTracking()
                  .OrderBy(c => c.StartDate)
                  .Select(c => new InternCorsesScheduleForExamOutput
                  {
                      Id = c.Id,
                      StartDateString = c.StartDate.ToString("yyyy-MM-dd"),
                      EndDateString = c.EndDate.ToString("yyyy-MM-dd"),
                  })
                  .ToListAsync();

            if (courseSchedule.Count > 0)
            {
                var scheduleIds = courseSchedule.Select(c => c.Id).ToList();

                var internalTraineeExists = await _unitOfWork.InternalCourseTrainees
                    .GetAll(t => !t.IsDeleted && scheduleIds.Contains(t.TrainingCourseScheduleId))
                    .Select(t => t.TrainingCourseScheduleId).Distinct().ToListAsync();

                courseSchedule = courseSchedule.Where(c => internalTraineeExists.Contains(c.Id)).ToList();
            }
            return courseSchedule;
        }

        private async Task<List<InternTraineeUsersForExam>> GetInternTranieeUsersForExam(int courseScheduleId)
        {
            var tranieeUsersDto = await (from t in _unitOfWork.InternalCourseTrainees.GetAll()
                                         join e in _unitOfWork.InternalCourseExams.GetAll(c => c.TrainingCourseScheduleId == courseScheduleId)
                                         on new { t.TrainingCourseScheduleId.Value, InternalCourseTraineeId = (int?)t.Id }
                                         equals new { e.TrainingCourseScheduleId.Value, e.InternalCourseTraineeId } into traineeExams
                                         from te in traineeExams.DefaultIfEmpty()
                                         where !t.IsDeleted
                                         && t.TrainingCourseScheduleId == courseScheduleId && t.IsAttendedExam != true
                                         select new InternTraineeUsersForExam
                                         {
                                             Id = t.Id,
                                             Code = t.Code,
                                             TraineeId = t.TraineeId,
                                             Email = t.Email,
                                             TraineeName = t.TraineeName,
                                             IdCardNumber = t.IdCardNumber,
                                             Phone = t.Phone,
                                             IsSelected = te != null
                                         }).AsNoTracking()
                                         .ToListAsync();


            return tranieeUsersDto;
        }


        #endregion

        public async Task<OperationOutput> GetExternalCourseScheduleListForExam(CorsesScheduleByCourseId RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var scheduleList = await GetExternalCourseScheduleForExam(RequestedData);

            if (scheduleList.Count > 0)
            {
                foreach (var item in scheduleList)
                {
                    item.Trainees = await GetExternalTranieeForExam(item.Id.Value);
                }

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.CourseScheduleEntity, scheduleList));


        }

        #region HELPER METHODS GetExternalCourseScheduleListForEXam

        private async Task<List<ExternalCorsesScheduleForExamOutput>> GetExternalCourseScheduleForExam(CorsesScheduleByCourseId RequestedData)
        {
            var courseSchedule = await _unitOfWork.TrainingCourseSchedule
                  .GetAll(c => c.CourseId == RequestedData.CourseId
                  && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                  && c.ReferenceId == RequestedData.ReferenceId
                  && c.IsDeleted == false
                  && c.StartDate.Date <= TransactionDate.Date)
                  .AsNoTracking()
                  .OrderBy(c => c.StartDate)
                  .Select(c => new ExternalCorsesScheduleForExamOutput
                  {
                      Id = c.Id,
                      StartDateString = c.StartDate.ToString("yyyy-MM-dd"),
                      EndDateString = c.EndDate.ToString("yyyy-MM-dd"),
                  })
                  .ToListAsync();

            if (courseSchedule.Count > 0)
            {
                var scheduleIds = courseSchedule.Select(c => c.Id).ToList();

                var externalTraineeExists = await _unitOfWork.ExternalCourseTraniees
                    .GetAll(t => t.IsDeleted != true && t.Status == (int)ExamEnums.TraineeStatus.Enrolled && scheduleIds.Contains(t.TrainingCourseScheduleId))
                    .Select(t => t.TrainingCourseScheduleId).Distinct().ToListAsync();

                courseSchedule = courseSchedule.Where(c => externalTraineeExists.Contains(c.Id)).ToList();
            }
            return courseSchedule;

        }


        private async Task<List<ExternalTraineeForExamOutput>> GetExternalTranieeForExam(int courseScheduleId)
        {

            var externalTrainee = await _unitOfWork.ExternalCourseTraniees
                .GetAll(c => c.TrainingCourseScheduleId == courseScheduleId && c.IsDeleted != true
                 && c.Status == (int)ExamEnums.TraineeStatus.Enrolled
                 && c.IsAttendedExam != true).AsNoTrackingWithIdentityResolution()
                .Include(c => c.Grade)
                .ToListAsync();

            List<ExternalTraineeForExamOutput> externalTraineeDto = GetExternalTraineeWithSelectedInExams(externalTrainee);

            return externalTraineeDto;
        }

        private List<ExternalTraineeForExamOutput> GetExternalTraineeWithSelectedInExams(List<ExternalCourseTraniees> externalTrainee)
        {
            return (from t in externalTrainee
                    join e in _unitOfWork.ExternalCourseExams.GetAll()
                    on t.Id equals e.ExternalCourseTranieeId
                    into traineeExams
                    from te in traineeExams.DefaultIfEmpty()
                    select new ExternalTraineeForExamOutput
                    {
                        Id = t.Id,
                        Code = t.Code,
                        Email = t.Email,
                        FullName = t.FullName,
                        GenderAr = t.Gender == (int)Enums.Gender.Male ? "ذكر" : "انثى",
                        GenderEn = t.Gender == (int)Enums.Gender.Male ? "Male" : "Female",
                        GradeTypeAr = t.Grade != null ? t.Grade.NameAr : string.Empty,
                        GradeTypeEn = t.Grade != null ? t.Grade.NameEn : string.Empty,
                        GradeTitle = t.GradeTitle,
                        GradeYear = t.GradeYear,
                        IdCardNumber = t.IdCardNumber,
                        Phone = t.Phone,
                        Status = t.Status,
                        IsSelected = te != null
                    }).ToList();
        }


        #endregion


        public async Task<OperationOutput> AssignExamToInternalTraineeList(TraineeExamInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.Exam)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);

            if (courseSchedule is null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var internalCourseExams = await _unitOfWork.InternalCourseExams
                .GetAll(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId
                && c.StartAt == null && c.EndAt == null)
                .AsNoTracking()
                .ToListAsync();


            var removedTraineeExams = internalCourseExams.Where(x => !RequestedData.Trainees.Select(v => v.Id ?? 0).Contains(x.InternalCourseTraineeId.Value)).ToList();
            if (removedTraineeExams.Count > 0)
            {
                foreach (var user in removedTraineeExams)
                {
                    _unitOfWork.InternalCourseExams.Delete(user);
                }
            }
            var newExamTrainees = RequestedData.Trainees.Where(x => !internalCourseExams.Select(v => v.InternalCourseTraineeId).Contains(x.Id ?? 0)).ToList();
            var editedExamTrainees = RequestedData.Trainees.Where(x => internalCourseExams.Select(v => v.InternalCourseTraineeId).Contains(x.Id ?? 0)).ToList();

            if (newExamTrainees.Count > 0)
            {
                var internalCourseExamsList = new List<Models.InternalCourseExams>();

                foreach (var item in newExamTrainees)
                {
                    Models.InternalCourseExams internalExam = new();
                    RequestedData.Adapt(internalExam, RequestedData.AddInternalTraineeConfig(RequestOwner.Id.Value, courseSchedule.ExamId, item.Id.Value, TransactionDate));
                    internalCourseExamsList.Add(internalExam);
                }

                await _unitOfWork.InternalCourseExams.BulkInsertAsync(internalCourseExamsList);

            }
            if (editedExamTrainees.Count > 0)
            {
                var updatedCourseExamsList = new List<Models.InternalCourseExams>();

                foreach (var item in editedExamTrainees)
                {
                    var updateditem = internalCourseExams.FirstOrDefault(c => c.InternalCourseTraineeId == item.Id);
                    if (updateditem is not null)
                    {
                        updateditem.FromDate = RequestedData.FromDate;
                        updateditem.ToDate = RequestedData.ToDate;
                        updatedCourseExamsList.Add(updateditem);
                    }
                }

                await _unitOfWork.InternalCourseExams.BulkUpdateAsync(updatedCourseExamsList);
            }

            await _unitOfWork.CompleteAsync();

            await SendEmailToInternTraineesExam(new SendEmailToTraineesExam
            {
                ExamName = courseSchedule.Exam.TitleAr,
                FromDate = RequestedData.FromDate.GetValueOrDefault().ToString("yyyy-MM-dd hh:mm"),
                ToDate = RequestedData.ToDate.GetValueOrDefault().ToString("yyyy-MM-dd hh:mm"),
                Trainees = RequestedData.Trainees

            });

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> AssignExamToExternalTraineeList(TraineeExamInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.Exam)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);

            if (courseSchedule is null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var externalCourseExams = await _unitOfWork.ExternalCourseExams
                .GetAll(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId
                && c.StartAt == null && c.EndAt == null)
                .AsNoTracking()
                .ToListAsync();

            var removedTraineeExams = externalCourseExams.Where(x => !RequestedData.Trainees.Select(v => v.Id ?? 0).Contains(x.ExternalCourseTranieeId.Value)).ToList();
            if (removedTraineeExams.Count > 0)
            {
                foreach (var user in removedTraineeExams)
                {
                    _unitOfWork.ExternalCourseExams.Delete(user);
                }
            }

            var newExamTrainees = RequestedData.Trainees.Where(x => !externalCourseExams.Select(v => v.ExternalCourseTranieeId).Contains(x.Id ?? 0)).ToList();
            var editedExamTrainees = RequestedData.Trainees.Where(x => externalCourseExams.Select(v => v.ExternalCourseTranieeId).Contains(x.Id ?? 0)).ToList();

            if (newExamTrainees.Count > 0)
            {
                var externalCourseExamsList = new List<Models.ExternalCourseExams>();

                foreach (var item in newExamTrainees)
                {
                    Models.ExternalCourseExams externalExam = new();
                    RequestedData.Adapt(externalExam, RequestedData.AddExternalTraineeConfig(RequestOwner.Id.Value, courseSchedule.ExamId, item.Id.Value, TransactionDate));
                    externalCourseExamsList.Add(externalExam);
                }

                await _unitOfWork.ExternalCourseExams.BulkInsertAsync(externalCourseExamsList);
            }
            if (editedExamTrainees.Count > 0)
            {
                var updatedExternalCourseExamsList = new List<Models.ExternalCourseExams>();

                foreach (var item in editedExamTrainees)
                {
                    var updateditem = externalCourseExams.FirstOrDefault(c => c.ExternalCourseTranieeId == item.Id);
                    if (updateditem is not null)
                    {
                        updateditem.FromDate = RequestedData.FromDate;
                        updateditem.ToDate = RequestedData.ToDate;
                        updatedExternalCourseExamsList.Add(updateditem);
                    }
                }

                await _unitOfWork.ExternalCourseExams.BulkUpdateAsync(updatedExternalCourseExamsList);

            }
            await _unitOfWork.CompleteAsync();
            await SendEmailToExternalTraineesExam(new SendEmailToTraineesExam
            {
                ExamName = courseSchedule.Exam.TitleAr,
                FromDate = RequestedData.FromDate.GetValueOrDefault().ToString("yyyy-MM-dd hh:mm"),
                ToDate = RequestedData.ToDate.GetValueOrDefault().ToString("yyyy-MM-dd hh:mm"),
                Trainees = RequestedData.Trainees
            });
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        #region HELPER METHOD SendEmailToTraineesExam

        private async Task<OperationOutput> SendEmailToInternTraineesExam(SendEmailToTraineesExam RequestedData)
        {

            var emailAddress = await _unitOfWork.InternalCourseTrainees.GetAll(c => RequestedData.Trainees.Select(u => u.Id).Contains(c.Id))
                .AsNoTracking().Select(c => c.Email).ToListAsync();

            if (emailAddress.Count > 0)
            {
                await SendEmailToTranieesForExam(RequestedData.ExamName, RequestedData.FromDate, RequestedData.ToDate, emailAddress);

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        private async Task<OperationOutput> SendEmailToExternalTraineesExam(SendEmailToTraineesExam RequestedData)
        {

            var emailAddress = await _unitOfWork.ExternalCourseTraniees.GetAll(c => RequestedData.Trainees.Select(u => u.Id).Contains(c.Id))
                .AsNoTracking().Select(c => c.Email).ToListAsync();

            if (emailAddress.Count > 0)
            {
                return await SendEmailToTranieesForExam(RequestedData.ExamName, RequestedData.FromDate, RequestedData.ToDate, emailAddress);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        private async Task<OperationOutput> SendEmailToTranieesForExam(string examName, string fromDate, string toDate, List<string> emailAddress)
        {
            string Body = string.Empty;
            string Subject = $" {TransactionDate.ToString("yyy-MM-dd hh:mm ")}  -  ادارة الخدمات التدريبية";


            string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            using (StreamReader reader = new StreamReader(Path.Combine(path, "ExamTraineesTemplateEmail.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("@currentDateHijri", Dates.ConvertFromGerogianToHijriDateString(TransactionDate));
            Body = Body.Replace("@currentDate", TransactionDate.ToString("yyyy-MM-dd"));
            Body = Body.Replace("@examName", examName);
            Body = Body.Replace("@fromDate", fromDate);
            Body = Body.Replace("@toDate", toDate);

            #region RabbitMQ

            DateTime endDateExam;
            var isValid = DateTime.TryParse(toDate, out endDateExam);

            if (isValid && endDateExam > DateTime.Now)
            {
                var result = SendEmailMessageByRabbitMQ(toDate, emailAddress, Body, Subject);

                if (result.IsValid)
                {
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

            #endregion

            //  await Email.SendEmailAsync(emailAddress.First(), Subject, Body, EmailServiceUrl, Token, null, null, emailAddress);
        }

        #region HELPER METHOD SendEmailToTraineesExam

        private ResultDto SendEmailMessageByRabbitMQ(string toDate, List<string> emailAddress, string Body, string Subject)
        {
            var emailInfo = new EmailInfoRecord
            {
                ToEmail = emailAddress.First(),
                Subject = Subject,
                Body = Body,
                BCCEmailAddresses = emailAddress
            };


            (_, var model) = _rabbitMqService.CreateConnection();
            _rabbitMqService.DeclareQueue_Quorum(model, _configuration.Queue, _configuration.Exchange, string.Empty, ExchangeType.Fanout);
            var expirationTime = DateTime.Parse(toDate).Subtract(DateTime.Now);
            return _rabbitMqService.SendMessage(model, _configuration.Exchange, emailInfo, string.Empty, expirationTime);
        }

        #endregion

        #endregion

        #endregion

        #region CourseAdvertisement ( CP )

        public async Task<OperationOutput> GetCourseAdvertisementList(CourseAdvertisementCPListInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseAdvertisements = await _unitOfWork.CourseAdvertisement.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate, OrderBy.Descending,
                 c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var courseAdvertisementsDto = courseAdvertisements.Data.Adapt<List<CourseAdvertisementOutputListDto>>(CourseAdvertisementOutputListDto.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.CourseAdvertisements, courseAdvertisementsDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, courseAdvertisements.Pagination));
        }

        public async Task<OperationOutput> GetCourseAdvertisementDetails(CourseAdvertisementDetailsInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseAdvertisement = await _unitOfWork.CourseAdvertisement.GetAll().AsNoTrackingWithIdentityResolution()
               .Include(c => c.AdvertisementsCourses.Where(c => c.IsDeleted == false))
               .ThenInclude(c => c.TrainingCourseSchedule)
               .ThenInclude(c => c.Course)
               .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted == false);

            if (courseAdvertisement == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            CourseAdvertisementOutputDetailDto courseAdvertisementDto = new();

            courseAdvertisementDto = courseAdvertisement.Adapt(courseAdvertisementDto, CourseAdvertisementOutputDetailDto.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CourseAdvertisementEntity, courseAdvertisementDto));

        }


        #region SaveCourseAdvertisement

        public async Task<OperationOutput> GetCoursesAvailableByReference(CoursesAvailableByReferenceInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var allCourseSchedule = await _unitOfWork.TrainingCourseSchedule
                 .GetAll(c => c.ReferenceId == RequestedData.ReferenceId
                 && c.IsDeleted == false && c.IsActive == true && c.IsClosed == false
                 && c.StartDate.Date >= TransactionDate.Date && c.EndDate.Date > TransactionDate.Date).AsNoTrackingWithIdentityResolution()
                .Include(c => c.Course)
                .ToListAsync();

            var courses = allCourseSchedule
                .GroupBy(c => c.CourseId)
                .Select(g => g.OrderBy(c => c.StartDate).First())
                .OrderBy(c => c.StartDate).ToList();

            var coursesDto = courses.Adapt<List<CoursesAvailableOutput>>(CoursesAvailableOutput.SelectForAddConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.Courses, coursesDto));

        }

        public async Task<OperationOutput> SaveCourseAdvertisement(CourseAdvertisementInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.CourseAdvertisement courseAdvertisement;

            if (!RequestedData.Id.HasValue)
            {
                courseAdvertisement = new();
                RequestedData.Adapt(courseAdvertisement, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.CourseAdvertisement.Add(courseAdvertisement);
            }
            else
            {
                courseAdvertisement = await _unitOfWork.CourseAdvertisement.GetAll()
                    .Include(c => c.AdvertisementsCourses)
                    .FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

                if (courseAdvertisement == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(courseAdvertisement, RequestedData.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.CourseAdvertisement.Update(courseAdvertisement);

                var removedCourseAdvertisementsCourses = courseAdvertisement.AdvertisementsCourses.Where(x => !RequestedData.CourseAdvertisementsCoursesDto.Select(v => v.Id ?? 0).Contains(x.Id)).ToList();

                if (removedCourseAdvertisementsCourses.Count > 0)
                {
                    foreach (var course in removedCourseAdvertisementsCourses)
                    {
                        course.IsDeleted = true;
                        course.IsActive = false;
                        _unitOfWork.AdvertisementsCourses.Update(course);
                    }
                }

                var newCourseAdvertisementsCourses = RequestedData.CourseAdvertisementsCoursesDto.Where(x => !courseAdvertisement.AdvertisementsCourses.Select(v => v.Id).Contains(x.Id ?? 0)).ToList();

                if (newCourseAdvertisementsCourses.Count > 0)
                {
                    foreach (var course in newCourseAdvertisementsCourses)
                    {
                        var advertisementsCourses = new AdvertisementsCourses
                        {
                            CourseAdvertisementId = RequestedData.Id,
                            TrainingCourseScheduleId = course.TrainingCourseScheduleId,
                            TrainingCourseTypeId = course.TrainingCourseTypeId,
                            IsActive = true,
                            IsDeleted = false,
                        };
                        _unitOfWork.AdvertisementsCourses.Add(advertisementsCourses);
                    }

                }

            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = courseAdvertisement.Id;

            return await GetCourseAdvertisementDetails(new CourseAdvertisementDetailsInput { Id = RequestedData.Id });
        }

        #endregion

        public async Task<OperationOutput> ActivationCourseAdvertisement(ActivationCourseAdvertisement RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var courseAdvertisement = await _unitOfWork.CourseAdvertisement.GetAll()
                        .Include(c => c.AdvertisementsCourses.Where(c => c.IsDeleted == false))
                        .FirstOrDefaultAsync(c => c.Id == RequestedData.Id);

                    if (courseAdvertisement is null)
                        return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

                    if (!RequestedData.IsActive)
                    {
                        foreach (var item in courseAdvertisement.AdvertisementsCourses)
                        {
                            item.IsActive = false;
                            _unitOfWork.AdvertisementsCourses.Update(item);
                        }
                        courseAdvertisement.IsActive = false;
                    }
                    else
                    {
                        foreach (var item in courseAdvertisement.AdvertisementsCourses)
                        {
                            item.IsActive = true;
                            _unitOfWork.AdvertisementsCourses.Update(item);
                        }
                        courseAdvertisement.IsActive = true;
                    }
                    await _unitOfWork.CompleteAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    await dbContextTransaction.RollbackAsync();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> DeletionCourseAdvertisement(DeletionCourseAdvertisement RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var courseAdvertisement = await _unitOfWork.CourseAdvertisement.GetAll()
                        .Include(c => c.AdvertisementsCourses.Where(c => c.IsDeleted == false))
                        .FirstOrDefaultAsync(c => c.Id == RequestedData.Id);

                    if (courseAdvertisement is null)
                        return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);


                    foreach (var item in courseAdvertisement.AdvertisementsCourses)
                    {
                        item.IsActive = false;
                        item.IsDeleted = true;
                        _unitOfWork.AdvertisementsCourses.Update(item);
                    }
                    courseAdvertisement.IsActive = false;
                    courseAdvertisement.IsDeleted = true;

                    await _unitOfWork.CompleteAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    await dbContextTransaction.RollbackAsync();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }



        #endregion

        #region  CERTIFCATIONS

        public async Task<OperationOutput> SaveCertifacte(CertificateInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
            Certificate certificate;

            if (!RequestedData.Id.HasValue)
            {
                certificate = new();
                RequestedData.Adapt(certificate, RequestedData.AddConfig(RequestOwner.Id.Value, ImagesSavePath, TransactionDate));
                _unitOfWork.Certificates.Add(certificate);
            }
            else
            {
                certificate = await _unitOfWork.Certificates.GetAll().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

                if (certificate == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(certificate, RequestedData.UpdateConfig(RequestOwner.Id.Value, ImagesSavePath, certificate, TransactionDate));
                _unitOfWork.Certificates.Update(certificate);


            }
            await _unitOfWork.CompleteAsync();

            RequestedData.Id = certificate.Id;

            return await GetCertifacteDetails(new CertificateDetailInput { Id = RequestedData.Id });
        }

        public async Task<OperationOutput> GetCertifacteDetails(CertificateDetailInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var certifcate = await _unitOfWork.Certificates.GetAll().AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted == false);

            if (certifcate == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            CertificateDetailsOutputDto CertificateDto = new();

            CertificateDto = certifcate.Adapt(CertificateDto, CertificateDetailsOutputDto.SelectConfigForDetails(ImagesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CertifcateEntity, CertificateDto));

        }

        public async Task<OperationOutput> GetCertifacationList(CertificateListInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var certifications = await _unitOfWork.Certificates.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate, OrderBy.Descending,
                 c => c.CreatedByNavigation, u => u.UpdatedByNavigation);

            var certificationDto = certifications.Data.Adapt<List<CertificationListOutputDto>>(CertificationListOutputDto.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Certificates, certificationDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, certifications.Pagination));
        }

        public async Task<OperationOutput> ActivationCertificate(ActivationCertificate RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive)
            {
                var trainingCourseScheduleCounts = await _unitOfWork.TrainingCourseSchedule.GetAll()
                    .AsNoTracking()
                    .CountAsync(c => c.EndDate.Date <= TransactionDate.Date && c.IsDeleted != true && c.CertificateId == RequestedData.Id);
                if (trainingCourseScheduleCounts > 0)

                    return GetOperationOutput(header: Enums.ServiceMessages.NotAllowDeactiveCertificate);
            }


            await _unitOfWork.Certificates.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> DeletionCertificate(DeletionCertificate RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var trainingCourseScheduleCounts = await _unitOfWork.TrainingCourseSchedule.GetAll()
                .AsNoTracking()
                .CountAsync(c => c.EndDate.Date <= TransactionDate.Date && c.IsDeleted != true && c.CertificateId == RequestedData.Id);

            if (trainingCourseScheduleCounts > 0)

                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowDeletedCertificate);

            await _unitOfWork.Certificates.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                      sett => sett.SetProperty(x => x.IsDeleted, true)
                      .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                      .SetProperty(y => y.UpdatedDate, TransactionDate));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        #endregion

        #region CP Exam Results - Report

        public async Task<OperationOutput> GetExamResultReportByYear(ExamResultByYear RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var trainingCourseSchedule = await GetTrainingCourseSchedule(RequestedData);

            if (trainingCourseSchedule.Count == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Dictionary<int?, int> traineeCounts;
            Dictionary<int?, ExamTotalTraniee> examApplicants;

            if (RequestedData.TypeId == (int)ExamEnums.CourseType.Internal)
            {
                traineeCounts = await GetInternalTotalTranieePerSchedule(trainingCourseSchedule);
                examApplicants = await GetInternalTotalExamApplicants(trainingCourseSchedule);
            }
            else
            {
                traineeCounts = await GetExternalTotalTranieePerSchedule(trainingCourseSchedule);
                examApplicants = await GetExternalTotalExamApplicants(trainingCourseSchedule);
            }

            var result = GetCoursesTotalsReport(trainingCourseSchedule, traineeCounts, examApplicants);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Courses, result));
        }


        #region HELPER METHODS GetExamResultReportByYear
        private async Task<List<CourseScheduleReport>> GetTrainingCourseSchedule(ExamResultByYear RequestedData)
        {
            return await _unitOfWork.TrainingCourseSchedule
                .GetAll(c => c.ReferenceId == RequestedData.ReferenceId
                && c.DepartmentReferenceId == RequestedData.DepartmentReferenceId
                && c.Course.Type == RequestedData.TypeId && c.IsDeleted == false
                && c.EndDate.Date <= TransactionDate.Date
                && c.StartDate.Year == RequestedData.Year).AsNoTrackingWithIdentityResolution()
               .Include(c => c.Course)
               .Select(c => new CourseScheduleReport(c.Course.Code, c.Id, c.CourseId.Value,
               c.StartDate, c.EndDate, c.Course.TitleAr, c.Course.TitleEn, c.Course.Type,
                c.Course.Type == (int)ExamEnums.CourseType.Internal ? "تدريب داخلى للعاملين" : "تدريب عام",
                c.Course.Type == (int)ExamEnums.CourseType.Internal ? "Internal" : "External"))
               .ToListAsync();
        }

        private async Task<Dictionary<int?, int>> GetInternalTotalTranieePerSchedule(List<CourseScheduleReport> trainingCourseSchedule)
        {
            var traineeCountDictionary = await _unitOfWork.InternalCourseTrainees
                  .GetAll(c => trainingCourseSchedule.Select(t => t.Id)
                  .Contains(c.TrainingCourseScheduleId.Value) && !c.IsDeleted)
                  .GroupBy(tc => tc.TrainingCourseScheduleId)
                  .Select(g => new { TrainingCourseScheduleId = g.Key, TotalTrainee = g.Count() })
                  .ToDictionaryAsync(g => g.TrainingCourseScheduleId, g => g.TotalTrainee);

            return traineeCountDictionary;
        }

        private async Task<Dictionary<int?, int>> GetExternalTotalTranieePerSchedule(List<CourseScheduleReport> trainingCourseSchedule)
        {
            var traineeCountDictionary = await _unitOfWork.ExternalCourseTraniees
                  .GetAll(c => trainingCourseSchedule.Select(t => t.Id)
                  .Contains(c.TrainingCourseScheduleId.Value) && c.IsDeleted != true)
                  .GroupBy(tc => tc.TrainingCourseScheduleId)
                  .Select(g => new { TrainingCourseScheduleId = g.Key, TotalTrainee = g.Count() })
                  .ToDictionaryAsync(g => g.TrainingCourseScheduleId, g => g.TotalTrainee);

            return traineeCountDictionary;
        }

        private async Task<Dictionary<int?, ExamTotalTraniee>> GetInternalTotalExamApplicants(List<CourseScheduleReport> trainingCourseSchedule)
        {
            var traineeCountDictionary = await _unitOfWork.InternalCourseExams
                .GetAll(c => trainingCourseSchedule.Select(t => t.Id)
                .Contains(c.TrainingCourseScheduleId.Value))
                .GroupBy(tc => tc.TrainingCourseScheduleId)
                .Select(g => new ExamTotalTraniee(
                    g.Key,
                    g.Count(),
                    g.Count(tc => tc.IsSuccess == true),
                    g.Count() > 0 && g.Count(tc => tc.IsSuccess == true) > 0 ? Math.Round((g.Count(tc => tc.IsSuccess == true) * 100.0) / g.Count(), 2) : 0.0))
                .ToDictionaryAsync(g => g.TrainingCourseScheduleId);

            return traineeCountDictionary;
        }

        private async Task<Dictionary<int?, ExamTotalTraniee>> GetExternalTotalExamApplicants(List<CourseScheduleReport> trainingCourseSchedule)
        {
            var traineeCountDictionary = await _unitOfWork.ExternalCourseExams
                 .GetAll(c => trainingCourseSchedule.Select(t => t.Id)
                 .Contains(c.TrainingCourseScheduleId.Value))
                 .GroupBy(tc => tc.TrainingCourseScheduleId)
                 .Select(g => new ExamTotalTraniee(
                     g.Key,
                     g.Count(),
                     g.Count(tc => tc.IsSuccess == true),
                     g.Count() > 0 && g.Count(tc => tc.IsSuccess == true) > 0 ? Math.Round((g.Count(tc => tc.IsSuccess == true) * 100.0) / g.Count(), 2) : 0.0))
                 .ToDictionaryAsync(g => g.TrainingCourseScheduleId);

            return traineeCountDictionary;
        }

        private static List<ExamResultByYearOutputDto> GetCoursesTotalsReport(List<CourseScheduleReport> trainingCourseSchedule, Dictionary<int?, int> traineeCounts, Dictionary<int?, ExamTotalTraniee> examApplicants)
        {
            return trainingCourseSchedule
                .GroupBy(c => c.CourseId).Select(c => new ExamResultByYearOutputDto
                {
                    CourseId = c.Key,
                    CourseTitleAr = c.First().CourseTitleAr,
                    CourseTitleEn = c.First().CourseTitleEn,
                    Code = c.First().Code,
                    TrainingCourseSchedule = c.Select(t => new TrainingCourseScheduleReport
                    {
                        Id = t.Id,
                        StartDate = t.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = t.EndDate.ToString("yyyy-MM-dd"),
                        TotalTraniee = traineeCounts.ContainsKey(t.Id) ? traineeCounts[t.Id] : 0,
                        NumberOfExamApplicants = examApplicants.ContainsKey(t.Id) ? examApplicants[t.Id].TotalTrainee : 0,
                        TotalPassTraniee = examApplicants.ContainsKey(t.Id) ? examApplicants[t.Id].TotalPassTraniee : 0,
                        SuccessRate = examApplicants.ContainsKey(t.Id) ? $"{examApplicants[t.Id].SuccessRate.ToString()}%" : "0%"
                    }).ToList()
                }).ToList();
        }


        #endregion

        public async Task<OperationOutput> GetExamResultDetailByScheduleId(ExamResultDetailByScheduleId RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
            CourseScheduleReport trainingCourseSchedule = await GetTrainigScduleDetailReport(RequestedData);

            if (trainingCourseSchedule is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            List<TranieeByScheduleId> traniees;
            List<ExamTranieeByScheduleId> examTraniees;

            if (trainingCourseSchedule.CourseType == (int)ExamEnums.CourseType.Internal)
            {
                traniees = await GetInternalTraniee(trainingCourseSchedule.Id, RequestedData.Pagination);
                examTraniees = await GetExamInternalTraniee(traniees);
            }
            else
            {
                traniees = await GetExtenalTraniee(trainingCourseSchedule.Id, RequestedData.Pagination);
                examTraniees = await GetExamExternalTraniee(traniees);
            }
            List<ExamResultDetailOutputDto> result = GetExamTraineeDetails(traniees, examTraniees);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.CourseEntity, result),
                 new OutputDictionary(OperationOutputKeys.CourseScheduleEntity, trainingCourseSchedule),
                 new OutputDictionary(OperationOutputKeys.Pagination, RequestedData.Pagination));

        }

        #region HELPER METHODS GetExamResultDetailByScheduleId
        private async Task<CourseScheduleReport> GetTrainigScduleDetailReport(ExamResultDetailByScheduleId RequestedData)
        {
            return await _unitOfWork.TrainingCourseSchedule
                .GetAll(c => c.Id == RequestedData.TrainingCourseScheduleId.Value)
                .AsNoTrackingWithIdentityResolution()
                .Include(c => c.Course)
                .Select(c => new CourseScheduleReport(c.Course.Code, c.Id, c.CourseId.Value,
                c.StartDate, c.EndDate, c.Course.TitleAr, c.Course.TitleEn, c.Course.Type,
                c.Course.Type == (int)ExamEnums.CourseType.Internal ? "تدريب داخلى للعاملين" : "تدريب عام",
                c.Course.Type == (int)ExamEnums.CourseType.Internal ? "Internal" : "External"))
                .FirstOrDefaultAsync();
        }

        private async Task<List<TranieeByScheduleId>> GetInternalTraniee(int trainingCourseScheduleId, ApplicationOperation.Pagination pagination)
        {
            var numberOfRecord = await _unitOfWork.InternalCourseTrainees.GetAll()
                .CountAsync(c => c.TrainingCourseScheduleId == trainingCourseScheduleId && !c.IsDeleted);

            var traniees = await _unitOfWork.InternalCourseTrainees
                .GetAll(c => c.TrainingCourseScheduleId == trainingCourseScheduleId && !c.IsDeleted)
                .Select(c => new TranieeByScheduleId { Id = c.Id, Code = c.Code, TrainingCourseScheduleId = c.TrainingCourseScheduleId.Value, TranieeName = c.TraineeName, IsAttendedExam = c.IsAttendedExam })
                .Skip((pagination.CurrentPageIndex.Value - 1) * (pagination.RecordPerPage.HasValue ? pagination.RecordPerPage.Value : DefaultPaginationCount))
                .Take((pagination.RecordPerPage.HasValue ? pagination.RecordPerPage.Value : DefaultPaginationCount))
                .ToListAsync();

            pagination.TotalPagesCount = Math.Ceiling((float)numberOfRecord / (pagination.RecordPerPage.HasValue ? (float)pagination.RecordPerPage.Value : DefaultPaginationCount));
            pagination.TotalItemsCount = numberOfRecord;

            return traniees;
        }
        private async Task<List<TranieeByScheduleId>> GetExtenalTraniee(int trainingCourseScheduleId, ApplicationOperation.Pagination pagination)
        {
            var numberOfRecord = await _unitOfWork.ExternalCourseTraniees.GetAll()
               .CountAsync(c => c.TrainingCourseScheduleId == trainingCourseScheduleId && c.IsDeleted != true);

            var traniees = await _unitOfWork.ExternalCourseTraniees
                .GetAll(c => c.TrainingCourseScheduleId == trainingCourseScheduleId && c.IsDeleted != true)
                .Select(c => new TranieeByScheduleId { Id = c.Id, Code = c.Code, TrainingCourseScheduleId = c.TrainingCourseScheduleId.Value, TranieeName = c.FullName, IsAttendedExam = c.IsAttendedExam })
                .Skip((pagination.CurrentPageIndex.Value - 1) * (pagination.RecordPerPage.HasValue ? pagination.RecordPerPage.Value : DefaultPaginationCount))
                .Take((pagination.RecordPerPage.HasValue ? pagination.RecordPerPage.Value : DefaultPaginationCount))
                .ToListAsync();

            return traniees;
        }

        private async Task<List<ExamTranieeByScheduleId>> GetExamInternalTraniee(List<TranieeByScheduleId> Traniees)
        {
            return await _unitOfWork.InternalCourseExams
                .GetAll(c => Traniees.Select(t => t.Id).Contains(c.InternalCourseTraineeId))
                .Select(c => new ExamTranieeByScheduleId
                {
                    ExamCourseTranieeId = c.Id,
                    ScuduleCourseTranieeId = c.InternalCourseTraineeId,
                    ExamDate = c.FromDate.Value.ToString("yyyy-MM-dd"),
                    ExamTime = $"{c.FromDate.Value.ToString("hh:mm tt")}{c.ToDate.Value.ToString("hh:mm tt")}",
                    Result = c.Result,
                    IsSuccess = c.IsSuccess,
                    SuccessRate = c.IsSuccess == true && c.Result > 0 ? (c.Result / 100) * 100.0 : 0
                })
                .ToListAsync();
        }

        private async Task<List<ExamTranieeByScheduleId>> GetExamExternalTraniee(List<TranieeByScheduleId> Traniees)
        {
            return await _unitOfWork.ExternalCourseExams
                .GetAll(c => Traniees.Select(t => t.Id).Contains(c.ExternalCourseTranieeId))
                .Select(c => new ExamTranieeByScheduleId
                {
                    ExamCourseTranieeId = c.Id,
                    ScuduleCourseTranieeId = c.ExternalCourseTranieeId,
                    ExamDate = c.FromDate.Value.ToString("yyyy-MM-dd"),
                    ExamTime = $"{c.FromDate.Value.ToString("hh:mm tt")}{c.ToDate.Value.ToString("hh:mm tt")}",
                    Result = c.Result,
                    IsSuccess = c.IsSuccess,
                    SuccessRate = c.IsSuccess == true && c.Result > 0 ? (c.Result / 100) * 100.0 : 0
                })
                .ToListAsync();
        }

        private static List<ExamResultDetailOutputDto> GetExamTraineeDetails(List<TranieeByScheduleId> traniees, List<ExamTranieeByScheduleId> examTraniees)
        {
            return (from traniee in traniees
                    join examTraniee in examTraniees
                    on traniee.Id equals examTraniee.ScuduleCourseTranieeId into exmTrainees
                    from examTraniee in exmTrainees.DefaultIfEmpty()
                    select new ExamResultDetailOutputDto
                    {
                        Id = traniee.Id,
                        Code = traniee.Code,
                        TranieeName = traniee.TranieeName,
                        ExamCourseTranieeId = examTraniee != null ? examTraniee.ExamCourseTranieeId : null,
                        IsAttendedExam = traniee.IsAttendedExam,
                        AttendedExamAr = traniee.IsAttendedExam == true ? "تم الحضور" : "لم يتم الحضور",
                        AttendedExamEn = traniee.IsAttendedExam == true ? "Attended" : "Not Attended",
                        Result = examTraniee != null ? examTraniee.Result : 0,
                        StatusAr = examTraniee != null ? examTraniee.IsSuccess == true ? "ناجح" : examTraniee.IsSuccess == false ? "راسب" : string.Empty : string.Empty,
                        StatusEn = examTraniee != null ? examTraniee.IsSuccess == true ? "Passed" : examTraniee.IsSuccess == false ? "Failed" : string.Empty : string.Empty,
                        SuccessRate = examTraniee != null ? examTraniee.IsSuccess == true ? $"{examTraniee.SuccessRate.ToString()}%" : "0%" : "0%"
                    }).ToList();
        }

        #endregion


        public async Task<OperationOutput> GetInternalExamAnswers(ExamAnswerActionInput RequestedData)
        {
            Dtos.Exam ExamDto = new Dtos.Exam();
            List<Dtos.ExamQuestionAnswer> UserAnswers;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examAnswer = await _unitOfWork.ExamInternalTranieesAnswerAction.GetAll().Where(x => x.InternalCourseExamsId == RequestedData.ExamCourseTranieeId.Value).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            if (examAnswer == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            var Exam = await _unitOfWork.Exams.GetAll()
                .Include(s => s.ExamQuestions)
                .ThenInclude(a => a.ExamDataSources)
                .Include(s => s.ExamQuestions)
                .ThenInclude(t => t.QuestionType)
                .Where(x => x.Id == examAnswer.ExamId)
                .AsNoTracking().FirstOrDefaultAsync();

            if (Exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Exam.Adapt(ExamDto, Dtos.Exam.SelectConfig(null));

            UserAnswers = _unitOfWork.ExamInternalTranieesQuestionAnswer.GetAll()
                    .Where(d => d.ExamInternalTranieesAnswerActionId == examAnswer.Id)
                    .ToList().Adapt<List<Dtos.ExamQuestionAnswer>>();

            var ExamResult = await _unitOfWork.InternalCourseExams.GetAll()
                .Where(x => x.Id == RequestedData.ExamCourseTranieeId)
                .Select(r => new
                {
                    StartAt = r.StartAt != null ? r.StartAt.Value.ToString("yyyy-MM-dd") : null,
                    EndAt = r.EndAt != null ? r.EndAt.Value.ToString("yyyy-MM-dd") : null,
                    Result = r.Result.ToString(),
                    IsSuccess = r.IsSuccess.Value,
                    IsSuccessAr = r.IsSuccess.Value == true ? "ناجح" : "راسب",
                    IsSuccessEn = r.IsSuccess.Value == true ? "Pass" : "Fail"

                }).FirstOrDefaultAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamResult, ExamResult),
                   new OutputDictionary(OperationOutputKeys.ExamsEntity, ExamDto),
                   new OutputDictionary(OperationOutputKeys.UserAnswers, UserAnswers));
        }

        public async Task<OperationOutput> GetExternalExamAnswers(ExamAnswerActionInput RequestedData)
        {
            Dtos.Exam ExamDto = new Dtos.Exam();
            List<Dtos.ExamQuestionAnswer> UserAnswers;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examAnswer = await _unitOfWork.ExamExternalTranieesAnswerAction.GetAll().Where(x => x.ExternalCourseExamsId == RequestedData.ExamCourseTranieeId.Value).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            if (examAnswer == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            var Exam = await _unitOfWork.Exams.GetAll()
                .Include(s => s.ExamQuestions)
                .ThenInclude(a => a.ExamDataSources)
                .Include(s => s.ExamQuestions)
                .ThenInclude(t => t.QuestionType)
                .Where(x => x.Id == examAnswer.ExamId)
                .AsNoTracking().FirstOrDefaultAsync();

            if (Exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Exam.Adapt(ExamDto, Dtos.Exam.SelectConfig(null));

            UserAnswers = _unitOfWork.ExamExternalTranieesQuestionAnswer.GetAll()
                    .Where(d => d.ExamExternalTranieesAnswerActionId == examAnswer.Id)
                    .ToList().Adapt<List<Dtos.ExamQuestionAnswer>>();

            var ExamResult = await _unitOfWork.ExternalCourseExams.GetAll()
                .Where(x => x.Id == RequestedData.ExamCourseTranieeId)
                .Select(r => new
                {
                    StartAt = r.StartAt != null ? r.StartAt.Value.ToString("yyyy-MM-dd") : null,
                    EndAt = r.EndAt != null ? r.EndAt.Value.ToString("yyyy-MM-dd") : null,
                    Result = r.Result.ToString(),
                    IsSuccess = r.IsSuccess.Value,
                    IsSuccessAr = r.IsSuccess.Value == true ? "ناجح" : "راسب",
                    IsSuccessEn = r.IsSuccess.Value == true ? "Pass" : "Fail"

                }).FirstOrDefaultAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamResult, ExamResult),
                   new OutputDictionary(OperationOutputKeys.ExamsEntity, ExamDto),
                   new OutputDictionary(OperationOutputKeys.UserAnswers, UserAnswers));
        }

        #endregion

        #region PORTAL APIS

        #region CourseAdvertiesment for portal

        public async Task<OperationOutput> GetCourseAdvertiesmentListForPortal(CourseAdvertisementPortalListInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var courseAdvertiesments = await _unitOfWork.CourseAdvertisement.FindAllByPaginationAsync(RequestedData.Filteration(TransactionDate), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Ascending, c => c.AdvertisementsCourses);

            var courseAdvertiesmentsDto = courseAdvertiesments.Data.ToList().Adapt<List<CourseAdvertisementListOutputDto>>(CourseAdvertisementListOutputDto.SelectConfig(TransactionDate));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Careers, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.CourseAdvertisements, courseAdvertiesmentsDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, courseAdvertiesments.Pagination));
        }

        public async Task<OperationOutput> GetCourseAdvertiesmentDetailForPortal(CourseAdvertisementPortalDetailInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var courseAdvertiesment = await _unitOfWork.CourseAdvertisement
                .GetAll()
                .AsNoTrackingWithIdentityResolution()
                .Include(c => c.AdvertisementsCourses
                .Where(t => t.TrainingCourseSchedule.Course.Type == RequestedData.CourseTypeId))
                .ThenInclude(c => c.TrainingCourseSchedule)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.Id);


            CourseAdvertisementDetailOutputDto courseAdvertiesmentDto = new();

            courseAdvertiesmentDto = courseAdvertiesment.Adapt(courseAdvertiesmentDto, CourseAdvertisementDetailOutputDto.SelectConfig(TransactionDate));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CourseAdvertisementEntity, courseAdvertiesmentDto));

        }

        public async Task<OperationOutput> GetCourseDetailForPortal(CourseDetailInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var course = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);


            CoursesDetailOutput coursesDetailOutput = new();

            coursesDetailOutput = course.Adapt(coursesDetailOutput, CoursesDetailOutput.SelectForCourseDetailConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CourseAdvertisementEntity, coursesDetailOutput));


        }

        #endregion

        #region Intern Traniees

        public async Task<OperationOutput> SaveInternalCourseTraineeByUser(InternalCourseTraineesInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.InternalCourseTrainees internalTraniee;

            if (string.IsNullOrEmpty(RequestedData.IdCardNumber) || !RequestedData.TrainingCourseScheduleId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var traniee = await _unitOfWork.InternalCourseTrainees.GetAll().AsNoTracking()
             .FirstOrDefaultAsync(x => x.IdCardNumber.Trim() == RequestedData.IdCardNumber.Trim()
             && x.IsDeleted != true
             && x.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId);

            if (traniee is not null)
                return GetOperationOutput(header: Enums.ServiceMessages.AlreadyAppliedCourse);


            var userData = await _unitOfWork.User.GetAll()
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.IdCardNumber.Trim() == RequestedData.IdCardNumber.Trim()
               && !c.IsDeleted && !c.IsBlocked);

            if (userData is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyCourse);


            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);

            if (courseSchedule is null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (courseSchedule.Course.Type != (int)ExamEnums.CourseType.Internal)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyCourse);

            if (userData.ReferenceId != courseSchedule.DepartmentReferenceId)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyCourse);


            internalTraniee = new();
            RequestedData.Adapt(internalTraniee, RequestedData.AddConfig(RequestOwner.Id.Value, courseSchedule.CourseId.Value, userData, TransactionDate));

            _unitOfWork.InternalCourseTrainees.Add(internalTraniee);

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> QueryInternalTraniees(QueryTrainee RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.IdCardNumber))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var traniee = await _unitOfWork.InternalCourseTrainees.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.TrainingCourseSchedule)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId
                 && c.IdCardNumber.Trim() == RequestedData.IdCardNumber.Trim()
                 && c.IsDeleted == false);

            if (traniee is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var tranieeDto = traniee.Adapt<QueryInternalTraineeOutputDto>(QueryInternalTraineeOutputDto.SelectConfig());

            var tranieeExam = await _unitOfWork.InternalCourseExams.GetAll()
                .Include(c => c.Exam)
                .Include(c => c.TrainingCourseSchedule)
                .FirstOrDefaultAsync(c => c.InternalCourseTraineeId == traniee.Id
                && c.TrainingCourseScheduleId == traniee.TrainingCourseScheduleId
                && c.TrainingCourseSchedule.EndDate.Date >= TransactionDate.Date);



            FillInternalTraineeDtoByExamData(tranieeDto, tranieeExam);

            if (tranieeDto.IsSuccess == true && traniee.TrainingCourseSchedule.CertificateId.HasValue)
            {
                await GetCertificationDataAndSaveLogToInternalTrainee(traniee, tranieeDto, tranieeExam);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                            new OutputDictionary(OperationOutputKeys.Traniee, tranieeDto));
        }


        #region HELPER METHODS  QueryInternalTraniees
        private void FillInternalTraineeDtoByExamData(QueryInternalTraineeOutputDto tranieeDto, InternalCourseExams tranieeExam)
        {
            if (tranieeExam is null)
            {
                tranieeDto.ExamDateStringAr = "موعد الاختبار: لم يحدد";
                tranieeDto.ExamDateStringEn = "Exam date: Not specified";
                tranieeDto.IsExamAvailable = false;
            }
            else
            {

                tranieeDto.TranieeExamId = tranieeExam.Id;
                tranieeDto.ExamDateStringAr = $"موعد الاختبار : {tranieeExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {tranieeExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";
                tranieeDto.ExamDateStringEn = $"Exam date : {tranieeExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {tranieeExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";

                tranieeDto.IsExamAvailable = tranieeExam.IsSuccess == null && tranieeExam.FromDate.Value <= TransactionDate
                && tranieeExam.ToDate.Value > TransactionDate ? true : false;

                tranieeDto.ExamResultStringAr = tranieeExam.IsSuccess == true ? "ناجح" : tranieeExam.IsSuccess == false ? "راسب" : "لم يتم بدء موعد الاختبار ";
                tranieeDto.ExamResultStringEn = tranieeExam.IsSuccess == true ? "Passed" : tranieeExam.IsSuccess == false ? "Failed" : "The exam date has not started.";
                tranieeDto.ExamTitleAr = tranieeExam.Exam != null ? tranieeExam.Exam.TitleAr : string.Empty;
                tranieeDto.ExamTitleEn = tranieeExam.Exam != null ? tranieeExam.Exam.TitleEn : string.Empty;
                tranieeDto.IsSuccess = tranieeExam.IsSuccess;
            }
        }
        private async Task GetCertificationDataAndSaveLogToInternalTrainee(InternalCourseTrainees traniee, QueryInternalTraineeOutputDto tranieeDto, InternalCourseExams tranieeExam)
        {
            CertificateData certificate = new();

            var certificateModel = await _unitOfWork.Certificates.GetAll()
                .AsNoTracking().FirstOrDefaultAsync(c => c.Id == traniee.TrainingCourseSchedule.CertificateId);

            certificate = certificateModel.Adapt<CertificateData>();
            certificate.CertificateId = certificateModel.Id;

            if (certificateModel.UpdatedDate > tranieeExam.EndAt)
            {
                var certificateLog = await _unitOfWork.CertificateLog.GetAll().AsNoTracking()
                      .FirstOrDefaultAsync(c => c.InternalCourseExamsId == tranieeExam.Id);

                certificate = certificateLog.Adapt<CertificateData>();
            }

            var certificateTheme = await _unitOfWork.CertificateThemes.GetAll()
               .AsNoTracking().Select(c => new { c.Id, c.Name })
               .FirstOrDefaultAsync(c => c.Id == traniee.TrainingCourseSchedule.CertificateThemeId);

            if (certificate != null)
                tranieeDto.TraineeCertificate = certificate.Adapt<TraineeCertificate>(TraineeCertificate.SelectConfig(ImagesGetPath, traniee.TraineeName, certificateTheme != null ? certificateTheme.Name : string.Empty, tranieeExam.EndAt, tranieeExam.CertificateNumber));

            if (string.IsNullOrEmpty(tranieeExam.CertificateNumber) && tranieeDto.TraineeCertificate != null)
            {
                tranieeExam.CertificateNumber = tranieeDto.TraineeCertificate.CertificateNumber;

                CertificateLog certificateLog = certificate.Adapt<CertificateLog>();

                certificateLog.InternalCourseExamsId = tranieeExam.Id;
                certificateLog.CreatedDate = TransactionDate;

                _unitOfWork.CertificateLog.Add(certificateLog);

                await _unitOfWork.CompleteAsync();
            }
        }

        #endregion


        #endregion

        #region External Traniees

        public async Task<OperationOutput> GetGradeLookup()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var grads = await _unitOfWork.MajorLookup.GetAll(c => c.TypeId == GradeTypeId && c.IsDeleted != true)
                .AsNoTracking()
                .Select(c => new GradeLookup
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn
                }).ToListAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Grades, grads));

        }

        public async Task<OperationOutput> SaveExternalCourseTraniees(ExternalCourseTranieeInputDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var traniee = await _unitOfWork.ExternalCourseTraniees.GetAll().AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdCardNumber.Trim() == RequestedData.IdCardNumber.Trim()
                && x.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId && x.IsDeleted != true);

            if (traniee is not null)
                return GetOperationOutput(header: Enums.ServiceMessages.AlreadyAppliedCourse);

            var courseSchedule = await _unitOfWork.TrainingCourseSchedule.GetAll().AsNoTrackingWithIdentityResolution()
               .Include(c => c.Course)
               .FirstOrDefaultAsync(c => c.Id == RequestedData.TrainingCourseScheduleId);

            if (courseSchedule is null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (courseSchedule.Course.Type != (int)ExamEnums.CourseType.External)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyCourse);


            Models.ExternalCourseTraniees externalTraniee;

            if (!RequestedData.Id.HasValue)
            {
                externalTraniee = new();
                RequestedData.Adapt(externalTraniee, RequestedData.AddConfig(RequestOwner.Id.Value, courseSchedule.CourseId.Value, TransactionDate));
                _unitOfWork.ExternalCourseTraniees.Add(externalTraniee);
            }
            else
            {
                externalTraniee = await _unitOfWork.ExternalCourseTraniees.GetAll().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);
                if (externalTraniee == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(externalTraniee, RequestedData.UpdateConfig(RequestOwner.Id.Value, courseSchedule.CourseId.Value, TransactionDate));
                _unitOfWork.ExternalCourseTraniees.Update(externalTraniee);
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        //الاستعلام عن حالة التسجيل بالدورة التدربية

        public async Task<OperationOutput> QueryExternalTraniees(QueryTrainee RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.IdCardNumber))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var traniee = await _unitOfWork.ExternalCourseTraniees.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.TrainingCourseSchedule)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(c => c.TrainingCourseScheduleId == RequestedData.TrainingCourseScheduleId
                 && c.IdCardNumber.Trim() == RequestedData.IdCardNumber.Trim()
                 && c.IsDeleted != true);

            if (traniee is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var tranieeDto = traniee.Adapt<QueryExternalTraineeOutputDto>(QueryExternalTraineeOutputDto.SelectConfig());

            var tranieeExam = await _unitOfWork.ExternalCourseExams.GetAll()
                .Include(c => c.Exam)
                .Include(c => c.TrainingCourseSchedule)
                .FirstOrDefaultAsync(c => c.ExternalCourseTranieeId == traniee.Id
                && c.TrainingCourseScheduleId == traniee.TrainingCourseScheduleId
                && c.TrainingCourseSchedule.EndDate.Date >= TransactionDate.Date);


            FillExternalTraineeDtoByExamData(tranieeDto, tranieeExam);

            if (tranieeDto.IsSuccess == true && traniee.TrainingCourseSchedule.CertificateId.HasValue)
            {
                await GetCertificationDataAndSaveLogToExternalTrainee(traniee, tranieeDto, tranieeExam);
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                            new OutputDictionary(OperationOutputKeys.Traniee, tranieeDto));
        }


        #region HELPER METHOD QueryExternalTraniees

        private void FillExternalTraineeDtoByExamData(QueryExternalTraineeOutputDto tranieeDto, ExternalCourseExams tranieeExam)
        {
            if (tranieeExam is null)
            {
                tranieeDto.ExamDateStringAr = "موعد الاختبار: لم يحدد";
                tranieeDto.ExamDateStringEn = "Exam date: Not specified";
                tranieeDto.IsExamAvailable = false;
            }
            else
            {


                tranieeDto.TranieeExamId = tranieeExam.Id;
                tranieeDto.ExamDateStringAr = $"موعد الاختبار : {tranieeExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {tranieeExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";
                tranieeDto.ExamDateStringEn = $"Exam date : {tranieeExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {tranieeExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";

                tranieeDto.IsExamAvailable = tranieeExam.IsSuccess == null && tranieeExam.FromDate.Value <= TransactionDate
                && tranieeExam.ToDate.Value > TransactionDate ? true : false;

                tranieeDto.ExamResultStringAr = tranieeExam.IsSuccess == true ? "ناجح" : tranieeExam.IsSuccess == false ? "راسب" : "لم يتم بدء موعد الاختبار ";
                tranieeDto.ExamResultStringEn = tranieeExam.IsSuccess == true ? "Passed" : tranieeExam.IsSuccess == false ? "Failed" : "The exam date has not started.";
                tranieeDto.ExamTitleAr = tranieeExam.Exam != null ? tranieeExam.Exam.TitleAr : string.Empty;
                tranieeDto.ExamTitleEn = tranieeExam.Exam != null ? tranieeExam.Exam.TitleEn : string.Empty;

                tranieeDto.IsSuccess = tranieeExam.IsSuccess;
            }
        }
        private async Task GetCertificationDataAndSaveLogToExternalTrainee(ExternalCourseTraniees traniee, QueryExternalTraineeOutputDto tranieeDto, ExternalCourseExams tranieeExam)
        {
            CertificateData certificate = new();

            var certificateModel = await _unitOfWork.Certificates.GetAll()
             .AsNoTracking().FirstOrDefaultAsync(c => c.Id == traniee.TrainingCourseSchedule.CertificateId);

            certificate = certificateModel.Adapt<CertificateData>();
            certificate.CertificateId = certificateModel.Id;

            if (certificateModel.UpdatedDate > tranieeExam.EndAt)
            {
                var certificateLog = await _unitOfWork.CertificateLog.GetAll().AsNoTracking()
                      .FirstOrDefaultAsync(c => c.ExternalCourseExamsId == tranieeExam.Id);

                certificate = certificateLog.Adapt<CertificateData>();
            }

            var certificateTheme = await _unitOfWork.CertificateThemes.GetAll()
               .AsNoTracking().Select(c => new { c.Id, c.Name })
               .FirstOrDefaultAsync(c => c.Id == traniee.TrainingCourseSchedule.CertificateThemeId);

            if (certificate != null)
                tranieeDto.TraineeCertificate = certificate.Adapt<TraineeCertificate>(TraineeCertificate.SelectConfig(ImagesGetPath, traniee.FullName, certificateTheme != null ? certificateTheme.Name : string.Empty, tranieeExam.EndAt, tranieeExam.CertificateNumber));

            if (string.IsNullOrEmpty(tranieeExam.CertificateNumber) && tranieeDto.TraineeCertificate != null)
            {
                tranieeExam.CertificateNumber = tranieeDto.TraineeCertificate.CertificateNumber;
                CertificateLog certificateLog = certificate.Adapt<CertificateLog>();

                certificateLog.ExternalCourseExamsId = tranieeExam.Id;
                certificateLog.CreatedDate = TransactionDate;

                _unitOfWork.CertificateLog.Add(certificateLog);
                await _unitOfWork.CompleteAsync();
            }
        }

        #endregion


        #endregion

        #region Exam for portal

        //like as GetJobAppExam

        public async Task<OperationOutput> GetExamInfoForTrainee(GetTraineeExamDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.TraineeExamId.HasValue || !RequestedData.CourseTypeId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.CourseTypeId == (int)ExamEnums.CourseType.Internal)

                return await GetExamInfoForInternalTraniee(RequestedData);

            else return await GetExamInfoForExternalTraniee(RequestedData);

        }

        #region HELPER METHODS GetExamInfoForTrainee

        private async Task<OperationOutput> GetExamInfoForInternalTraniee(GetTraineeExamDto RequestedData)
        {
            int examTimeCounter = 0;
            var internalCourseExam = _unitOfWork.InternalCourseExams.GetById(RequestedData.TraineeExamId.Value);

            if (internalCourseExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var internalTrainee = await _unitOfWork.InternalCourseTrainees.GetAll().AsNoTrackingWithIdentityResolution()
                .Include(c => c.TrainingCourseSchedule)
                .FirstOrDefaultAsync(c => c.Id == internalCourseExam.InternalCourseTraineeId.Value);

            if (internalTrainee is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
            else
            {
                var userTraniee = await _unitOfWork.User.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == internalTrainee.TraineeId);

                if (userTraniee is null || userTraniee.ReferenceId != internalTrainee.TrainingCourseSchedule.DepartmentReferenceId)
                    return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);
            }

            if (internalCourseExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (internalCourseExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (internalCourseExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.ApplyedBefore);

            var exam = await _unitOfWork.Exams.GetAll()
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(a => a.ExamDataSources)
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(t => t.QuestionType)
                  .Where(x => x.Id == internalCourseExam.ExamId)
                  .FirstOrDefaultAsync();

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var examDto = exam.Adapt<Dtos.Exam>(Dtos.Exam.SelectConfig(true));

            examTimeCounter = exam.Duration.Value;

            if (internalCourseExam.StartAt.HasValue)
            {
                if (internalCourseExam.StartAt.Value.AddMinutes(exam.Duration.Value) < TransactionDate)
                    return GetOperationOutput(header: Enums.ServiceMessages.ExamTimeFinished);

                var totalMinutes = (internalCourseExam.ToDate.Value - TransactionDate).TotalMinutes;
                examTimeCounter = exam.Duration.Value - (int)(TransactionDate - internalCourseExam.StartAt.Value).TotalMinutes;

                examTimeCounter = (int)totalMinutes <= examTimeCounter ? (int)totalMinutes : examTimeCounter;
            }
            else
            {
                internalCourseExam.StartAt = TransactionDate;
                _unitOfWork.InternalCourseExams.Update(internalCourseExam);
                _unitOfWork.Complete();
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                              new OutputDictionary(OperationOutputKeys.ExamsEntity, examDto),
                              new OutputDictionary(OperationOutputKeys.ExamTimeCounter, examTimeCounter));
        }

        private async Task<OperationOutput> GetExamInfoForExternalTraniee(GetTraineeExamDto RequestedData)
        {
            int examTimeCounter = 0;
            var externalCourseExam = _unitOfWork.ExternalCourseExams.GetById(RequestedData.TraineeExamId.Value);

            if (externalCourseExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (externalCourseExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (externalCourseExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (externalCourseExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.ApplyedBefore);

            var exam = await _unitOfWork.Exams.GetAll()
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(a => a.ExamDataSources)
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(t => t.QuestionType)
                  .Where(x => x.Id == externalCourseExam.ExamId)
                  .FirstOrDefaultAsync();

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var examDto = exam.Adapt<Dtos.Exam>(Dtos.Exam.SelectConfig(true));
            examTimeCounter = exam.Duration.Value;
            if (externalCourseExam.StartAt.HasValue)
            {

                if (externalCourseExam.StartAt.Value.AddMinutes(exam.Duration.Value) < TransactionDate)
                    return GetOperationOutput(header: Enums.ServiceMessages.ExamTimeFinished);

                var totalMinutes = (externalCourseExam.ToDate.Value - TransactionDate).TotalMinutes;
                examTimeCounter = exam.Duration.Value - (int)(TransactionDate - externalCourseExam.StartAt.Value).TotalMinutes;

                examTimeCounter = (int)totalMinutes <= examTimeCounter ? (int)totalMinutes : examTimeCounter;
            }
            else
            {
                externalCourseExam.StartAt = TransactionDate;
                _unitOfWork.ExternalCourseExams.Update(externalCourseExam);
                _unitOfWork.Complete();
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                              new OutputDictionary(OperationOutputKeys.ExamsEntity, examDto),
                              new OutputDictionary(OperationOutputKeys.ExamTimeCounter, examTimeCounter));
        }


        #endregion

        //like as SaveJobAppExamAnswers

        public async Task<OperationOutput> SaveExamAnswers(TranieeExamAnswerActions RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.TraineeExamId.HasValue || !RequestedData.CourseTypeId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.CourseTypeId == (int)ExamEnums.CourseType.Internal)

                return await SaveInternalTraineeExamAnswer(RequestedData);

            else return await SaveExternalTraineeExamAnswer(RequestedData);

        }


        #region HELPER METHODS SaveExamAnswers
        private async Task<OperationOutput> SaveInternalTraineeExamAnswer(TranieeExamAnswerActions RequestedData)
        {
            Models.ExamInternalTranieesAnswerAction examAnswerAction = new();

            var internalCourseExam = await _unitOfWork.InternalCourseExams.GetByIdAsync(RequestedData.TraineeExamId.Value);

            if (internalCourseExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (internalCourseExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (internalCourseExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (!internalCourseExam.StartAt.HasValue || internalCourseExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);

            var exam = _unitOfWork.Exams.GetById(internalCourseExam.ExamId.Value);

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (internalCourseExam.StartAt.HasValue && (internalCourseExam.StartAt.Value.AddMinutes(exam.Duration.Value + 2) < TransactionDate))
            {
                internalCourseExam.EndAt = TransactionDate;
                internalCourseExam.IsSuccess = false;

                _unitOfWork.InternalCourseExams.Update(internalCourseExam);
                _unitOfWork.Complete();
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);
            }

            internalCourseExam.EndAt = TransactionDate;
            (internalCourseExam.Result, internalCourseExam.IsSuccess) = CalculatResult(RequestedData.ExamQuestionAnswers, exam);

            _unitOfWork.InternalCourseExams.Update(internalCourseExam);

            examAnswerAction.CreatedBy = RequestOwner.Id;
            examAnswerAction.CreatedDate = TransactionDate;
            examAnswerAction.ExamId = internalCourseExam.ExamId;
            examAnswerAction.InternalCourseExamsId = internalCourseExam.Id;
            examAnswerAction.Note = RequestedData.Note;
            foreach (var answer in RequestedData.ExamQuestionAnswers)
            {
                var dbAns = new Models.ExamInternalTranieesQuestionAnswer();
                dbAns.Text = answer.Text;
                dbAns.Value = answer.Value;
                dbAns.DataSourceId = answer.DataSourceId;
                dbAns.QuestionId = answer.QuestionId;
                dbAns.ExamInternalTranieesAnswerActionId = examAnswerAction.Id;
                examAnswerAction.ExamInternalTranieesQuestionAnswer.Add(dbAns);
            }

            _unitOfWork.ExamInternalTranieesAnswerAction.Add(examAnswerAction);

            var internalTrainee = await _unitOfWork.InternalCourseTrainees.GetByIdAsync(internalCourseExam.InternalCourseTraineeId.Value);

            if (internalTrainee is not null)
            {
                internalTrainee.IsAttendedExam = true;
                internalCourseExam.AnswerTotalTime = Math.Round(internalCourseExam.EndAt.Value.Subtract(internalCourseExam.StartAt.Value).TotalMinutes, 2);
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        private async Task<OperationOutput> SaveExternalTraineeExamAnswer(TranieeExamAnswerActions RequestedData)
        {
            Models.ExamExternalTranieesAnswerAction examAnswerAction = new();

            var externalCourseExam = await _unitOfWork.ExternalCourseExams.GetByIdAsync(RequestedData.TraineeExamId.Value);

            if (externalCourseExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (externalCourseExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (externalCourseExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (!externalCourseExam.StartAt.HasValue || externalCourseExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);

            var exam = _unitOfWork.Exams.GetById(externalCourseExam.ExamId.Value);

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (externalCourseExam.StartAt.HasValue && (externalCourseExam.StartAt.Value.AddMinutes(exam.Duration.Value + 2) < TransactionDate))
            {
                externalCourseExam.EndAt = TransactionDate;
                externalCourseExam.IsSuccess = false;

                _unitOfWork.ExternalCourseExams.Update(externalCourseExam);
                _unitOfWork.Complete();
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);
            }

            externalCourseExam.EndAt = TransactionDate;
            (externalCourseExam.Result, externalCourseExam.IsSuccess) = CalculatResult(RequestedData.ExamQuestionAnswers, exam);

            _unitOfWork.ExternalCourseExams.Update(externalCourseExam);

            examAnswerAction.CreatedBy = RequestOwner.Id;
            examAnswerAction.CreatedDate = TransactionDate;
            examAnswerAction.ExamId = externalCourseExam.ExamId;
            examAnswerAction.ExternalCourseExamsId = externalCourseExam.Id;
            examAnswerAction.Note = RequestedData.Note;
            foreach (var answer in RequestedData.ExamQuestionAnswers)
            {
                var dbAns = new Models.ExamExternalTranieesQuestionAnswer();


                dbAns.Text = answer.Text;
                dbAns.Value = answer.Value;
                dbAns.DataSourceId = answer.DataSourceId;
                dbAns.QuestionId = answer.QuestionId;
                dbAns.ExamExternalTranieesAnswerActionId = examAnswerAction.Id;
                examAnswerAction.ExamExternalTranieesQuestionAnswer.Add(dbAns);
            }

            _unitOfWork.ExamExternalTranieesAnswerAction.Add(examAnswerAction);

            var externalTrainee = await _unitOfWork.ExternalCourseTraniees.GetByIdAsync(externalCourseExam.ExternalCourseTranieeId.Value);

            if (externalTrainee is not null)
            {
                externalTrainee.IsAttendedExam = true;
                externalCourseExam.AnswerTotalTime = Math.Round(externalCourseExam.EndAt.Value.Subtract(externalCourseExam.StartAt.Value).TotalMinutes, 2);

            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }


        public (double, bool) CalculatResult(List<Dtos.ExamQuestionAnswer> QuestionsAnswers, Models.Exam exam)
        {
            double result = 0.0;
            foreach (var ans in QuestionsAnswers)
            {
                if (ans.QuestionId != null && ans.DataSourceId != null)
                {
                    var mark = _unitOfWork.ExamQuestions.GetById(ans.QuestionId.Value).Mark;

                    var isCorrect = _unitOfWork.ExamDataSources.GetById(ans.DataSourceId.Value).IsCorrect;
                    if (mark != null && isCorrect.Value == true)
                        result += mark.Value;
                }
            }
            return (result, result >= exam.SuccessMark);
        }


        #endregion

        #endregion

        #endregion




    }
}
