
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Core.Helpers;
using RM.Jobs.Dtos;
using RM.Jobs.Records;
using RM.Jobs.Services;


namespace RM.Jobs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobsController 
    {
        private readonly IJobAdvertisementService _jobAdvertisementService;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IJobApplicationExamsService _jobApplicationExamsService;

        public JobsController(IJobAdvertisementService jobAdvertisementService,IJobApplicationService jobApplicationService,IJobApplicationExamsService jobApplicationExamsService)
        {
            _jobAdvertisementService = jobAdvertisementService;
            _jobApplicationService = jobApplicationService;
            _jobApplicationExamsService = jobApplicationExamsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetQualifications()
        {
            return await _jobAdvertisementService.GetQualifications();
        }

        [HttpPost]
        [Produces("application/json")]
        public  async Task<OperationOutput> CareerModelActions(CareerModelActionsRecord RequestedData)
        {
            return await _jobAdvertisementService.CareerModelActions(RequestedData.Adapt<Dtos.JobCareer>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetJobAdvertiesmentDetails(GetJobAdvertiesmentDetailsRecord RequestedData)
        {
            return  await _jobAdvertisementService.GetJobAdvertiesmentDetails(RequestedData.Adapt<Dtos.JobAdvertisement>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetJobAdvertiesmentList(GetJobAdvertiesmentListRecord RequestedData)
        {
           return await _jobAdvertisementService.GetJobAdvertiesmentList(RequestedData.Adapt<Dtos.JobAdvertisement>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> SaveJobAdvertiesment(SaveJobAdvertiesmentRecord RequestedData)
        {
            return  await _jobAdvertisementService.SaveJobAdvertiesment(RequestedData.Adapt<Dtos.JobAdvertisement>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> JobAdvertiesmentModelActions(JobAdvertiesmentModelActionsRecord RequestedData)
        {
            return await  _jobAdvertisementService.ModelAction(RequestedData.Adapt<Dtos.JobAdvertisement>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetJobsApplicationsLookups(GetJobsApplicationsLookupsRecord RequestedData)
        {
            return await _jobApplicationService.GetJobApplicationsLookups(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetJobApplicationList(GetJobApplicationListRecord RequestedData)
        {
            return await _jobApplicationService.GetJobApplicationList(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetApplicationDetails(GetApplicationDetailsRecord RequestedData)
        {
            return  await _jobApplicationService.GetApplicationDetails(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveJobApplicationList(List<SaveJobApplicationListRecord> jobApplications)
        {
            return  await _jobApplicationService.SaveJobApplicationList(jobApplications.Adapt<List<Dtos.JobApplication>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> SendJobApplicationNotification(SendJobApplicationNotificationRecord jobApplicationNotification)
        {
            return  await  _jobApplicationService.SendJobApplicationNotification(jobApplicationNotification.Adapt<Dtos.JobApplicationNotification>());
        }
     
        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> AddJobApplication(AddJobApplicationRecord RequestdData)
        {
           return await  _jobApplicationService.AddJobApplication(RequestdData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> ApplicationModelActions(ApplicationModelActionsRecord RequestedData)
        {
            return await _jobApplicationService.ModelActions(RequestedData.Adapt<Dtos.JobApplication>());
        }

        
        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> QueryJobApplication(QueryJobApplicationRecord RequestedData)
        {
            return await _jobApplicationService.QueryJobApplication(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAdvertismentCareerLookup(GetAdvertismentCareerLookupRecord RequestedData)
        {
            return await _jobAdvertisementService.GetAdvertismentCareerLookup(RequestedData.Adapt<Dtos.JobAdvertisement>());
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> QueryApplication(QueryApplicationRecord RequestedData)
        {
             return await _jobApplicationService.QueryApplication(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetMilitaryJobApplicationInfo(GetMilitaryJobApplicationInfoRecord RequestedData)
        {
            return  await _jobApplicationService.GetMilitaryJobApplicationInfo(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _jobAdvertisementService.GetPathOfResource(fileName);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetJobCareerDetails(GetJobCareerDetailsRecord RequestedData)
        {
            return  await _jobAdvertisementService.GetJobCareerDetails(RequestedData.Adapt<Dtos.JobCareer>());
        }
      
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveJobLookups(Dtos.JobLookUp RequestedData)
        {
            return  await _jobAdvertisementService.SaveJobLookups(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SearchApplication(SearchApplicationRecord RequestedData)
        {
            return  await _jobApplicationService.SearchApplication(RequestedData.Adapt<Dtos.JobApplication>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> RejectJobAppList(List<RejectJobAppListRecord> jobApplications)
        {
            return await _jobApplicationService.RejectJobAppList(jobApplications.Adapt<List<Dtos.JobApplication>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> ReAgreeJobAppList(List<ReAgreeJobAppListRecord> jobApplications)
        {
            return await _jobApplicationService.ReAgreeJobAppList(jobApplications.Adapt<List<Dtos.JobApplication>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteJobAppList(List<DeleteJobAppListRecord> jobApplications)
        {
           return await _jobApplicationService.DeleteJobAppList(jobApplications.Adapt<List<Dtos.JobApplication>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddJobApplicationExamsList(AddJobApplicationExamsListRecord jobApplications)
        {
            return await _jobApplicationExamsService.AddJobApplicationExamsList(jobApplications.Adapt<Dtos.JobApplicationExams>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetJobAppExamInfo(GetJobAppExamInfoRecord jobApplication)
        {
            return await _jobApplicationExamsService.GetJobAppExamInfo(jobApplication.Adapt<JobApplicationExam>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetJobAppExam(GetJobAppExamRecord jobApplication)
        {
            return await _jobApplicationExamsService.GetJobAppExam(jobApplication.Adapt<JobApplicationExam>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveJobAppExamAnswers(SaveJobAppExamAnswersRecord jobApplication)
        {
            return await _jobApplicationExamsService.SaveJobAppExamAnswers(jobApplication.Adapt<ExamAnswerAction>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> UpdateJobApplicationExamsList(UpdateJobApplicationExamsListRecord jobApplications)
        {
            return await _jobApplicationExamsService.UpdateJobApplicationExamsList(jobApplications.Adapt<JobApplicationExams>());
        }

    }
}
