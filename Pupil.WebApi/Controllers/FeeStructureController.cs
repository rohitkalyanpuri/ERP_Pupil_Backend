using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/feestructure")]
    [ApiController]
    public class FeeStructureController : ControllerBase
    {
        private readonly FeeStructureService _service;
        public FeeStructureController(FeeStructureService service)
        {
            _service = service;
        }
        [HttpPost, Route("addupdate")]
        public async Task<IActionResult> Add(FeeStructureRequestDc requestObj)
        {
            if (requestObj.FeeStructure.FeeStructureId == 0)
            {
                var response = await _service.CreateAsync(requestObj);
                return Ok(response);
            }
            else
            {
                var response = await _service.UpdateAsync(requestObj);
                return Ok(response);
            }
        }

        [HttpGet, Route("getlist")]
        public async Task<IActionResult> Get()
        {
            var stuctures = await _service.GetAllAsync();
            return Ok(stuctures);
        }

        [HttpGet, Route("getbyid/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stuctures = await _service.GetById(id);
            return Ok(stuctures);
        }
    }
}
