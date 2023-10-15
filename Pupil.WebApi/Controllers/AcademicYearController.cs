using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/academic")]
    [ApiController]
    public class AcademicYearController : ControllerBase
    {
        private readonly AcademicYearService _service;

        public AcademicYearController(AcademicYearService service)
        {
            _service = service;
        }

        [HttpGet, Route("getacademiclist")]
        public async Task<IActionResult> Get()
        {
            var academics = await _service.GetAllASync();
            return Ok(academics);
        }

        [HttpGet, Route("gettobindtodropdown")]
        public async Task<IActionResult> GetBindAsync()
        {
            var academics = await _service.GetBindAsync();
            return Ok(academics);
        }

        [HttpPost, Route("addacademic")]
        public async Task<IActionResult> Add(AcademicYearDc academicYearDc)
        {
            var academicetails = await _service.CreateAsync(academicYearDc);
            return Ok(academicetails);
        }

        [HttpPut, Route("editacademic")]
        public async Task<IActionResult> Update(AcademicYearDc academicYearDc)
        {
            var academicetails = await _service.UpdateAsync(academicYearDc);
            return Ok(academicetails);
        }

        [HttpDelete, Route("deleteacademic/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));

        }
    }
}
