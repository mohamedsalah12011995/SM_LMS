using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Courses.Dtos;
using RM.Courses.Records;
using RM.Courses.Services;

namespace RM.Courses.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;

        public CoursesController(ICourseService service)
        {
            _service = service;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList(CourseDto model)
        {
            return Ok(await _service.GetCourseseList(model));
        }

        [HttpPost("details")]
        public async Task<IActionResult> Details(CourseDto model)
        {
            return Ok(await _service.GetCourseseDetails(model));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(CourseDto model)
        {
            return Ok(await _service.SaveCourse(model));
        }
        [HttpPost("updateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] SaveCourseRecord request)
        {
            var result = await _service.UpdateCourseById(request.Adapt<CourseDto>());
            return Ok(result);
        }
        [HttpPost("toggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromBody] SaveCourseRecord request)
        {
            var result = await _service.ToggleCourseStatus(request.Adapt<CourseDto>());

            return Ok(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(CourseDto model)
        {
            return Ok(await _service.DeleteCourse(model));
        }

        [HttpPost("getAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromBody] CourseCategoryDto RequestedData)
        {
            return Ok(await _service.GetAllCategories(RequestedData));
        }

        [HttpPost("saveCategory")]
        public async Task<IActionResult> SaveCategory([FromBody] SaveCourseCategoryRecord model)
        {
            return Ok(await _service.SaveCategories(model));
        }

        [HttpPost("getAllInstructors")]
        public async Task<IActionResult> GetAllInstructors([FromBody] InstructorDto model)
        {
            return Ok(await _service.GetAllInstructors(model));
        }

        [HttpPost("saveInstructor")]
        public async Task<IActionResult> SaveInstructor([FromBody] SaveInstructorRecord model)
        {
            return Ok(await _service.SaveInstructor(model));
        }

        [HttpPost("getAllTags")]
        public async Task<IActionResult> GetAllTags([FromBody] CourseTagDto model)
        {
            return Ok(await _service.GetAllTags(model));
        }

        [HttpPost("saveTag")]
        public async Task<IActionResult> SaveTag([FromBody] SaveTagRecord model)
        {
            return Ok(await _service.SaveTag(model));
        }

        [HttpPost("saveLearningOutcome")]
        public async Task<IActionResult> SaveLearningOutcome([FromBody] CourseDto model)
        {
            return Ok(await _service.SaveCourseLearningOutcomes(model));
        }

        [HttpPost("savePrerequisite")]
        public async Task<IActionResult> SavePrerequisite([FromBody] CourseDto model)
        {
            return Ok(await _service.SaveCoursePrerequisites(model));
        }

        [HttpPost("saveTargetAudience")]
        public async Task<IActionResult> SaveTargetAudience([FromBody] CourseDto model)
        {
            return Ok(await _service.SaveCourseTargetAudiences(model));
        }

       
    }
}