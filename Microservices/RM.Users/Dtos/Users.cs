using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class Users : BaseDto<Users, Models.User>
    {
        public Users()
        {
            Entities = new List<EntitiesItems>();

            UsersEntitiesReferences = new List<UsersEntityReference>();
            MajorReferences = new List<MajorReference>();
            UsersEntitiesReferences = new List<UsersEntityReference>();
        }

        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreateBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? RoleId { get; set; }

        [JsonIgnore]
        public int? ReferenceParentId { get; set; }

        [JsonIgnore]
        public int? ReferenceRootId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }

        [JsonIgnore]
        public int? ReferenceMajorId { get; set; }

        [JsonIgnore]
        public int? NationalityId { get; set; }
        [JsonIgnore]
        public int? GenderId { get; set; }
        [JsonIgnore]
        public int? CommunicateWayId { get; set; }
        [JsonIgnore]
        public int? YearsOfExperienceId { get; set; }
        [JsonIgnore]
        public int? CountryOfResidenceId { get; set; }
        [JsonIgnore]
        public int? CityOfResidenceId { get; set; }
        [JsonIgnore]
        public int? EducationalQualificationsId { get; set; }
        [JsonIgnore]
        public int? WorkAreaId { get; set; }
        [JsonIgnore]
        public int? LoginWayId { get; set; }
        [JsonIgnore]
        public int? JobRole { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string referenceParentId { set { ReferenceParentId = Accessor.Set(value); } get { return Accessor.Get(ReferenceParentId); } }
        public string referenceRootId { set { ReferenceRootId = Accessor.Set(value); } get { return Accessor.Get(ReferenceRootId); } }


        public string referenceMajorId { set { ReferenceMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferenceMajorId); } }
        public string createBy { set { CreateBy = Accessor.Set(value); } get { return Accessor.Get(CreateBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }
        public string roleId { set { RoleId = Accessor.Set(value); } get { return Accessor.Get(RoleId); } }
        public string nationalityId { set { NationalityId = Accessor.Set(value); } get { return Accessor.Get(NationalityId); } }
        public string genderId { set { GenderId = Accessor.Set(value); } get { return Accessor.Get(GenderId); } }
        public string communicateWayId { set { CommunicateWayId = Accessor.Set(value); } get { return Accessor.Get(CommunicateWayId); } }
        public string yearsOfExperienceId { set { YearsOfExperienceId = Accessor.Set(value); } get { return Accessor.Get(YearsOfExperienceId); } }
        public string countryOfResidenceId { set { CountryOfResidenceId = Accessor.Set(value); } get { return Accessor.Get(CountryOfResidenceId); } }
        public string cityOfResidenceId { set { CityOfResidenceId = Accessor.Set(value); } get { return Accessor.Get(CityOfResidenceId); } }
        public string educationalQualificationsId { set { EducationalQualificationsId = Accessor.Set(value); } get { return Accessor.Get(EducationalQualificationsId); } }
        public string loginWayId { set { LoginWayId = Accessor.Set(value); } get { return Accessor.Get(LoginWayId); } }
        public string workAreaId { set { WorkAreaId = Accessor.Set(value); } get { return Accessor.Get(WorkAreaId); } }
        public string jobRole { set { JobRole = Accessor.Set(value); } get { return Accessor.Get(JobRole); } }
        public string ReferenceText { get; set; }
        public string MajorReferenceText { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmployeeId { get; set; }
        public string DomainUser { get; set; }
        public string IdCardNumber { get; set; }
        public string Details { get; set; }
        public string Cv { get; set; }
        public string ReferenceMajorName { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceMajorNameEn { get; set; }
        public string ReferenceNameEn { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDatestring { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDatestring { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? AcceptWorkOnSite { get; set; }
        public bool? WorkInGovSectors { get; set; }
        public bool? HasPhoneNumber { get; set; }
        public bool? HasEmail { get; set; }
        public string desiredUserName { get; set; }
        public string ReferenceUrl { get; set; }
        public string Otp { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<EntitiesItems> Entities { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MajorReference> MajorReferences { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UsersEntityReference> UsersEntitiesReferences { get; set; }

        public class UserReference : BaseDto<UserReference, Models.Reference>
        {
            public UserReference()
            {
                ReferenceJobRole = new List<ReferenceJobRole>();
            }
            [JsonIgnore]
            public int? Id { get; set; }
            public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
            public string NameAr { get; set; }
            public string NameEn { get; set; }

            [JsonIgnore]
            public int? ParentId { get; set; }
            public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }

            [JsonIgnore]
            public int? ReferencesMajorId { get; set; }

            public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }


            public List<ReferenceJobRole> ReferenceJobRole { get; set; }

            public List<AdminMenuUserDto> AdminMenu { get; set; }
            public bool? IsPortal { get; set; }


            public static TypeAdapterConfig SelectConfig()
            {
                return new TypeAdapterConfig()
                    .NewConfig<Models.Reference, UserReference>()
                    .Map(dest => dest.ReferenceJobRole, src => src.ReferencesJobRoles != null ? src.ReferencesJobRoles.Adapt<List<ReferenceJobRole>>(Dtos.Users.ReferenceJobRole.SelectConfig()) : new List<ReferenceJobRole>())
                    .Map(dest => dest.AdminMenu, src => src.AdminMenus != null ? src.AdminMenus.Adapt<List<AdminMenuUserDto>>(AdminMenuUserDto.SelectConfig()) : new List<AdminMenuUserDto>())
                    .Config;
            }

        }
        public class ReferenceJobRole : BaseDto<ReferenceJobRole, Models.ReferencesJobRole>
        {
            [JsonIgnore]
            public int? Id { get; set; }
            public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
            public string NameAr { get; set; }
            public string NameEn { get; set; }

            public static TypeAdapterConfig SelectConfig()
            {
                return new TypeAdapterConfig()
                    .NewConfig<Models.ReferencesJobRole, ReferenceJobRole>()
                    .Ignore(x => x.ID)
                    .Map(dest => dest.Id, src => src.JobRole != null ? src.JobRole.Id : 0)
                    .Map(dest => dest.NameAr, src => src.JobRole != null ? src.JobRole.NameAr : null)
                    .Map(dest => dest.NameEn, src => src.JobRole != null ? src.JobRole.NameEn : null)
                      .Config;
            }

        }

        public ExpressionStarter<Models.User> Filteration()
        {
            var filter = PredicateBuilder.New<Models.User>(true);

            if (ReferenceId is not null)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (!string.IsNullOrEmpty(Name))
                filter.And(u => u.Name.Contains(Name));

            if (!string.IsNullOrEmpty(UserName))
                filter.And(u => u.UserName.Contains(UserName));

            if (!string.IsNullOrEmpty(Phone))
                filter.And(u => u.Phone.Contains(Phone));

            if (!string.IsNullOrEmpty(EmployeeId))
                filter.And(u => u.EmployeeId.Contains(EmployeeId));

            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));

            if (!string.IsNullOrEmpty(IdCardNumber))
                filter.And(u => u.IdCardNumber.Contains(IdCardNumber));

            if (LoginWayId is not null)
                filter.And(u => u.LoginWayId == LoginWayId);
            if (IsBlocked.HasValue)
                filter.And(u => u.IsBlocked == IsBlocked);

            filter.And(u => u.IsDeleted == false);

            return filter;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Users, Models.User>().IgnoreNullValues(true)
                .Ignore(dest => dest.UsersEntitiesReferences)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
           .Map(dest => dest.Password, src =>
            string.IsNullOrEmpty(src.Password)
            ? null
            : (src.Password.StartsWith("$2a$") || src.Password.StartsWith("$2b$") || src.Password.StartsWith("$2y$"))
            ? src.Password
            : BCrypt.Net.BCrypt.HashPassword(src.Password)
             )
                .Ignore(x => x.UsersEntities)
                .Ignore(x => x.UsersEntitiesReferences)


                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Users, Models.User>().IgnoreNullValues(true)
                .Map(dest => dest.CreateBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsBlocked, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.Password, src => BCrypt.Net.BCrypt.HashPassword(src.Password))
                .Ignore(x => x.UsersEntities)
                .Ignore(x => x.UsersEntitiesReferences)
                .Config;
        }



        public static TypeAdapterConfig SelectConfig()
        {

            return new TypeAdapterConfig()
                .NewConfig<Models.User, Users>().IgnoreNullValues(true)
              .Map(dest => dest.CreatedDatestring, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDatestring, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ReferenceMajorName, src => src.Reference != null ? src.Reference.ReferencesMajor.NameAr : null)
                .Map(dest => dest.ReferenceName, src => src.Reference != null ? src.Reference.NameAr : null)
                .Ignore(x => x.referenceParentId)
                .Map(dest => dest.ReferenceParentId, src => src.Reference != null ? src.Reference.ParentId : 0)
                .Ignore(x => x.referenceMajorId)
                .Map(dest => dest.ReferenceMajorId, src => src.Reference != null ? src.Reference.ReferencesMajorId : null)
                .Map(dest => dest.ReferenceNameEn, src => src.Reference != null ? src.Reference.NameEn : null)
                .Map(dest => dest.ReferenceMajorNameEn, src => src.Reference != null ? src.Reference.ReferencesMajor.NameEn : null)
                .Map(dest => dest.HasPhoneNumber, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.Phone) ? true : false)
                .Map(dest => dest.HasEmail, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.Email) ? true : false)
                .Map(dest => dest.Entities, src => src.UsersEntities.Any() ? src.UsersEntities.Adapt<List<EntitiesItems>>() : new List<EntitiesItems>())
                .Map(dest => dest.UsersEntitiesReferences, src => src.UsersEntitiesReferences.Any() ? src.UsersEntitiesReferences.Adapt<List<UsersEntityReference>>() : new List<UsersEntityReference>())

                .Config;
        }

        public TypeAdapterConfig AddUserADConfig(UserAD userAD, Users requestedData, int userADDefaultReference)
        {
            return new TypeAdapterConfig()
                .NewConfig<Users, Models.User>().IgnoreNullValues(true)
                .Map(dest => dest.Name, src => userAD.Body.userInformation.FullName)
                .Map(dest => dest.IdCardNumber, src => userAD.Body.userInformation.IdentityNumber)
                .Map(dest => dest.Phone, src => userAD.Body.userInformation.MobileNumber)
                .Map(dest => dest.UserName, src => userAD.Body.userInformation.username)
                .Map(dest => dest.Password, src => requestedData.Password)
                .Map(dest => dest.RoleId, src => (int)Enums.UsersRoles.Employee)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsBlocked, src => false)
                .Map(dest => dest.LoginWayId, src => (int)Enums.LoginWay.ActiveDirectory)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.ReferenceId, src => userADDefaultReference)

                .Config;
        }

        public static TypeAdapterConfig CustomMapUserADConfig(Models.Reference reference)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.User, Users>().IgnoreNullValues(true)
                .Ignore(x => x.referenceParentId)
                .Ignore(x => x.referenceMajorId)
                .Map(dest => dest.ReferenceParentId, src => reference.ParentId)
                .Map(dest => dest.ReferenceMajorId, src => reference.ReferencesMajorId)
                .Map(dest => dest.ReferenceName, src => reference.NameAr)
                .Map(dest => dest.ReferenceMajorName, src => reference.ReferencesMajor.NameAr)
                .Map(dest => dest.ReferenceNameEn, src => reference.NameEn)
                .Map(dest => dest.ReferenceMajorNameEn, src => reference.ReferencesMajor.NameEn)

                .Map(dest => dest.Entities, src => new List<EntitiesItems>())


                .Config;
        }
        public static TypeAdapterConfig MapUserFromUserADConfig(UserAD userAD, int userADDefaultReference)
        {
            return new TypeAdapterConfig()
                .NewConfig<UserAD, Users>().IgnoreNullValues(true)
                .Map(dest => dest.Name, src => userAD.Body.userInformation.FullName)
                .Map(dest => dest.IdCardNumber, src => userAD.Body.userInformation.IdentityNumber)
                .Map(dest => dest.Phone, src => userAD.Body.userInformation.MobileNumber)
                .Map(dest => dest.UserName, src => userAD.Body.userInformation.username)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsBlocked, src => false)
                .Map(dest => dest.LoginWayId, src => (int)Enums.LoginWay.ActiveDirectory)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.ReferenceId, src => userADDefaultReference)
                .Config;
        }

        public static TypeAdapterConfig AddUserFromYaqeenIntegrationConfig(Yaqeen itemYaqeenInfo, int ReferenceId, string IdNo, DateTime geogionDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Yaqeen, Models.User>().IgnoreNullValues(true)
                .Map(dest => dest.GenderId, src => itemYaqeenInfo.Gender == "M" ? (int)Enums.Gender.Male : (int)Enums.Gender.Fmale)
                .Map(dest => dest.IdCardNumber, src => IdNo)
                .Map(dest => dest.ReferenceId, src => ReferenceId)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsBlocked, src => false)
                .Map(dest => dest.BirthDate, src => geogionDate)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.RoleId, src => (int)Enums.UsersRoles.NormalUser)
                .Map(dest => dest.Name, src => $"{itemYaqeenInfo.FirstName}  {itemYaqeenInfo.FatherName}  {itemYaqeenInfo.ThirdName}  {itemYaqeenInfo.FamilyName}  ")
                .Config;
        }

    }
    public class UsersAnonymous
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string Name { get; set; }
    }

}
