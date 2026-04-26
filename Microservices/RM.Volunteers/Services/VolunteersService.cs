
using RM.Volunteers.Dtos;
using RM.Volunteers.UnitOfWorks;
using static RM.Volunteers.Dtos.OperationOutput;
using RM.Core.Services;
using RM.Core.Helpers;
using RM.Core.Integrations;
using Mapster;
using RM.Core.Consts;
using System.Data.Entity;

namespace RM.Volunteers.Services
{
    public class VolunteersService:BaseService,IVolunteersService
    {
        private readonly IUnitOfWork _unitOfWork;
        public VolunteersService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
      
        public async Task<OperationOutput> SaveVolunteers(Dtos.Volunteers RequestdData)
        {
            Models.Volunteer DbItem = new Models.Volunteer();

            if (UseCapcha)
                if (string.IsNullOrEmpty(RequestdData.Capcha) || !GoogleCapcha.CheckCapchaSession(CapchaSecret, RequestdData.Capcha))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if (RequestdData.Id.HasValue)
            {
                DbItem = _unitOfWork.Volunteers.GetById(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Volunteers.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Volunteers.Add(DbItem);
            }

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestdData.ReferenceId, (int)Enums.Entities.Volunteers, null, Enums.InteractionStatisticsType.ViewsCount);

            await _unitOfWork.CompleteAsync();
            return await GetVolunteersDetails(DbItem.Id);
        }

        public async Task<OperationOutput> GetVolunteersList(Dtos.Volunteers RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var result = await _unitOfWork.Volunteers.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, 
                OrderBy.Descending,a => a.Age, d => d.District, g => g.Gender, q => q.Qualification);

            var resultDto = result.Data.Adapt<List<Dtos.Volunteers>>(Dtos.Volunteers.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.VolunteersEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Volunteers)),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }

        public async Task<OperationOutput> GetVolunteersLookups()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var AgeRanges = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.AgeRange)
                .OrderByDescending(x => x.Id)
                .AsNoTracking().ToList().Adapt<List<Dtos.VolunteersLookups>>();

            var Districts = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.District)
                .OrderByDescending(x => x.Id)
                .AsNoTracking().ToList().Adapt<List<Dtos.VolunteersLookups>>();

            var Qualifications = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.Qualifications)
                .OrderByDescending(x => x.Id)
                .AsNoTracking().ToList().Adapt<List<Dtos.VolunteersLookups>>();

            var Genders = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.Gender)
                .OrderByDescending(x => x.Id)
                .AsNoTracking().ToList().Adapt<List<Dtos.VolunteersLookups>>();


            var VolunteeringFields = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.VolunteeringField)
                .OrderByDescending(x => x.Id)
                .AsNoTracking().ToList().Adapt<List<Dtos.VolunteersLookups>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Genders, Genders),
                   new OutputDictionary(OperationOutputKeys.AgeRanges, AgeRanges),
                   new OutputDictionary(OperationOutputKeys.Districts, Districts),
                   new OutputDictionary(OperationOutputKeys.Qualifications, Qualifications),
                   new OutputDictionary(OperationOutputKeys.VolunteeringFields, VolunteeringFields));
        }

        public async Task<OperationOutput> GetVolunteersDetails(Dtos.Volunteers RequestedData)
        {
            return await GetVolunteersDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetVolunteersDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Volunteers.FindAll(x=>x.Id==Id, a => a.Age,d => d.District, g => g.Gender, q => q.Qualification,f=>f.VolunteerField)
                   .FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Volunteers>(Dtos.Volunteers.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.VolunteersEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ItemDto.entityId));
        }
    }
}
