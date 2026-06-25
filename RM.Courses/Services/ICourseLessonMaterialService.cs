using RM.Courses.Dtos;
using RM.Courses.Dtos.OperationOutput;

namespace RM.Courses.Services
{ 

    public interface ICourseLessonMaterialService
    {
        Task<OperationOutput> GetMaterialsList(CoursLessoneMaterialDto RequestedData);
        Task<OperationOutput> SaveMaterial(CoursLessoneMaterialDto RequestedData);
        Task<OperationOutput> UpdateMaterialById(CoursLessoneMaterialDto RequestedData);
        Task<OperationOutput> DeleteMaterial(CoursLessoneMaterialDto RequestedData);
    }
}
  
