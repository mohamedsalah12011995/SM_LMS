using Mapster;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Courses.Dtos;
using RM.Courses.Dtos.OperationOutput;
using RM.Courses.Models;
using RM.Courses.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using static RM.Courses.Dtos.OperationOutput.OperationOutput;

namespace RM.Courses.Services
{
    public class CourseLessonService :
        BaseService,
        ICourseLessonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseLessonService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)

            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetSectionsList(CourseLessonDto RequestedData)
        {
            var data = await _unitOfWork.CourseSections
                .GetAll()
                .Where(x => x.CourseId == RequestedData.CourseId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.SectionsEntity, data.Adapt<List<CourseLessonDto>>()));
        }

        public async Task<OperationOutput> SaveSection(CourseLessonDto RequestedData)
        {
            CourseSection DbItem = new();

            if (RequestedData.Id > 0) 
            {
                DbItem = await _unitOfWork.CourseSections.GetByIdAsync(RequestedData.Id);

                if (DbItem == null)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseSections.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseSections.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> UpdateSectionById(CourseLessonDto RequestedData)
        {
            var dbItem = await _unitOfWork.CourseSections.GetByIdAsync(RequestedData.Id);
            if (dbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            RequestedData.Adapt(dbItem);

            _unitOfWork.CourseSections.Update(dbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeleteSection(CourseLessonDto RequestedData)
        {
            var DbItem = await _unitOfWork.CourseSections.GetByIdAsync(RequestedData.Id);

            if (DbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            _unitOfWork.CourseSections.Delete(DbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetLessonsList(CourseLessonDto RequestedData)
        {
            var data = await _unitOfWork.CourseLessons
                .GetAll()
                .Where(x => x.CourseSectionId == RequestedData.CourseSectionId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.LessonsEntity, data.Adapt<List<CourseLessonDto>>()));
        }

        public async Task<OperationOutput> GetLessonDetails(CourseLessonDto RequestedData)
        {
            var data = await _unitOfWork.CourseLessons
                .GetAll()
                .Include(x => x.Materials)
                .FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

            if (data == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.LessonsEntity, data.Adapt<CourseLessonDto>()));
        }

        public async Task<OperationOutput> SaveLesson(CourseLessonDto RequestedData)
        {
            CourseLesson DbItem = new();

            if (RequestedData.Id > 0)
            {
                DbItem = await _unitOfWork.CourseLessons.GetByIdAsync(RequestedData.Id);

                if (DbItem == null)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseLessons.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseLessons.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> UpdateLessonById(CourseLessonDto RequestedData)
        {
            var dbItem = await _unitOfWork.CourseLessons.GetByIdAsync(RequestedData.Id);
            if (dbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            RequestedData.Adapt(dbItem);

            _unitOfWork.CourseLessons.Update(dbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> DeleteLesson(CourseLessonDto RequestedData)
        {
            var DbItem = await _unitOfWork.CourseLessons.GetByIdAsync(RequestedData.Id);

            if (DbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            _unitOfWork.CourseLessons.Delete(DbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}