using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/division")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly DivisionService _service;

        public DivisionController(DivisionService service)
        {
            _service = service;
        }

        [HttpGet, Route("getdivisionlist")]
        public async Task<IActionResult> Get()
        {
            var divisions = await _service.GetAllAsync();
            return Ok(divisions);
        }

        [HttpGet, Route("gettobindtodropdown")]
        public async Task<IActionResult> GetBindGradeASync()
        {
            var divions = await _service.GetDivisonsToBind();
            return Ok(divions);
        }
        [HttpPost, Route("adddivision")]
        public async Task<IActionResult> Add(DivisionDc divisionDc)
        {
            var divisionDetails = await _service.CreateAsync(divisionDc);
            return Ok(divisionDetails);
        }

        [HttpPut, Route("editdivision")]
        public async Task<IActionResult> Update(DivisionDc divisionDc)
        {
            var divisionDetails = await _service.UpdateAsync(divisionDc);
            return Ok(divisionDetails);
        }

        [HttpDelete, Route("deletedivision/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));

        }
    }
}
