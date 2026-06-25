using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Courses.Dtos;
using RM.Courses.Dtos.OperationOutput;
using RM.Courses.Records;
using RM.Courses.UnitOfWorks;
using RM.EF.Models.Course;
using RM.Models;
using static RM.Courses.Dtos.OperationOutput.OperationOutput;


namespace RM.Courses.Services
{
    public class CourseService : BaseService, ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)

            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetCourseseList(CourseDto RequestedData)
        {
            var data = await _unitOfWork.Courses
                .GetAll()
                .Where(x => x.ReferenceId == RequestedData.ReferenceId &&
                           (!RequestedData.IsActive || x.IsActive == RequestedData.IsActive))
                .ToListAsync();

            var mappedData = data.Adapt<List<CourseDto>>();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.CoursesEntity, mappedData));
        }

        public async Task<OperationOutput> GetCourseseDetails(CourseDto RequestedData)
        {
            var data = await _unitOfWork.Courses
                .GetAll()
                .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.ReferenceId == RequestedData.ReferenceId);

            if (data == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var mappedData = data.Adapt<CourseDto>();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.CoursesEntity, mappedData));
        }

        public async Task<OperationOutput> SaveCourse(CourseDto RequestedData)
        {
            Course DbItem;

            if (RequestedData.Id > 0)
            {
                DbItem = await _unitOfWork.Courses.GetByIdAsync(RequestedData.Id);

                if (DbItem == null || DbItem.ReferenceId != RequestedData.ReferenceId)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);

                DbItem.UpdatedDate = DateTime.Now;
                _unitOfWork.Courses.Update(DbItem);
            }
            else
            {
                DbItem = RequestedData.Adapt<Course>();

                DbItem.CreatedDate = DateTime.Now;
                DbItem.IsActive = true;
                DbItem.IsDeleted = false;

                await _unitOfWork.Courses.AddAsync(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> UpdateCourseById(CourseDto RequestedData)
        {
            var dbItem = await _unitOfWork.Courses.GetByIdAsync(RequestedData.Id);

            if (dbItem == null || dbItem.ReferenceId != RequestedData.ReferenceId)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            RequestedData.Adapt(dbItem);
            dbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.Courses.Update(dbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> ToggleCourseStatus(CourseDto RequestedData)
        {
            var dbItem = await _unitOfWork.Courses.GetByIdAsync(RequestedData.Id);

            if (dbItem == null || dbItem.ReferenceId != RequestedData.ReferenceId)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            dbItem.IsActive = RequestedData.IsActive;
            dbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.Courses.Update(dbItem);

            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeleteCourse(CourseDto RequestedData)
        {
            var DbItem = await _unitOfWork.Courses.GetByIdAsync(RequestedData.Id);

            if (DbItem == null || DbItem.ReferenceId != RequestedData.ReferenceId)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            _unitOfWork.Courses.Delete(DbItem);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public OperationOutput ModelActions(CourseDto RequestedData)
        {
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetAllCategories(CourseCategoryDto RequestedData)
        {
            var data = await _unitOfWork.CourseCategorys
                .GetAll()
                .Where(x => x.ReferenceId == RequestedData.ReferenceId &&
                           (!RequestedData.IsActive || x.IsActive == RequestedData.IsActive))
                .ToListAsync();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.CategoriesEntity, data.Adapt<List<CourseCategoryDto>>()));
        }

        public async Task<OperationOutput> SaveCategories(SaveCourseCategoryRecord model)
        {
            CourseCategory category;

            if (model.Id > 0)
            {
                category = await _unitOfWork.CourseCategorys.GetByIdAsync(model.Id.Value);
                if (category == null || category.ReferenceId != model.ReferenceId)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                model.Adapt(category);
            }
            else
            {
                category = model.Adapt<CourseCategory>();
                await _unitOfWork.CourseCategorys.AddAsync(category);
            }

            category.ReferenceId = model.ReferenceId;
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetAllInstructors(InstructorDto RequestedData)
        {
            var data = await _unitOfWork.Instructors
                .GetAll()
                .Where(x => x.ReferenceId == RequestedData.ReferenceId)
                .ToListAsync();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.CoursesEntity, data.Adapt<List<InstructorDto>>()));
        }

        public async Task<OperationOutput> SaveInstructor(SaveInstructorRecord model)
        {
            Instructor DbItem = new();

            if (model.Id > 0)
            {
                DbItem = await _unitOfWork.Instructors.GetByIdAsync(model.Id.Value);
                if (DbItem == null || DbItem.ReferenceId != model.ReferenceId)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                model.Adapt(DbItem);
                _unitOfWork.Instructors.Update(DbItem);
            }
            else
            {
                DbItem = model.Adapt<Instructor>();
                await _unitOfWork.Instructors.AddAsync(DbItem);
            }

            DbItem.ReferenceId = model.ReferenceId;
            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetAllTags(CourseTagDto RequestedData)
        {
            var data = await _unitOfWork.CourseTags
                .GetAll()
                .Where(x => x.ReferenceId == RequestedData.ReferenceId)
                .ToListAsync();

            return OperationOutput.GetOperationOutput(
                header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.CoursesEntity, data.Adapt<List<CourseTagDto>>()));
        }

        public async Task<OperationOutput> SaveTag(SaveTagRecord model)
        {
            CourseTag DbItem = new();

            if (model.Id > 0)
            {
                DbItem = await _unitOfWork.CourseTags.GetByIdAsync(model.Id.Value);
                if (DbItem == null || DbItem.ReferenceId != model.ReferenceId)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                model.Adapt(DbItem);
                _unitOfWork.CourseTags.Update(DbItem);
            }
            else
            {
                DbItem = model.Adapt<CourseTag>();
                await _unitOfWork.CourseTags.AddAsync(DbItem);
            }

            DbItem.ReferenceId = model.ReferenceId;
            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> SaveCourseLearningOutcomes(CourseDto RequestedData)
        {
            CourseLearningOutcome DbItem = new();

            if (RequestedData.Id > 0)
            {
                DbItem = await _unitOfWork.CourseLearningOutcomes.GetByIdAsync(RequestedData.Id);
                if (DbItem == null) return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseLearningOutcomes.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseLearningOutcomes.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> SaveCoursePrerequisites(CourseDto RequestedData)
        {
            CoursePrerequisite DbItem = new();

            if (RequestedData.Id > 0)
            {
                DbItem = await _unitOfWork.CoursePrerequisites.GetByIdAsync(RequestedData.Id);
                if (DbItem == null) return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);
                _unitOfWork.CoursePrerequisites.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem);
                _unitOfWork.CoursePrerequisites.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> SaveCourseTargetAudiences(CourseDto RequestedData)
        {
            CourseTargetAudience DbItem = new();

            if (RequestedData.Id > 0)
            {
                DbItem = await _unitOfWork.CourseTargetAudiences.GetByIdAsync(RequestedData.Id);
                if (DbItem == null) return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseTargetAudiences.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem);
                _unitOfWork.CourseTargetAudiences.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

       
    }

}