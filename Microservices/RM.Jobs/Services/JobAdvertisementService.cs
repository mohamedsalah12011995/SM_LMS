

using RM.Jobs.Dtos;
using static RM.Jobs.Dtos.OperationOutput;
using RM.Core.Services;
using RM.Jobs.UnitOfWorks;
using RM.Core.Helpers;
using RM.Core.Consts;
using RM.Core.Integrations;
using Microsoft.EntityFrameworkCore;
using static RM.Core.Helpers.Enums;
using Mapster;
using DocumentFormat.OpenXml.Spreadsheet;

namespace RM.Jobs.Services
{
    public class JobAdvertisementService:BaseService,IJobAdvertisementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public JobAdvertisementService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetAdvertismentCareerLookup(Dtos.JobAdvertisement RequestedData)
        {
            var specifications = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Specifications && c.ReferenceId == RequestedData.ReferenceId)
                .ToList().Adapt<List<Dtos.JobLookUp>>();

            var tags = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Tags && c.ReferenceId == RequestedData.ReferenceId)
                .ToList().Adapt<List<Dtos.JobLookUp>>();

            var skills = _unitOfWork.JobLookUp.FindAll(c => c.TypeId == (int)JobLookupsType.Skills && c.ReferenceId == RequestedData.ReferenceId)
                .ToList().Adapt<List<Dtos.JobLookUp>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Specifications, specifications),
                   new OutputDictionary(OperationOutputKeys.Tags, tags),
                   new OutputDictionary(OperationOutputKeys.Skills, skills));
        }

        public async Task<OperationOutput> GetQualifications()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var qualifications = _unitOfWork.MajorLookup.FindAll(c => c.TypeId==(int)Enums.MajorLookupsTypes.Qualifications)
                .ToList().Adapt<List<Dtos.Qualification>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.Qualifications, qualifications));
        }

        public async Task< OperationOutput> GetJobAdvertiesmentList(Dtos.JobAdvertisement RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal == true || RequestUserRole == Enums.UsersRoles.NormalUser)
            {
                RequestedData.IsActive = true;
                RequestedData.IsContinuing = true;
            }
            var JobAdvertiesments = await _unitOfWork.JobAdvertisement.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending);

            var JobAdvertiesmentsDto = JobAdvertiesments.Data.ToList().Adapt<List<Dtos.JobAdvertisement>>(Dtos.JobAdvertisement.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Careers, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.JobAdvertisementEntity, JobAdvertiesmentsDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, JobAdvertiesments.Pagination));
        }

        public async Task<OperationOutput> SaveJobAdvertiesment(Dtos.JobAdvertisement RequestedData)
        {
            Models.JobAdvertisement DbItem = new Models.JobAdvertisement();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.JobAdvertisement.GetById(RequestedData.Id.Value);
                if(DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
            }

            if(RequestedData.jobCareers != null)
            foreach (var jobCareer in RequestedData.jobCareers)
            {
                if (jobCareer.Id == null)
                    FillJobCareer(DbItem, jobCareer,DbItem.Type);
                else
                {
                    var careerModel = _unitOfWork.JobCareers.GetAll().FirstOrDefault(x => x.Id == jobCareer.Id);
                    if (careerModel != null)
                    {
                        UpdateJobCareer(jobCareer, careerModel, DbItem);
                        _unitOfWork.JobCareers.Update(careerModel);
                    }
                }
            }
        
            #region Save Joblookups  
            SaveTages(RequestedData);
            SaveSpecifications(RequestedData);
            #endregion

            if (!RequestedData.Id.HasValue) _unitOfWork.JobAdvertisement.Add(DbItem);
            else _unitOfWork.JobAdvertisement.Update(DbItem);
          
            _unitOfWork.Complete();
            return await GetJobAdvertiesmentDetails(DbItem.Id);
        }

        private static void UpdateJobCareer(Dtos.JobCareer jobCareer, Models.JobCareer career,Models.JobAdvertisement dbItem)
        {       
            career.JobCondationsAr = jobCareer.JobCondationsAr;
            career.JobCondationsEn = jobCareer.JobCondationsEn;
            career.JobLocationAr = jobCareer.JobLocationAr;
            career.JobLocationEn = jobCareer.JobLocationEn;
            career.MaxLimit = jobCareer.MaxLimit;
            career.QualificationId = jobCareer.QualificationId;
            career.Skills = jobCareer.ListOfSkills != null ? Strings.ConvertListToString(jobCareer.ListOfSkills.Select(x => x.Name).ToList()) : null;
            career.Specifications = jobCareer.ListOfSpecifications != null ? Strings.ConvertListToString(jobCareer.ListOfSpecifications.Select(x => x.Name).ToList()) : null;
            career.SpecificationsEn = jobCareer.ListOfSpecifications != null ? Strings.ConvertListToString(jobCareer.ListOfSpecifications.Select(x => x.NameEn).ToList()) : null;
            career.Tags = jobCareer.ListOfTags != null ?Strings.ConvertListToString(jobCareer.ListOfTags.Select(x => x.Name).ToList()) : null;
            career.TitleAr = jobCareer.TitleAr;
            career.TitleEn = jobCareer.TitleEn;
            career.Type = dbItem.Type;
            career.IsNoticeBeneficiaries = jobCareer.IsNoticeBeneficiaries;
        }

        private void SaveTages(Dtos.JobAdvertisement RequestedData)
        {
            if (RequestedData.jobCareers.Any() && RequestedData.jobCareers.First().ListOfTags!=null &&  RequestedData.jobCareers.First().ListOfTags.Count()>0)
            {
                var newTags = RequestedData.jobCareers.SelectMany(x => x.ListOfTags.Where(t => t.Id == null || t.id == string.Empty)).ToList();
                if (newTags.Any())
                {
                    _unitOfWork.JobLookUp.AddRange(newTags.Select(t => new Models.JobLookUp
                    {
                        Name = t.Name,
                        NameEn = t.NameEn,
                        ReferenceId = RequestedData.ReferenceId,
                        TypeId = (int)JobLookupsType.Tags
                    }));
                }
            }
        }

        private void SaveSpecifications(Dtos.JobAdvertisement RequestedData)
        {
            if (RequestedData.jobCareers.Any() && RequestedData.jobCareers.First().ListOfSpecifications != null&& RequestedData.jobCareers.First().ListOfSpecifications.Count() > 0)
            {
                var newSpecifications = RequestedData.jobCareers.SelectMany(x => x.ListOfSpecifications.Where(t => t.Id == null || t.id == string.Empty)).ToList();
                if (newSpecifications.Any())
                {
                    _unitOfWork.JobLookUp.AddRange(newSpecifications.Select(t => new Models.JobLookUp
                    {
                        Name = t.Name,
                        NameEn = t.NameEn,
                        ReferenceId = RequestedData.ReferenceId,
                        TypeId = (int)JobLookupsType.Specifications
                    }));
                }
            }
        }

        private void SaveSkills(Dtos.JobAdvertisement RequestedData)
        {
            if (RequestedData.jobCareers.Any() && RequestedData.jobCareers.First().ListOfSkills!=null&& RequestedData.jobCareers.First().ListOfSkills.Count() > 0)
            {
                var newSkills = RequestedData.jobCareers.SelectMany(x => x.ListOfSkills.Where(t => t.Id == null || t.id == string.Empty)).ToList();
                if (newSkills.Any())
                {
                    _unitOfWork.JobLookUp.AddRange(newSkills.Select(t => new Models.JobLookUp
                    {
                        Name = t.Name,
                        NameEn = t.NameEn,
                        ReferenceId = RequestedData.ReferenceId,
                        TypeId = (int)JobLookupsType.Skills
                    }));
                }
            }
        }

        private static void FillJobCareer(Models.JobAdvertisement DbItem, Dtos.JobCareer jobCareer,int? type)
        {

            DbItem.JobCareers.Add(new Models.JobCareer
            {
                IsActive = false,
                IsDeleted = false,
                JobAdvertisementId = DbItem.Id,
                JobCondationsAr = jobCareer.JobCondationsAr,
                JobCondationsEn = jobCareer.JobCondationsEn,
                JobLocationAr = jobCareer.JobLocationAr,
                JobLocationEn = jobCareer.JobLocationEn,
                MaxLimit = jobCareer.MaxLimit,
                QualificationId = jobCareer.QualificationId,
                Skills = jobCareer.ListOfSkills !=null? Strings.ConvertListToString(jobCareer.ListOfSkills.Select(x => x.Name).ToList()):null,
                Specifications = jobCareer.ListOfSpecifications != null ? Strings.ConvertListToString(jobCareer.ListOfSpecifications.Select(x => x.Name).ToList()):null,
                SpecificationsEn = jobCareer.ListOfSpecifications != null ? Strings.ConvertListToString(jobCareer.ListOfSpecifications.Select(x => x.NameEn).ToList()) : null,

                Tags = jobCareer.ListOfTags != null ? Strings.ConvertListToString(jobCareer.ListOfTags.Select(x => x.Name).ToList()):null,
                TitleAr = jobCareer.TitleAr,
                TitleEn = jobCareer.TitleEn,
                IsNoticeBeneficiaries= jobCareer.IsNoticeBeneficiaries,
                Type=type

            });
        }

        public async Task< OperationOutput> GetJobAdvertiesmentDetails(Dtos.JobAdvertisement RequestedData)
        {
            return await GetJobAdvertiesmentDetails(RequestedData.Id);
        }
        public async Task<OperationOutput> GetJobAdvertiesmentDetails(int? Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.JobAdvertisement.GetAll()
                .Include( c => c.JobCareers)
                .Where( x=> x.Id==Id && x.IsDeleted==false)
                .Where(x=> IsPortal==true? x.EndDate.Date >= DateTime.Now.Date && x.StartDate.Date <= DateTime.Now.Date && x.IsActive==true:true)
                .AsNoTracking().FirstOrDefault();

            if (Item == null )
                return GetOperationOutput(header: Enums.ServiceMessages.AccessDenied);
            
            var ItemDto = Item.Adapt<Dtos.JobAdvertisement>(Dtos.JobAdvertisement.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.Careers, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.JobAdvertisementEntity, ItemDto));
        }
        public async Task<OperationOutput> ModelAction(Dtos.JobAdvertisement RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.JobAdvertisement.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;

            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = DateTime.Now;
            _unitOfWork.JobAdvertisement.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> CareerModelActions(Dtos.JobCareer RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.JobCareers.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            var deleteVal= RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;
            var careerApp = _unitOfWork.JobApplication.GetAll().Where(c => c.JobCareerId == RequestedData.Id).FirstOrDefault();

            if (deleteVal == true)
            {
                if (careerApp is null)
                    DbItem.IsDeleted = deleteVal;
                else
                    return GetOperationOutput(header: Enums.ServiceMessages.ErrorDeleteMessage);
            }
            else { DbItem.IsDeleted = deleteVal; }
         
            _unitOfWork.JobCareers.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task< OperationOutput> GetJobCareerDetails( Dtos.JobCareer RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var career = _unitOfWork.JobCareers.GetAll().Where(x => x.Id == RequestedData.Id.Value)
                .Include( c => c.Qualification)
                .Include(a=>a.JobAdvertisement)
                .FirstOrDefault();

            if (career == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);


            if (IsPortal == true)
               if (career.JobAdvertisement.StartDate.Date > TransactionDate.Date || career.JobAdvertisement.EndDate.Date < TransactionDate.Date || career.JobAdvertisement.IsActive!=true)
                  return GetOperationOutput(header: Enums.ServiceMessages.AccessDenied);

            var careerDto = career.Adapt<Dtos.JobCareer>(Dtos.JobCareer.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Careers, careerDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.JobAdvertisementEntity, careerDto));
        }

        public async Task <OperationOutput>SaveJobLookups(Dtos.JobLookUp RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.JobLookUp DbItem = new Models.JobLookUp();
            if (RequestedData.Id != null)
            {
                DbItem = _unitOfWork.JobLookUp.Find(c => c.Id == RequestedData.Id);
                if(DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig());
                _unitOfWork.JobLookUp.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig());
                _unitOfWork.JobLookUp.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

    }
}
