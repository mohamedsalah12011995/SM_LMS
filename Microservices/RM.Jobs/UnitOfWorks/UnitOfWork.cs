using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;


namespace RM.Jobs.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<JobAdvertisement> JobAdvertisement { get; private set; }
        public IBaseRepository<JobCareer> JobCareers { get; private set; }
        public IBaseRepository<JobApplication> JobApplication { get; private set; }
        public IBaseRepository<JobLookUp> JobLookUp { get; private set; }
        public IBaseRepository<Exam> Exams { get; private set; }
        public IBaseRepository<ExamQuestion> ExamQuestions { get; private set; }
        public IBaseRepository<ExamDataSource> ExamDataSources { get; private set; }
        public IBaseRepository<ExamAnswerAction> ExamAnswerActions { get; private set; }
        public IBaseRepository<ExamQuestionAnswer> ExamQuestionAnswers { get; private set; }
        public IBaseRepository<ExamQuestionType> ExamQuestionTypes { get; private set; }
        public IBaseRepository<JobApplicationExams> JobApplicationExams { get; private set; }
        public IBaseRepository<MajorLookup> MajorLookup { get; private set; }
        public IBaseRepository<MajorLookupsType> MajorLookupsType { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            JobAdvertisement = new BaseRepository<JobAdvertisement>(context);
            JobCareers= new BaseRepository<JobCareer>(context);
            JobApplication= new BaseRepository<JobApplication>(context);
            JobLookUp= new BaseRepository<JobLookUp>(context);
            Exams=new BaseRepository<Exam>(context);
            ExamAnswerActions =new BaseRepository<ExamAnswerAction>(context);
            ExamQuestionAnswers=new BaseRepository<ExamQuestionAnswer>(context);
            ExamQuestionTypes=new BaseRepository<ExamQuestionType>(context);
            JobApplicationExams=new BaseRepository<JobApplicationExams>(context);
            MajorLookup=new BaseRepository<MajorLookup>(context);
            ExamQuestions=new BaseRepository<ExamQuestion>(context);
            ExamDataSources=new BaseRepository<ExamDataSource>(context);
            MajorLookupsType = new BaseRepository<MajorLookupsType>(context);
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