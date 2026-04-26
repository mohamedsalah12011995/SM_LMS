using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RM.Core.Consts;
using RM.Core.Extensions;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Users.Dtos;
using RM.Users.UnitOfWorks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static RM.Users.Dtos.OperationOutput;
using static RM.Users.Dtos.Users;

namespace RM.Users.Services
{
    public class UsersService : BaseService, IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string activeDirectoryServices;
        public List<int> allowedAdminLogin;
        int userADDefaultReference = 0;

        string userLoginADUrl = string.Empty;
        string userLoginADIsValid = string.Empty;
        string domainName;
        string SMSServiceUrl;
        int innovationLoginSourceReferenceId;
        int fitrEventLoginSourceReferenceId;
        public UsersService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            InitConfigurations(_unitOfWork.Configuration);
            SetUserLoginFromActiveDirectoryProp();
            GetIAMLoginSourcesFromConfiguration();
        }

        #region HELPER METHODS >> CONSTRACTOR
        private void InitConfigurations(IConfiguration Configuration)
        {

            activeDirectoryServices = Configuration.ReadConfigurationFromSection("ActiveDirectoryService");

            allowedAdminLogin = new List<int>()
            {
                (int)Enums.UsersRoles.Administrator,
                (int)Enums.UsersRoles.Reporter
            };
        }
        private void SetUserLoginFromActiveDirectoryProp()
        {
            userLoginADUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("UserLoginADUrl");
            userLoginADIsValid = _unitOfWork.Configuration.ReadConfigurationFromSection("UserLoginADIsValid");
            userADDefaultReference = int.Parse(_unitOfWork.Configuration.ReadConfigurationFromSection("UserADDefaultReference"));
            domainName = _unitOfWork.Configuration.ReadConfigurationFromSection("DomainName");
            SMSServiceUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("SMSServiceUrl");
        }

        private void GetIAMLoginSourcesFromConfiguration()
        {
            innovationLoginSourceReferenceId = int.Parse(_unitOfWork.Configuration.ReadConfigurationFromSection("InnovationLoginSourceReferenceId"));
            fitrEventLoginSourceReferenceId = int.Parse(_unitOfWork.Configuration.ReadConfigurationFromSection("FitrEventLoginSourceReferenceId"));
        }


        #endregion

        public async Task<OperationOutput> SaveUser(Dtos.Users RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.Password))
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);


            Models.User userModel;

            if (RequestedData.Id.HasValue)
            {
                userModel = await _unitOfWork.User.GetAll(u => u.Id == RequestedData.Id.Value)
                                .Include(c => c.UsersEntities)
                                .Include(c => c.UsersEntitiesReferences).FirstOrDefaultAsync();

                if (userModel == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                _unitOfWork.User.Update(RequestedData.Adapt(userModel, RequestedData.UpdateConfig(RequestOwner.Id)));
            }
            else
            {
                var duplicated = await _unitOfWork.User.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.UserName == RequestedData.UserName);

                if (duplicated is not null)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);
                userModel = new Models.User();
                _unitOfWork.User.Add(RequestedData.Adapt(userModel, RequestedData.AddConfig(RequestOwner.Id)));
            }

            AddOrUpdateUserEntities(RequestedData, ref userModel);
            AndOrUpdateUsersEntitiesReferences(RequestedData, ref userModel);

            await _unitOfWork.CompleteAsync();

            RequestedData.Id = userModel.Id;
            return await GetUserDetails(RequestedData);

        }

        #region HELPER METHODS >> SAVE USER

        private void AddOrUpdateUserEntities(Dtos.Users RequestedData, ref Models.User userModel)
        {

            foreach (var userEntityId in userModel.UsersEntities.Select(x => x.EntityId.Value).Where(x => !RequestedData.Entities.Select(z => z.EntityId.Value).Contains(x)).ToList())
            {
                var itemToDelete = userModel.UsersEntities.Where(x => x.EntityId == userEntityId).FirstOrDefault();
                if (itemToDelete != null)
                {
                    userModel.UsersEntities.Remove(itemToDelete);
                    _unitOfWork.UsersEntity.Delete(itemToDelete);
                }
            }

            foreach (var userEntity in RequestedData.Entities)
            {
                if (IsAllPermissionNullOrFalse(userEntity))
                {
                    var itemToDelete = userModel.UsersEntities.Where(x => x.EntityId == userEntity.EntityId).FirstOrDefault();
                    if (itemToDelete != null)
                    {
                        userModel.UsersEntities.Remove(itemToDelete);
                        _unitOfWork.UsersEntity.Delete(itemToDelete);
                    }
                }
                else
                {
                    var isNew = false;
                    var item = userModel.UsersEntities.FirstOrDefault(x => x.EntityId == userEntity.EntityId);
                    if (item == null)
                    {
                        isNew = true;
                        item = new Models.UsersEntity();
                        item.EntityId = userEntity.EntityId;
                        item.UserId = RequestedData.Id;
                    }

                    item.Add = userEntity.Add;
                    item.Edit = userEntity.Edit;
                    item.Delete = userEntity.Delete;
                    item.Activate = userEntity.Activate;
                    item.View = userEntity.View;
                    item.Reports = userEntity.Reports;
                    item.List = userEntity.List;

                    if (isNew)
                        userModel.UsersEntities.Add(item);
                    else
                        _unitOfWork.UsersEntity.Update(item);

                }
            }
        }
        private void AndOrUpdateUsersEntitiesReferences(Dtos.Users RequestedData, ref Models.User userModel)
        {
            foreach (var userEntitiesRef in userModel.UsersEntitiesReferences.Select(x => (x.EntityId.Value, x.ReferenceId.Value)).Where(x => !RequestedData.UsersEntitiesReferences.Select(z => (z.EntityId.Value, z.ReferenceId.Value)).Contains(x)).ToList())
            {
                var itemToDelete = userModel.UsersEntitiesReferences.Where(x => x.EntityId == userEntitiesRef.Item1 && x.ReferenceId == userEntitiesRef.Item2).FirstOrDefault();
                userModel.UsersEntitiesReferences.Remove(itemToDelete);
                _unitOfWork.UsersEntityReference.Delete(itemToDelete);
            }

            foreach (var userEntityReference in RequestedData.UsersEntitiesReferences)
            {
                if (IsAllPermissionNullOrFalse(userEntityReference))
                {

                    var itemToDelete = userModel.UsersEntitiesReferences.Where(x => x.EntityId == userEntityReference.EntityId && x.ReferenceId == userEntityReference.ReferenceId).FirstOrDefault();
                    if (itemToDelete != null)
                    {
                        userModel.UsersEntitiesReferences.Remove(itemToDelete);
                        _unitOfWork.UsersEntityReference.Delete(itemToDelete);
                    }
                }
                else
                {
                    var isNew = false;
                    var item = userModel.UsersEntitiesReferences.FirstOrDefault(x => x.EntityId == userEntityReference.EntityId && x.ReferenceId == userEntityReference.ReferenceId);
                    if (item == null)
                    {
                        isNew = true;
                        item = new Models.UsersEntityReference();
                        item.ReferenceId = userEntityReference.ReferenceId;
                        item.EntityId = userEntityReference.EntityId;
                        item.UserId = RequestedData.Id;
                    }

                    item.Add = userEntityReference.Add;
                    item.Edit = userEntityReference.Edit;
                    item.Delete = userEntityReference.Delete;
                    item.Activate = userEntityReference.Activate;
                    item.View = userEntityReference.View;
                    item.Reports = userEntityReference.Reports;
                    item.List = userEntityReference.List;

                    if (isNew)
                        userModel.UsersEntitiesReferences.Add(item);
                    else
                        _unitOfWork.UsersEntityReference.Update(item);
                }
            }
        }

        bool IsAllPermissionNullOrFalse(object userObj)
        {
            return userObj.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(bool?))
                .Select(pi => (bool?)pi.GetValue(userObj))
                .All(value => value != true);
        }

        #endregion

        public OperationOutput GenerateToken2(string userName)
        {
            Dtos.Users user = new Dtos.Users();
            Dtos.Users RequestedData = new Dtos.Users();

            RequestedData.desiredUserName = userName;
            var obj = new { RequestedData.desiredUserName };

            var userAD = InvokeService<UserAD>.Invoke(userLoginADUrl, obj).GetAwaiter().GetResult();
            if (userAD.Body is not null && userAD.Body.userInformation is not null)
            {
                user = CreateUserFromActiveDirctory(userAD, RequestedData);
            }

            user.UserName = userName;
            if (user.Password is not null)
                user.Password = Cryptography.AES.Encrypt(user.Password);

            return GenerateUserToken(user);
        }

        public async Task<OperationOutput> UserLoginOTP(Dtos.Users RequestedData)
        {
            UserAD userAD = null;

            var userloginModel = await _unitOfWork.User.GetAll().Include(c => c.UsersEntities)
                 .Include(r => r.Reference).ThenInclude(r => r.ReferencesMajor)
                 .Where(x => x.UserName == RequestedData.UserName).AsNoTracking().FirstOrDefaultAsync();

            var userloginDto = userloginModel.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

            if (userloginDto == null)
                userAD = CreateUserIfExistsInTheActiveDirectory(RequestedData, ref userloginDto);


            if (userloginDto == null && userAD.Header.IsSuccess == false)
                return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);


            if (userloginDto.LoginWayId == (int)Enums.LoginWay.Password)
            {
                userloginDto.LoginWayId = null;
                //  if (userloginDto.Password.Trim() != RequestedData.Password.Trim())
                if (!BCrypt.Net.BCrypt.Verify(RequestedData.Password, userloginDto.Password))
                    return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);

            }

            if (userloginDto.IsBlocked == true)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);

            if (userloginDto.LoginWayId == (int)Enums.LoginWay.ActiveDirectory)
            {
                var userACD = new
                {
                    username = RequestedData.UserName,
                    password = RequestedData.Password,
                };

                var _userAD = InvokeService<UserAD>.Invoke(userLoginADIsValid, userACD).GetAwaiter().GetResult();
                if (_userAD.Body.loginSuccess == false)
                    return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);
            }

            //if (userloginDto.ReferenceId != null)
            //    userloginDto.ReferenceRootId = GetRoot(userloginDto.ReferenceId);

            //userloginDto.Password = null;
            //userloginDto.UsersEntitiesReferences = GetUsersEntityReference(userloginDto);
            var sms = new
            {
                number = userloginModel.Phone,
                message = Strings.RandomDigits(4).ToString()
            };

            var sendOTP = InvokeService<SMSIntegration>.Invoke(SMSServiceUrl, sms).GetAwaiter().GetResult();

            if (sendOTP.Header.IsSuccess == true)
            {
                userloginModel.OTP = sms.message;
                userloginModel.OTPDate = DateTime.Now;
                _unitOfWork.User.Update(userloginModel);
                await _unitOfWork.CompleteAsync();

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }
            else
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        public async Task<OperationOutput> CheckOTP(Dtos.Users RequestedData)
        {
            var userloginModel = await _unitOfWork.User.GetAll().Include(c => c.UsersEntities)
                 .Include(r => r.Reference).ThenInclude(r => r.ReferencesMajor)
                 .Where(x => x.UserName == RequestedData.UserName).AsNoTracking().FirstOrDefaultAsync();

            if (userloginModel != null && userloginModel.OTP == RequestedData.Otp && userloginModel.OTPDate.Value.AddMinutes(10) < TransactionDate)
                return await UserLogin(RequestedData);
            return GetOperationOutput(header: Enums.ServiceMessages.InValidOTP);
        }

        public async Task<OperationOutput> UserLogin(Dtos.Users RequestedData)
        {
            UserAD userAD = null;

            var userloginModel = await _unitOfWork.User.GetAll().Include(c => c.UsersEntities)
                 .Include(r => r.Reference).ThenInclude(r => r.ReferencesMajor)
                 .Where(x => x.UserName == RequestedData.UserName).AsNoTracking().FirstOrDefaultAsync();

            var userloginDto = userloginModel.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

            if (userloginDto == null)
                userAD = CreateUserIfExistsInTheActiveDirectory(RequestedData, ref userloginDto);


            if (userloginDto == null && userAD.Header.IsSuccess == false)
                return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);


            if (userloginDto.LoginWayId == (int)Enums.LoginWay.Password)
            {
                userloginDto.LoginWayId = null;
                if (userloginDto.Password.Trim() != RequestedData.Password.Trim())
                    return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);

            }

            if (userloginDto.IsBlocked == true)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);

            if (userloginDto.LoginWayId == (int)Enums.LoginWay.ActiveDirectory)
            {
                var userACD = new
                {
                    username = RequestedData.UserName,
                    password = RequestedData.Password,
                };

                var _userAD = InvokeService<UserAD>.Invoke(userLoginADIsValid, userACD).GetAwaiter().GetResult();
                if (_userAD.Body.loginSuccess == false)
                    return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);
            }

            if (userloginDto.ReferenceId != null)
                userloginDto.ReferenceRootId = GetRoot(userloginDto.ReferenceId);

            userloginDto.Password = null;
            userloginDto.UsersEntitiesReferences = GetUsersEntityReference(userloginDto);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.UserEntity, userloginDto),
             new OutputDictionary(OperationOutputKeys.Entities, userloginDto.Entities),
             new OutputDictionary(OperationOutputKeys.UserJWT, GenerateToken(userloginDto)));
        }

        public async Task<OperationOutput> ChangeUserReference(Dtos.Users RequestedData)
        {
            var userModel = await _unitOfWork.User.GetAll().Include(c => c.UsersEntities)
                .Include(r => r.Reference)
                 .ThenInclude(r => r.ReferencesMajor).Where(x => x.Id == RequestedData.Id).AsNoTracking().FirstOrDefaultAsync();

            var userDto = userModel.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

            userDto.Password = null;

            userDto.UsersEntitiesReferences = GetUsersEntityReference(userDto);

            if (userDto.ReferenceId != RequestedData.ReferenceId)
            {
                userDto.ReferenceRootId = GetRoot(RequestedData.ReferenceId);

                var entities = await _unitOfWork.UsersEntityReference.GetAll(x => x.UserId == userDto.Id && x.ReferenceId == RequestedData.ReferenceId).AsNoTracking().ToListAsync();

                userDto.Entities = entities.Adapt<List<Dtos.EntitiesItems>>();

                userDto.ReferenceId = RequestedData.ReferenceId;
                userDto.ReferenceName = userDto.UsersEntitiesReferences.Any() ? userDto.UsersEntitiesReferences.FirstOrDefault(x => x.ReferenceId == RequestedData.ReferenceId)?.NameAr : string.Empty;
                userDto.ReferenceNameEn = userDto.UsersEntitiesReferences.Any() ? userDto.UsersEntitiesReferences.FirstOrDefault(x => x.ReferenceId == RequestedData.ReferenceId)?.NameEn : string.Empty;
                userDto.ReferenceUrl = userDto.UsersEntitiesReferences.Any() ? userDto.UsersEntitiesReferences.FirstOrDefault(x => x.ReferenceId == RequestedData.ReferenceId)?.ReferenceUrl : string.Empty;

            }
            else userDto.ReferenceRootId = GetRoot(userDto.ReferenceId);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.UserEntity, userDto),
             new OutputDictionary(OperationOutputKeys.Entities, userDto.Entities),
             new OutputDictionary(OperationOutputKeys.UserJWT, GenerateToken(userDto)));
        }

        #region HELPER METHODS >> UserLogin & ChangeUserReference
        private UserAD CreateUserIfExistsInTheActiveDirectory(Dtos.Users RequestedData, ref Dtos.Users userloginDto)
        {
            UserAD userAD;
            var userActiveDirectory = new { desiredUserName = RequestedData.UserName };
            userAD = InvokeService<UserAD>.Invoke(userLoginADUrl, userActiveDirectory).GetAwaiter().GetResult();

            if (userAD.Body is not null && userAD.Body.userInformation is not null)
                userloginDto = CreateUserFromActiveDirctory(userAD, RequestedData);
            return userAD;
        }

        private Dtos.Users CreateUserFromActiveDirctory(UserAD userAD, Dtos.Users RequestedData)
        {
            var userModel = _unitOfWork.User.GetAll().AsNoTracking().FirstOrDefault(f => f.UserName == RequestedData.desiredUserName);
            if (userModel == null)
            {
                userModel = new Models.User();
                _unitOfWork.User.Add(RequestedData.Adapt(userModel, RequestedData.AddUserADConfig(userAD, RequestedData, userADDefaultReference)));
                _unitOfWork.Complete();

            }

            var reference = _unitOfWork.References.GetAll()
               .Include(c => c.ReferencesMajor)
               .FirstOrDefault(u => u.Id == userModel.ReferenceId);

            return userModel.Adapt<Dtos.Users>(Dtos.Users.CustomMapUserADConfig(reference));

        }
        private List<Dtos.UsersEntityReference> GetUsersEntityReference(Dtos.Users userDto)
        {
            List<Dtos.UsersEntityReference> UserReferences = new List<Dtos.UsersEntityReference>() { new Dtos.UsersEntityReference { ReferenceId = userDto.ReferenceId, NameAr = userDto.ReferenceName, NameEn = userDto.ReferenceNameEn, ReferenceUrl = userDto.ReferenceUrl } };
            UserReferences.AddRange(_unitOfWork.UsersEntityReference.FindAll(u => u.UserId == userDto.Id, r => r.Reference)
                            .GroupBy(x => new { x.ReferenceId }, (key, group) => new Dtos.UsersEntityReference
                            {
                                ReferenceId = key.ReferenceId,
                                NameAr = group.First()?.Reference?.NameAr,
                                NameEn = group.First()?.Reference?.NameEn,
                                ReferenceUrl = group.First()?.Reference?.Url
                            }).ToList());
            return UserReferences;
        }

        private int GetRoot(int? id)
        {
            var current = _unitOfWork.References.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (current.ParentId is not null)
                return GetRoot(current.ParentId.Value);

            else
                return current.Id;
        }

        #endregion

        public OperationOutput CheckUserLogged()
        {
            if (!string.IsNullOrEmpty(domainName))
            {
                var _domainName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[0];
                var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];

                if (_domainName == domainName)
                {
                    var obj = new { desiredUserName = userName };

                    // call api InternalServices/AD/GetUserInfo

                    var userAD = InvokeService<UserAD>.Invoke(userLoginADUrl, obj).Result;

                    if (userAD.Body is not null && userAD.Body.userInformation is not null)
                    {
                        var userDto = userAD.Adapt<Dtos.Users>(Dtos.Users.MapUserFromUserADConfig(userAD, userADDefaultReference));

                        return GetOperationOutput(header: Enums.ServiceMessages.loginSuccess,
                        new OutputDictionary(OperationOutputKeys.UserEntity, userDto),
                        new OutputDictionary(OperationOutputKeys.UserJWT, GenerateToken(userDto)));
                    }

                    else return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);
                }
                else return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);
            }
            else return GetOperationOutput(header: Enums.ServiceMessages.InvalidUserNameOrPassword);
        }

        public async Task<OperationOutput> GetUserInfoByToken(string Token)
        {
            JWTHelper.Users jwTItem = JWTHelper.DecryptToken(Token);
            if (jwTItem is not null)
            {
                var user = await _unitOfWork.User.GetAll(x => x.Id == jwTItem.Id).AsNoTracking().FirstOrDefaultAsync();
                var userDto = user.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKeys.UserEntity, userDto));
            }
            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
        }

        public async Task<OperationOutput> IAMLogin(string IdNo, string DobHijri, string IdType, string LoginSources)
        {
            int ReferenceId = 0;

            if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(LoginSources))
                return GetOperationOutput(header: Enums.ServiceMessages.LoginNoPermission);

            // DetectLoginSource
            Enums.IAMLoginSources enumLoginSource = GetLoginSource(LoginSources, ref ReferenceId);

            if (enumLoginSource == 0)
                return GetOperationOutput(header: Enums.ServiceMessages.LoginNoPermission);

            var geogionDate = Dates.ConvertUmAlquraStringDateToGeorogian(DobHijri);
            var hijriDate = Dates.ConvertFromGerogianToHijri(geogionDate);

            var userModel = await _unitOfWork.User.GetAll(x => x.IdCardNumber == IdNo.Trim()).AsNoTracking().FirstOrDefaultAsync();

            var userDto = userModel.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

            if (userDto == null)
            {
                Yaqeen itemYaqeenInfo = Integration.Yaqeen.GetYaqeenInfo(IdNo, hijriDate);

                if (itemYaqeenInfo.UserType == 0)
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

                var userItem = new Models.User();
                _unitOfWork.User.Add(itemYaqeenInfo.Adapt(userItem, Dtos.Users.AddUserFromYaqeenIntegrationConfig(itemYaqeenInfo, ReferenceId, IdNo, geogionDate)));
                _unitOfWork.Complete();

                userDto = new Dtos.Users();
                userDto.Id = userItem.Id;
                userDto.Name = userItem.Name;
                userDto.HasPhoneNumber = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(userItem.Phone) ? false : true;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.UserJWT, GenerateToken(userDto)));
        }

        #region HELPER METHOD >> IAMLogin
        private Enums.IAMLoginSources GetLoginSource(string LoginSources, ref int ReferenceId)
        {
            Enums.IAMLoginSources enumLoginSource;
            switch (LoginSources.ToLower())
            {
                case "innovation":
                    {
                        enumLoginSource = Enums.IAMLoginSources.Innovation;
                        ReferenceId = innovationLoginSourceReferenceId;
                        break;
                    }
                case "fitrevent":
                    {
                        enumLoginSource = Enums.IAMLoginSources.FitrEvent;
                        ReferenceId = fitrEventLoginSourceReferenceId;
                        break;

                    }
                default:
                    {
                        enumLoginSource = 0;
                        break;
                    }
            }

            return enumLoginSource;
        }

        #endregion


        public async Task<OperationOutput> GetUserLookps()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var permissionEtities = _unitOfWork.Entity.GetAll().AsNoTracking().Where(c => c.ParentId == null)
                .Include(c => c.InverseParent).ToList();

            var permissionEtitiesDto = permissionEtities.Adapt<List<Dtos.Entity>>(Dtos.Entity.SelectConfig());

            var userRoles = await _unitOfWork.Role.GetAll().AsNoTracking().ToListAsync();
            var userRolesDto = userRoles.Adapt<List<Dtos.Roles>>();

            var MajorReferences = await _unitOfWork.ReferencesMajor.GetAll().Where(x => x.IsDeleted != true)
                .Include(x => x.References)
                .ThenInclude(x => x.AdminMenus)
                .Include(x => x.References)
                .ThenInclude(x => x.ReferencesJobRoles)
                .ThenInclude(x => x.JobRole)
                .AsNoTracking().ToListAsync();

            var MajorReferencesDto = MajorReferences.Adapt<List<Dtos.MajorReference>>(Dtos.MajorReference.SelectConfig());
            var portalReferencesDto = MajorReferences.Adapt<List<Dtos.MajorReference>>(Dtos.MajorReference.PortalSelectConfig());
            var majorTree = FillMajorTree(MajorReferencesDto);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.PermissionEtities, permissionEtitiesDto),
               new OutputDictionary(OperationOutputKeys.UserRoles, userRolesDto),
               new OutputDictionary(OperationOutputKeys.UserMajorReference, MajorReferencesDto),
               new OutputDictionary(OperationOutputKeys.UserMajorReferenceTree, majorTree),
               new OutputDictionary(OperationOutputKeys.PortalReferences, portalReferencesDto));

        }

        #region HELPER METHODS >> GetUserLookps
        private List<MajorReferenceTree> FillMajorTree(List<Dtos.MajorReference> userMajorReferenceDto)
        {
            List<MajorReferenceTree> MajorTree = new List<MajorReferenceTree>();
            foreach (var major in userMajorReferenceDto)
            {
                var mt = major.Adapt<MajorReferenceTree>(MajorReferenceTree.SelectConfig());
                mt.References = GenerateUserReferenceTree(major.References, null, null, null).Adapt<List<UserReferenceTree>>();
                MajorTree.Add(mt);
            }
            return MajorTree;
        }


        public IEnumerable<Dtos.UserReferenceTree> GenerateUserReferenceTree(List<Dtos.Users.UserReference> references, int? parentId, List<ReferenceJobRole> ReferenceJobRole, List<AdminMenuUserDto> AdminMenu)
        {
            foreach (var r in references.Where(r => r.ParentId == parentId))
            {
                var t = r.Adapt<Dtos.UserReferenceTree>(Dtos.UserReferenceTree.SelectConfig(ReferenceJobRole, AdminMenu));
                t.Children = GenerateUserReferenceTree(references, r.Id, t.ReferenceJobRole, t.AdminMenu);
                yield return t;
            }
        }

        #endregion


        public async Task<OperationOutput> GetTotalUserLookups()
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            OperationOutput Result = new OperationOutput();

            var loginWay = _unitOfWork.LoginWay.GetAll().Select(x => new Dtos.LoginWay() { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).ToList();

            Result = await GetUserLookps();
            Result.Output.Add(OperationOutputKeys.LoginWay, loginWay);
            return Result;
        }

        public OperationOutput GenerateUserToken(Dtos.Users user = null)
        {
            if (user == null)
                user = new Dtos.Users();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.UserJWT, GenerateToken(user)),
                 new OutputDictionary(OperationOutputKeys.UserEntity, user));
        }

        public async Task<OperationOutput> GetUsersList(Dtos.Users RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var UserDbItem = _unitOfWork.User.GetAll().Where(x => x.Id == RequestOwner.Id).AsNoTracking().FirstOrDefault();
            var UserRole = (Enums.UsersRoles)UserDbItem.RoleId;

            var users = await _unitOfWork.User.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Ascending, x => x.Reference, x => x.Reference.ReferencesMajor);

            users.Data.Where(x => RequestedData.ReferenceMajorId != null ? x.Reference.ReferencesMajorId == RequestedData.ReferenceMajorId : true)
                      .Where(x => UserRole != Enums.UsersRoles.Administrator ? x.RoleId != (int)Enums.UsersRoles.Administrator : true);



            var usersDto = users.Data.Adapt<List<Dtos.Users>>(Dtos.Users.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.UserEntity, usersDto),
             new OutputDictionary(OperationOutputKeys.Pagination, users.Pagination));
        }

        public async Task<OperationOutput> GetUserDetails(Dtos.Users RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userModel = await _unitOfWork.User.GetAll(x => x.Id == RequestedData.Id)
                .Include(c => c.Reference)
                .ThenInclude(c => c.ReferencesMajor)
                .Include(c => c.UsersEntities)
                .ThenInclude(c => c.Entity)
                .Include(c => c.UsersEntitiesReferences)
                .AsNoTracking().FirstOrDefaultAsync();

            var userDto = userModel.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

            if (userModel.UsersEntities.Any())
            {
                var userEnitiiesDtos = new List<Dtos.EntitiesItems>();
                foreach (var userEntity in userModel.UsersEntities)
                {
                    userEnitiiesDtos.Add(userEntity.Adapt<Dtos.EntitiesItems>(Dtos.EntitiesItems.SelectConfig(userEntity.Entity)));
                }
                userDto.Entities = userEnitiiesDtos;
            }


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.UserEntity, userDto));

        }

        public async Task<OperationOutput> UserAction(Dtos.Users RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsBlocked.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            var user = await _unitOfWork.User.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);
            if (user == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            _unitOfWork.User.Update(RequestedData.Adapt(user, RequestedData.UpdateConfig(RequestOwner.Id)));
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> CompleteUserRegistration(Dtos.Users RequestedData)
        {
            var user = await _unitOfWork.User.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

            if (!Strings.CheckSaudiMobileNumber(RequestedData.Phone) || !Strings.CheckEmailValidity(RequestedData.Email))
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            _unitOfWork.User.Update(RequestedData.Adapt(user, RequestedData.UpdateConfig(RequestOwner.Id)));
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        #region HELPER METHODS >> GenerateToken
        private static string GenerateToken(UsersAnonymous RequestedData)
        {
            string tokenKey = "SecureKeyRequiredforvalidationAdmin";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);


            List<Claim> list = new List<Claim>();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(list),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Strings.CompressString(tokenHandler.WriteToken(token));
        }
        private static string GenerateToken(Dtos.Users RequestedData)
        {
            string tokenKey = "SecureKeyRequiredforvalidationAdmin";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            List<Claim> list = new List<Claim>();

            if (RequestedData is not null)
            {
                RequestedData.Entities = new List<EntitiesItems>();
                list.Add(new Claim("UserEntity", JsonConvert.SerializeObject(RequestedData)));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(list),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Strings.CompressString(tokenHandler.WriteToken(token));
        }

        #endregion

        public async Task<int> CheckUserName(string userName)
        {
            return await _unitOfWork.User.CountAsync(x => x.UserName == userName);
        }

        public async Task<OperationOutput> CheckUserLoginFromActiveDirectory(Dtos.Users RequestedData)
        {

            RequestedData.UserName = "website";
            RequestedData.Password = "web12345678*";

            var user = await InvokeService<UserAD>.Invoke(userLoginADUrl, RequestedData);
            var userDB = await _unitOfWork.User.GetAll().Where(x => x.UserName == RequestedData.desiredUserName
              && x.IsDeleted == false).Select(c => new
              {
                  id = Accessor.Get<int?>(c.Id),
                  c.UserName
              }).FirstOrDefaultAsync();

            var isUseExist = userDB is not null ? true : false;

            if (userDB is null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.UserAD, user),
            new OutputDictionary(OperationOutputKeys.UserDB, userDB),
            new OutputDictionary(OperationOutputKeys.IsUserExist, isUseExist));
        }

        #region Advanced Search 
        public async Task<OperationOutput> GetAllMajorsWithReferencesTree(ReferencesTree RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var maxId = await _unitOfWork.References.GetAll(x => x.IsDeleted != true).Select(x => x.Id).MaxAsync();

            var majors = await _unitOfWork.ReferencesMajor.GetAll()
                .Where(x => RequestedData.ReferencesMajorId != null ? x.Id == RequestedData.ReferencesMajorId : true)
                .Where(x => x.IsDeleted != true)
                .Select(x => new Models.Reference() { Id = x.Id + maxId, NameAr = x.NameAr, NameEn = x.NameEn, ReferencesMajorId = x.Id })
                .AsNoTracking().ToListAsync();

            var references = await _unitOfWork.References.GetAll()
                .Where(x => RequestedData.ReferencesMajorId != null ? x.ReferencesMajorId == RequestedData.ReferencesMajorId : true)
                .Where(x => x.IsDeleted != true)
                .Select(x => new Models.Reference() { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn, ReferencesMajorId = x.ReferencesMajorId, ParentId = x.ParentId == null ? x.ReferencesMajorId + maxId : x.ParentId })
                .AsNoTracking().ToListAsync();

            references.AddRange(majors);

            var referencesTree = GenerateReferenceTree(references, null).Adapt<List<Dtos.ReferencesTree>>(Dtos.ReferencesTree.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
          new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesTree));


        }

        #region HELPER METHOD >> Advanced Search 

        public IEnumerable<Dtos.ReferencesTree> GenerateReferenceTree(List<Models.Reference> references, int? parentId)
        {
            foreach (var r in references.Where(r => r.ParentId == parentId))
            {
                var t = r.Adapt<Dtos.ReferencesTree>(Dtos.ReferencesTree.SelectConfig());
                t.Children = GenerateReferenceTree(references, r.Id);
                yield return t;
            }
        }
        #endregion

        #endregion

        public async Task<OperationOutput> RefreshToken(Dtos.Users RequestedData)
        {
            var user = await _unitOfWork.User.GetAll()
                 .Include(c => c.UsersEntities)
                 .Include(r => r.Reference)
                 .ThenInclude(r => r.ReferencesMajor)
                 .Where(x => x.Id == RequestOwner.Id).AsNoTracking().FirstOrDefaultAsync();

            var userDto = user.Adapt<Dtos.Users>(Dtos.Users.SelectConfig());

            userDto.Password = null;

            var UserReferences = GetUsersEntityReference(userDto);

            if (userDto.ReferenceId != RequestedData.ReferenceId)
            {
                userDto.ReferenceRootId = GetRoot(RequestedData.ReferenceId);

                var entities = _unitOfWork.UsersEntityReference.GetAll(x => x.UserId == userDto.Id && x.ReferenceId == RequestedData.ReferenceId).AsNoTracking().ToList();

                userDto.Entities = entities.Adapt<List<Dtos.EntitiesItems>>();

                userDto.ReferenceId = RequestedData.ReferenceId;
                userDto.ReferenceName = UserReferences.Any() ? UserReferences.FirstOrDefault(x => x.ReferenceId == RequestedData.ReferenceId)?.NameAr : string.Empty;
                userDto.ReferenceNameEn = UserReferences.Any() ? UserReferences.FirstOrDefault(x => x.ReferenceId == RequestedData.ReferenceId)?.NameEn : string.Empty;
                userDto.ReferenceUrl = UserReferences.Any() ? UserReferences.FirstOrDefault(x => x.ReferenceId == RequestedData.ReferenceId)?.ReferenceUrl : string.Empty;

            }
            else userDto.ReferenceRootId = GetRoot(userDto.ReferenceId);

            userDto.UsersEntitiesReferences = UserReferences;

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.UserEntity, userDto),
            new OutputDictionary(OperationOutputKeys.Entities, userDto.Entities),
            new OutputDictionary(OperationOutputKeys.UserJWT, GenerateToken(userDto)));

        }
    }
}