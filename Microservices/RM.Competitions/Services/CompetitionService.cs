using Microsoft.EntityFrameworkCore;
using RM.Competitions.Dtos;
using RM.Competitions.UnitOfWorks;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Models.Competitions;
using System.Text.Json;

namespace RM.Competitions.Services
{
    public class CompetitionService:BaseService, ICompetitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApplicationSettings applicationSettings;

        public CompetitionService(IUnitOfWork unitOfWork, ApplicationSettings ApplicationSettings,IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            applicationSettings = ApplicationSettings;

        }

        private OperationOutput GetCompetitorsType()
        {
            OperationOutput Result = new OperationOutput();
            List<Dtos.CompetitorsType> Item;
            Item = _unitOfWork.CompetitorsTypes.GetAll().Select(x => new Dtos.CompetitorsType()
            {
                _id = x.Id,
                NameAr = x.NameAr,

            }).ToList();

            Result.Output = new System.Collections.Generic.Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.CompetitorsType,Item}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);

            return Result;
        }
        private OperationOutput GetAttachmentType()
        {
            OperationOutput Result = new OperationOutput();
            List<Dtos.AttachmentType> Item;
            Item = _unitOfWork.AttachmentTypes.GetAll().Select(x => new Dtos.AttachmentType()
            {
                _id = x.Id,
                NameAr = x.NameAr,
                _acceptedExtention = x.AcceptedExtention,
                MaxCount = x.MaxCount,
                MinCount = x.MinCount,
                Description = x.Description,
                IsRequired = x.IsRequired,

            }).ToList();

            Result.Output = new System.Collections.Generic.Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.AttchmentType,Item}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);

            return Result;
        }
        private OperationOutput GetGandidatesType()
        {
            OperationOutput Result = new OperationOutput();
            List<Dtos.BooleansType> Item;

            Item = new List<Dtos.BooleansType>()
            {
                new Dtos.BooleansType(){value="null",NameAr="بانتظار الترشيح" },
                new Dtos.BooleansType(){value=true.ToString().ToLower(),NameAr="تم الترشيح" },
                new Dtos.BooleansType(){value=false.ToString().ToLower(),NameAr="رفض  الترشيح" }
            };

            Result.Output = new System.Collections.Generic.Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.CandidateType,Item}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);

            return Result;

        }
        private OperationOutput GetAttachmentCompleteType()
        {
            OperationOutput Result = new OperationOutput();
            List<Dtos.BooleansType> Item;

            Item = new List<Dtos.BooleansType>()
            {
                new Dtos.BooleansType(){value=true.ToString().ToLower(),NameAr="تم اكمال الملفات" },
                new Dtos.BooleansType(){value=false.ToString().ToLower(),NameAr="لم يكمل الملفات" }
            };

            Result.Output = new System.Collections.Generic.Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.AttachmentCompleteType,Item}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);

            return Result;
        }


        public OperationOutput Registration(Dtos.Competitors RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            string ValidationText = string.Empty;
            Competitor DbItem = null;
            Enums.GardensCompetitorsType gardensCompetitorsType = (Enums.GardensCompetitorsType)RequestedData._typeId;
            string FileExtention;

            if (!Strings.CheckIdCardNumberValidity(RequestedData.IdCard) || !Strings.CheckSaudiMobileNumber(RequestedData.Phone) || !Strings.CheckEmailValidity(RequestedData.Email))
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                return Result;
            }
            DbItem = _unitOfWork.Competitors.GetAll().Where(x => x.Phone == RequestedData.Phone || x.Email == RequestedData.Email || x.IdCard == RequestedData.IdCard).FirstOrDefault();
            if (DbItem != null)
            {
                ValidationText = "عذرا , بعض بيانات التسجيل قد تكون مكررة لمستفيداخر";
                if (DbItem.Phone == RequestedData.Phone)
                {
                    ValidationText += " ,رقم الجوال مكرر";
                }
                if (DbItem.Email == RequestedData.Phone)
                {
                    ValidationText += " ,البريد الالكتروني مكرر";
                }
                if (DbItem.IdCard == RequestedData.IdCard)
                {
                    ValidationText += ",رقم الهوية مكرر";
                }
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.UsersIsExists);
                Result.Header.Message = ValidationText;
                Result.Header.CustomMessage = ValidationText;
                return Result;
            }

            DbItem = new Competitor();
            switch (gardensCompetitorsType)
            {
                case Enums.GardensCompetitorsType.Students:
                    {
                        DbItem.CityId = RequestedData._cityId;
                        DbItem.CityName = RequestedData.CityName;
                        DbItem.Email = RequestedData.Email;
                        DbItem.IsLecturer = RequestedData.IsLecturer;
                        DbItem.IsTeam = RequestedData.IsTeam;
                        DbItem.RepresentsUniversity = RequestedData.RepresentsUniversity;
                        DbItem.UniversityName = RequestedData.UniversityName;
                        DbItem.SpecializationName = RequestedData.SpecializationName;
                        DbItem.AcademicYear = RequestedData.AcademicYear;
                        if (RequestedData.IsTeam.Value && RequestedData.TeamMembers.Count == 0)
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                            return Result;
                        }

                        #region SaveUniversityApprovalDocument
                        FileExtention = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.UniversityApprovalDocBase64) ? Files.GetFileExtension(RequestedData.UniversityApprovalDocBase64) : null;
                        if (RequestedData.RepresentsUniversity.Value && (RequestedData.UniversityApprovalDocBase64 == null || FileExtention != "pdf"))
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                            return Result;
                        }
                        DbItem.UniversityApprovalDoc = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.UniversityApprovalDocBase64) ?
                        Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + "." + FileExtention, RequestedData.ProfileDocBase64, FilesSavePath) : DbItem.UniversityApprovalDoc;
                        FileExtention = String.Empty;
                        #endregion

                        break;
                    }
                case Enums.GardensCompetitorsType.GroupsAndTeams:
                    {
                        DbItem.Email = RequestedData.Email;
                        DbItem.TeamName = RequestedData.TeamName;
                        if (RequestedData.TeamMembers.Count == 0)
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                            return Result;
                        }
                        break;
                    }
                case Enums.GardensCompetitorsType.Companies:
                    {
                        DbItem.CommercialNumber = RequestedData.CommercialNumber;
                        DbItem.YearsOfExperience = RequestedData.YearsOfExperience;
                        DbItem.SpecializationName = RequestedData.SpecializationName;
                        if (RequestedData.TeamMembers.Count == 0)
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                            return Result;
                        }
                        break;
                    }
                default:
                    {
                        Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                        return Result;
                    }

            }

            #region SaveProfileDocument
            FileExtention = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ProfileDocBase64) ? Files.GetFileExtension(RequestedData.ProfileDocBase64) : null;
            if (RequestedData.ProfileDocBase64 == null || FileExtention != "pdf")
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                return Result;
            }
            DbItem.ProfileDoc = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ProfileDocBase64) ?
              Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + "." + FileExtention, RequestedData.ProfileDocBase64, FilesSavePath) : DbItem.ProfileDoc;
            #endregion

            DbItem.CreatedDate = TransactionDate;
            DbItem.FullName = RequestedData.FullName;
            DbItem.IdCard = RequestedData.IdCard;
            DbItem.Phone = RequestedData.Phone;
            DbItem.TypeId = RequestedData._typeId;
            DbItem.IsCompleteAttachFile = false;
            foreach (var item in RequestedData.TeamMembers)
            {
                DbItem.TeamMembers.Add(new TeamMember()
                {
                    FullName = item.FullName,
                    IdCard = item.IdCard,
                });
            }
            _unitOfWork.Competitors.Add(DbItem);
            _unitOfWork.Complete();


            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public async Task<OperationOutput> CompetitorsCandidates(Dtos.Competitors RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            Competitor DbItem;
            if (RequestUserRole != Enums.UsersRoles.Administrator)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                return Result;
            }
            DbItem = _unitOfWork.Competitors.GetById(RequestedData._id.Value);
            if (!RequestedData._id.HasValue || !RequestedData.IsCandidated.HasValue)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                return Result;
            }
            if (DbItem.IsCandidated.HasValue)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.OperationAlreadyDone);
                Result.Header.CustomMessage = "عذرا , تم اجراء العملية للمستفيد مسبقا لا يمكن تنفيذ عملية الترشيح مرة اخرى";
                return Result;
            }
            DbItem.IsCandidated = RequestedData.IsCandidated;
            DbItem.CandidatedBy = RequestOwner.Id;
            DbItem.CandidatedDate = TransactionDate;

            _unitOfWork.Competitors.Update(DbItem);
            _unitOfWork.Complete();

            Integrations.SMS.Send(DbItem.Phone, applicationSettings.GardensCompetition.CompetitionCandidateSuccessSMS);
            Result = await GetCompetitorsDetails(DbItem.Id);
            //Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public OperationOutput GetCompetitorsList(Dtos.Competitors RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            List<Dtos.Competitors> Item;
            bool SearchForIsCandidated = false;
            bool SearchForIsCompleteAttachFile = false;

            if (RequestUserRole != Enums.UsersRoles.Administrator)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                return Result;
            }

            #region QueryFilterHandler

            RequestedData.IsCandidated = RequestedData.IsCandidatedText.ToLower() == "null" ? null : RequestedData.IsCandidatedText.ToLower() == "true" ? true : false;
            SearchForIsCandidated = RequestedData.IsCandidatedText.ToLower() != "null" && RequestedData.IsCandidatedText.ToLower() != "true" && RequestedData.IsCandidatedText.ToLower() != "false" ? false : true;

            RequestedData.IsCompleteAttachFile = RequestedData.IsCompleteAttachFileText.ToLower() == "true" ? true : false;
            SearchForIsCompleteAttachFile = RequestedData.IsCompleteAttachFileText.ToLower() != "true" && RequestedData.IsCompleteAttachFileText.ToLower() != "false" ? false : true;

            #endregion


            Item = _unitOfWork.Competitors.GetAll().Include(x => x.Type).Include(x => x.TeamMembers)
                .Where(x => RequestedData._typeId.HasValue ? x.TypeId == RequestedData._typeId : true)
                .Where(x => SearchForIsCandidated ? x.IsCandidated == RequestedData.IsCandidated : true)
                .Where(x => SearchForIsCompleteAttachFile ? x.IsCompleteAttachFile == RequestedData.IsCompleteAttachFile : true)
                //.Where(x => RequestedData.IsCandidated.HasValue ? x.TypeId == RequestedData._typeId : true)
                .Select(x => new Dtos.Competitors()
                {
                    _id = x.Id,
                    FullName = x.FullName,
                    _typeId = x.TypeId,
                    TypeName = x.Type.NameAr,
                    IdCard = x.IdCard,
                    Phone = x.Phone,
                    CreatedDate = x.CreatedDate,
                    TeamMemberCount = x.TeamMembers.Count(),
                    ProfileDoc = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(x.ProfileDoc) ? FilesGetPath + "/" + x.ProfileDoc : null,
                    IsCandidated = x.IsCandidated,
                    IsCompleteAttachFile = x.IsCompleteAttachFile,

                }).ToList();
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.CompetitionEntity,Item}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public async Task<OperationOutput> GetCompetitorsDetails(Dtos.Competitors RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            return await GetCompetitorsDetails(RequestedData._id);
        }
        public async Task<OperationOutput> GetCompetitorsDetails(int? Id)
        {
            OperationOutput Result = new OperationOutput();
            List<Dtos.Cities> CityItem = null;

            if (RequestUserRole != Enums.UsersRoles.Administrator)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                return Result;
            }
            var IntegrationCityResult = await Integrations.GetLookups.GetCoutryCitiesLookup(Enums.MajorLookupsTypes.City, Enums.SpecificCountries.SaudiArabia, applicationSettings.LookupsSeviceUrl, Token);
            CityItem = Integrations.InvokeService.DeserilizeServiceResponseToObject<List<Dtos.Cities>>(IntegrationCityResult, OperationOutput.OperationOutputKeys.LookupsEntity);
            var Item =  _unitOfWork.Competitors.GetAll()
                .Include(v => v.Type).Include(v => v.TeamMembers).Include(x => x.Attachments)
                .Where(x => x.Id == Id).AsEnumerable()
                .Select(x => new Dtos.Competitors()
                {
                    _id = x.Id,
                    _typeId = x.TypeId,
                    _cityId = x.CityId,
                    AcademicYear = x.AcademicYear,
                    CommercialNumber = x.CommercialNumber,
                    CreatedDate = x.CreatedDate,
                    Email = x.Email,
                    FullName = x.FullName,
                    IdCard = x.IdCard,
                    IsCandidated = x.IsCandidated,
                    IsLecturer = x.IsLecturer,
                    IsTeam = x.IsTeam,
                    Phone = x.Phone,
                    RepresentsUniversity = x.RepresentsUniversity,
                    SpecializationName = x.SpecializationName,
                    TeamName = x.TeamName,
                    YearsOfExperience = x.YearsOfExperience,
                    IsCandidatedText = x.IsCandidated.HasValue ? x.IsCandidated.Value ? "تمت الموافقة على طلب الترشيح" : "تم رفض طلب الترشيح" : "لم يحدد بعد",
                    IsLecturerText = x.TypeId == (int)Enums.GardensCompetitorsType.Students ? x.IsLecturer.Value ? "مدرس جامعي" : "طالب" : null,
                    IsTeamText = x.TypeId == (int)Enums.GardensCompetitorsType.Students ? x.IsTeam.Value ? "فريق عمل" : "فرد" : null,
                    RepresentsUniversityText = x.TypeId == (int)Enums.GardensCompetitorsType.Students ? x.RepresentsUniversity.Value ? "يمثل الجامعة" : "لا يمثل الجامعة" : null,
                    IsCompleteAttachFile = x.IsCompleteAttachFile,
                    UniversityName = x.UniversityName,
                    Code = Strings.ConvertNumberToStringWithPaddingDigits(x.Id, 5, x.TypeId.Value, 2),
                    UniversityApprovalDoc = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(x.UniversityApprovalDoc) ? FilesGetPath + "/" + x.UniversityApprovalDoc : null,
                    ProfileDoc = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(x.ProfileDoc) ? FilesGetPath + "/" + x.ProfileDoc : null,
                    TypeName = x.Type.NameAr,
                    CityName = x.CityId.HasValue ? CityItem.Where(city => city.ID == Cryptography.AES.Encrypt(x.CityId.Value)).FirstOrDefault().NameAr : null,
                    Attachments = x.IsCompleteAttachFile == true ? new Attachments()
                    {
                        ProjectDrawingsAutocad = x.Attachments.Where(a => a.TypeId == (int)Enums.AttachmentsType.ProjectDrawingsAutocad).Select(a => new Dtos.Attachments.AttachmentBase()
                        {
                            FileName = FilesGetPath + "/" + a.FileName,

                        }).ToList(),
                        ProjectDrawingsImages = x.Attachments.Where(a => a.TypeId == (int)Enums.AttachmentsType.ProjectDrawingsImages).Select(a => new Dtos.Attachments.AttachmentBase()
                        {
                            FileName = FilesGetPath + "/" + a.FileName,

                        }).ToList(),
                        ProjectDescription = x.Attachments.Where(a => a.TypeId == (int)Enums.AttachmentsType.ProjectDescription).Select(a => new Dtos.Attachments.AttachmentBase()
                        {
                            FileName = FilesGetPath + "/" + a.FileName,

                        }).ToList(),
                        ProjectPresentation = x.Attachments.Where(a => a.TypeId == (int)Enums.AttachmentsType.ProjectPresentation).Select(a => new Dtos.Attachments.AttachmentBase()
                        {
                            FileName = FilesGetPath + "/" + a.FileName,

                        }).ToList(),
                        ProjectVideos = x.Attachments.Where(a => a.TypeId == (int)Enums.AttachmentsType.ProjectVideo).Select(a => new Dtos.Attachments.AttachmentBase()
                        {
                            FileName = FilesGetPath + "/" + a.FileName,

                        }).ToList()

                    } : null,

                    TeamMembers = x.TeamMembers.Select(t => new Dtos.TeamMembers()
                    {
                        _id = t.Id,
                        FullName = t.FullName,
                        IdCard = t.IdCard,
                        Phone = t.Phone
                    }).ToList()
                }).FirstOrDefault();

            Result.Output = new Dictionary<string, object>() {
                { OperationOutput.OperationOutputKeys.CompetitionEntity,Item}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public OperationOutput LoginOTP(Dtos.Users RequestedData)
        {
            OperationOutput Result = new OperationOutput();

            Dtos.Users Item = new Dtos.Users();
            Dtos.Auth ItemAuth = new Dtos.Auth();
            string OTPMessage = "";

            var competitorsItem = _unitOfWork.Competitors.GetAll().Where(x => x.Phone == RequestedData.Phone).Select(x => new Dtos.Competitors()
            {
                _id = x.Id,
                Phone = x.Phone,
                IsCandidated = x.IsCandidated,

            }).FirstOrDefault();
            if (competitorsItem == null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.InvalidPhoneNumber);
                return Result;
            }
            if (competitorsItem.IsCandidated != true)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TheUserNotPermittedToLogin);
                Result.Header.CustomMessage = competitorsItem.IsCandidated.HasValue ? "عذرا, لقد تم رفض طلب الترشيح الخاص بك " : "عذرا , لم يتم دراسة طلب الترشيح الخاص بكم, عند وجود اي تحديثات سيتم ابلاغكم من خلال الرسائل النصية";
                return Result;
            }

            Item.Phone = competitorsItem.Phone;
            Item.OTPCode = Strings.GenerateRandomOTP(6);
            OTPMessage = applicationSettings.OTPCodeMessage.Replace("@code", Item.OTPCode);
            Integrations.SMS.Send(Item.Phone, OTPMessage);

            ItemAuth.VCode = Cryptography.AES.Encrypt(JsonSerializer.Serialize(Item));
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.CompetitionEntity,ItemAuth}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public OperationOutput OTPVerification(Dtos.Auth RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            Dtos.Users UserItem = null;

            Dtos.Auth ItemAuth = new Dtos.Auth();
            UserItem = JsonSerializer.Deserialize<Dtos.Users>(Cryptography.AES.Decrypt(RequestedData.VCode));
            if (UserItem.OTPCode != RequestedData.OTPCode)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.SMSCodeWrong);
                return Result;
            }
            var ComItem = _unitOfWork.Competitors.GetAll().Where(x => x.Phone == UserItem.Phone)
               .Select(x => new Dtos.Competitors
               {
                   _id = x.Id,

               }).FirstOrDefault();
            if (ComItem == null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                return Result;
            }
            UserItem._id = ComItem._id;
            ItemAuth.VCode = Cryptography.AES.Encrypt(JsonSerializer.Serialize(UserItem));
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.CompetitionEntity,ItemAuth}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public OperationOutput GetCompetitorsStatistics()
        {
            OperationOutput Result = new OperationOutput();
            Dtos.Statistics Item = new Dtos.Statistics();

            Item.RegisterdCompetitors.Companies = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Companies).Count();
            Item.RegisterdCompetitors.Teams = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.GroupsAndTeams).Count();
            Item.RegisterdCompetitors.Students = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Students).Count();
            Item.RegisterdCompetitors.Total = Item.RegisterdCompetitors.Companies + Item.RegisterdCompetitors.Teams + Item.RegisterdCompetitors.Students;

            Item.CandidatedCompetitors.Companies = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Companies && x.IsCandidated == true).Count();
            Item.CandidatedCompetitors.Teams = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.GroupsAndTeams && x.IsCandidated == true).Count();
            Item.CandidatedCompetitors.Students = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Students && x.IsCandidated == true).Count();
            Item.CandidatedCompetitors.Total = Item.CandidatedCompetitors.Companies + Item.CandidatedCompetitors.Teams + Item.CandidatedCompetitors.Students;

            Item.WaitingCompetitors.Companies = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Companies && !x.IsCandidated.HasValue).Count();
            Item.WaitingCompetitors.Teams = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.GroupsAndTeams && !x.IsCandidated.HasValue).Count();
            Item.WaitingCompetitors.Students = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Students && !x.IsCandidated.HasValue).Count();
            Item.WaitingCompetitors.Total = Item.WaitingCompetitors.Companies + Item.WaitingCompetitors.Teams + Item.WaitingCompetitors.Students;

            Item.RejectedCompetitors.Companies = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Companies && x.IsCandidated == false).Count();
            Item.RejectedCompetitors.Teams = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.GroupsAndTeams && x.IsCandidated == false).Count();
            Item.RejectedCompetitors.Students = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Students && x.IsCandidated == false).Count();
            Item.RejectedCompetitors.Total = Item.RejectedCompetitors.Companies + Item.RejectedCompetitors.Teams + Item.RejectedCompetitors.Students;

            Item.CompetitorsCompleteAttachments.Companies = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Companies && x.IsCandidated == true && x.IsCompleteAttachFile == true).Count();
            Item.CompetitorsCompleteAttachments.Teams = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.GroupsAndTeams && x.IsCandidated == true && x.IsCompleteAttachFile == true).Count();
            Item.CompetitorsCompleteAttachments.Students = _unitOfWork.Competitors.GetAll().Where(x => x.TypeId == (int)Enums.GardensCompetitorsType.Students && x.IsCandidated == true && x.IsCompleteAttachFile == true).Count();
            Item.CompetitorsCompleteAttachments.Total = Item.CompetitorsCompleteAttachments.Companies + Item.CompetitorsCompleteAttachments.Teams + Item.CompetitorsCompleteAttachments.Students;



            Result.Output = new Dictionary<string, object>() {
                { OperationOutput.OperationOutputKeys.CompetitionEntity,Item}
            };

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public OperationOutput CompleteAwardRequirements(Dtos.Attachments RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            Competitor CompetitorItem = null;
            List<Dtos.AttachmentType> AttachmentTypeDbItem;

            Dtos.AttachmentType ProjectDrawingsImagesItem;
            Dtos.AttachmentType ProjectDescriptionFilesItem;
            Dtos.AttachmentType ProjectDrawingsAutocadItem;
            Dtos.AttachmentType ProjectPresentationFilesItem;
            Dtos.AttachmentType ProjectVideosFilesItem;

            //List<string> AcceptedImages = new List<string>() { "jpg", "png" };
            //List<string> AcceptedDescription = new List<string>() { "pdf" };
            //List<string> AcceptedPresentation = new List<string>() { "pdf" };
            //List<string> AcceptedAutocad = new List<string>() { "dwg" };
            //List<string> AcceptedVideo = new List<string>() { "mp4" };


            int ProjectDrawingsImagesCount = 0;
            int ProjectDescriptionFilesCount = 0;
            int ProjectDrawingsAutocadCount = 0;
            int ProjectPresentationFilesCount = 0;
            int ProjectVideosFilesCount = 0;
            string FileName = string.Empty;

            CompetitorItem = _unitOfWork.Competitors.GetAll().Include(x => x.Attachments).Where(x => x.Id == RequestOwner.Id && x.IsCandidated == true).FirstOrDefault();
            if (CompetitorItem == null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                return Result;
            }

            AttachmentTypeDbItem = _unitOfWork.AttachmentTypes.GetAll().Select(x => new Dtos.AttachmentType()
            {
                _id = x.Id,
                MaxCount = x.MaxCount,
                MinCount = x.MinCount,
                IsRequired = x.IsRequired,
                _acceptedExtention = x.AcceptedExtention,
            }).ToList();

            ProjectDrawingsImagesItem = AttachmentTypeDbItem.Where(v => v._id == (int)Enums.AttachmentsType.ProjectDrawingsImages).FirstOrDefault();
            ProjectDescriptionFilesItem = AttachmentTypeDbItem.Where(v => v._id == (int)Enums.AttachmentsType.ProjectDescription).FirstOrDefault();
            ProjectDrawingsAutocadItem = AttachmentTypeDbItem.Where(v => v._id == (int)Enums.AttachmentsType.ProjectDrawingsAutocad).FirstOrDefault();
            ProjectPresentationFilesItem = AttachmentTypeDbItem.Where(v => v._id == (int)Enums.AttachmentsType.ProjectPresentation).FirstOrDefault();
            ProjectVideosFilesItem = AttachmentTypeDbItem.Where(v => v._id == (int)Enums.AttachmentsType.ProjectVideo).FirstOrDefault();

            if ((ProjectDrawingsImagesItem.IsRequired == true && RequestedData.ProjectDrawingsImages.Count == 0)
                || (ProjectDrawingsAutocadItem.IsRequired == true && RequestedData.ProjectDrawingsAutocad.Count == 0)
                || (ProjectDescriptionFilesItem.IsRequired == true && RequestedData.ProjectDescription.Count == 0)
                || (ProjectPresentationFilesItem.IsRequired == true && RequestedData.ProjectPresentation.Count == 0)
                || (ProjectVideosFilesItem.IsRequired == true && RequestedData.ProjectVideos.Count == 0)
                )
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                return Result;
            }

            ProjectDrawingsImagesCount = RequestedData.ProjectDrawingsImages.Select(x => ProjectDrawingsImagesItem.AcceptedExtention.Contains(Files.GetFileExtension(x.FileBase64))).Count();
            ProjectDescriptionFilesCount = RequestedData.ProjectDescription.Select(x => ProjectDescriptionFilesItem.AcceptedExtention.Contains(Files.GetFileExtension(x.FileBase64))).Count();
            ProjectDrawingsAutocadCount = RequestedData.ProjectDrawingsAutocad.Select(x => ProjectDrawingsAutocadItem.AcceptedExtention.Contains(Files.GetFileExtension(x.FileBase64))).Count();
            ProjectPresentationFilesCount = RequestedData.ProjectPresentation.Select(x => ProjectPresentationFilesItem.AcceptedExtention.Contains(Files.GetFileExtension(x.FileBase64))).Count();
            ProjectVideosFilesCount = RequestedData.ProjectVideos.Select(x => ProjectVideosFilesItem.AcceptedExtention.Contains(Files.GetFileExtension(x.FileBase64))).Count();


            if ((ProjectDrawingsImagesItem.IsRequired == true && ProjectDrawingsImagesCount < ProjectDrawingsImagesItem.MinCount && ProjectDrawingsImagesCount > ProjectDrawingsImagesItem.MinCount)
                || (ProjectDescriptionFilesItem.IsRequired == true && ProjectDescriptionFilesCount < ProjectDescriptionFilesItem.MinCount && ProjectDescriptionFilesCount > ProjectDescriptionFilesItem.MaxCount)
                || (ProjectDrawingsImagesItem.IsRequired == true && ProjectDrawingsAutocadCount < ProjectDrawingsAutocadItem.MinCount && ProjectDrawingsAutocadCount > ProjectDrawingsAutocadItem.MaxCount)
                || (ProjectPresentationFilesItem.IsRequired == true && ProjectPresentationFilesCount < ProjectPresentationFilesItem.MinCount && ProjectPresentationFilesCount > ProjectPresentationFilesItem.MaxCount)
                || (ProjectVideosFilesItem.IsRequired == true && ProjectVideosFilesCount < ProjectVideosFilesItem.MinCount && ProjectVideosFilesCount > ProjectVideosFilesItem.MaxCount))
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                return Result;
            }

            Result = SaveAttachment(ref CompetitorItem, RequestedData.ProjectDrawingsImages, Enums.AttachmentsType.ProjectDrawingsImages);
            if (!Result.Header.Success) return Result;

            Result = SaveAttachment(ref CompetitorItem, RequestedData.ProjectDrawingsAutocad, Enums.AttachmentsType.ProjectDrawingsAutocad);
            if (!Result.Header.Success) return Result;

            Result = SaveAttachment(ref CompetitorItem, RequestedData.ProjectDescription, Enums.AttachmentsType.ProjectDescription);
            if (!Result.Header.Success) return Result;

            Result = SaveAttachment(ref CompetitorItem, RequestedData.ProjectPresentation, Enums.AttachmentsType.ProjectPresentation);
            if (!Result.Header.Success) return Result;

            Result = SaveAttachment(ref CompetitorItem, RequestedData.ProjectVideos, Enums.AttachmentsType.ProjectVideo);
            if (!Result.Header.Success) return Result;

            CompetitorItem.ComplateAttachDate = TransactionDate;
            CompetitorItem.IsCompleteAttachFile = true;

            _unitOfWork.Competitors.Update(CompetitorItem);
            _unitOfWork.Complete();

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
        public async Task<OperationOutput> GetLookups()
        {
            OperationOutput Result = new OperationOutput();
            OperationOutput Districts = new OperationOutput();
            List<Dtos.CompetitorsType> ItemCompetitorsType;
            List<Dtos.AttachmentType> ItemAttachmentType;
            List<Dtos.BooleansType> ItemCandidateType;
            List<Dtos.BooleansType> ItemAttachmentCompleteType;

            ItemAttachmentType = (List<Dtos.AttachmentType>)GetAttachmentType().Output[OperationOutput.OperationOutputKeys.AttchmentType];
            ItemCompetitorsType = (List<Dtos.CompetitorsType>)GetCompetitorsType().Output[OperationOutput.OperationOutputKeys.CompetitorsType];
            ItemCandidateType = (List<Dtos.BooleansType>)GetGandidatesType().Output[OperationOutput.OperationOutputKeys.CandidateType];
            ItemAttachmentCompleteType = (List<Dtos.BooleansType>)GetAttachmentCompleteType().Output[OperationOutput.OperationOutputKeys.AttachmentCompleteType];
            Districts = await Integrations.GetLookups.GetCoutryCitiesLookup(Enums.MajorLookupsTypes.City, Enums.SpecificCountries.SaudiArabia, applicationSettings.LookupsSeviceUrl, Token);

            Result.Output = new Dictionary<string, object>() {
                { OperationOutput.OperationOutputKeys.CompetitorsType,ItemCompetitorsType},
                { OperationOutput.OperationOutputKeys.CountryCities,Districts.Output[OperationOutput.OperationOutputKeys.LookupsEntity]},
                { OperationOutput.OperationOutputKeys.AttchmentType,ItemAttachmentType},
                { OperationOutput.OperationOutputKeys.CandidateType,ItemCandidateType},
                { OperationOutput.OperationOutputKeys.AttachmentCompleteType,ItemAttachmentCompleteType}
            };
            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);

            return Result;
        }
        public OperationOutput SaveAttachment(ref Competitor CompetitorItem, List<Dtos.Attachments.AttachmentBase> AttachList, Enums.AttachmentsType Type)
        {
            OperationOutput Result = new OperationOutput();
            string FileName = string.Empty;

            foreach (var item in AttachList)
            {
                //if (!Strings.CheckBase64Validiy(item.FileBase64))
                //{
                //    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                //    return Result;
                //}
                FileName = Strings.GenerateGUID() + "." + Files.GetFileExtension(item.FileBase64);

                Files.SaveBase64FileToServer(FileName, item.FileBase64, FilesSavePath);
                CompetitorItem.Attachments.Add(new Attachment()
                {
                    TypeId = (int)Type,
                    FileName = FileName,
                    CompetitorId = CompetitorItem.Id,
                    CreatedDate = DateTime.Now,
                });
            }


            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }
    }
}
