using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace RM.Core.Helpers
{
    public static class JWTHelper
    {
        public static string GenerateAnonToken()
        {
            string tokenKey = "SecureKeyRequiredforvalidationAdmin";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {


                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Strings.CompressString(tokenHandler.WriteToken(token));
        }
        public static Users DecryptToken(string token)
        {

            var handler = new JwtSecurityTokenHandler();
            string token1 = token.Replace("bearer ", string.Empty).Replace("Bearer ", string.Empty);
            var jwtSecurityToken = handler.ReadJwtToken(token1);
            return JsonConvert.DeserializeObject<Users>(jwtSecurityToken.Claims.ToList()[0].ToString().Replace("UserEntity:", string.Empty));
            //return JsonConvert.DeserializeObject<Users>(Strings.DecompressString(jwtSecurityToken.Claims.ToList()[0].ToString()).Replace("UserEntity:", string.Empty));
        }

        public class Users
        {
            [JsonIgnore]
            public int? Id { get; set; }
            public string ID { set { Id = !RM.Core.Helpers.Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(RM.Core.Helpers.Cryptography.AES.Decrypt(value)) : (int?)null; } get { return this.Id.HasValue ? RM.Core.Helpers.Cryptography.AES.Encrypt(this.Id.Value) : null; } }

            public string Name { get; set; }

            public DateTime? BirthDate { get; set; }

            public string Email { get; set; }

            public string Phone { get; set; }

            public string UserName { get; set; }

            public string Password { get; set; }

            public string EmployeeId { get; set; }

            public string DomainUser { get; set; }

            public string IdCardNumber { get; set; }
            [JsonIgnore]
            public int? ReferenceId { get; set; }

            public string? referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
            [JsonIgnore]
            public int? Type { get; set; }

            public string? type { set { Type = Accessor.Set(value); } get { return Accessor.Get<int?>(Type); } }
            public DateTime? CreatedDate { get; set; }
            [JsonIgnore]
            public int? CreateBy { get; set; }

            public string? createBy { set { CreateBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreateBy); } }

            public DateTime? UpdatedDate { get; set; }
            [JsonIgnore]
            public int? UpdatedBy { get; set; }

            public string? updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }

            public bool? IsDeleted { get; set; }

            public bool? IsBlocked { get; set; }
            [JsonIgnore]
            public int? RoleId { get; set; }

            public string? roleId { set { RoleId = Accessor.Set(value); } get { return Accessor.Get<int?>(RoleId); } }

            public List<UserPermissionLevel> PermissionLevel { get; set; }

            public List<PermissionEtities> Entities { get; set; }

            public List<UserMajorReference> MajorReferences { get; set; }




            public class PermissionEtities
            {

                public int? Id { get; set; }
                public string ID { set { Id = !RM.Core.Helpers.Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(RM.Core.Helpers.Cryptography.AES.Decrypt(value)) : (int?)null; } get { return RM.Core.Helpers.Cryptography.AES.Encrypt(this.Id.Value); } }
                public string NameAr { get; set; }
                public string NameEn { get; set; }
            }
            public class UserRole
            {

                public int? Id { get; set; }
                public string ID
                {
                    set { Id = !RM.Core.Helpers.Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(RM.Core.Helpers.Cryptography.AES.Decrypt(value)) : (int?)null; }
                    get { return RM.Core.Helpers.Cryptography.AES.Encrypt(this.Id.Value); }
                }
                public string NameAr { get; set; }
                public string NameEn { get; set; }
            }
            public class UserMajorReference
            {

                public int? Id { get; set; }
                public string ID
                {
                    set { Id = !RM.Core.Helpers.Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(RM.Core.Helpers.Cryptography.AES.Decrypt(value)) : (int?)null; }
                    get { return RM.Core.Helpers.Cryptography.AES.Encrypt(this.Id.Value); }
                }
                public string NameAr { get; set; }
                public string NameEn { get; set; }
                public List<UserReference> References { get; set; }
            }
            public class UserReference
            {

                public int? Id { get; set; }
                public string ID
                {
                    set { Id = !RM.Core.Helpers.Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(RM.Core.Helpers.Cryptography.AES.Decrypt(value)) : (int?)null; }
                    get { return RM.Core.Helpers.Cryptography.AES.Encrypt(Id.Value); }
                }
                public string NameAr { get; set; }
                public string NameEn { get; set; }

            }
            public class UserPermissionLevel
            {

                public int? Id { get; set; }
                public string ID
                {
                    set { Id = !RM.Core.Helpers.Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(RM.Core.Helpers.Cryptography.AES.Decrypt(value)) : (int?)null; }
                    get { return RM.Core.Helpers.Cryptography.AES.Encrypt(Id.Value); }
                }
                public string NameAr { get; set; }
                public string NameEn { get; set; }
            }
        }
    }


}
