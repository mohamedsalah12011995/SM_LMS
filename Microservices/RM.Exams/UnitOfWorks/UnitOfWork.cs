using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.Exams.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExternalPortal_v2Context _context;

        // Private Lazy properties
        private readonly Lazy<IBaseRepository<Exam>> _exams;
        private readonly Lazy<IBaseRepository<ExamQuestion>> _examQuestions;
        private readonly Lazy<IBaseRepository<ExamDataSource>> _examDataSources;
        private readonly Lazy<IBaseRepository<ExamAnswerAction>> _examAnswerActions;
        private readonly Lazy<IBaseRepository<ExamQuestionAnswer>> _examQuestionAnswers;
        private readonly Lazy<IBaseRepository<ExamQuestionType>> _examQuestionTypes;
        private readonly Lazy<IBaseRepository<JobApplicationExams>> _jobApplicationExams;
        private readonly Lazy<IBaseRepository<MajorLookup>> _majorLookup;
        private readonly Lazy<IBaseRepository<User>> _user;
        private readonly Lazy<IBaseRepository<TrainingCourse>> _trainingCourses;
        private readonly Lazy<IBaseRepository<TrainingCourseSchedule>> _trainingCourseSchedule;
        private readonly Lazy<IBaseRepository<TrainingCourseType>> _trainingCourseType;
        private readonly Lazy<IBaseRepository<Reference>> _references;
        private readonly Lazy<IBaseRepository<InternalCourseTrainees>> _internalCourseTrainees;
        private readonly Lazy<IBaseRepository<ExternalCourseTraniees>> _externalCourseTraniees;
        private readonly Lazy<IBaseRepository<InternalCourseExams>> _internalCourseExams;
        private readonly Lazy<IBaseRepository<ExternalCourseExams>> _externalCourseExams;
        private readonly Lazy<IBaseRepository<CourseAdvertisement>> _courseAdvertisement;
        private readonly Lazy<IBaseRepository<AdvertisementsCourses>> _advertisementsCourses;
        private readonly Lazy<IBaseRepository<ExamExternalTranieesAnswerAction>> _examExternalTranieesAnswerAction;
        private readonly Lazy<IBaseRepository<ExamExternalTranieesQuestionAnswer>> _examExternalTranieesQuestionAnswer;
        private readonly Lazy<IBaseRepository<ExamInternalTranieesAnswerAction>> _examInternalTranieesAnswerAction;
        private readonly Lazy<IBaseRepository<ExamInternalTranieesQuestionAnswer>> _examInternalTranieesQuestionAnswer;
        private readonly Lazy<IBaseRepository<Certificate>> _certificates;
        private readonly Lazy<IBaseRepository<CertificateThemes>> _certificateThemes;
        private readonly Lazy<IBaseRepository<UserApplicationExam>> _userApplicationExam;
        private readonly Lazy<IBaseRepository<ExamUserApplicationAnswerAction>> _examUserApplicationAnswerAction;
        private readonly Lazy<IBaseRepository<ExamUserApplicationQuestionAnswer>> _examUserApplicationQuestionAnswer;
        private readonly Lazy<IBaseRepository<CertificateLog>> _certificateLog;

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IMapper Mappers { get; private set; }

        public IBaseRepository<Exam> Exams => _exams.Value;
        public IBaseRepository<ExamQuestion> ExamQuestions => _examQuestions.Value;
        public IBaseRepository<ExamDataSource> ExamDataSources => _examDataSources.Value;
        public IBaseRepository<ExamAnswerAction> ExamAnswerActions => _examAnswerActions.Value;
        public IBaseRepository<ExamQuestionAnswer> ExamQuestionAnswers => _examQuestionAnswers.Value;
        public IBaseRepository<ExamQuestionType> ExamQuestionTypes => _examQuestionTypes.Value;
        public IBaseRepository<JobApplicationExams> JobApplicationExams => _jobApplicationExams.Value;
        public IBaseRepository<MajorLookup> MajorLookup => _majorLookup.Value;
        public IBaseRepository<User> User => _user.Value;
        public IBaseRepository<TrainingCourse> TrainingCourses => _trainingCourses.Value;
        public IBaseRepository<TrainingCourseSchedule> TrainingCourseSchedule => _trainingCourseSchedule.Value;
        public IBaseRepository<TrainingCourseType> TrainingCourseType => _trainingCourseType.Value;
        public IBaseRepository<Reference> References => _references.Value;
        public IBaseRepository<InternalCourseTrainees> InternalCourseTrainees => _internalCourseTrainees.Value;
        public IBaseRepository<ExternalCourseTraniees> ExternalCourseTraniees => _externalCourseTraniees.Value;
        public IBaseRepository<InternalCourseExams> InternalCourseExams => _internalCourseExams.Value;
        public IBaseRepository<ExternalCourseExams> ExternalCourseExams => _externalCourseExams.Value;
        public IBaseRepository<CourseAdvertisement> CourseAdvertisement => _courseAdvertisement.Value;
        public IBaseRepository<AdvertisementsCourses> AdvertisementsCourses => _advertisementsCourses.Value;
        public IBaseRepository<ExamExternalTranieesAnswerAction> ExamExternalTranieesAnswerAction => _examExternalTranieesAnswerAction.Value;
        public IBaseRepository<ExamExternalTranieesQuestionAnswer> ExamExternalTranieesQuestionAnswer => _examExternalTranieesQuestionAnswer.Value;
        public IBaseRepository<ExamInternalTranieesAnswerAction> ExamInternalTranieesAnswerAction => _examInternalTranieesAnswerAction.Value;
        public IBaseRepository<ExamInternalTranieesQuestionAnswer> ExamInternalTranieesQuestionAnswer => _examInternalTranieesQuestionAnswer.Value;
        public IBaseRepository<Certificate> Certificates => _certificates.Value;
        public IBaseRepository<CertificateThemes> CertificateThemes => _certificateThemes.Value;
        public IBaseRepository<UserApplicationExam> UserApplicationExam => _userApplicationExam.Value;
        public IBaseRepository<ExamUserApplicationAnswerAction> ExamUserApplicationAnswerAction => _examUserApplicationAnswerAction.Value;
        public IBaseRepository<ExamUserApplicationQuestionAnswer> ExamUserApplicationQuestionAnswer => _examUserApplicationQuestionAnswer.Value;
        public IBaseRepository<CertificateLog> CertificateLog => _certificateLog.Value;


        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            sp_SearchEngine_Result = new SearchEngineRepository<SearchEngine>(_context);

            _exams = new Lazy<IBaseRepository<Exam>>(() => new BaseRepository<Exam>(context));
            _examQuestions = new Lazy<IBaseRepository<ExamQuestion>>(() => new BaseRepository<ExamQuestion>(context));
            _examDataSources = new Lazy<IBaseRepository<ExamDataSource>>(() => new BaseRepository<ExamDataSource>(context));
            _examAnswerActions = new Lazy<IBaseRepository<ExamAnswerAction>>(() => new BaseRepository<ExamAnswerAction>(context));
            _examQuestionAnswers = new Lazy<IBaseRepository<ExamQuestionAnswer>>(() => new BaseRepository<ExamQuestionAnswer>(context));
            _examQuestionTypes = new Lazy<IBaseRepository<ExamQuestionType>>(() => new BaseRepository<ExamQuestionType>(context));
            _jobApplicationExams = new Lazy<IBaseRepository<JobApplicationExams>>(() => new BaseRepository<JobApplicationExams>(context));
            _majorLookup = new Lazy<IBaseRepository<MajorLookup>>(() => new BaseRepository<MajorLookup>(context));
            _user = new Lazy<IBaseRepository<User>>(() => new BaseRepository<User>(context));
            _trainingCourses = new Lazy<IBaseRepository<TrainingCourse>>(() => new BaseRepository<TrainingCourse>(context));
            _trainingCourseSchedule = new Lazy<IBaseRepository<TrainingCourseSchedule>>(() => new BaseRepository<TrainingCourseSchedule>(context));
            _trainingCourseType = new Lazy<IBaseRepository<TrainingCourseType>>(() => new BaseRepository<TrainingCourseType>(context));
            _references = new Lazy<IBaseRepository<Reference>>(() => new BaseRepository<Reference>(context));
            _internalCourseTrainees = new Lazy<IBaseRepository<InternalCourseTrainees>>(() => new BaseRepository<InternalCourseTrainees>(context));
            _externalCourseTraniees = new Lazy<IBaseRepository<ExternalCourseTraniees>>(() => new BaseRepository<ExternalCourseTraniees>(context));
            _internalCourseExams = new Lazy<IBaseRepository<InternalCourseExams>>(() => new BaseRepository<InternalCourseExams>(context));
            _externalCourseExams = new Lazy<IBaseRepository<ExternalCourseExams>>(() => new BaseRepository<ExternalCourseExams>(context));
            _courseAdvertisement = new Lazy<IBaseRepository<CourseAdvertisement>>(() => new BaseRepository<CourseAdvertisement>(context));
            _advertisementsCourses = new Lazy<IBaseRepository<AdvertisementsCourses>>(() => new BaseRepository<AdvertisementsCourses>(context));
            _examExternalTranieesAnswerAction = new Lazy<IBaseRepository<ExamExternalTranieesAnswerAction>>(() => new BaseRepository<ExamExternalTranieesAnswerAction>(context));
            _examExternalTranieesQuestionAnswer = new Lazy<IBaseRepository<ExamExternalTranieesQuestionAnswer>>(() => new BaseRepository<ExamExternalTranieesQuestionAnswer>(context));
            _examInternalTranieesAnswerAction = new Lazy<IBaseRepository<ExamInternalTranieesAnswerAction>>(() => new BaseRepository<ExamInternalTranieesAnswerAction>(context));
            _examInternalTranieesQuestionAnswer = new Lazy<IBaseRepository<ExamInternalTranieesQuestionAnswer>>(() => new BaseRepository<ExamInternalTranieesQuestionAnswer>(context));
            _certificates = new Lazy<IBaseRepository<Certificate>>(() => new BaseRepository<Certificate>(context));
            _certificateThemes = new Lazy<IBaseRepository<CertificateThemes>>(() => new BaseRepository<CertificateThemes>(context));
            _userApplicationExam = new Lazy<IBaseRepository<UserApplicationExam>>(() => new BaseRepository<UserApplicationExam>(context));
            _examUserApplicationAnswerAction = new Lazy<IBaseRepository<ExamUserApplicationAnswerAction>>(() => new BaseRepository<ExamUserApplicationAnswerAction>(context));
            _examUserApplicationQuestionAnswer = new Lazy<IBaseRepository<ExamUserApplicationQuestionAnswer>>(() => new BaseRepository<ExamUserApplicationQuestionAnswer>(context));
            _certificateLog = new Lazy<IBaseRepository<CertificateLog>>(() => new BaseRepository<CertificateLog>(context));
        }
        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter)
        {
            return _context.Set<EntitiesLatestUpdate>().FromSqlRaw(ProcedureName + ProcedureParams, Parameter);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}