using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Interfaces;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/grade")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _service;

        public GradeController(IGradeService service)
        {
            _service = service;
        }

        [HttpGet, Route("getgradelist")]
        public IActionResult Get()
        {
            var grades = _service.GetAllSync();
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
