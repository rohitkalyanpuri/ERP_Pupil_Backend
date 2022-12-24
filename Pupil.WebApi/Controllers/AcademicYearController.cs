using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Interfaces;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/academic")]
    [ApiController]
    public class AcademicYearController : ControllerBase
    {
        private readonly IAcademicYearService _service;

        public AcademicYearController(IAcademicYearService service)
        {
            _service = service;
        }

        [HttpGet, Route("getacademiclist")]
        public IActionResult Get()
        {
            var grades = _service.GetAllSync();
            return Ok(grades);
        }

        [HttpPost, Route("addacademic")]
        public async Task<IActionResult> Add(AcademicYearDc academicYearDc)
        {
            var gradeDetails = await _service.CreateAsync(academicYearDc);
            return Ok(gradeDetails);
        }

        [HttpPut, Route("editacademic")]
        public async Task<IActionResult> Update(AcademicYearDc academicYearDc)
        {
            var gradeDetails = await _service.UpdateAsync(academicYearDc);
            return Ok(gradeDetails);
        }

        [HttpDelete, Route("deleteacademic/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));

        }
    }
}
