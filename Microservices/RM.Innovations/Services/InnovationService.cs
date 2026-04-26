
using Common;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Innovations.Dtos;
using RM.Innovations.UnitOfWorks;
using System.Text.Json;
using static RM.Innovations.Dtos.OperationOutput;

namespace RM.Innovations.Services
{
    public class InnovationService : BaseService, IInnovationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string InnovationPortalLink { get; set; }
        private string BeneficiarySMSMessage { get; set; }
        private readonly EmailConfiguration _emailConfig;

        public InnovationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, EmailConfiguration emailConfig, RedisConfiguration RedisConfiguration, IDistributedCache redisCache)
            : base(httpContextAccessor, unitOfWork.Configuration, RedisConfiguration, redisCache)
        {
            _unitOfWork = unitOfWork;
            InnovationPortalLink = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("InnovationPortalLink").Value;
            BeneficiarySMSMessage = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("BeneficiarySMSMessage").Value;
            _emailConfig = emailConfig;
        }
        public async Task<OperationOutput> Save(Dtos.Ideas RequestedData)
        {
            bool IsNew = false;
            string FileExtention = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.AttachmentBase64) ? Files.GetFileExtension(RequestedData.AttachmentBase64) : null;
            if (FileExtention != null && FileExtention != "pdf")
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = RequestedData.Id.HasValue ? _unitOfWork.Ideas.GetById(RequestedData.Id.Value) : null;
            var DbItemUser = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);

            if (DbItemUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            if (DbItem == null)
            {
                IsNew = true;
                DbItem = new Models.Idea();
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                var DbItemAction = new Models.IdeaAction();
                DbItemAction.CreatedDate = TransactionDate;
                DbItemAction.CreatedBy = RequestOwner.Id;
                DbItemAction.Type = (int)Enums.InnovationIdeaActions.New;
                DbItem.IdeaActions.Add(DbItemAction);
            }
            else
                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));

            if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(DbItemUser.Phone) || Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(DbItemUser.Email))
                if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.User.Phone) || Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.User.Email))
                    return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItemUser.Phone = RequestedData.User.Phone;
            DbItemUser.Email = RequestedData.User.Email;

            DbItem.Attachment = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.AttachmentBase64) ?
                Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + "." + FileExtention, RequestedData.AttachmentBase64, FilesSavePath) : DbItem.Attachment;


            if (IsNew)
            {
                _unitOfWork.Ideas.Add(DbItem);
                BeneficiarySMSMessage = BeneficiarySMSMessage.Replace("@Code", DbItem.Code.ToString());
                SMS.Send(DbItemUser.Phone, BeneficiarySMSMessage);
                await SendingEmailToBeneficiary(DbItemUser.Name,DbItemUser.Email, DbItem.Code.Value);
            }
            else _unitOfWork.Ideas.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                RedisCache.SetString("Ideas_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Code, DbItem.Code));
        }

        public async Task<OperationOutput> AddIdea940(Dtos.Ideas RequestedData)
        {
            var UserDbItem = _unitOfWork.Users.Find(x => x.Phone == RequestedData.User.Phone);
            if (UserDbItem == null)
            {
                UserDbItem = new Models.User();
                UserDbItem.Name = RequestedData.User.Name;
                UserDbItem.Phone = RequestedData.User.Phone;
                UserDbItem.Email = RequestedData.User.Email;
                _unitOfWork.Users.Add(UserDbItem);
            }
            else
            {
                UserDbItem.Email = RequestedData.User.Email;
            }
            await _unitOfWork.CompleteAsync();
            RequestedData.User.Id = UserDbItem.Id;
            RequestOwner.Id = UserDbItem.Id;
            return await Save(RequestedData);
        }

        public async Task<OperationOutput> AddPublicIdea(Dtos.Ideas RequestedData)
        {
            try
            {
                Models.Idea DbItem = new Models.Idea();
                if (UseCapcha)
                    if (string.IsNullOrEmpty(RequestedData.Capcha) || !GoogleCapcha.CheckCapchaSession(CapchaSecret, RequestedData.Capcha))
                        return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

                if (RequestOwner == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


                string FileExtention = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.AttachmentBase64) ? Files.GetFileExtension(RequestedData.AttachmentBase64) : null;
                if (FileExtention != null && FileExtention != "pdf")
                    return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);


                if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.MobileNo) || Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.Email))
                    return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

                RequestedData.Adapt(DbItem, RequestedData.AddPublicConfig(FileExtention, FilesSavePath));


                _unitOfWork.Ideas.Add(DbItem);
                await _unitOfWork.CompleteAsync();

                DbItem.LastActionId = DbItem.IdeaActions.FirstOrDefault().Id;
                _unitOfWork.Ideas.Update(DbItem);

                //   BeneficiarySMSMessage = BeneficiarySMSMessage.Replace("@Code", DbItem.Code.ToString());
                await _unitOfWork.CompleteAsync();

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.Code, DbItem.Code));
            }
            catch (Exception ex)
            {
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
            }
        }
        public async Task<OperationOutput> AddIdea(Dtos.Ideas RequestedData)
        {
            Models.User UserDbItem;
            UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            if (UserDbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            if (!Strings.CheckSaudiMobileNumber(UserDbItem.Phone) && !Strings.CheckSaudiMobileNumber(RequestedData.User.Phone))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (!Strings.CheckEmailValidity(UserDbItem.Email) && !Strings.CheckEmailValidity(RequestedData.User.Email))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            UserDbItem.Phone = Strings.CheckSaudiMobileNumber(RequestedData.User.Phone) ? RequestedData.User.Phone : UserDbItem.Phone;
            UserDbItem.Email = Strings.CheckEmailValidity(RequestedData.User.Email) ? RequestedData.User.Email : UserDbItem.Email;
            UserDbItem.ReferenceId = RequestedData.ReferenceId;
            _unitOfWork.Users.Update(UserDbItem);
            await _unitOfWork.CompleteAsync();
            return await Save(RequestedData);
        }
        public async Task<OperationOutput> GetUserIdeas()
        {
            var Item = _unitOfWork.Ideas.GetAll()
                .Include(x => x.StatusNavigation)
                .Include(x => x.IdeaActions).ThenInclude(v => v.TypeNavigation)
                .Where(x => x.CreatedBy == RequestOwner.Id && x.IsDeleted != true)
                .AsNoTracking().ToList()
                .Adapt<List<Dtos.Ideas>>(Dtos.Ideas.SelectConfig(FilesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.IdeaEntity, Item));
        }
        public async Task< OperationOutput> GetIdeasList(Dtos.Ideas RequestedData)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            if (UserDbItem == null || UserDbItem.JobRole == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            var UserJobRole = (Enums.JobRole) UserDbItem.JobRole;
            var AllowManageComments = (UserJobRole == Enums.JobRole.ContentManager) ? true : false;

            var Result = _unitOfWork.Ideas.GetAll()
                 .Include(x => x.CreatedByNavigation).Include(x => x.LastAction)
                 .ThenInclude(x => x.TypeNavigation)
                 .Include(x => x.PriorityNavigation).Include(x => x.StatusNavigation)
                 .Include(x => x.TypeNavigation).Include(x => x.CategoryNavigation)
                 .Where(x => UserJobRole == Enums.JobRole.EntityRepresentative ? (x.ToReference == UserDbItem.ReferenceId && x.LastAction.Type == (int)Enums.InnovationIdeaActions.TransToReference)
                 : UserJobRole == Enums.JobRole.DigitalTransformationCommittee ? (x.LastAction.Type == (int)Enums.InnovationIdeaActions.TransToJobRole)
                 : UserJobRole == Enums.JobRole.ContentManager ? true : false)
                 .Where(x => RequestedData.ActionId.HasValue ? x.IdeaActions.Count > 0 && x.LastAction.Type == RequestedData.ActionId : true)
                 .Where(RequestedData.Filteration(UserJobRole == Enums.JobRole.EntityRepresentative))
                 .OrderByDescending(x => x.CreatedDate)
                 .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var ResultDto = Result.Data.ToList().Adapt<List<Dtos.Ideas>>(Dtos.Ideas.SelectConfig(FilesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.IdeaEntity, ResultDto),
                  new OutputDictionary(OperationOutputKeys.Pagination, Result.Pagination),
                  new OutputDictionary(OperationOutputKeys.AllowManageComments, AllowManageComments));
        }

        public async Task<OperationOutput> GetPublicIdeasList(Dtos.Ideas RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Ideas_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Ideas_CacheDate");
                var Ideas = await RedisCache.GetStringAsync("Ideas");
                if (Ideas == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetPublicIdeas(RequestedData);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Ideas", JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Ideas_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Ideas, jsonOptions);
                return Result;
            }
            else
            {
                return await GetPublicIdeas(RequestedData);
            }
        }

        public async Task<OperationOutput> GetPublicIdeas(Dtos.Ideas RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var LatestCount = _unitOfWork.Ideas.GetAll().Include(x => x.LastAction).Where(x=> x.IsShow==true && x.ReferenceId==RequestedData.ReferenceId && x.LastAction.Type != (int)Enums.InnovationIdeaActions.New).Count();
            var ProgressCount = _unitOfWork.Ideas.GetAll().Include(x => x.LastAction).Where(x => x.IsShow == true && x.ReferenceId == RequestedData.ReferenceId && x.LastAction.Type == (int)Enums.InnovationIdeaActions.TransToJobRole).Count();
            var ExecutCount = _unitOfWork.Ideas.GetAll().Include(x => x.LastAction).Where(x => x.IsShow == true && x.ReferenceId == RequestedData.ReferenceId && x.LastAction.Type == (int)Enums.InnovationIdeaActions.TransToReference).Count();
            var CompleteCount = _unitOfWork.Ideas.GetAll().Include(x => x.LastAction).Where(x => x.IsShow == true && x.ReferenceId == RequestedData.ReferenceId && x.LastAction.Type == (int)Enums.InnovationIdeaActions.FinallyReplay).Count();

            var Result = _unitOfWork.Ideas.GetAll()
                .Include(x => x.CreatedByNavigation).Include(x => x.LastAction)
                .ThenInclude(x => x.TypeNavigation)
                .Include(x => x.PriorityNavigation).Include(x => x.StatusNavigation)
                .Include(x => x.TypeNavigation).Include(x => x.CategoryNavigation)
                .Where(RequestedData.Filteration())
                .Where(x => RequestedData.ActionId.HasValue ? x.LastAction != null && RequestedData.ActionId != (int)Enums.InnovationIdeaActions.New ? x.LastAction.Type == RequestedData.ActionId : x.LastAction.Type != (int)Enums.InnovationIdeaActions.New : true)
                .OrderByDescending(x => x.CreatedDate).AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var ResultDto = Result.Data.ToList().Adapt<List<Dtos.Ideas>>(Dtos.Ideas.SelectConfig(FilesGetPath));
            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Innovations, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.IdeaEntity, ResultDto),
                   new OutputDictionary(OperationOutputKeys.LatestCount, LatestCount),
                   new OutputDictionary(OperationOutputKeys.ProgressCount, ProgressCount),
                   new OutputDictionary(OperationOutputKeys.ExecutCount, ExecutCount),
                   new OutputDictionary(OperationOutputKeys.CompleteCount, CompleteCount),
                   new OutputDictionary(OperationOutputKeys.Pagination, Result.Pagination));

        }


        public async Task<OperationOutput> GetIdeasLookups()
        {
            IdeasLookups Item = new IdeasLookups();

            Item.Type = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.IdeaType).AsNoTracking().ToList().Adapt<List<IdeasLookups.Lookups>>();
            Item.Category = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.IdeaCategory).AsNoTracking().ToList().Adapt<List<IdeasLookups.Lookups>>();
            Item.Priority = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.IdeaPriority).AsNoTracking().ToList().Adapt<List<IdeasLookups.Lookups>>();
            Item.Status = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.IdeaStatus).AsNoTracking().ToList().Adapt<List<IdeasLookups.Lookups>>();
            Item.Actions = _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.IdeaActions).AsNoTracking().ToList().Adapt<List<IdeasLookups.Lookups>>();

            Item.Capability = new List<IdeasLookups.Lookups> 
            {
                new IdeasLookups.Lookups() { NameAr = "-- غير محدد--", Status = "0" },
                new IdeasLookups.Lookups() { NameAr = "قابلة للتطبيق", Status = "true" },
                new IdeasLookups.Lookups() { NameAr = "غير قابلة للتطبيق", Status = "false" }
            };

            Item.Feasibility = new List<IdeasLookups.Lookups>()
            {
                new IdeasLookups.Lookups() { NameAr = "-- غير محدد--", Status = "0" },
                new IdeasLookups.Lookups() { NameAr = "مجدية", Status = "true" },
                new IdeasLookups.Lookups() { NameAr = "غير مجدية", Status = "false" }
            };

            Item.NeedsBudget = new List<IdeasLookups.Lookups>()
            {
                new IdeasLookups.Lookups() { NameAr = "-- غير محدد--", Status = "0" },
                new IdeasLookups.Lookups() { NameAr = "تطلب", Status = "true" },
                new IdeasLookups.Lookups() { NameAr = "لا تطلب", Status = "false" }
            };

            Item.ToReference = _unitOfWork.IdeasCompetentAuthority.GetAll().Include(x => x.Reference).Select(x => new IdeasLookups.Lookups()
            {
                Id = x.ReferenceId,
                NameAr = x.Reference.NameAr,

            }).AsNoTracking().ToList();

            Item.ToJobRole = new List<IdeasLookups.Lookups>
            {
                new IdeasLookups.Lookups{Id=(int)Enums.JobRole.DigitalTransformationCommittee,NameAr="هيئة التحول الرقمي",NameEn="Digital Transformation Committee"},
            };

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.IdeaEntity, Item));

        }

        public async Task<OperationOutput> GetIdeasDetails(Dtos.Ideas RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Ideas_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Ideas_CacheDate");
                var Ideas = await RedisCache.GetStringAsync("Ideas_" + RequestedData.ID);
                if (Ideas == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetIdeasDetails(RequestedData.Id);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Ideas_" + RequestedData.ID, JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Ideas_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Ideas, jsonOptions);
                return Result;
            }
            else
            {
                return await GetIdeasDetails(RequestedData.Id);
            }
        }
        public async Task<OperationOutput> GetIdeasDetails(int? Id)
        {
            ActionPermission ActionPermission = new ActionPermission();
            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            var Item = _unitOfWork.Ideas.GetAll()
                .Include(x=>x.IdeaComments)
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.StatusNavigation)
                .Include(x => x.ToReferenceNavigation)
                .Include(x => x.TypeNavigation)
                .Include(x => x.CategoryNavigation)
                .Include(x => x.PriorityNavigation)
                .Include(x => x.LastAction)
                .Include(x => x.IdeaActions)
                .ThenInclude(x => x.CreatedByNavigation)
                .Include(x => x.IdeaActions)
                .ThenInclude(x => x.TypeNavigation)
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Ideas>(Dtos.Ideas.SelectConfig(FilesGetPath));

            switch (ItemDto.ActionId)
            {
                case (int)Enums.InnovationIdeaActions.New: { ItemDto.ActionId = (int)Enums.InnovationIdeaActions.TransToJobRole; break; }
                case (int)Enums.InnovationIdeaActions.TransToJobRole: { ItemDto.ActionId = (int)Enums.InnovationIdeaActions.TransToReference; break; }
                case (int)Enums.InnovationIdeaActions.TransToReference: { ItemDto.ActionId = (int)Enums.InnovationIdeaActions.RepliedFromReference; break; }
                case (int)Enums.InnovationIdeaActions.RepliedFromReference: { ItemDto.ActionId = (int)Enums.InnovationIdeaActions.FinallyReplay; break; }
                default: { ItemDto.ActionId = null; break; }
            }

            if (UserDbItem != null && UserDbItem.JobRole != null)
            {
               var UserJobRole = (Enums.JobRole)UserDbItem.JobRole;

                ActionPermission.AllowSave = (UserJobRole == Enums.JobRole.ContentManager && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.TransToJobRole ||
                            UserJobRole == Enums.JobRole.EntityRepresentative && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.RepliedFromReference) ? true : false;

                ActionPermission.AllowDecline = (UserJobRole == Enums.JobRole.ContentManager && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.TransToJobRole ||
                            UserJobRole == Enums.JobRole.DigitalTransformationCommittee && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.TransToReference) ? true : false;

                ActionPermission.AllowTransToJobRole = (UserJobRole == Enums.JobRole.ContentManager && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.TransToJobRole && ItemDto.Category.HasValue) ? true : false;
                ActionPermission.AllowTransToReference = (UserJobRole == Enums.JobRole.DigitalTransformationCommittee && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.TransToReference && ItemDto.Category.HasValue) ? true : false;

                ActionPermission.AllowClose = ((UserJobRole == Enums.JobRole.ContentManager && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.FinallyReplay)
                            || (UserJobRole == Enums.JobRole.EntityRepresentative && ItemDto.ActionId == (int)Enums.InnovationIdeaActions.RepliedFromReference
                            && ItemDto.Type.HasValue && ItemDto.Priority.HasValue && ItemDto.NeedsBudget.HasValue && ItemDto.Status.HasValue
                            && ItemDto.Feasibility.HasValue && ItemDto.Capability.HasValue && ItemDto.NeedsPeriod.HasValue)) ? true : false;

                ActionPermission.IsJobRoleContentManager = (UserJobRole == Enums.JobRole.ContentManager) ? true : false;
                ActionPermission.IsJobRoleEntityRepresentative = (UserJobRole == Enums.JobRole.EntityRepresentative) ? true : false;
                ActionPermission.IsJobRoleDigitalTransformationCommittee = (UserJobRole == Enums.JobRole.DigitalTransformationCommittee) ? true : false;

                ItemDto.ActionPermission = ActionPermission;

                //ItemDto.User = new Users()
                //{
                //    Name = Item.CreatedByNavigation != null ? Item.CreatedByNavigation.Name : string.Empty,
                //    Email = Item.CreatedByNavigation != null ? Item.CreatedByNavigation.Email : string.Empty,
                //    Phone = Item.CreatedByNavigation != null ? Item.CreatedByNavigation.Phone : string.Empty,
                //};
            }

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, (int)Enums.Entities.Innovations, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.IdeaEntity, ItemDto));
        }

        public async Task<OperationOutput> SaveIdeaDetail(Dtos.Ideas RequestedData)
        {
            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            var DbItem = _unitOfWork.Ideas.GetById(RequestedData.Id.Value);

            if (UserDbItem.JobRole == (int)Enums.JobRole.EntityRepresentative && (!DbItem.ToReference.HasValue || DbItem.ToReference != UserDbItem.ReferenceId))
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            if (UserDbItem.JobRole == (int)Enums.JobRole.ContentManager)
            {
                DbItem.IsShow = RequestedData.IsShow.HasValue ? RequestedData.IsShow.Value:DbItem.IsShow;
                DbItem.Category = RequestedData.Category;
            }
            else if (UserDbItem.JobRole == (int)Enums.JobRole.EntityRepresentative)
            {
                DbItem.Type = RequestedData.Type;
                DbItem.Priority = RequestedData.Priority;
                DbItem.Feasibility = RequestedData.Feasibility;
                DbItem.NeedsBudget = RequestedData.NeedsBudget;
                DbItem.Capability = RequestedData.Capability;
                DbItem.NeedsPeriod = RequestedData.NeedsPeriod;
                DbItem.Status = RequestedData.Status;
            }

            _unitOfWork.Ideas.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                await RedisCache.SetStringAsync("Ideas_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return await GetIdeasDetails(DbItem.Id);
        }
        public async Task<OperationOutput> IdeaAction(Dtos.Ideas RequestedData)
        {
            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            var DbItem = _unitOfWork.Ideas.GetAll().Include(x => x.IdeaActions).Where(x => x.Id == RequestedData.Id.Value).FirstOrDefault();
            var ActionDbItemLast = DbItem.IdeaActions.OrderByDescending(x => x.Id).FirstOrDefault();
            if (UserDbItem.JobRole == (int)Enums.JobRole.ContentManager && RequestedData.ActionId == (int)Enums.InnovationIdeaActions.TransToReference)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            if (UserDbItem.JobRole == (int)Enums.JobRole.DigitalTransformationCommittee)
            {
                if (RequestedData.ActionId == (int)Enums.InnovationIdeaActions.TransToReference && ((!DbItem.Category.HasValue && !RequestedData.Category.HasValue)
                    || !RequestedData.ActionId.HasValue || RequestedData.ActionId == (int)Enums.InnovationIdeaActions.New
                    || RequestedData.ActionId == (int)Enums.InnovationIdeaActions.RepliedFromReference
                    || (RequestedData.ActionId == (int)Enums.InnovationIdeaActions.TransToReference && !RequestedData.ToReference.HasValue)))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            }
            var ActionDbItem = new Models.IdeaAction();
            ActionDbItem.CreatedDate = TransactionDate;
            ActionDbItem.IdeaId = DbItem.Id;
            ActionDbItem.Type = RequestedData.ActionId;
            ActionDbItem.Note = RequestedData.ActionNote;
            ActionDbItem.CreatedBy = UserDbItem.Id;
            DbItem.Category = DbItem.Category ?? RequestedData.Category;

            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = TransactionDate;
            DbItem.ToReference = (UserDbItem.JobRole == (int)Enums.JobRole.DigitalTransformationCommittee && RequestedData.ActionId == (int)Enums.InnovationIdeaActions.TransToReference) ? RequestedData.ToReference : DbItem.ToReference;

            _unitOfWork.IdeaActions.Add(ActionDbItem);
            await _unitOfWork.CompleteAsync();
            DbItem.LastActionId = ActionDbItem.Id;

            _unitOfWork.Ideas.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            return await GetIdeasDetails(DbItem.Id);
        }
        public async Task<OperationOutput> SendingEmailToBeneficiary(string UserName,string UserEmail,long IdeaCode)
        {
            string Body = string.Empty;
            string Subject=string.Empty;
            List<MailboxAddress> MailBoxAddress = new List<MailboxAddress>();
            Message message = null;
            MimeMessage FinalMessage = null;

            if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(UserEmail))
                return GetOperationOutput(header: Enums.ServiceMessages.InValidData);

            using (StreamReader reader = new StreamReader(Path.Combine("Templates", "EmailToBeneficiary.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("@Name", UserName);
            Body = Body.Replace("@Link", InnovationPortalLink);
            Body = Body.Replace("@Code", IdeaCode.ToString());
            Subject = "منصة الابتكار الرقمي , مشاركة الافكار";


            MailBoxAddress.Add(new MailboxAddress(UserName, UserEmail));
            message = new Message(MailBoxAddress, Subject, Body);
            FinalMessage = Email.CreateEmailMessage(message, _emailConfig);
            Email.Send(FinalMessage, _emailConfig);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public byte[] Export(Dtos.Ideas RequestedData)
        {
            if (RequestOwner.Id == null)
                return null;

            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            var UserJobRole = (Enums.JobRole)UserDbItem.JobRole;

            var Item = _unitOfWork.Ideas.GetAll()
                .Include(x => x.CreatedByNavigation).Include(x => x.LastAction)
                .ThenInclude(x => x.TypeNavigation)
                .Include(x => x.PriorityNavigation).Include(x => x.StatusNavigation)
                .Include(x => x.TypeNavigation).Include(x => x.CategoryNavigation)
                .Where(x => UserJobRole == Enums.JobRole.EntityRepresentative ? (x.ToReference == UserDbItem.ReferenceId && x.LastAction.Type == (int)Enums.InnovationIdeaActions.TransToReference)
                : UserJobRole == Enums.JobRole.ContentManager ? true : false)
                .Where(RequestedData.Filteration(UserJobRole == Enums.JobRole.EntityRepresentative))
                .OrderByDescending(x => x.Id).Adapt<List<Dtos.IdeasExport>>(Dtos.IdeasExport.ExportConfig(FilesGetPath));


            var dataTable = Helpers.CreateDataTable(Item);
            var _data = _unitOfWork.Ideas.GetFileArray(dataTable);

            return _data;

        }

        public async Task<OperationOutput> IdeaAgreement(Dtos.Ideas RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsAgree.HasValue && !RequestedData.Id.HasValue))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Ideas.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            if (RequestedData.IsAgree == true)
                DbItem.AgreeCount += 1;
            else
                DbItem.DisAgreeCount += 1;

            _unitOfWork.Ideas.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                await RedisCache.SetStringAsync("Ideas_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return await GetIdeasDetails(DbItem.Id);
        }

        public async Task<OperationOutput> IdeaChangeAgreement(Dtos.Ideas RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsAgree.HasValue && !RequestedData.Id.HasValue))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Ideas.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsAgree == true)
            {
                DbItem.AgreeCount += 1;
                if (DbItem.DisAgreeCount > 0)
                    DbItem.DisAgreeCount -= 1;
            }
            else
            {
                DbItem.DisAgreeCount += 1;
                if (DbItem.AgreeCount > 0)
                    DbItem.AgreeCount -= 1;
            }

            _unitOfWork.Ideas.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                await RedisCache.SetStringAsync("Ideas_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return await GetIdeasDetails(DbItem.Id);
        }

        public async Task<OperationOutput> IdeasStatisticCounter(Dtos.Ideas RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Ideas_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("IdeasStatistic_CacheDate");
                var Ideas = await RedisCache.GetStringAsync("IdeasStatistic");
                if (Ideas == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await IdeasStatistic(RequestedData);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("IdeasStatistic", JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("IdeasStatistic_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Ideas, jsonOptions);
                return Result;
            }
            else
            {
                return await IdeasStatistic(RequestedData);
            }
        }
        public async Task<OperationOutput> IdeasStatistic(Dtos.Ideas RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var ideasCount = await _unitOfWork.Ideas.CountAsync(x => x.ReferenceId == RequestedData.ReferenceId);
            var ideasCommentsCount = await _unitOfWork.IdeaComments.GetAll().Include(i => i.Idea).CountAsync(x => x.Idea.ReferenceId == RequestedData.ReferenceId && x.IsApproved==true);
            var ideasVoteCount = await _unitOfWork.Ideas.GetAll().SumAsync(x=>x.AgreeCount+x.DisAgreeCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.IdeasCount, ideasCount),
                    new OutputDictionary(OperationOutputKeys.IdeasCommentsCount, ideasCommentsCount),
                    new OutputDictionary(OperationOutputKeys.IdeasVoteCount, ideasVoteCount));
        }

        public async Task<OperationOutput> TransferSuggestionToInnovation(Dtos.ContactUs RequestedData)
        {
            var DbItemUser = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);
            if (DbItemUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            var DbItem = new Models.Idea();
            DbItem.Code = Strings.RandomDigits(TransactionDate, 1, 9999999999);
            DbItem.IdeaAddress = RequestedData.Subject;
            DbItem.IdeaDescription = RequestedData.Description;
            DbItem.IsActive = true;
            DbItem.IsDeleted = false;
            DbItem.IdeaExist = RequestedData.IdeaExist;
            DbItem.Country = RequestedData.Country;
            DbItem.City = RequestedData.City;
            DbItem.CreatedBy = DbItemUser.Id;
            DbItem.CreatedDate = TransactionDate;        
            DbItem.Feasibility = true;
            DbItem.Capability = true;
            DbItem.FullName = RequestedData.Name;
            DbItem.MobileNo = RequestedData.MobileNo;
            DbItem.Email = RequestedData.Email;
            DbItem.EntityId = (int)Enums.Entities.Innovations;
            DbItem.ReferenceId = RequestedData.ReferenceId;

            var DbItemAction = new Models.IdeaAction();
            DbItemAction.CreatedDate = TransactionDate;
            DbItemAction.CreatedBy = RequestOwner.Id;
            DbItemAction.Type = (int)Enums.InnovationIdeaActions.New;
            DbItem.IdeaActions.Add(DbItemAction);

            _unitOfWork.Ideas.Add(DbItem);
            BeneficiarySMSMessage = BeneficiarySMSMessage.Replace("@Code", DbItem.Code.ToString());
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.Id, Accessor.Get(DbItem.Id)));
        }
    }
}

