
using Microsoft.EntityFrameworkCore;
using RM.OpenData.Dtos;
using RM.OpenData.UnitOfWorks;
using RM.Core.Services;
using RM.Core.Helpers;
using Mapster;
using static RM.OpenData.Dtos.OperationOutput;
using Microsoft.Extensions.Caching.Memory;


namespace RM.OpenData.Services
{
    public class OpenDataTempService:BaseService, IOpenDataTempService
    {
        private string OpenDataIntegrationUrl = "";
        private readonly IUnitOfWork _unitOfWork;

        public OpenDataTempService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            OpenDataIntegrationUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("OpenDataIntegrationUrl").Value;
        }

        public async Task<OperationOutput> SyncWithOpenDataTemp(Dtos.RequestRecords RequestedData)
        {
            int newData = 0;
            int updatedData = 0;
            List<Models.OpenDataTemp> ItemsList = new List<Models.OpenDataTemp>();
            Models.OpenDataTemp DbItem;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var RequestRecord = new { categoryId = RequestedData.CategoryId, regionId = RequestedData.RegionId, year = RequestedData.Year, fromMonth = RequestedData.FromMonth, toMonth = RequestedData.ToMonth , IsHijri=!RequestedData.IsGregorian };
            var OpenDataResult = await InvokeService<OpenDataIntegration>.Invoke(OpenDataIntegrationUrl, RequestRecord);

            if (OpenDataResult.Header == null || !OpenDataResult.Header.IsSuccess)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var UserNavigation = _unitOfWork.User.Find(x => x.Id == RequestOwner.Id);
            OpenDataStatistics Statistics = OpenDataResult.Body.OpenDataStatistics;
            switch (RequestedData.CategoryId)
            {
                case (int)Enums.Category.Arresting:
                    {
                        foreach (var od in Statistics.ArrestingDetailed)
                        {
                            int nd, ud = 0;
                            (nd, ud, DbItem) = SaveArrestingDetailedRecord(od, (int)Enums.OpenDataSubTypes.BorderSecurityViolators, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveArrestingDetailedRecord(od, (int)Enums.OpenDataSubTypes.ResidenceViolators, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                        }
                        break;
                    }
                case (int)Enums.Category.Infiltration:
                    {
                        foreach (var od in Statistics.InfiltrationDetailed)
                        {
                            int nd, ud = 0;
                            (nd, ud, DbItem) = SaveInfiltrationDetailedRecord(od, (int)Enums.OpenDataSubTypes.Infiltrations, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                        }
                        break;
                    }
                case (int)Enums.Category.Smuggling:
                    {
                        foreach (var od in Statistics.SmugglingDetailed)
                        {
                            int nd, ud = 0;
                            (nd, ud, DbItem) = SaveSmugglingDetailedRecord(od, (int)Enums.OpenDataSubTypes.Smuggling, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                        }
                        break;
                    }
                case (int)Enums.Category.SearchShareMission:
                    {
                        foreach (var od in Statistics.SearchShareMissionDetailed)
                        {
                            int nd, ud = 0;
                            (nd, ud, DbItem) = SaveSearchShareMissionDetailedRecord(od, (int)Enums.OpenDataSubTypes.MarineRescue, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveSearchShareMissionDetailedRecord(od, (int)Enums.OpenDataSubTypes.WildRescue, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveSearchShareMissionDetailedRecord(od, (int)Enums.OpenDataSubTypes.ProvideBacking, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                        }
                        break;
                    }
                case (int)Enums.Category.Violations:
                    {
                        foreach (var od in Statistics.ViolationsDetailed)
                        {
                            int nd, ud = 0;
                            (nd, ud, DbItem) = SaveViolationsDetailedRecord(od, (int)Enums.OpenDataSubTypes.Hashish, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveViolationsDetailedRecord(od, (int)Enums.OpenDataSubTypes.Khat, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveViolationsDetailedRecord(od, (int)Enums.OpenDataSubTypes.Shabu, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveViolationsDetailedRecord(od, (int)Enums.OpenDataSubTypes.ProhibitedDrugs, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                            (nd, ud, DbItem) = SaveViolationsDetailedRecord(od, (int)Enums.OpenDataSubTypes.NarcoticPills, RequestedData.ReferenceId, RequestedData.IsGregorian, UserNavigation);
                            newData += nd; updatedData += ud; ItemsList.Add(DbItem);
                        }
                        break;
                    }
                default: { break; }
            }

            await _unitOfWork.CompleteAsync();
            var ItemsListDto = ItemsList.Adapt<List<Dtos.OpenData>>(Dtos.OpenData.SelectDataTempConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.OpenDataTempEntity, ItemsListDto),
                       new OutputDictionary(OperationOutputKeys.NewOpenDataTempCount, newData),
                       new OutputDictionary(OperationOutputKeys.UpdatedOpenDataTempCount, updatedData));
        }

        private (int, int, Models.OpenDataTemp DbItem) SaveArrestingDetailedRecord(ArrestingDetailed od, int typeId, int? referenceId,bool? IsGregorian,Models.User UserNavigation)
        {
            Models.OpenDataTemp DbItem;
            int newData = 0;
            int updatedData = 0;
            DbItem = _unitOfWork.OpenDataTemp.GetAll().Include(d => d.District).Where(x => x.Year == od.Year && x.Month == od.Month && x.District.MapId == od.RegionId && x.TypeId == typeId && x.ReferenceId == referenceId).FirstOrDefault();

            if (DbItem != null)
            {
                if (typeId == (int)Enums.OpenDataSubTypes.BorderSecurityViolators)
                {
                    DbItem.Value = od.BordersSecuritySysViolatorsCount;
                    updatedData++;
                }
                if (typeId == (int)Enums.OpenDataSubTypes.ResidenceViolators)
                {
                    DbItem.Value = od.AccomodationSecuritySysViolatorsCount;
                    updatedData++;
                }

                DbItem.ModifiedBy = RequestOwner.Id;
                DbItem.ModifiedDate = TransactionDate;
                DbItem.ModifiedByNavigation = UserNavigation;
                DbItem.CreatedByNavigation = _unitOfWork.User.Find(x => x.Id == DbItem.CreatedBy);

                DbItem.IsConfirm = false;
                _unitOfWork.OpenDataTemp.Update(DbItem);
            }
            else
            {

                DbItem = new Models.OpenDataTemp();
                DbItem.CreatedBy = RequestOwner.Id;
                DbItem.CreatedByNavigation = UserNavigation;
                DbItem.CreatedDate = TransactionDate;
                DbItem.ReferenceId = referenceId;
                DbItem.DistrictId = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District).Id;
                DbItem.District = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District);
                DbItem.Type = _unitOfWork.MajorLookup.Find(d => d.Id == typeId);
                DbItem.TypeId = typeId;
                DbItem.Year = od.Year;
                DbItem.Month = od.Month;
                DbItem.IsConfirm = false;
                DbItem.IsGregorian = IsGregorian;

                if (typeId == (int)Enums.OpenDataSubTypes.BorderSecurityViolators) 
                    DbItem.Value = od.BordersSecuritySysViolatorsCount;

                if (typeId == (int)Enums.OpenDataSubTypes.ResidenceViolators) 
                    DbItem.Value = od.AccomodationSecuritySysViolatorsCount;

                newData++;
                _unitOfWork.OpenDataTemp.Add(DbItem);
            }

            return (newData, updatedData,DbItem);
        }

        private (int, int, Models.OpenDataTemp DbItem) SaveInfiltrationDetailedRecord(InfiltrationDetailed od, int typeId, int? referenceId, bool? IsGregorian, Models.User UserNavigation)
        {
            Models.OpenDataTemp DbItem;
            int newData = 0;
            int updatedData = 0;
            DbItem = _unitOfWork.OpenDataTemp.GetAll().Include(d => d.District).Where(x => x.Year == od.Year && x.Month == od.Month && x.District.MapId == od.RegionId && x.TypeId == typeId && x.ReferenceId == referenceId).FirstOrDefault();

            if (DbItem != null)
            {
                DbItem.Value = od.Count;
                updatedData++;
                DbItem.ModifiedBy = RequestOwner.Id;
                DbItem.ModifiedDate = TransactionDate;
                DbItem.ModifiedByNavigation = UserNavigation;
                DbItem.CreatedByNavigation = _unitOfWork.User.Find(x => x.Id == DbItem.CreatedBy);
                DbItem.IsConfirm = false;
                _unitOfWork.OpenDataTemp.Update(DbItem);
            }
            else
            {
                DbItem = new Models.OpenDataTemp();
                DbItem.CreatedBy = RequestOwner.Id;
                DbItem.CreatedByNavigation = UserNavigation;
                DbItem.CreatedDate = TransactionDate;
                DbItem.ReferenceId = referenceId;
                DbItem.DistrictId = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District).Id;
                DbItem.District = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District);
                DbItem.Type = _unitOfWork.MajorLookup.Find(d => d.Id == typeId);
                DbItem.TypeId = typeId;
                DbItem.Year = od.Year;
                DbItem.Month = od.Month;
                DbItem.Value = od.Count;
                DbItem.IsConfirm = false;
                DbItem.IsGregorian = IsGregorian;
                newData++;
                _unitOfWork.OpenDataTemp.Add(DbItem);
            }

            return (newData, updatedData, DbItem);
        }

        private (int, int, Models.OpenDataTemp DbItem) SaveSmugglingDetailedRecord(SmugglingDetailed od, int typeId, int? referenceId, bool? IsGregorian, Models.User UserNavigation)
        {
            Models.OpenDataTemp DbItem;
            int newData = 0;
            int updatedData = 0;
            DbItem = _unitOfWork.OpenDataTemp.GetAll().Include(d => d.District).Where(x => x.Year == od.Year && x.Month == od.Month && x.District.MapId == od.RegionId && x.TypeId == typeId && x.ReferenceId == referenceId).FirstOrDefault();

            if (DbItem != null)
            {
                DbItem.Value = od.Count;
                updatedData++;
                DbItem.ModifiedBy = RequestOwner.Id;
                DbItem.ModifiedDate = TransactionDate;
                DbItem.ModifiedByNavigation = UserNavigation;
                DbItem.CreatedByNavigation = _unitOfWork.User.Find(x => x.Id == DbItem.CreatedBy);
                DbItem.IsConfirm = false;
                _unitOfWork.OpenDataTemp.Update(DbItem);
            }
            else
            {
                DbItem = new Models.OpenDataTemp();
                DbItem.CreatedBy = RequestOwner.Id;
                DbItem.CreatedByNavigation = UserNavigation;
                DbItem.CreatedDate = TransactionDate;
                DbItem.ReferenceId = referenceId;
                DbItem.DistrictId = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District).Id;
                DbItem.District = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District);
                DbItem.Type = _unitOfWork.MajorLookup.Find(d => d.Id == typeId);
                DbItem.TypeId = typeId;
                DbItem.Year = od.Year;
                DbItem.Month = od.Month;
                DbItem.Value = od.Count;
                DbItem.IsConfirm = false;
                DbItem.IsGregorian = IsGregorian;
                newData++;
                _unitOfWork.OpenDataTemp.Add(DbItem);
            }

            return (newData, updatedData, DbItem);
        }


        private (int, int, Models.OpenDataTemp DbItem) SaveSearchShareMissionDetailedRecord(SearchShareMissionDetailed od, int typeId, int? referenceId, bool? IsGregorian, Models.User UserNavigation)
        {
            Models.OpenDataTemp DbItem;
            int newData = 0;
            int updatedData = 0;
            DbItem = _unitOfWork.OpenDataTemp.GetAll().Include(d => d.District).Where(x => x.Year == od.Year && x.Month == od.Month && x.District.MapId == od.RegionId && x.TypeId == typeId && x.ReferenceId == referenceId).FirstOrDefault();

            if (DbItem != null)
            {
                if (typeId == (int)Enums.OpenDataSubTypes.MarineRescue)
                {
                    DbItem.Value = od.MarinCount;
                    updatedData++;
                }
                if (typeId == (int)Enums.OpenDataSubTypes.WildRescue)
                {
                    DbItem.Value = od.WildRescue;
                    updatedData++;
                }

                if (typeId == (int)Enums.OpenDataSubTypes.ProvideBacking)
                {
                    DbItem.Value = od.ProvideBacking;
                    updatedData++;
                }
                DbItem.ModifiedBy = RequestOwner.Id;
                DbItem.ModifiedDate = TransactionDate;
                DbItem.ModifiedByNavigation = UserNavigation;
                DbItem.CreatedByNavigation = _unitOfWork.User.Find(x => x.Id == DbItem.CreatedBy);
                DbItem.IsConfirm = false;

                _unitOfWork.OpenDataTemp.Update(DbItem);

            }
            else
            {
                DbItem = new Models.OpenDataTemp();
                DbItem.CreatedBy = RequestOwner.Id;
                DbItem.CreatedByNavigation = UserNavigation;
                DbItem.CreatedDate = TransactionDate;
                DbItem.ReferenceId = referenceId;
                DbItem.DistrictId = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District).Id;
                DbItem.District = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District);
                DbItem.Type = _unitOfWork.MajorLookup.Find(d => d.Id == typeId);
                DbItem.TypeId = typeId;
                DbItem.Year = od.Year;
                DbItem.Month = od.Month;
                DbItem.IsConfirm = false;
                DbItem.IsGregorian = IsGregorian;

                if (typeId == (int)Enums.OpenDataSubTypes.MarineRescue)
                    DbItem.Value = od.MarinCount; ;

                if (typeId == (int)Enums.OpenDataSubTypes.WildRescue)
                    DbItem.Value = od.WildRescue;

                if (typeId == (int)Enums.OpenDataSubTypes.ProvideBacking)
                    DbItem.Value = od.ProvideBacking; ;

                newData++;
                _unitOfWork.OpenDataTemp.Add(DbItem);
            }

            return (newData, updatedData,DbItem);
        }

        private (int, int, Models.OpenDataTemp DbItem) SaveViolationsDetailedRecord(ViolationsDetailed od, int typeId, int? referenceId, bool? IsGregorian, Models.User UserNavigation)
        {
            Models.OpenDataTemp DbItem;
            int newData = 0;
            int updatedData = 0;
            DbItem = _unitOfWork.OpenDataTemp.GetAll().Include(d => d.District).Where(x => x.Year == od.Year && x.Month == od.Month && x.District.MapId == od.RegionId && x.TypeId == typeId && x.ReferenceId == referenceId).FirstOrDefault();

            if (DbItem != null)
            {
                if (typeId == (int)Enums.OpenDataSubTypes.Hashish)
                {
                    DbItem.Value = od.HayKgCount;
                    updatedData++;
                }
                if (typeId == (int)Enums.OpenDataSubTypes.Khat)
                {
                    DbItem.Value = od.QatKgCount;
                    updatedData++;
                }

                if (typeId == (int)Enums.OpenDataSubTypes.Shabu)
                {
                    DbItem.Value = od.ShabuKgCount;
                    updatedData++;
                }

                if (typeId == (int)Enums.OpenDataSubTypes.ProhibitedDrugs)
                {
                    DbItem.Value = od.ProhibitedDrugsCount;
                    updatedData++;
                }

                if (typeId == (int)Enums.OpenDataSubTypes.NarcoticPills)
                {
                    DbItem.Value = od.NarcoticPillsCount;
                    updatedData++;
                }
                DbItem.ModifiedBy = RequestOwner.Id;
                DbItem.ModifiedDate = TransactionDate;
                DbItem.ModifiedByNavigation = UserNavigation;
                DbItem.CreatedByNavigation = _unitOfWork.User.Find(x => x.Id == DbItem.CreatedBy);
                DbItem.IsConfirm = false;

                _unitOfWork.OpenDataTemp.Update(DbItem);
            }
            else
            {
                DbItem = new Models.OpenDataTemp();
                DbItem.CreatedBy = RequestOwner.Id;
                DbItem.CreatedByNavigation = UserNavigation;
                DbItem.CreatedDate = TransactionDate;
                DbItem.ReferenceId = referenceId;
                DbItem.DistrictId = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District).Id;
                DbItem.District = _unitOfWork.MajorLookup.Find(x => x.MapId == od.RegionId && x.TypeId == (int)Enums.MajorLookupsTypes.District);
                DbItem.Type = _unitOfWork.MajorLookup.Find(d => d.Id == typeId);
                DbItem.TypeId = typeId;
                DbItem.Year = od.Year;
                DbItem.Month = od.Month;
                DbItem.IsConfirm = false;
                DbItem.IsGregorian = IsGregorian;

                if (typeId == (int)Enums.OpenDataSubTypes.Hashish)
                    DbItem.Value = od.HayKgCount; ;

                if (typeId == (int)Enums.OpenDataSubTypes.Khat)
                    DbItem.Value = od.QatKgCount;

                if (typeId == (int)Enums.OpenDataSubTypes.Shabu)
                    DbItem.Value = od.ShabuKgCount; ;

                if (typeId == (int)Enums.OpenDataSubTypes.ProhibitedDrugs)
                    DbItem.Value = od.ProhibitedDrugsCount;

                if (typeId == (int)Enums.OpenDataSubTypes.NarcoticPills)
                    DbItem.Value = od.NarcoticPillsCount; ;

                newData++;
                _unitOfWork.OpenDataTemp.Add(DbItem);
            }

            return (newData, updatedData, DbItem);
        }
        public async Task<OperationOutput> GetOpenDataTempList(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            bool AllowPublish = false;
            Models.User UserDbItem = _unitOfWork.User.GetAll().Where(x => x.Id == RequestOwner.Id).FirstOrDefault();
            if (UserDbItem != null)
                AllowPublish = ((Enums.JobRole)UserDbItem.JobRole == Enums.JobRole.MediaOffice) ? true : false;

            var result = _unitOfWork.OpenDataTemp.GetAll()
                .Where(RequestedData.FilterationDataTemp())
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .Include(x => x.District)
                .Include(x => x.Type)
                .OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate)
                .ThenByDescending(a => a.CreatedDate)
                .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.OpenData>>(Dtos.OpenData.SelectDataTempConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.OpenDataTempEntity, resultDto),
                       new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination),
                       new OutputDictionary(OperationOutputKeys.AllowPublish, AllowPublish));
        }

        public async Task<OperationOutput> GetOpenDataTempDetails(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = await _unitOfWork.OpenDataTemp.GetAll()
                 .Where(x => x.Id == RequestedData.Id)
                 .AsNoTracking().FirstOrDefaultAsync();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.OpenData>(Dtos.OpenData.SelectDataTempConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataTempEntity, ItemDto));
        }

        public async Task<OperationOutput> GetOpenDataTempByFiledDetails(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = await _unitOfWork.OpenDataTemp.GetAll()
                 .Where(x => x.Year == RequestedData.Year && x.Month == RequestedData.Month && x.DistrictId == RequestedData.DistrictId && x.TypeId == RequestedData.TypeId && x.ReferenceId == RequestedData.ReferenceId)
                 .AsNoTracking().FirstOrDefaultAsync();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.OpenData>(Dtos.OpenData.SelectDataTempConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataTempEntity, ItemDto));
        }

        public async Task<OperationOutput> DeleteOpenDataTemp(List<Dtos.OpenData> RequestedData)
        {
           if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

           await _unitOfWork.OpenDataTemp.ExecuteDeleteAsync(x => RequestedData.Select(x => x.Id).ToList().Contains(x.Id));
           await _unitOfWork.CompleteAsync();

           return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }


        public async Task<OperationOutput> ConfirmOpenDataTemp(List<Dtos.OpenData> RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            await _unitOfWork.OpenDataTemp.ExecuteUpdateAsync(x => RequestedData.Select(x => x.Id).ToList().Contains(x.Id),
                    sett => sett.SetProperty(x=>x.IsConfirm,true));
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }


        public async Task<OperationOutput> SaveOpenDataTemp(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = _unitOfWork.OpenDataTemp.GetAll()
                .Where(x => x.Year == RequestedData.Year && x.Month == RequestedData.Month && x.DistrictId == RequestedData.DistrictId && x.TypeId == RequestedData.TypeId && x.ReferenceId == RequestedData.ReferenceId)
                .FirstOrDefault();

            if (DbItem != null)
            {
                RequestedData.Adapt(DbItem, RequestedData.UpdateDataTempConfig(RequestOwner.Id));
                _unitOfWork.OpenDataTemp.Update(DbItem);
            }
            else
            {
                DbItem = new Models.OpenDataTemp();
                RequestedData.Adapt(DbItem, RequestedData.AddDataTempConfig(RequestOwner.Id));
                _unitOfWork.OpenDataTemp.Add(DbItem);
            }

           await _unitOfWork.CompleteAsync();

           return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.OpenDataTempEntity, DbItem.Adapt<Dtos.OpenData>(Dtos.OpenData.SelectDataTempConfig())));
        }


        public async Task<OperationOutput> SyncAllWithOpenData(Dtos.OpenData RequestedData)
        {

            int newData = 0;
            int updatedData = 0;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var OpenDataTemps = _unitOfWork.OpenDataTemp.GetAll()
                .Where(x => x.ReferenceId == RequestedData.ReferenceId).ToList();

            foreach(var od in OpenDataTemps)
            {
                Models.OpenData DbItem;
                DbItem = _unitOfWork.OpenData.GetAll()
                    .Where(x => x.Year == od.Year && x.Month == od.Month && x.DistrictId == od.DistrictId && x.TypeId == od.TypeId && x.ReferenceId == od.ReferenceId)
                    .FirstOrDefault();

                if (DbItem != null)
                {
                    if (DbItem.Value != od.Value)
                    {
                        DbItem.Value = od.Value;
                        updatedData++;
                    }
                    DbItem.ModifiedBy = RequestOwner.Id;
                    DbItem.ModifiedDate = TransactionDate;
                    _unitOfWork.OpenData.Update(DbItem);
                }
                else
                {
                    DbItem = new Models.OpenData();
                    DbItem.CreatedBy = RequestOwner.Id;
                    DbItem.CreatedDate = TransactionDate;
                    DbItem.ReferenceId = RequestedData.ReferenceId;
                    DbItem.DistrictId = od.DistrictId;
                    DbItem.TypeId = od.TypeId;
                    DbItem.Year = od.Year;
                    DbItem.Month = od.Month;
                    DbItem.Value = od.Value;
                    DbItem.IsGregorian = od.IsGregorian;
                    newData++;
                    _unitOfWork.OpenData.Add(DbItem);
                }
            }

            _unitOfWork.OpenDataTemp.DeleteRange(OpenDataTemps);
            await _unitOfWork.CompleteAsync();
            _unitOfWork.MemoryCache.Set("OpenDataLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.NewOpenDataTempCount, newData),        
                   new OutputDictionary(OperationOutputKeys.UpdatedOpenDataTempCount, updatedData));
        }


        public async Task<OperationOutput> SyncWithOpenData(List<Dtos.OpenData> RequestedDataList)
        {
            OperationOutput Result = new OperationOutput();

            int newData = 0;
            int updatedData = 0;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            foreach (var od in RequestedDataList)
            {
                Models.OpenData DbItem;

                DbItem = _unitOfWork.OpenData.GetAll()
                    .Where(x => x.Year == od.Year && x.Month == od.Month && x.DistrictId == od.DistrictId && x.TypeId == od.TypeId && x.ReferenceId==od.ReferenceId)
                    .FirstOrDefault();

                if (DbItem != null)
                {
                    DbItem.Value = od.Value;
                    updatedData++;
                    DbItem.ModifiedBy = RequestOwner.Id;
                    DbItem.ModifiedDate = TransactionDate;
                    _unitOfWork.OpenData.Update(DbItem);

                }
                else
                {
                    DbItem = new Models.OpenData();
                    DbItem.CreatedBy = RequestOwner.Id;
                    DbItem.CreatedDate = TransactionDate;
                    DbItem.ReferenceId = od.ReferenceId;
                    DbItem.DistrictId = od.DistrictId;
                    DbItem.TypeId = od.TypeId;
                    DbItem.Year = od.Year;
                    DbItem.Month = od.Month;
                    DbItem.Value = od.Value;
                    DbItem.IsGregorian = od.IsGregorian;

                    newData++;
                    _unitOfWork.OpenData.Add(DbItem);

                }
            }

            await _unitOfWork.OpenDataTemp.ExecuteDeleteAsync(x => RequestedDataList.Select(x => x.Id).ToList().Contains(x.Id));
            await _unitOfWork.CompleteAsync();
            _unitOfWork.MemoryCache.Set("OpenDataLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.NewOpenDataTempCount, newData),
                   new OutputDictionary(OperationOutputKeys.UpdatedOpenDataTempCount, updatedData));
        }

        public async Task<OperationOutput> ConfirmAllOpenDataTemp(Dtos.OpenData RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            List<Models.OpenDataTemp> OpenDataTemps = new List<Models.OpenDataTemp>();
            Models.OpenDataTemp DbItem;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            OpenDataTemps = _unitOfWork.OpenDataTemp.GetAll().Where(x=>x.ReferenceId==RequestedData.ReferenceId).ToList();

            foreach (var od in OpenDataTemps)
            {
                await _unitOfWork.OpenDataTemp.ExecuteUpdateAsync(x => x.Year == od.Year && x.Month == od.Month && x.DistrictId == od.DistrictId && x.TypeId == od.TypeId && x.ReferenceId == od.ReferenceId,
                        sett => sett.SetProperty(x => x.IsConfirm, true)
                                    .SetProperty(x => x.ModifiedBy, RequestOwner.Id)
                                    .SetProperty(x => x.ModifiedDate, TransactionDate));
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.UpdatedOpenDataTempCount, OpenDataTemps.Count()));
        }
    }
}
