using RM.Core.Services;
using RM.Exams.Dtos;
using RM.Exams.Dtos.Exams.Certifications;
using RM.Exams.Dtos.ExamTrainingCourses.AssignToExam;
using RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee;
using RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment;
using RM.Exams.Dtos.ExamTrainingCourses.PortalDtos;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule;
using RM.Exams.Records.Exam.Certifications;
using RM.Exams.Records.ExamTrainingCourses;
using RM.Exams.Records.ExamTrainingCourses.Portal;
using RM.Exams.Records.ExamTrainingCourses.Reports;

namespace RM.Exams.Services
{
    public interface IExamTrainingCoursesService : IBaseService
    {

        #region TrainigCourses
        Task<OperationOutput> GetTrainigCourseLookups();
        Task<OperationOutput> GetTrainigCourseList(TrainigCourseListDto RequestedData);
        Task<OperationOutput> SaveTrainigCourse(TrainigCourseInputDto RequestedData);
        Task<OperationOutput> GetTrainigCourseDetail(TrainigCourseById RequestedData);
        Task<OperationOutput> ActivationTrainigCourse(TrainigCourseActivation RequestedData);
        Task<OperationOutput> DeletionTrainigCourse(TrainigCourseById RequestedData);

        #endregion

        #region TrainingCourseSchedule

        Task<OperationOutput> GetCourseScheduleLookups(CourseScheduleLookup RequestedData);
        Task<OperationOutput> GetTrainingCourseScheduleList(CourseScheduleListInput RequestedData);
        Task<OperationOutput> GetLookupsForAddCourseSchedule(LookupCourseScheduleForAdd RequestedData);
        Task<OperationOutput> SaveTrainingCourseSchedule(TrainingCourseScheduleInputDto RequestedData);
        Task<OperationOutput> GetTrainigCourseScheduleDetail(TrainigCourseScheduleById RequestedData);
        Task<OperationOutput> ActivationTrainigCourseSchedule(TrainigCourseScheduleActivation RequestedData);
        Task<OperationOutput> DeletionTrainigCourseSchedule(TrainigCourseScheduleById RequestedData);

        #endregion

        #region Assign trainees for training courses

        Task<OperationOutput> GetLookupsForAssignTrainees();
        Task<OperationOutput> GetActivationCourseList(ActivationCorsesInputDto RequestedData);
        Task<OperationOutput> GetInternalCourseScheduleList(CorsesScheduleByCourseId RequestedData);
        Task<List<InternTraineeUsers>> PaginateCourseSchedulInternUsers(PaginateInternalUsers RequestedData);
        Task<OperationOutput> GetExternalCourseScheduleList(CorsesScheduleByCourseId RequestedData);
        Task<List<ExternalTraineeOutput>> PaginateCourseSchedulExtenalTraniee(PaginateInternalUsers RequestedData);
        Task<OperationOutput> AssignInternTrainees(AssignInternalTraineesInputDto RequestedData);
        Task<OperationOutput> AssignExternalTrainees(AssignExternalTraineesInputDto RequestedData);
        Task<OperationOutput> TransferExternalTrainees(TransferExternalTraineesInputDto RequestedData);
        Task<OperationOutput> DeleteExternalTrainees(DeleteExternalTraineesInputDto RequestedData);
        Task<OperationOutput> SendEmailToInternTrainees(SendEmailToTrainees RequestedData);
        Task<OperationOutput> SendEmailToExternalTrainees(SendEmailToTrainees RequestedData);

        #endregion

        #region Assign trainees for Exams

        Task<OperationOutput> GetActivationCourseListForExam(ActivationCorsesInputDto RequestedData);
        Task<OperationOutput> GetInternalCourseScheduleListForExam(CorsesScheduleByCourseId RequestedData);
        Task<OperationOutput> GetExternalCourseScheduleListForExam(CorsesScheduleByCourseId RequestedData);
        Task<OperationOutput> AssignExamToInternalTraineeList(TraineeExamInputDto RequestedData);
        Task<OperationOutput> AssignExamToExternalTraineeList(TraineeExamInputDto RequestedData);


        #endregion

        #region CourseAdvertisement ( CP )
        Task<OperationOutput> GetCourseAdvertisementList(CourseAdvertisementCPListInputDto RequestedData);

        Task<OperationOutput> GetCourseAdvertisementDetails(CourseAdvertisementDetailsInput RequestedData);

        // SaveCourseAdvertisement
        Task<OperationOutput> GetCoursesAvailableByReference(CoursesAvailableByReferenceInput RequestedData);
        Task<OperationOutput> SaveCourseAdvertisement(CourseAdvertisementInputDto RequestedData);
        //

        Task<OperationOutput> ActivationCourseAdvertisement(ActivationCourseAdvertisement RequestedData);
        Task<OperationOutput> DeletionCourseAdvertisement(DeletionCourseAdvertisement RequestedData);

        #endregion

        #region CP Exam Results - Report
        Task<OperationOutput> GetExamResultReportByYear(ExamResultByYear RequestedData);
        Task<OperationOutput> GetExamResultDetailByScheduleId(ExamResultDetailByScheduleId RequestedData);
        Task<OperationOutput> GetInternalExamAnswers(ExamAnswerActionInput RequestedData);
        Task<OperationOutput> GetExternalExamAnswers(ExamAnswerActionInput RequestedData);
        #endregion

        #region  CERTIFCATIONS

        public Task<OperationOutput> SaveCertifacte(CertificateInputDto RequestedData);
        Task<OperationOutput> GetCertifacteDetails(CertificateDetailInput RequestedData);
        Task<OperationOutput> GetCertifacationList(CertificateListInputDto RequestedData);
        Task<OperationOutput> ActivationCertificate(ActivationCertificate RequestedData);
        Task<OperationOutput> DeletionCertificate(DeletionCertificate RequestedData);
        #endregion

        #region PORTAL APIS


        Task<OperationOutput> GetCourseAdvertiesmentListForPortal(CourseAdvertisementPortalListInputDto RequestedData);

        Task<OperationOutput> GetCourseAdvertiesmentDetailForPortal(CourseAdvertisementPortalDetailInputDto RequestedData);
        Task<OperationOutput> GetCourseDetailForPortal(CourseDetailInputDto RequestedData);
        Task<OperationOutput> SaveInternalCourseTraineeByUser(InternalCourseTraineesInputDto RequestedData);
        Task<OperationOutput> QueryInternalTraniees(QueryTrainee RequestedData);

        Task<OperationOutput> GetGradeLookup();
        Task<OperationOutput> SaveExternalCourseTraniees(ExternalCourseTranieeInputDto RequestedData);
        Task<OperationOutput> QueryExternalTraniees(QueryTrainee RequestedData);

        Task<OperationOutput> GetExamInfoForTrainee(GetTraineeExamDto RequestedData);
        Task<OperationOutput> SaveExamAnswers(TranieeExamAnswerActions RequestedData);


        #endregion

    }
}
