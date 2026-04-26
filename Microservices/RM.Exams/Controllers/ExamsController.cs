using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Exams.Dtos;
using RM.Exams.Dtos.Exams.Certifications;
using RM.Exams.Dtos.ExamStanalone;
using RM.Exams.Dtos.ExamStanalone.PortalDtos;
using RM.Exams.Dtos.ExamTrainingCourses.AssignToExam;
using RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee;
using RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment;
using RM.Exams.Dtos.ExamTrainingCourses.PortalDtos;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule;
using RM.Exams.Records;
using RM.Exams.Records.Exam.Certifications;
using RM.Exams.Records.ExamStandalone;
using RM.Exams.Records.ExamTrainingCourses;
using RM.Exams.Records.ExamTrainingCourses.Portal;
using RM.Exams.Records.ExamTrainingCourses.Reports;
using RM.Exams.Services;

namespace RM.Exams.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamsController
    {
        private readonly IExamsService examsService;
        private readonly IAnswerActionService answerActionService;
        private readonly IExamTrainingCoursesService examTrainingCoursesService;
        public ExamsController(IExamsService _examsService, IAnswerActionService _answerActionService, IExamTrainingCoursesService _examTrainingCoursesService)
        {
            examsService = _examsService;
            answerActionService = _answerActionService;
            examTrainingCoursesService = _examTrainingCoursesService;
        }

        [HttpPost]

        public async Task<OperationOutput> GetExamLookups(GetExamLookupsRecord RequestedData)
        {
            return await examsService.GetExamLookups(RequestedData.Adapt<Dtos.Exam>());
        }

        [HttpPost]

        public async Task<OperationOutput> GetExamsList(GetExamsListRecord RequestedData)
        {
            return await examsService.GetExamsList(RequestedData.Adapt<Dtos.Exam>());
        }

        [HttpPost]

        public async Task<OperationOutput> SaveExamAll(SaveExamAllRecord RequestedData)
        {
            return await examsService.SaveExamAll(RequestedData.Adapt<Dtos.Exam>());
        }

        [HttpPost]

        public async Task<OperationOutput> SaveExam(SaveExamRecord RequestedData)
        {
            return await examsService.SaveExam(RequestedData.Adapt<Dtos.Exam>());
        }

        [HttpPost]

        public async Task<OperationOutput> SaveQuestion(SaveQuestionRecord RequestedData)
        {
            return await examsService.SaveQuestion(RequestedData.Adapt<Dtos.ExamQuestion>());
        }

        [HttpPost]

        public async Task<OperationOutput> GetQuestionDetails(GetQuestionDetailsRecord RequestedData)
        {
            return await examsService.GetQuestionDetails(RequestedData.Adapt<Dtos.ExamQuestion>());
        }

        [HttpPost]

        public async Task<OperationOutput> GetExamDetails(GetExamDetailsRecord RequestedData)
        {
            return await examsService.GetExamDetails(RequestedData.Adapt<Dtos.Exam>());
        }

        [HttpPost]

        public async Task<OperationOutput> QuestionModelActions(QuestionModelActionsRecord RequestedData)
        {
            return await examsService.QuestionModelActions(RequestedData.Adapt<Dtos.ExamQuestion>());
        }

        [HttpPost]

        public async Task<OperationOutput> ModelActions(ModelActionsRecord RequestedData)
        {
            return await examsService.ModelActions(RequestedData.Adapt<Dtos.Exam>());
        }

        [HttpPost]

        public async Task<OperationOutput> SaveAnswerAction(SaveAnswerActionRecord RequestedData)
        {
            return await answerActionService.SaveAnswerAction(RequestedData.Adapt<Dtos.ExamAnswerAction>());
        }

        [HttpPost]

        public async Task<OperationOutput> GetUserExamAnswers(GetUserExamAnswersRecord RequestedData)
        {
            return await answerActionService.GetUserExamAnswers(RequestedData.Adapt<Dtos.ExamAnswerAction>());
        }

        #region Exam Training Courses

        [HttpPost]

        public async Task<OperationOutput> GetTrainigCourseLookups()
        {
            return await examTrainingCoursesService.GetTrainigCourseLookups();
        }

        [HttpPost]

        public async Task<OperationOutput> GetTrainigCourseList(TrainigCourseListDto RequestedData)
        {
            return await examTrainingCoursesService.GetTrainigCourseList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SaveTrainigCourse(TrainigCourseInputDto RequestedData)
        {
            return await examTrainingCoursesService.SaveTrainigCourse(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetTrainigCourseDetail(TrainigCourseById RequestedData)
        {
            return await examTrainingCoursesService.GetTrainigCourseDetail(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> ActivationTrainigCourse(TrainigCourseActivation RequestedData)
        {
            return await examTrainingCoursesService.ActivationTrainigCourse(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> DeletionTrainigCourse(TrainigCourseById RequestedData)
        {
            return await examTrainingCoursesService.DeletionTrainigCourse(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCourseScheduleLookups(CourseScheduleLookup RequestedData)
        {
            return await examTrainingCoursesService.GetCourseScheduleLookups(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetTrainingCourseScheduleList(CourseScheduleListInput RequestedData)
        {
            return await examTrainingCoursesService.GetTrainingCourseScheduleList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetLookupsForAddCourseSchedule(LookupCourseScheduleForAdd RequestedData)
        {
            return await examTrainingCoursesService.GetLookupsForAddCourseSchedule(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SaveTrainingCourseSchedule(TrainingCourseScheduleInputDto RequestedData)
        {
            return await examTrainingCoursesService.SaveTrainingCourseSchedule(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> GetTrainigCourseScheduleDetail(TrainigCourseScheduleById RequestedData)
        {
            return await examTrainingCoursesService.GetTrainigCourseScheduleDetail(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> ActivationTrainigCourseSchedule(TrainigCourseScheduleActivation RequestedData)
        {
            return await examTrainingCoursesService.ActivationTrainigCourseSchedule(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> DeletionTrainigCourseSchedule(TrainigCourseScheduleById RequestedData)
        {
            return await examTrainingCoursesService.DeletionTrainigCourseSchedule(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetLookupsForAssignTrainees()
        {
            return await examTrainingCoursesService.GetLookupsForAssignTrainees();
        }

        [HttpPost]

        public async Task<OperationOutput> GetActivationCourseList(ActivationCorsesInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetActivationCourseList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetInternalCourseScheduleList(CorsesScheduleByCourseId RequestedData)
        {
            return await examTrainingCoursesService.GetInternalCourseScheduleList(RequestedData);
        }

        [HttpPost]

        public async Task<List<InternTraineeUsers>> PaginateCourseSchedulInternUsers(PaginateInternalUsers RequestedData)
        {
            return await examTrainingCoursesService.PaginateCourseSchedulInternUsers(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> GetExternalCourseScheduleList(CorsesScheduleByCourseId RequestedData)
        {
            return await examTrainingCoursesService.GetExternalCourseScheduleList(RequestedData);
        }

        [HttpPost]

        public async Task<List<ExternalTraineeOutput>> PaginateCourseSchedulExtenalTraniee(PaginateInternalUsers RequestedData)
        {
            return await examTrainingCoursesService.PaginateCourseSchedulExtenalTraniee(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> AssignInternTrainees(AssignInternalTraineesInputDto RequestedData)
        {
            return await examTrainingCoursesService.AssignInternTrainees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> AssignExternalTrainees(AssignExternalTraineesInputDto RequestedData)
        {
            return await examTrainingCoursesService.AssignExternalTrainees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> TransferExternalTrainees(TransferExternalTraineesInputDto RequestedData)
        {
            return await examTrainingCoursesService.TransferExternalTrainees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> DeleteExternalTrainees(DeleteExternalTraineesInputDto RequestedData)
        {
            return await examTrainingCoursesService.DeleteExternalTrainees(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> SendEmailToInternTrainees(SendEmailToTrainees RequestedData)
        {
            return await examTrainingCoursesService.SendEmailToInternTrainees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SendEmailToExternalTrainees(SendEmailToTrainees RequestedData)
        {
            return await examTrainingCoursesService.SendEmailToExternalTrainees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetActivationCourseListForExam(ActivationCorsesInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetActivationCourseListForExam(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetInternalCourseScheduleListForExam(CorsesScheduleByCourseId RequestedData)
        {
            return await examTrainingCoursesService.GetInternalCourseScheduleListForExam(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> GetExternalCourseScheduleListForExam(CorsesScheduleByCourseId RequestedData)
        {
            return await examTrainingCoursesService.GetExternalCourseScheduleListForExam(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> AssignExamToInternalTraineeList(TraineeExamInputDto RequestedData)
        {
            return await examTrainingCoursesService.AssignExamToInternalTraineeList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> AssignExamToExternalTraineeList(TraineeExamInputDto RequestedData)
        {
            return await examTrainingCoursesService.AssignExamToExternalTraineeList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCourseAdvertisementList(CourseAdvertisementCPListInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetCourseAdvertisementList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCourseAdvertisementDetails(CourseAdvertisementDetailsInput RequestedData)
        {
            return await examTrainingCoursesService.GetCourseAdvertisementDetails(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCoursesAvailableByReference(CoursesAvailableByReferenceInput RequestedData)
        {
            return await examTrainingCoursesService.GetCoursesAvailableByReference(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SaveCourseAdvertisement(CourseAdvertisementInputDto RequestedData)
        {
            return await examTrainingCoursesService.SaveCourseAdvertisement(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> ActivationCourseAdvertisement(ActivationCourseAdvertisement RequestedData)
        {
            return await examTrainingCoursesService.ActivationCourseAdvertisement(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> DeletionCourseAdvertisement(DeletionCourseAdvertisement RequestedData)
        {
            return await examTrainingCoursesService.DeletionCourseAdvertisement(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SaveInternalCourseTraineeByUser(InternalCourseTraineesInputDto RequestedData)
        {
            return await examTrainingCoursesService.SaveInternalCourseTraineeByUser(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> QueryInternalTraniees(QueryTrainee RequestedData)
        {
            return await examTrainingCoursesService.QueryInternalTraniees(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> GetGradeLookup()
        {
            return await examTrainingCoursesService.GetGradeLookup();
        }

        [HttpPost]

        public async Task<OperationOutput> SaveExternalCourseTraniees(ExternalCourseTranieeInputDto RequestedData)
        {
            return await examTrainingCoursesService.SaveExternalCourseTraniees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> QueryExternalTraniees(QueryTrainee RequestedData)
        {
            return await examTrainingCoursesService.QueryExternalTraniees(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCourseAdvertiesmentListForPortal(CourseAdvertisementPortalListInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetCourseAdvertiesmentListForPortal(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCourseAdvertiesmentDetailForPortal(CourseAdvertisementPortalDetailInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetCourseAdvertiesmentDetailForPortal(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetExamInfoForTrainee(GetTraineeExamDto RequestedData)
        {
            return await examTrainingCoursesService.GetExamInfoForTrainee(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCourseDetailForPortal(CourseDetailInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetCourseDetailForPortal(RequestedData);
        }
        [HttpPost]

        public async Task<OperationOutput> SaveExamAnswers(TranieeExamAnswerActions RequestedData)
        {
            return await examTrainingCoursesService.SaveExamAnswers(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetExamResultReportByYear(ExamResultByYear RequestedData)
        {
            return await examTrainingCoursesService.GetExamResultReportByYear(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetExamResultDetailByScheduleId(ExamResultDetailByScheduleId RequestedData)
        {
            return await examTrainingCoursesService.GetExamResultDetailByScheduleId(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetInternalExamAnswers(ExamAnswerActionInput RequestedData)
        {
            return await examTrainingCoursesService.GetInternalExamAnswers(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetExternalExamAnswers(ExamAnswerActionInput RequestedData)
        {
            return await examTrainingCoursesService.GetExternalExamAnswers(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SaveCertifacte(CertificateInputDto RequestedData)
        {
            return await examTrainingCoursesService.SaveCertifacte(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCertifacteDetails(CertificateDetailInput RequestedData)
        {
            return await examTrainingCoursesService.GetCertifacteDetails(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetCertifacationList(CertificateListInputDto RequestedData)
        {
            return await examTrainingCoursesService.GetCertifacationList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> ActivationCertificate(ActivationCertificate RequestedData)
        {
            return await examTrainingCoursesService.ActivationCertificate(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> DeletionCertificate(DeletionCertificate RequestedData)
        {
            return await examTrainingCoursesService.DeletionCertificate(RequestedData);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
          => examTrainingCoursesService.GetPathOfResource(fileName);

        #endregion


        #region Exam Standalone


        [HttpPost]

        public async Task<OperationOutput> GetLookupsForAssignUsersExam(ExamAssignLookup RequestedData)
        {
            return await examsService.GetLookupsForAssignUsersExam(RequestedData);
        }


        [HttpPost]
        public async Task<OperationOutput> GetUserDepartmentsForAssignExam(UserDepartmentInput RequestedData)
        {
            return await examsService.GetUserDepartmentsForAssignExam(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> AssignExamToUserList(UserExamInput RequestedData)
        {
            return await examsService.AssignExamToUserList(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> SendEmailToUsersExam(SendEmailToUsersExam RequestedData)
        {
            return await examsService.SendEmailToUsersExam(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> GetDepartmentExamList()
        {
            return await examsService.GetDepartmentExamList();
        }

        [HttpPost]
        public async Task<OperationOutput> QueryUserApplicationExam(QueryUserAppExam RequestedData)
        {
            return await examsService.QueryUserApplicationExam(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> GetExamInfoForUserApplication(GetUserExamDto RequestedData)
        {
            return await examsService.GetExamInfoForUserApplication(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> SaveUserApplicationExamAnswers(UserApplicationExamAnswerActions RequestedData)
        {
            return await examsService.SaveUserApplicationExamAnswers(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> GetExamUserApplicationResultReportByYear(ExamUserApplicationResultByYear RequestedData)
        {
            return await examsService.GetExamUserApplicationResultReportByYear(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> GetExamResultDetailByExamId(ExamResultDetailByExamIdInput RequestedData)
        {
            return await examsService.GetExamResultDetailByExamId(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> GetUserApplicationExamAnswers(ExamUserApplicationAnswerActionInput RequestedData)
        {
            return await examsService.GetUserApplicationExamAnswers(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> GetDepartmentExamListForResults(ExamListForResultInput RequestedData)
        {
            return await examsService.GetDepartmentExamListForResults(RequestedData);
        }
        #endregion

    }
}
