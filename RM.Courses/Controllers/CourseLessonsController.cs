using Microsoft.AspNetCore.Mvc;
using RM.Courses.Dtos;

using RM.Courses.Services;

namespace RM.Courses.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseLessonsController : ControllerBase
    {
        private readonly ICourseLessonService _service;

        public CourseLessonsController(
            ICourseLessonService service)
        {
            _service = service;
        }
        [HttpPost("sectionsList")]
        public async Task<IActionResult> GetSectionsList([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.GetSectionsList(model));
        }

        [HttpPost("saveSection")]
        public async Task<IActionResult> SaveSection([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.SaveSection(model));
        }
        [HttpPost("updateSection")]
        public async Task<IActionResult> UpdateSection([FromBody] CourseLessonDto request)
        {
            var result = await _service.UpdateSectionById(request);
            return Ok(result);
        }
        [HttpPost("deleteSection")]
        public async Task<IActionResult> DeleteSection([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.DeleteSection(model));
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.GetLessonsList(model));
        }

        [HttpPost("details")]
        public async Task<IActionResult> Details([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.GetLessonDetails(model));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.SaveLesson(model));
        }
        [HttpPost("updateLesson")]
        public async Task<IActionResult> UpdateLesson([FromBody] CourseLessonDto request)
        {
            var result = await _service.UpdateLessonById(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] CourseLessonDto model)
        {
            return Ok(await _service.DeleteLesson(model));
        }
    }
}