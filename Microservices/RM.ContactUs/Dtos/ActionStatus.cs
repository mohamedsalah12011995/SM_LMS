namespace RM.ContactUs.Dtos
{
    public class ActionStatus
    {
        public bool TransferToFollowUpOfficer { get; set; } 
        public bool Reject { get; set; } 
        public bool TransferToRegion { get; set; } 
        public bool TransferToDepartment { get; set; } 
        public bool Return { get; set; } 
        public bool TransferTo { get; set; } 
        public bool UnderProcess { get; set; } 
        public bool Processed { get; set; } 
        public bool Closed { get; set; } 
        public bool Replay { get; set; } 
        public bool Reopen { get; set; }

    }




}
