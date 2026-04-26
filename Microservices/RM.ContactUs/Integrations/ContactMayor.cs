
using System.Collections.Generic;
using System.Linq;

namespace RM.ContactUs.Integrations
{
    public class ContactMayor
    {

        private bool IsTransaction = false;
        ContactMayorIntegration.ContactMayorServiceClient contactMayorClient = new ContactMayorIntegration.ContactMayorServiceClient();
        public ContactMayor()
        {

        }

        public decimal InsertComplain(Dtos.ContactUs contactUs)
        {
            ContactMayorIntegration.WSComplaint complaint = new ContactMayorIntegration.WSComplaint();
            complaint.Details = contactUs.Description;
            complaint.Email = contactUs.Email;
            complaint.ImageURL = contactUs.FileUrl;
            complaint.IqamaID = contactUs.UserId;
            complaint.Mobile = contactUs.MobileNo;
            complaint.Name = contactUs.Name;
            complaint.Title = contactUs.Subject;
            return contactMayorClient.InsertComplaint(complaint);
        }

        public Dtos.ContactUs GetMayorComplain(Dtos.ContactUs contactUs)
        {
            try
            {
                
                ContactMayorIntegration.WSComplaint complaint = new ContactMayorIntegration.WSComplaint();
                complaint = contactMayorClient.GetComplaint(long.Parse(contactUs.ComplainId), contactUs.MobileNo);
                contactUs.Comments = new();
                contactUs.Replies = new();
                //complaint = contactMayorClient.GetComments((long)contactUs.ComplainId, contactUs.MobileNo);
                if (complaint==null)
                {
                    return null;
                }
                foreach (var item in contactMayorClient.GetReplies(long.Parse(contactUs.ComplainId), contactUs.MobileNo))
                {
                    contactUs.Replies.Add(new System.Tuple<string, string,string>(item.Title, item.Text,item.CreatedOn.ToShortDateString()));
                }
                foreach (var item in contactMayorClient.GetComments(long.Parse(contactUs.ComplainId), contactUs.MobileNo))
                {
                    contactUs.Comments.Add(new System.Tuple<string, string, string>(item.RoleName, item.Comment,item.CreatedOn.ToShortDateString()));
                }
                contactUs.Description = complaint.Details;
                contactUs.Email = complaint.Email;
                contactUs.UserId = complaint.IqamaID;
                contactUs.MobileNo = complaint.Mobile;
                contactUs.Name = complaint.Name;
                contactUs.Subject = complaint.Title;
                
                return contactUs;
            }
            catch (System.Exception)
            {

                return null;
            }
        }
    }
}
