using IntegrationService.Records;
using IntegrationService.Records.Hars;
using IntegrationService.Records.SMS;
using Mapster;
using RM.Core.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IntegrationService.Services
{
    public class TestService: ITestService
    {
        public TestService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

        }


        #region YaqeenWS
        public async Task<GetPersonInfoDataByPassport> GetGccInfoByPassport(GetGccInfoByPassportRequestBody personInfo)
        {
            return new GetPersonInfoDataByPassport()
            {
                Header = new Header { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new BodyIntegration
                {
                    UserInformation = new GetPersonInfoResponseBody()
                    {
                        ArabicName = "محمد رشاد",
                        EnglishName = "Mhd Rashad",
                        DateOfBirthG = "22-7-1987",
                        DateOfBirthH = "1-5-1408",
                        Gender = "Male",
                        IdExpiryDateH = "1-1-1448",
                        IdIssueDateH = "1-1-1445",
                        IdIssuePlace = "riyadh",
                        IdVersionNumber = 5,
                        LifeStatus = "0",
                        LogId = 123456789,
                        NationalityDesc = "SA",
                        OccupationDesc = "مهندس",
                        PassportExpiryDate = "1-1-2028",
                        PassportNumber = 123456789,
                        SecurityStatus = "0",
                        SponsorMoiNumber = "12345678",
                        SponsorName = "3S",
                        VisaNumber = "123456789"
                    }
                }

            };
        }

        public async Task<GetPersonInfoData> GetPersonInfoWithSecuirtyStatus(GetPersonInfoRequestBody personInfo)
        {
            return new GetPersonInfoData()
            {
                Header = new Header { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new BodyIntegration
                {
                    UserInformation = new GetPersonInfoResponseBody()
                    {
                        ArabicName = "محمد رشاد",
                        EnglishName = "Mhd Rashad",
                        DateOfBirthG = "22-7-1987",
                        DateOfBirthH = "1-5-1408",
                        Gender = "Male",
                        IdExpiryDateH = "1-1-1448",
                        IdIssueDateH = "1-1-1445",
                        IdIssuePlace = "riyadh",
                        IdVersionNumber = 5,
                        LifeStatus = "0",
                        LogId = 123456789,
                        NationalityDesc = "SA",
                        OccupationDesc = "مهندس",
                        PassportExpiryDate = "1-1-2028",
                        PassportNumber = 123456789,
                        SecurityStatus = "0",
                        SponsorMoiNumber = "12345678",
                        SponsorName = "3S",
                        VisaNumber = "123456789"
                    }
                }

            };
        }

        public async Task<GetPersonInfoData> GetGccInfoByNIN(GetPersonInfoRequestBody personInfo)
        {
            return new GetPersonInfoData()
            {
                Header = new Header { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new BodyIntegration
                {
                    UserInformation = new GetPersonInfoResponseBody()
                    {
                        ArabicName = "محمد رشاد",
                        EnglishName = "Mhd Rashad",
                        DateOfBirthG = "22-7-1987",
                        DateOfBirthH = "1-5-1408",
                        Gender = "Male",
                        IdExpiryDateH = "1-1-1448",
                        IdIssueDateH = "1-1-1445",
                        IdIssuePlace = "riyadh",
                        IdVersionNumber = 5,
                        LifeStatus = "0",
                        LogId = 123456789,
                        NationalityDesc = "SA",
                        OccupationDesc = "مهندس",
                        PassportExpiryDate = "1-1-2028",
                        PassportNumber = 123456789,
                        SecurityStatus = "0",
                        SponsorMoiNumber = "12345678",
                        SponsorName = "3S",
                        VisaNumber = "123456789"
                    }
                }

            };
        }

        public async Task<GetPersonInfoData> GetPersonInfoWithDetailedSecuirtyStatus(GetPersonInfoRequestBody personInfo)
        {
            return new GetPersonInfoData()
            {
                Header = new Header { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new BodyIntegration
                {
                    UserInformation = new GetPersonInfoResponseBody()
                    {
                        ArabicName = "محمد رشاد",
                        EnglishName = "Mhd Rashad",
                        DateOfBirthG = "22-7-1987",
                        DateOfBirthH = "1-5-1408",
                        Gender = "Male",
                        IdExpiryDateH = "1-1-1448",
                        IdIssueDateH = "1-1-1445",
                        IdIssuePlace = "riyadh",
                        IdVersionNumber = 5,
                        LifeStatus = "0",
                        LogId = 123456789,
                        NationalityDesc = "SA",
                        OccupationDesc = "مهندس",
                        PassportExpiryDate = "1-1-2028",
                        PassportNumber = 123456789,
                        SecurityStatus = "0",
                        SponsorMoiNumber = "12345678",
                        SponsorName = "3S",
                        VisaNumber = "123456789"
                    }
                }

            };
        }

        public async Task<GetCarInfoData> GetCarInfoByPlate(GetCarInfoByPlateRequestBody carInfo)
        {
            return new GetCarInfoData()
            {
                Header = new Header { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new CarBodyIntegration
                {
                    CarInformation = new GetCarInfoByPlateResponseBody()
                    {
                        LegalStatus = false,
                        LogId = 0,
                        MajorColor = "black",
                        ModelYear = 2019,
                        OwnerId = 123456789,
                        RegExpiryHDate = "1-1-1448",
                        RegIssueDate = "1-1-1445",
                        RegPlace = "الخدمات الالكترونية",
                        SequenceNumber = 123456789,
                        VehicleMaker = "هونداي",
                        VehicleModel = "اكسنت"
                    }
                }

            };
        }
        public async Task<GetPersonInfoData> GetPersonInfo(GetPersonInfoRequestBody personInfo)
        {
            return new GetPersonInfoData()
            {
                Header = new Header { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new BodyIntegration
                {
                    UserInformation = new GetPersonInfoResponseBody()
                    {
                        ArabicName = "محمد رشاد",
                        EnglishName = "Mhd Rashad",
                        DateOfBirthG = "22-7-1987",
                        DateOfBirthH = "1-5-1408",
                        Gender = "Male",
                        IdExpiryDateH = "1-1-1448",
                        IdIssueDateH = "1-1-1445",
                        IdIssuePlace = "riyadh",
                        IdVersionNumber = 5,
                        LifeStatus = "0",
                        LogId = 123456789,
                        NationalityDesc = "SA",
                        OccupationDesc = "مهندس",
                        PassportExpiryDate = "1-1-2028",
                        PassportNumber = 123456789,
                        SecurityStatus = "0",
                        SponsorMoiNumber = "12345678",
                        SponsorName = "3S",
                        VisaNumber = "123456789"
                    }
                }

            };
        }

        public async Task<GetSMSData> SendMsg(SendMsgRecord data)
        {
            return new GetSMSData() 
            { 
                Header = new Header() { IsSuccess = true, Message = "" }            
            };

        }

        #endregion


    }
}
