using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record DeletionCourseAdvertisement
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

   


    }
}
