using RM.Core.Helpers;

namespace RM.GateWay
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public const string UserEntity = "UserEntity";
        public const string PermissionLevel = "PermissionLevel";
        public const string PermissionEtities = "PermissionEtities";
        public const string UserRoles = "UserRoles";
        public const string UserMajorReference = "UserMajorReference";
        public const string UserJWT = "UserJWT";
        public const string LoginWay= "LoginWay";
        public const string Pagination = "Pagination";
        public const string UserMajorReferenceTree = "UserMajorReferenceTree";
        public const string UserAD = "UserAD";
        public const string EmployeeEntity = "EmployeeEntity";
        public const string EmployeePayroll = "EmployeePayroll";
        public const string UserName = "UserName";
        public const string Name = "Name";
        public const string IsUserExist = "IsUserExist";
        public const string IsAuthenticated = "IsAuthenticated";
        public const string PortalReferences = "PortalReferences";
        public const string UserDB= "UserDB";


        public class OperationOutputKeys
        {
            public const string ReferencesMajors = "ReferencesMajors";
            public const string ReferencesEntity = "ReferencesEntity";
            public const string Pagination = "Pagination";
        }





    }
}
