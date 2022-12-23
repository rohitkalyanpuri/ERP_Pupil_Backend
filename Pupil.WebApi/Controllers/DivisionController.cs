using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Interfaces;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/division")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly IDivisionService _service;

        public DivisionController(IDivisionService service)
        {
            _service = service;
        }

        [HttpGet, Route("getdivisionlist")]
        public IActionResult Get()
        {
            var divisions = _service.GetAllSync();
            return Ok(divisions);
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
