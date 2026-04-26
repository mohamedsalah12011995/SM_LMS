
using DocumentFormat.OpenXml.Office2010.Excel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Lookups.Dtos;
using RM.Lookups.Records;
using RM.Lookups.UnitOfWorks;

namespace RM.Lookups.Services
{
    public class CronSettingsService :BaseService,ICronSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CronSettingsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetLookups(CronSettings RequestedData)
        {
            var Entities = _unitOfWork.Entity.GetAll().AsNoTracking().ToList();
            var EntitiesDto = Entities.Adapt<List<Dtos.Recommendations>>();

            var CronTypes = new List<Recommendations> { 
                new Recommendations { Id = (int) Enums.CronType.WhenFinishToDate , NameAr = Helpers.CronName((int)Enums.CronType.WhenFinishToDate,"ar") ,NameEn = Helpers.CronName((int)Enums.CronType.WhenFinishToDate,"en")}, 
                new Recommendations { Id = (int) Enums.CronType.EveryDay , NameAr = Helpers.CronName((int)Enums.CronType.EveryDay,"ar") ,NameEn = Helpers.CronName((int)Enums.CronType.EveryDay,"en")} ,
                new Recommendations { Id = (int) Enums.CronType.EveryWeek , NameAr = Helpers.CronName((int)Enums.CronType.EveryWeek,"ar") ,NameEn = Helpers.CronName((int)Enums.CronType.EveryWeek,"en")} ,
                new Recommendations { Id = (int) Enums.CronType.EveryMonth , NameAr = Helpers.CronName((int)Enums.CronType.EveryMonth,"ar") ,NameEn = Helpers.CronName((int)Enums.CronType.EveryMonth,"en")} ,
                new Recommendations { Id = (int) Enums.CronType.EveryQuaters , NameAr = Helpers.CronName((int)Enums.CronType.EveryQuaters,"ar") ,NameEn = Helpers.CronName((int)Enums.CronType.EveryQuaters,"en")}         
            };
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary("CronTypes", CronTypes),
                new OutputDictionary(OperationOutput.OperationOutputKeys.Entities, EntitiesDto));
        }

        public async Task<OperationOutput> GetUsersLookup(CronSettings RequestedData)
        {
            //var Users = await _unitOfWork.UsersEntity.GetAll().Include(x => x.User).Where(x => x.EntityId == RequestedData.EntityId && x.Reports == true)
            //    .Select(x => x.User).Where(u => !string.IsNullOrEmpty(u.Email)).OrderBy(x=>x.Name)
            //    .AsNoTracking().ToListAsync();


            var Users = await _unitOfWork.Users.GetAll().Where(x=>x.Name.Contains(RequestedData.SearchWord) || x.Email.Contains(RequestedData.SearchWord))
                    .Where(u => !string.IsNullOrEmpty(u.Email))
                    .AsNoTracking().ToListAsync();

            var UsersDto = Users.Where(x => Strings.CheckEmailValidity(x.Email)).Adapt<List<Dtos.Users>>();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary("UsersList", UsersDto));
        }

        public async Task<OperationOutput> GetAllAsync(CronSettings dto)
        {
            var CronSettingsList =  _unitOfWork.CronSettings.GetAll().Where(dto.Filteration()).AsNoTracking().ToList();
            var CronSettingsDtos = CronSettingsList.Adapt<List<CronSettings>>(CronSettings.SelectConfig());

            foreach(var cron in  CronSettingsDtos)
            {
                var Users= _unitOfWork.Users.GetAll().Where(x => cron.Emails.Contains(x.Email)).ToList();
                cron.Users = Users.Adapt< List<Dtos.Users>>();
            }

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutput.OperationOutputKeys.CronSettings, CronSettingsDtos));
        }

        public async Task<OperationOutput> SaveAsync(List<CronSettings> RequestedData)
        {
            foreach (var dto in RequestedData)
            {
                if (dto.Id.HasValue)
                {
                    var Item = await _unitOfWork.CronSettings.GetAll().Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
                    if (Item == null)
                        return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                    dto.Adapt(Item, dto.UpdateConfig(RequestOwner.Id));
                    _unitOfWork.CronSettings.Update(Item);
                }
                else
                {
                    var Exist = _unitOfWork.CronSettings.Count(x => x.EntityId == dto.EntityId && x.SubEntityId == dto.SubEntityId && x.CronTypeId == dto.CronTypeId) > 0;
                    if (Exist)
                        return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

                    var Item = dto.Adapt(new RM.Models.CronSettings(), dto.AddConfig(RequestOwner.Id));
                    _unitOfWork.CronSettings.Add(Item);
                }
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetAsync(CronSettings dto)
        {
            return await GetAsync(dto.Id);
        }


        private async Task<OperationOutput> GetAsync(int? Id)
        {
            var Item = await _unitOfWork.CronSettings.GetAll().Where(x => x.Id == Id)
                .AsNoTracking().FirstOrDefaultAsync();

            if (Item == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt(new CronSettings(), CronSettings.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutput.OperationOutputKeys.CronSettings, ItemDto));
        }

        public async Task<OperationOutput> AddAsync(CronSettings dto)
        {
            var Exist = _unitOfWork.CronSettings.Count(x => x.EntityId == dto.EntityId && x.CronTypeId == dto.CronTypeId) > 0;
            if (Exist)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.ValueIsExist);

            var Item = dto.Adapt(new RM.Models.CronSettings(), dto.AddConfig(RequestOwner.Id));
            _unitOfWork.CronSettings.Add(Item);
            await _unitOfWork.CompleteAsync();
            return await GetAsync(Item.Id);
        }


        public async Task<OperationOutput> UpdateAsync(CronSettings dto)
        {
            var Item = await _unitOfWork.CronSettings.GetAll().Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (Item == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            dto.Adapt(Item, dto.UpdateConfig(RequestOwner.Id));

            _unitOfWork.CronSettings.Update(Item);
            await _unitOfWork.CompleteAsync();

            return await GetAsync(Item.Id);
        }

        public async Task<OperationOutput> DeleteAsync(CronSettings dto)
        {

            var Item = await _unitOfWork.CronSettings.GetByIdAsync(dto.Id.Value);
            if (Item == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            _unitOfWork.CronSettings.Delete(Item);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

    }
}
