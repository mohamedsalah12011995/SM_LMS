
using System.Collections.Generic;
using System.Linq;

namespace RM.ContactUs.Integrations
{
    public class RM940Complain
    {

        Amana940Complain.AmanaReceiverSoapClient amanaReceiver = new Amana940Complain.AmanaReceiverSoapClient(Amana940Complain.AmanaReceiverSoapClient.EndpointConfiguration.AmanaReceiverSoap12);
        string _userName;
        string _password;



        public RM940Complain(string UserName, string Password)
        {
            _userName = UserName;
            _password = Password;
        }

        public async Task< int> InsertComplain(Dtos.ContactUs contactUs)
        {
            Amana940Complain.ReplyMessages reply;
            try
            {
                //TODO add address in entity
                reply = await amanaReceiver.AddComplaintsWithSubCategoryAsync(contactUs.MobileNo, contactUs.Name, contactUs.CategoryID, contactUs.SubCategoryID, contactUs.Description, contactUs.Address, "", "",contactUs.FileUrl!=null? contactUs.FileUrl:"", 4, "", _userName, _password);
            }
            catch (System.Exception ex)
            {

                return -1;
            }
            return reply.Code;
        }

        public async Task<Dtos.ContactUs> GetComplain(Dtos.ContactUs contactUs)
        {
            Amana940Complain.ComplaintDetails[] reply;
            try
            {   
                reply = await amanaReceiver.ComplaintDetailsAsync(contactUs.MobileNo, contactUs.ComplainId,_userName, _password);
                if (reply.Length>0)
                {
                    contactUs.ProblemStatus_Desc = reply[0].ProblemStatus_DescAr;
                    contactUs.CategoryDesc = reply[0].Category_DescAr;
                    contactUs.SubCategory_Desc = reply[0].SubCategory_DescAr;
                    contactUs.Problem_Desc = reply[0].Problem_Desc;
                    contactUs.District_Desc = reply[0].District_DescAr;
                    return contactUs;
                }
            }
            catch (System.Exception ex)
            {

                return null;
            }
            return null;
        }
    }
}
