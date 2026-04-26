using RM.Competitions.Dtos;
using RM.Core.Helpers;
using System.Threading.Tasks;

namespace RM.Competitions.Integrations
{
    public class GetLookups
    {
        public static async Task<OperationOutput> GetCityDistrictsLookup(Enums.MajorLookupsTypes DistrictType, Enums.SpecificCities RiyadhSpecCode, string ServiceUrl, string Token)
        {
            OperationOutput Result = new OperationOutput();
            var RequestedData = new
            {
                TypeId = Cryptography.AES.Encrypt((int)DistrictType),
                ParentId = Cryptography.AES.Encrypt((int)RiyadhSpecCode),
            };
            Result = await InvokeService.Invoke(ServiceUrl, Token, RequestedData);
            return Result;
        }
        public static async Task<OperationOutput> GetCoutryCitiesLookup(Enums.MajorLookupsTypes CityType, Enums.SpecificCountries SaudiArabiaSpecCode, string ServiceUrl, string Token)
        {
            OperationOutput Result = new OperationOutput();
            var RequestedData = new
            {
                TypeId = Cryptography.AES.Encrypt((int)CityType),
                ParentId = Cryptography.AES.Encrypt((int)SaudiArabiaSpecCode),
            };
            Result = await InvokeService.Invoke(ServiceUrl, Token, RequestedData);
            return Result;
        }
    }
}
