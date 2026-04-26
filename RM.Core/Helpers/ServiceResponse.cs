using System.Text.Json.Serialization;
using static RM.Core.Helpers.Enums;

namespace RM.Core.Helpers
{
    public class ApplicationOperation
    {

        private static readonly Dictionary<ServiceMessages, Func<ResponseOutput.ResponseContent>> responseActions
                          = new Dictionary<ServiceMessages, Func<ResponseOutput.ResponseContent>>
        {

         { ServiceMessages.NoPermission, () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NoPermission, Message = "لا يوجد صلاحية لأتمام العملية", MessageEn = "There is no permission to complete the process", TransType = "danger" } },
         { ServiceMessages.PasswordError , () =>  new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.PasswordError, Message = "كلمة المرور المدخلة غير صحيحة", MessageEn = "The entered password is incorrect", TransType = "danger" }},
         { ServiceMessages.TransactionErorr , () =>  new ResponseOutput.ResponseContent { Success = false, Code = (int) ServiceMessages.TransactionErorr, Message = "حدث خطأ اثناء تنفيذ العملية.. الرجاء المحاولة في وقت لاحق", MessageEn = "An error occurred while executing the operation.. Please try again later", TransType = "danger" }},
         { ServiceMessages.UserNotExist ,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.UserNotExist, Message = "اسم المستخدم المدخل غير موجود", MessageEn = "The username entered does not exist", TransType = "danger" }},
         { ServiceMessages.TransactionSuccess , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.TransactionSuccess, Message = "تم تنفيذ العملية بنجاح", MessageEn = "The operation was performed successfully", TransType = "success" }},
         { ServiceMessages.loginSuccess , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.loginSuccess, Message = "تسجيل دخول ناجح", MessageEn = "Successful login", TransType = "success" }},
         { ServiceMessages.LoginNoPermission , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.LoginNoPermission, Message = "لا يوجد للموظف صلاحية للدخول الى التطبيق", MessageEn = "The employee does not have access to the application", TransType = "warning" }},
         { ServiceMessages.RequiredFiled ,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.RequiredFiled, Message = "الرجاء ادخال جميع البيانات المطلوبة", MessageEn = "Please enter all required data", TransType = "warning" }},
         { ServiceMessages.IncorrectPhoneNumber , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.IncorrectPhoneNumber, Message = "رقم الجوال المدخل غير صحيح", MessageEn = "The mobile number entered is incorrect", TransType = "warning" }},
         { ServiceMessages.NoDataReturned , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NoDataReturned, Message = "لا يوجد بيانات ..", MessageEn = "There are no data..", TransType = "warning" }},
         { ServiceMessages.UsersIsExists , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.UsersIsExists, Message = "اسم المستخدم المطلوب موجود مسبقا", TransType = "warning" }},
         { ServiceMessages.UserIsBlocked , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.UserIsBlocked, Message = "المستخدم موقوف حاليا.. الرجاء التواصل مع الادارة", MessageEn = "The requested username already exists", TransType = "warning" }},
         { ServiceMessages.SessionExpired , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SessionExpired, Message = "انتهت صلاحية الدخول, الرجاء تسجيل الدخول من جديد", MessageEn = "Session has expired, please log in again", TransType = "danger" }},
         { ServiceMessages.InvalidUserNameOrPassword ,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.InvalidUserNameOrPassword, Message = "اسم المستخدم او كلمة المرور غير صحيح", MessageEn = "The username or password is incorrect", TransType = "danger" }},
         { ServiceMessages.SMSCodeWrong , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SMSCodeWrong, Message = "الكود المدخل غير صحيح", TransType = "danger" }},
         { ServiceMessages.MobileNumberNotRegistered,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MobileNumberNotRegistered, Message = "رقم الجوال غير مسجل من قبل", TransType = "success" }},
         { ServiceMessages.RequireAuthenticate , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.RequireAuthenticate, Message = "برجاء تسجيل الدخول", TransType = "danger" }},
         { ServiceMessages.MaxLimitReached , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MaxLimitReached, Message = "تم الوصول للحد الأقصى للطلبات لنوع الشجرة", TransType = "warning" }},
         { ServiceMessages.AnotherOpenRequest , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.AnotherOpenRequest, Message = "يوجد لديك طلب اخر مفتوح", TransType = "warning" }},
         { ServiceMessages.MaxImageLimitExceeded , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MaxImageLimitExceeded, Message = "تعديت الحد الأقصى للصور  ", TransType = "warning" }},
         { ServiceMessages.ImageNotBase64 , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ImageNotBase64, Message = "الصور غير صحيحة", TransType = "warning" }},
         { ServiceMessages.ImagesIsRequired , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ImagesIsRequired, Message = "برجاء إرفاق صور", TransType = "warning" }},
         { ServiceMessages.MobileNoIsExists , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ImagesIsRequired, Message = "رقم الجوال مسجل مسبقا", TransType = "warning" }},
         { ServiceMessages.NoTokenRequested , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NoTokenRequested, Message = "يرجى ارسال بيانات التحقق", TransType = "warning" }},
         { ServiceMessages.CommentInsertedSuccess , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.CommentInsertedSuccess, Message = "شكرا لك, تم استقبال مشاركتكم بنجاح", MessageEn = "Thank you, your Comment has been successfully received", TransType = "warning" }},
         { ServiceMessages.InvalidPhoneNumber , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.InvalidPhoneNumber, Message = "رقم الجوال المدخل غير صحيح", MessageEn = "Invalid Customer Phone Number", TransType = "warning" }},
         { ServiceMessages.TheUserNotPermittedToLogin , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.TheUserNotPermittedToLogin, Message = "المستخدم غير مصرح له بتسجيل الدخول الى هذه الخدمة", MessageEn = "The user is not authorized to log in to this service", TransType = "warning" }},
         { ServiceMessages.OperationAlreadyDone , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, لا يمكن تنفيذ العملية لانها منفذه مسبقا", MessageEn = "The operation is already done", TransType = "warning" }},
         { ServiceMessages.ErrorDeleteMessage , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, لا يمكنك اتمام عملية الحذف", MessageEn = "Sorry, you can't complete the deletion process", TransType = "warning" }},
         { ServiceMessages.FileExtentionError , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, غير مسموح بهذا النوع من الملفات", MessageEn = "Sorry, this file Extention Not Acceptable", TransType = "warning" }},
         { ServiceMessages.FileSizeError , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, حجم الملف المرفوع كبير جدا", MessageEn = "Sorry, this file size too Big", TransType = "warning" }},
         { ServiceMessages.NotAllowedPrintMessage ,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا,  غير مسموح للطباعة ", MessageEn = "Sorry, No printing allowed", TransType = "warning" }},
         { ServiceMessages.VolunteerSucessSave , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عزيزى المتطوع , نفيدكم بانه تم استقبال طلبكم بنجاح", MessageEn = "Dear volunteer, We inform you that your request has been successfully received", TransType = "warning" }},
         { ServiceMessages.ComplaintsSucessSave, () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عزيزي المستفيد, نفيدكم بانه تم استقبال طلبكم بنجاح , حيث يمكن الاستعلام عن الطلب من خلال الرقم المرجعى ", MessageEn = "Dear Beneficiary, We inform you that your request has been successfully received. You can inquire about the request through the reference number", TransType = "warning" }},
         { ServiceMessages.SuggestionSucessSave , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عزيزي المستفيد, نفيدكم بانه تم استقبال طلبكم بنجاح , حيث يمكن الاستعلام عن الطلب من خلال الرقم المرجعى ", MessageEn = "Dear Beneficiary, We inform you that your request has been successfully received. You can inquire about the request through the reference number", TransType = "warning" }},
         { ServiceMessages.NotAllowedApplyJob , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowedApplyJob, Message = "عفوا ,  لا يمكنك التقدم الى هذه الوظيفة  ", MessageEn = "Sorry, you can't apply for this job. ", TransType = "warning" }},
         { ServiceMessages.SentEmailSuccessfully , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.SentEmailSuccessfully, Message = " تم ارسال الاشعار بنجاح", MessageEn = "The notification has been sent successfully ", TransType = "warning" }},
         { ServiceMessages.NotAllowedInQueryJob , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowedInQueryJob, Message = "عفوا .. لا يمكنك الاستعلام عن هذه الوظيفة", MessageEn = "Sorry.. You cannot query for this job ", TransType = "warning" }},
         { ServiceMessages.ExpireApplyJob , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExpireApplyJob, Message = "عفوا .. انتهت فترة التقديم لهذه الوظيفة", MessageEn = "Sorry.. The application period for this job has expired ", TransType = "warning" }},
         { ServiceMessages.SubmittedJobApplication , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SubmittedJobApplication, Message = "عفوا .. تم التقدم لهذه الوظيفة مسبقا", MessageEn = "Sorry.. This job has already been applied for ", TransType = "warning" }},
         { ServiceMessages.JobClosed ,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.JobClosed, Message = " عفوا .. لم يتم فتح قبول الطلبات لهذه الوظيفة بالوقت الحالى", MessageEn = "Sorry.. Applications for this job are not yet open", TransType = "warning" }},
         { ServiceMessages.InValidData , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.InValidData, Message = " عفوا .. يرجى التحقق من صحة بيانات الاستعلام", MessageEn = "Sorry.. Please verify that the query data is correct", TransType = "warning" }},
         { ServiceMessages.WrongeData , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = " لايوجد بيانات - تاكد من البيانات المدخلة", MessageEn = "No data - Check the entered data", TransType = "warning" }},
         { ServiceMessages.ExamTimeFinished , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamTimeFinished, Message = "انتهى الوقت المخصص للامتحان", MessageEn = "Exam Duration Time Is Over", TransType = "warning" }},
         { ServiceMessages.ExamDateNotStarted , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamDateNotStarted, Message = "لم يحن تاريخ الامتحان", MessageEn = "It 's Not Exam Date", TransType = "warning" }},
         { ServiceMessages.ExamDateOver , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamDateOver, Message = "تجاوزت التاريخ المخصص للامتحان", MessageEn = "Exam Date Is Over", TransType = "warning" }},
         { ServiceMessages.NotAllowToApplyExam , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowToApplyExam, Message = "غير مسموح التقديم للامتحان", MessageEn = "Not Allow To Apply Exam", TransType = "warning" }},
         { ServiceMessages.ApplyedBefore , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ApplyedBefore, Message = "تم التقديم مسبقاً", MessageEn = "You already Applied ", TransType = "warning" }},
         { ServiceMessages.MustConnectUserToProject , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MustConnectUserToProject, Message = "هذا المستخدم ليس له صلاحية على المشاريع المقدم عليها تصاريح دخول ", MessageEn = "This user has no authority over projects that have access permissions", TransType = "warning" }},
         { ServiceMessages.FileExist ,() => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = "الملف موجود مسبقاً", MessageEn = " File Already Exist ", TransType = "warning" }},
         { ServiceMessages.DirectoryExist , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = "المجلد موجود مسبقاً", MessageEn = "Directory Already Exist ", TransType = "warning" }},
         { ServiceMessages.AccessDenied , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = " عفوا لا يمكنك الوصول للبيانات ", MessageEn = "Access Denied ", TransType = "warning" }},
         { ServiceMessages.ValueIsExist , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = "عفوا لا يمكنك تكرار القيمة  ", MessageEn = "Sorry, you cannot repeat a value  ", TransType = "warning" }},
         { ServiceMessages.PleaseRedisplayData , () => new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.PleaseRedisplayData, Message = " تمت العملية بنجاح ... يرجى اعادة عرض البيانات ", MessageEn = "The operation was completed successfully ... Please re-display the data ", TransType = "warning" }},
         { ServiceMessages.SourceMethodIsExist , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SourceMethodIsExist, Message = " اسم الميثودبالـ Backend مدخلة مسبقا ", MessageEn = "The 'Backend Method Name' already exists", TransType = "warning" }},
         { ServiceMessages.IncorrectIdentityData , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.IncorrectIdentityData, Message = " عفوا ... بيانات الهوية غير صحيحة ", MessageEn = "Incorrect identity data", TransType = "warning" }},
         { ServiceMessages.ApplicationNoNotFound , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ApplicationNoNotFound, Message = " عفوا ...لم يتم العثور على الرقم الوظيفى  ", MessageEn = "Application NO. not found", TransType = "warning" }},
         { ServiceMessages.InValidOTP , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ApplicationNoNotFound, Message = " عفوا ... رقم التحقق غير مطابق       ", MessageEn = "Wrong OTP !!", TransType = "warning" }},

        { ServiceMessages.NotAllowDeletionCourse , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowDeletionCourse, Message = " عفوا ...لا يمكن حذف الدورة التدريبية في الوقت الحالي، حيث توجد مواعيد مجدولة نشطة مرتبطة بها ", MessageEn = "Sorry...the course cannot be deleted at this time, as there are active scheduled appointments associated with it.", TransType = "warning" }},
        { ServiceMessages.ScheduleOverlapping , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ScheduleOverlapping, Message = " عفوا ...الجدول الزمني الجديد يتداخل مع جدول زمني موجود ", MessageEn = "Sorry...The new schedule overlaps with an existing schedule.", TransType = "warning" }},
        { ServiceMessages.NotAlloweDeletionScheduleRecord , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAlloweDeletionScheduleRecord, Message = " عفوا ...لا يمكنك حذف الجدولة الزمنية لبدء العمل بالدورة التدربية بالوقت الحالى ", MessageEn = "Sorry...you cannot delete the start schedule and training course at this time.", TransType = "warning" }},

       { ServiceMessages.NotAllowToApplyCourse , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowToApplyCourse, Message = "عفوا ...لا يمكنك التسجيل بهذه الدورة التدربية", MessageEn = "Sorry...you cannot register for this course.", TransType = "warning" }},
       { ServiceMessages.AlreadyAppliedCourse , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.AlreadyAppliedCourse, Message = "عفوا ... تم تسجيلك مسبقا على هذه الدورة التدريبية", MessageEn = "You have already registered for this course.", TransType = "warning" }},

      { ServiceMessages.NotAllowDeactiveCertificate , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowDeactiveCertificate, Message = "عفوا ... لا يمكنك الغاء تفعيل الشهادة بسبب ارتباطها بدورات تدريبية قائمة", MessageEn = "Sorry... you cannot deactivate the certificate because it is linked to existing training courses..", TransType = "warning" }},
      { ServiceMessages.NotAllowDeletedCertificate , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowDeletedCertificate, Message = "عفوا ... لا يمكنك  حذف الشهادة بسبب ارتباطها بدورات تدريبية قائمة", MessageEn = "Sorry... you cannot delete the certificate because it is linked to existing training courses..", TransType = "warning" }},
      { ServiceMessages.ExamDateEnded , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamDateEnded, Message = "عفوا ... تم انتهاء موعد الاختبار", MessageEn = " Sorry...the Exam has ended...", TransType = "warning" }},
      { ServiceMessages.MustAssignAllSelectedUsersToExam , () => new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MustAssignAllSelectedUsersToExam, Message = "عفوا ... يجب تعيين الاختبار لقائمة الموظفين قبل ارسال اشعار بالبريد الالكترونى", MessageEn = "Sorry...the test must be assigned to the employee list before an email notification can be sent.", TransType = "warning" }},
 };

        public static ResponseOutput.ResponseContent OperationResult(ServiceMessages messageType)
        {
            if (responseActions.TryGetValue(messageType, out var action))
            {
                return action.Invoke();
            }
            else
            {
                return new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.TransactionErorr, Message = $"Unknown command: {messageType}", MessageEn = $"Unknown message: {messageType}", TransType = "warning" };
            }
        }


        //public static ResponseOutput.ResponseContent OperationResult1(ServiceMessages MessageType)
        //{
        //    ResponseOutput.ResponseContent ResponseHeader = null;
        //    switch (MessageType)
        //    {
        //        case ServiceMessages.NoPermission:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NoPermission, Message = "لا يوجد صلاحية لأتمام العملية", MessageEn = "There is no permission to complete the process", TransType = "danger" };
        //            }
        //            break;
        //        case ServiceMessages.PasswordError:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.PasswordError, Message = "كلمة المرور المدخلة غير صحيحة", MessageEn = "The entered password is incorrect", TransType = "danger" };
        //            }
        //            break;

        //        case ServiceMessages.TransactionErorr:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.TransactionErorr, Message = "حدث خطأ اثناء تنفيذ العملية.. الرجاء المحاولة في وقت لاحق", MessageEn = "An error occurred while executing the operation.. Please try again later", TransType = "danger" };
        //            }
        //            break;
        //        case ServiceMessages.UserNotExist:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.UserNotExist, Message = "اسم المستخدم المدخل غير موجود", MessageEn = "The username entered does not exist", TransType = "danger" };
        //            }
        //            break;
        //        case ServiceMessages.TransactionSuccess:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.TransactionSuccess, Message = "تم تنفيذ العملية بنجاح", MessageEn = "The operation was performed successfully", TransType = "success" };
        //            }
        //            break;


        //        case ServiceMessages.loginSuccess:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.loginSuccess, Message = "تسجيل دخول ناجح", MessageEn = "Successful login", TransType = "success" };
        //            }
        //            break;
        //        case ServiceMessages.LoginNoPermission:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.LoginNoPermission, Message = "لا يوجد للموظف صلاحية للدخول الى التطبيق", MessageEn = "The employee does not have access to the application", TransType = "warning" };
        //            }
        //            break;

        //        case ServiceMessages.RequiredFiled:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.RequiredFiled, Message = "الرجاء ادخال جميع البيانات المطلوبة", MessageEn = "Please enter all required data", TransType = "warning" };
        //            }
        //            break;


        //        case ServiceMessages.IncorrectPhoneNumber:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.IncorrectPhoneNumber, Message = "رقم الجوال المدخل غير صحيح", MessageEn = "The mobile number entered is incorrect", TransType = "warning" };
        //            }
        //            break;

        //        case ServiceMessages.NoDataReturned:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NoDataReturned, Message = "لا يوجد بيانات ..", MessageEn = "There are no data..", TransType = "warning" };
        //            }
        //            break;
        //        case ServiceMessages.UsersIsExists:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.UsersIsExists, Message = "اسم المستخدم المطلوب موجود مسبقا", TransType = "warning" };
        //            }
        //            break;

        //        case ServiceMessages.UserIsBlocked:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.UserIsBlocked, Message = "المستخدم موقوف حاليا.. الرجاء التواصل مع الادارة", MessageEn = "The requested username already exists", TransType = "warning" };
        //            }
        //            break;
        //        case ServiceMessages.SessionExpired:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SessionExpired, Message = "انتهت صلاحية الدخول, الرجاء تسجيل الدخول من جديد", MessageEn = "Session has expired, please log in again", TransType = "danger" };
        //            }
        //            break;

        //        case ServiceMessages.InvalidUserNameOrPassword:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.InvalidUserNameOrPassword, Message = "اسم المستخدم او كلمة المرور غير صحيح", MessageEn = "The username or password is incorrect", TransType = "danger" };
        //                break;
        //            }

        //        case ServiceMessages.SMSCodeWrong:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SMSCodeWrong, Message = "الكود المدخل غير صحيح", TransType = "danger" };
        //                break;
        //            }
        //        case ServiceMessages.MobileNumberNotRegistered:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MobileNumberNotRegistered, Message = "رقم الجوال غير مسجل من قبل", TransType = "success" };
        //                break;
        //            }
        //        case ServiceMessages.RequireAuthenticate:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.RequireAuthenticate, Message = "برجاء تسجيل الدخول", TransType = "danger" };
        //                break;
        //            }
        //        case ServiceMessages.MaxLimitReached:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MaxLimitReached, Message = "تم الوصول للحد الأقصى للطلبات لنوع الشجرة", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.AnotherOpenRequest:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.AnotherOpenRequest, Message = "يوجد لديك طلب اخر مفتوح", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.MaxImageLimitExceeded:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MaxImageLimitExceeded, Message = "تعديت الحد الأقصى للصور  ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ImageNotBase64:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ImageNotBase64, Message = "الصور غير صحيحة", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ImagesIsRequired:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ImagesIsRequired, Message = "برجاء إرفاق صور", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.MobileNoIsExists:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ImagesIsRequired, Message = "رقم الجوال مسجل مسبقا", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.NoTokenRequested:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NoTokenRequested, Message = "يرجى ارسال بيانات التحقق", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.CommentInsertedSuccess:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.CommentInsertedSuccess, Message = "شكرا لك, تم استقبال مشاركتكم بنجاح", MessageEn = "Thank you, your Comment has been successfully received", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.InvalidPhoneNumber:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.InvalidPhoneNumber, Message = "رقم الجوال المدخل غير صحيح", MessageEn = "Invalid Customer Phone Number", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.TheUserNotPermittedToLogin:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.TheUserNotPermittedToLogin, Message = "المستخدم غير مصرح له بتسجيل الدخول الى هذه الخدمة", MessageEn = "The user is not authorized to log in to this service", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.OperationAlreadyDone:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, لا يمكن تنفيذ العملية لانها منفذه مسبقا", MessageEn = "The operation is already done", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.ErrorDeleteMessage:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, لا يمكنك اتمام عملية الحذف", MessageEn = "Sorry, you can't complete the deletion process", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.FileExtentionError:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, غير مسموح بهذا النوع من الملفات", MessageEn = "Sorry, this file Extention Not Acceptable", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.FileSizeError:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا, حجم الملف المرفوع كبير جدا", MessageEn = "Sorry, this file size too Big", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.NotAllowedPrintMessage:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عذرا,  غير مسموح للطباعة ", MessageEn = "Sorry, No printing allowed", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.VolunteerSucessSave:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عزيزى المتطوع , نفيدكم بانه تم استقبال طلبكم بنجاح", MessageEn = "Dear volunteer, We inform you that your request has been successfully received", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ComplaintsSucessSave:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عزيزي المستفيد, نفيدكم بانه تم استقبال طلبكم بنجاح , حيث يمكن الاستعلام عن الطلب من خلال الرقم المرجعى ", MessageEn = "Dear Beneficiary, We inform you that your request has been successfully received. You can inquire about the request through the reference number", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.SuggestionSucessSave:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.OperationAlreadyDone, Message = "عزيزي المستفيد, نفيدكم بانه تم استقبال طلبكم بنجاح , حيث يمكن الاستعلام عن الطلب من خلال الرقم المرجعى ", MessageEn = "Dear Beneficiary, We inform you that your request has been successfully received. You can inquire about the request through the reference number", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.NotAllowedApplyJob:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowedApplyJob, Message = "عفوا ,  لا يمكنك التقدم الى هذه الوظيفة  ", MessageEn = "Sorry, you can't apply for this job. ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.SentEmailSuccessfully:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.SentEmailSuccessfully, Message = " تم ارسال الاشعار بنجاح", MessageEn = "The notification has been sent successfully ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.NotAllowedInQueryJob:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowedInQueryJob, Message = "عفوا .. لا يمكنك الاستعلام عن هذه الوظيفة", MessageEn = "Sorry.. You cannot query for this job ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ExpireApplyJob:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExpireApplyJob, Message = "عفوا .. انتهت فترة التقديم لهذه الوظيفة", MessageEn = "Sorry.. The application period for this job has expired ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.SubmittedJobApplication:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SubmittedJobApplication, Message = "عفوا .. تم التقدم لهذه الوظيفة مسبقا", MessageEn = "Sorry.. This job has already been applied for ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.JobClosed:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.JobClosed, Message = " عفوا .. لم يتم فتح قبول الطلبات لهذه الوظيفة بالوقت الحالى", MessageEn = "Sorry.. Applications for this job are not yet open", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.InValidData:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.InValidData, Message = " عفوا .. يرجى التحقق من صحة بيانات الاستعلام", MessageEn = "Sorry.. Please verify that the query data is correct", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.WrongeData:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = " لايوجد بيانات - تاكد من البيانات المدخلة", MessageEn = "No data - Check the entered data", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.ExamTimeFinished:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamTimeFinished, Message = "انتهى الوقت المخصص للامتحان", MessageEn = "Exam Duration Time Is Over", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ExamDateNotStarted:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamDateNotStarted, Message = "لم يحن تاريخ الامتحان", MessageEn = "It 's Not Exam Date", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ExamDateOver:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ExamDateOver, Message = "تجاوزت التاريخ المخصص للامتحان", MessageEn = "Exam Date Is Over", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.NotAllowToApplyExam:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.NotAllowToApplyExam, Message = "غير مسموح التقديم للامتحان", MessageEn = "Not Allow To Apply Exam", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ApplyedBefore:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ApplyedBefore, Message = "تم التقديم مسبقاً", MessageEn = "You already Applied ", TransType = "warning" };
        //                break;
        //            }

        //        case ServiceMessages.MustConnectUserToProject:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.MustConnectUserToProject, Message = "هذا المستخدم ليس له صلاحية على المشاريع المقدم عليها تصاريح دخول ", MessageEn = "This user has no authority over projects that have access permissions", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.FileExist:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = "الملف موجود مسبقاً", MessageEn = " File Already Exist ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.DirectoryExist:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = "المجلد موجود مسبقاً", MessageEn = "Directory Already Exist ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.AccessDenied:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = " عفوا لا يمكنك الوصول للبيانات ", MessageEn = "Access Denied ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ValueIsExist:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.WrongeData, Message = "عفوا لا يمكنك تكرار القيمة  ", MessageEn = "Sorry, you cannot repeat a value  ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.PleaseRedisplayData:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = true, Code = (int)ServiceMessages.PleaseRedisplayData, Message = " تمت العملية بنجاح ... يرجى اعادة عرض البيانات ", MessageEn = "The operation was completed successfully ... Please re-display the data ", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.SourceMethodIsExist:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.SourceMethodIsExist, Message = " اسم الميثودبالـ Backend مدخلة مسبقا ", MessageEn = "The 'Backend Method Name' already exists", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.IncorrectIdentityData:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.IncorrectIdentityData, Message = " عفوا ... بيانات الهوية غير صحيحة ", MessageEn = "Incorrect identity data", TransType = "warning" };
        //                break;
        //            }
        //        case ServiceMessages.ApplicationNoNotFound:
        //            {
        //                ResponseHeader = new ResponseOutput.ResponseContent { Success = false, Code = (int)ServiceMessages.ApplicationNoNotFound, Message = " عفوا ...لم يتم العثور على الرقم الوظيفى  ", MessageEn = "Application NO. not found", TransType = "warning" };
        //                break;
        //            }
        //    }

        //    return ResponseHeader;
        //}

        public class ResponseOutputBase
        {
            [JsonPropertyName("header")]
            public ResponseContent Header { get; set; }
            public class ResponseContent
            {
                [JsonPropertyName("success")]
                public bool Success { get; set; }
                [JsonPropertyName("code")]
                public int Code { get; set; }
                [JsonPropertyName("message")]
                public string Message { get; set; }
                [JsonPropertyName("messageEn")]
                public string MessageEn { get; set; }
                [JsonPropertyName("hasArabicContent")]
                public bool HasArabicContent { get; set; } = true;
                [JsonPropertyName("hasEnglishContent")]
                public bool HasEnglishContent { get; set; } = true;
                [JsonPropertyName("customMessage")]
                public string CustomMessage { get; set; }
                [JsonPropertyName("customMessageEn")]
                public string CustomMessageEn { get; set; }
                [JsonPropertyName("transType")]
                public string TransType { get; set; }
                [JsonPropertyName("duration")]
                public string Duration { get; set; }
            }

        }

        public class ResponseOutput : ResponseOutputBase
        {
            [JsonPropertyName("output")]
            public Dictionary<string, object> Output { get; set; }
        }

        public class Pagination
        {
            public Pagination() { }
            public int? CurrentPageIndex { get; set; }
            public double? TotalPagesCount { get; set; }
            public int? RecordPerPage { get; set; }
            public long? TotalItemsCount { get; set; }


        }


    }
}

