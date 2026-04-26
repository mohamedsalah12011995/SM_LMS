using System;
using System.IO;
using System.Net;

namespace RM.Users.Integration
{
    public class Yaqeen
    {
        public static Dtos.Yaqeen GetYaqeenInfo(string IDCard, DateTime IHijriDate)
        {
            string HijriString = IHijriDate.ToString("dd-MM-yyyy");
            WebRequest request = WebRequest.Create("https://eservices.alriyadh.gov.sa/Handler/Yakeen2.ashx?idNo=" + IDCard + "&birthDate=" + HijriString);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Dtos.Yaqeen _UserInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dtos.Yaqeen>(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return _UserInfo;
        }
    }
}
