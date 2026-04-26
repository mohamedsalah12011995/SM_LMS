namespace RM.Permits.PermitEnum
{
    public class PermitEnums
    {

        public enum PermitRequestStatus
        {
            New = 1,
            Verified = 2,
            Canceld = 3,
            Rejected = 4,
        }

        public enum PermitTypes
        {
            Personal = 1,
            Car = 2,
        }
        public enum IdentityType
        {
            Citizen = 1,
            Foreign = 2,
        }

        public enum FlowStepper
        {
            ProjectManagerApproval = 1,
            PermitVerification = 2,
            VerificationAndApproval = 3,
            Print = 4
        }


    }
}
