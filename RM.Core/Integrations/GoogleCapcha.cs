using Newtonsoft.Json.Linq;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RM.Core.Integrations
{
    public static class GoogleCapcha
    {
        public static bool CheckCapchaSession(string SecretKey,string Capcha)
        {

            string URI = "https://www.google.com/recaptcha/api/siteverify";
            string myParameters = "secret=" + SecretKey + "&response=" + Capcha;
            using (WebClient wc = new WebClient())
            {

                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);
                JObject o = JObject.Parse(HtmlResult);
                if ((bool)o["success"] == true)
                {
                    return true;
                }

            }
            return false;
        }
    }
}
