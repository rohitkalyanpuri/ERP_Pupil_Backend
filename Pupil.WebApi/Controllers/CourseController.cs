
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        [HttpGet, Route("getcourselist")]
        public async Task<IActionResult> Get()
        {
            var Courses = await _service.GetAllAsync();
            return Ok(Courses);
        }

        [HttpGet, Route("gettobindtodropdown")]
        public async Task<IActionResult> GetBindCourseASync()
        {
            var Courses = await _service.GetCoursesToBind();
            return Ok(Courses);
        }

        [HttpPost, Route("addcourse")]
        public async Task<IActionResult> Add(CourseDc CourseDc)
        {
            var CourseDetails = await _service.CreateAsync(CourseDc);
            return Ok(CourseDetails);
        }

        [HttpPut, Route("editcourse")]
        public async Task<IActionResult> Update(CourseDc CourseDc)
        {
            var CourseDetails = await _service.UpdateAsync(CourseDc);
            return Ok(CourseDetails);
        }

        [HttpDelete, Route("deletecourse/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));

        }
    }
}
