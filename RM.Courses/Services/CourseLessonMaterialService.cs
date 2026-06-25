using Mapster;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Courses.Dtos;
using RM.Courses.Dtos.OperationOutput;
using RM.Courses.UnitOfWorks;
using RM.Models;
using Microsoft.EntityFrameworkCore;
using static RM.Courses.Dtos.OperationOutput.OperationOutput; 


namespace RM.Courses.Services
{

    public class CourseLessonMaterialService :
        BaseService,
        ICourseLessonMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseLessonMaterialService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)

            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetMaterialsList(CoursLessoneMaterialDto RequestedData)
        {
          
            var data = await _unitOfWork.CourseLessonMaterials
                .GetAll()
                .Include(x => x.CourseLesson)
                .Where(x => x.CourseLessonId == RequestedData.CourseLessonId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

        
            var mappedData = data.Adapt<List<CoursLessoneMaterialDto>>();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.MaterialsEntity, mappedData));
        }

        public async Task<OperationOutput> SaveMaterial(CoursLessoneMaterialDto RequestedData)
        {
            CourseLessonMaterial DbItem = new();

            if (RequestedData.Id > 0)
            {
                DbItem = await _unitOfWork.CourseLessonMaterials.GetByIdAsync(RequestedData.Id);

                if (DbItem == null)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseLessonMaterials.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseLessonMaterials.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> UpdateMaterialById(CoursLessoneMaterialDto RequestedData)
        {
            var dbItem = await _unitOfWork.CourseLessonMaterials.GetByIdAsync(RequestedData.Id);
            if (dbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            // 🚀 تحديث سريع ومباشر للمرفق
            RequestedData.Adapt(dbItem);

            _unitOfWork.CourseLessonMaterials.Update(dbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeleteMaterial(CoursLessoneMaterialDto RequestedData)
        {
            var DbItem = await _unitOfWork.CourseLessonMaterials.GetByIdAsync(RequestedData.Id);

            if (DbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            _unitOfWork.CourseLessonMaterials.Delete(DbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}