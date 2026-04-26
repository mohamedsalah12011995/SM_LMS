using System.IO;
using System.Net;

namespace RM.Core.Integrations
{
    public class SMS
    {
        public static bool Send(string Mobile, string MessageBody)
        {
            string appCode = "SRV";
            WebRequest request = WebRequest.Create("http://10.120.6.174:9010/Common/sms.svc/web/SendSMS?appCode=" + appCode + "&messageBody=" + MessageBody + "&mobileNo=" + Mobile);
            WebResponse Response = request.GetResponse();
            Stream dataStream = Response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            return true;
        }
    }
}
