
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/grade")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly GradeService _service;

        public GradeController(GradeService service)
        {
            _service = service;
        }

        [HttpGet, Route("getgradelist")]
        public async Task<IActionResult> Get()
        {
            var grades = await _service.GetAllAsync();
            return Ok(grades);
        }

        [HttpGet, Route("gettobindtodropdown")]
        public async Task<IActionResult> GetBindGradeASync()
        {
            var grades = await _service.GetBindGradeASync();
            return Ok(grades);
        }

        [HttpPost, Route("addgrade")]
        public async Task<IActionResult> Add(GradeDc gradeDc)
        {
            var gradeDetails = await _service.CreateAsync(gradeDc);
            return Ok(gradeDetails);
        }

        [HttpPut, Route("editgrade")]
        public async Task<IActionResult> Update(GradeDc gradeDc)
        {
            var gradeDetails = await _service.UpdateAsync(gradeDc);
            return Ok(gradeDetails);
        }

        [HttpDelete, Route("deletegrade/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));

        }
    }
}
