
namespace RM.Core.Helpers
{
    public class Enums
    {
        public enum UsersRoles
        {
            Administrator = 1,
            Reporter = 2,
            NormalUser = 3,
            SiteAdmin = 4,
            Employee = 5,
            PortalAdmin = 6
        }
        public enum ServiceMessages
        {
            TransactionErorr = 0,
            NoPermission = 101,
            UserNotExist = 102,
            TransactionSuccess = 103,
            PasswordError = 104,
            loginSuccess = 105,
            LoginNoPermission = 106,
            RequiredFiled = 107,
            IncorrectPhoneNumber = 108,
            NoDataReturned = 109,
            UsersIsExists = 110,
            UserIsBlocked = 111,
            SessionExpired = 112,
            InvalidUserNameOrPassword = 113,
            SMSCodeWrong = 114,
            MobileNumberNotRegistered = 115,
            RequireAuthenticate = 116,
            MaxLimitReached = 117,
            AnotherOpenRequest = 118,
            MaxImageLimitExceeded = 119,
            ImageNotBase64 = 120,
            ImagesIsRequired = 121,
            MobileNoIsExists = 122,
            NoTokenRequested = 123,
            CommentInsertedSuccess = 124,
            InvalidPhoneNumber = 125,
            TheUserNotPermittedToLogin = 126,
            OperationAlreadyDone = 127,
            ErrorDeleteMessage = 128,
            FileExtentionError = 129,
            FileSizeError = 130,
            NotAllowedPrintMessage = 131,

            VolunteerSucessSave = 132,
            ComplaintsSucessSave = 133,
            SuggestionSucessSave = 134,
            NotAllowedApplyJob = 135,
            SentEmailSuccessfully = 136,
            NotAllowedInQueryJob = 137,
            ExpireApplyJob = 138,
            SubmittedJobApplication = 139,
            JobClosed = 140,
            InValidData = 141,
            WrongeData = 142,
            ExamTimeFinished = 143,
            ExamDateNotStarted = 144,
            ExamDateOver = 145,
            NotAllowToApplyExam = 146,
            ApplyedBefore = 147,
            MustConnectUserToProject = 148,
            FileExist = 149,
            DirectoryExist = 150,
            AccessDenied = 151,
            ValueIsExist = 152,
            PleaseRedisplayData = 153,
            SourceMethodIsExist = 154,
            IncorrectIdentityData = 155,
            ApplicationNoNotFound = 156,
            InValidOTP = 157,
            NotAllowDeletionCourse = 158,
            ScheduleOverlapping = 159,
            NotAlloweDeletionScheduleRecord = 160,
            NotAllowToApplyCourse = 161,
            AlreadyAppliedCourse = 162,
            NotAllowDeactiveCertificate = 163,
            NotAllowDeletedCertificate = 164,
            ExamDateEnded = 165,
            MustAssignAllSelectedUsersToExam = 166



        }
        public enum RequestStatus
        {
            New = 1,
            Recieved = 2,
            Rejected = 3,
            Closed = 4
        }
        public enum IAMLoginSources
        {
            Innovation = 1,
            FitrEvent = 2,
        }
        public enum Entities
        {
            News = 1,
            Comments = 2,
            Rating = 3,
            MainMenu = 4,
            SubMenu = 5,
            Articles = 6,
            ExternalSites = 7,
            Versions = 8,
            Regulations = 9,
            Municipalities = 10,
            Departments = 11,
            MobileApplication = 12,
            ImageGallery = 13,
            VideoGallery = 14,
            InitiativesProjects = 15,
            Awards = 16,
            Princes = 17,
            Mayors = 18,
            GovServices = 19,
            EServices = 20,
            InvestmentsOpportunities = 22,
            InvestmentsCompetitions = 23,
            ContactUs = 24,
            Advertisments = 25,
            ContactUs_Mayor = 27,
            ContactUs_940 = 28,
            ContactUs_Deaf = 29,
            Partners_Local = 30,
            Partners = 31,
            MainSlider = 33,
            Volunteers = 34,
            Voting = 35,
            RmOfficials = 36,
            PortalLatestUpdate = 37,
            Agencies = 41,
            Complaint = 43,
            Innovations = 38,
            Careers = 53,
            OpenData_Request = 58,
            EntrancePermits = 55,
            Magazine = 57,
            FAQ = 61,
            Survey = 76,
            OpenData_Management = 63,
            Statistics = 64,
            PersonalPermit = 65,
            FollowPersonalPermit = 66,
            CarPermit = 67,
            FollowCarPermit = 68,
            CompanyInfo = 69,
            Platforms = 70,
            MenuTree = 71,
            ScientificLetters = 72,
            Book = 74,
            Quran = 75,
            Fatwa = 73,
            Feedbacks = 141,
            Suggestions = 59,
            JobAdvertisement = 52,
            JobCareer = 53,
            InteractionStatistics = 142,

        }
        public enum PermissionLevels
        {
            Read = 1,
            Write = 2,
            Reports = 3
        }
        public enum JobRole
        {
            ContentManager = 1,
            EntityRepresentative = 2,
            DigitalTransformationCommittee = 6,
            PermitSecurityAuditOfficer = 7,
            PermitPrintingOfficer = 8,
            OrganizationEmployee = 9,
            ContactUsManagement = 10,
            FollowUpContactUs = 11,
            ContactUsProcessingleaders = 12,
            ContactUsProcessingDepartments = 13,
            ContactUsQualityAssurance = 14,
            DataOffice = 15,
            MediaOffice = 16,
            Libarian = 17,
            FatwaManager = 18,
            PermitProjectManager = 19,
            PermitApprovalOfficer = 20

        }
        public enum EntitiesType
        {
            SystemEntities = 1,
            StructuralEntities = 2
        }
        public enum ReferenceMajor
        {
            AlRiyadhMunicipality = 1,
            MainMnicipalities = 2,
            SubMunicipalities = 3,
            Departments = 4,
            Agencies = 5,
        }
        public enum DocumentsType
        {
            Versions = 1,
        }
        public enum MenuType
        {
            MainMenu = 1,
            SubMenu = 2
        }
        public enum LoginWay
        {
            ActiveDirectory = 1,
            SMSVerifications = 2,
            Password = 3
        }

        public enum InteractionStatisticsType
        {
            ViewsCount = 1,
            TimeSpend = 2,
            NewVisitor = 3
        }
        public enum MajorLookupsTypes
        {
            Nationality = 1,
            Gender = 2,
            Country = 5,
            City = 6,
            Qualifications = 7,
            AgeRange = 9,
            District = 10,
            IdeaStatus = 11,
            IdeaCategory = 12,
            IdeaPriority = 13,
            IdeaType = 14,
            IdeaActions = 15,
            RequesterType = 16,
            VolunteeringField = 20,
            OpenDataType = 21,
            ScientificLettersDegree = 22,
            OrderActionType = 24,
            SurveyResult = 25,

            CronJobsAll = 26,
            CronEveryDay = 27,
            CronEveryWeek = 28,
            CronEveryMonth = 29,
            CronEveryQuarter = 30,
            CronEvery12Hour = 31,

        }
        public enum SpecificCountries
        {
            SaudiArabia = 235,
        }
        public enum SpecificCities
        {
            Riyadh = 292,
        }
        public enum InnovationIdeaActions
        {
            New = 437,
            TransToReference = 34,
            RepliedFromReference = 35,
            FinallyReplay = 36,
            TransToJobRole = 325,
            Declined = 326
        }
        public enum Gender
        {
            Male = 1,
            Fmale = 2
        }

        public enum GardensCompetitorsType
        {
            Students = 1,
            GroupsAndTeams = 2,
            Companies = 3
        }
        public enum AttachmentsType
        {
            ProjectDrawingsImages = 1,
            ProjectDrawingsAutocad = 2,
            ProjectDescription = 3,
            ProjectPresentation = 4,
            ProjectVideo = 5
        }

        public enum ContactStatus
        {
            New = 1,
            UnderProcess = 2,
            TransferTo = 3,
            Returned = 4,
            Rejected = 5,
            FinalRejected = 6,
            Done = 7,
            Closed = 8,
        }
        public enum DistributionGrades
        {
            Automatically = 1,
            ScoreForEachQuestion = 2

        }

        public enum ExamResultStatus
        {
            Pass = 1,
            Fail = 2,
            NotApply = 3,
            NotComplete = 4,
            ExpireDate = 5

        }

        public enum JobApplicationStatus
        {
            Filteration = 1,
            InitialAcceptance = 2,
            PassTest = 3,
            PassInterview = 4,
            Accreditation = 5
        }

        public enum JobLookupsType
        {
            Specifications = 1,
            Tags = 2,
            Skills = 3,
            Grade = 4
        }
        public enum JobAdvertisementType
        {
            Military = 1,
            Civil = 2,
        }

        public enum OpenDataTypes
        {
            Rescue = 385,
            Infiltrations = 380,
            Smuggling = 378,
            Confiscations = 389,
            Arrests = 382,
        }
        public enum OpenDataSubTypes
        {
            Infiltrations = 381,
            Smuggling = 379,
            BorderSecurityViolators = 383,
            ResidenceViolators = 384,
            ProvideBacking = 386,
            MarineRescue = 387,
            WildRescue = 388,
            Khat = 390,
            Hashish = 391,
            Shabu = 392,
            NarcoticPills = 393,
            ProhibitedDrugs = 394
        }
        public enum Category
        {
            Arresting = 1,
            Infiltration = 2,
            Smuggling = 3,
            SearchShareMission = 4,
            Violations = 5,
        }
        public enum OpenDataStatisticTypes
        {
            View = 1,
            PDF = 2,
            Excel = 3,
            CSV = 4,
            JSON = 5,
            XML = 6,
            HTML = 7

        }
        public enum OrderActionType
        {
            New = 413,
            Agree = 414,
            Rejected = 415,
            Received = 416,
            Returned = 417,
            Closed = 418

        }

        public enum CronType
        {
            WhenFinishToDate = 1,

            EveryDay = 2,
            EveryWeek = 3,
            EveryMonth = 4,
            EveryQuaters = 5,
        }

    }
}
