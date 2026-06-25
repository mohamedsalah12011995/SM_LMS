using Microsoft.AspNetCore.Mvc;
using RM.Courses.Dtos;
using RM.Courses.Services;

namespace RM.Courses.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseLessonMaterialsController : ControllerBase
    {
        private readonly ICourseLessonMaterialService _service;

        public CourseLessonMaterialsController(
            ICourseLessonMaterialService service)
        {
            _service = service;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] CoursLessoneMaterialDto model)
        {
            return Ok(await _service.GetMaterialsList(model));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] CoursLessoneMaterialDto model)
        {
            return Ok(await _service.SaveMaterial(model));
        }
        [HttpPost("updateMaterial")]
        public async Task<IActionResult> UpdateMaterial([FromBody] CoursLessoneMaterialDto request)
        {
            // يمرر الـ Dto الخاص بالمرفقات مباشرة للـ Service المسؤولة عنها
            var result = await _service.UpdateMaterialById(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] CoursLessoneMaterialDto model)
        {
            return Ok(await _service.DeleteMaterial(model));
        }
    }
}