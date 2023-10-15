using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/feetype")]
    [ApiController]
    public class FeeTypeController : ControllerBase
    {
        private readonly FeeTypeService _service;

        public FeeTypeController(FeeTypeService service)
        {
            _service = service;
        }

        [HttpGet, Route("getfeetypelist")]
        public async Task<IActionResult> Get()
        {
            var feeTypes = await _service.GetAllAsync();
            return Ok(feeTypes);
        }

        [HttpGet, Route("gettobindtodropdown")]
        public async Task<IActionResult> GetBindGradeASync()
        {
            var feeTypes = await _service.GetBindFeeTypeASync();
            return Ok(feeTypes);
        }

        [HttpPost, Route("addfeetype")]
        public async Task<IActionResult> Add(FeeTypesDc feeTypesDc)
        {
            var feeTypeDetails = await _service.CreateAsync(feeTypesDc);
            return Ok(feeTypeDetails);
        }

        [HttpPut, Route("editfeetype")]
        public async Task<IActionResult> Update(FeeTypesDc feeTypesDc)
        {
            var feeTypeDetails = await _service.UpdateAsync(feeTypesDc);
            return Ok(feeTypeDetails);
        }

        [HttpDelete, Route("deletefeetype/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
