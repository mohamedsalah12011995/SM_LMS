namespace RM.Exams.Dtos.ExamTrainingCourses.AssignToExam
{
    public class SendEmailToTraineesExam
    {
        public string ExamName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<TraineesForExam> Trainees { get; set; }
    }



}
