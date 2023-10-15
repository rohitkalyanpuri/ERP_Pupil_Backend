using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/exam")]
    [ApiController]
    public class ExamTypeController : ControllerBase
    {
        private readonly ExamTypeService _service;

        public ExamTypeController(ExamTypeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            return Ok(productDetails);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ExamType request)
        {
            return Ok(await _service.CreateAsync(request.Tname, request.Tdesc));
        }

        [HttpDelete]
        public async Task<string> Delete(int id)
        {
            await _service.Delete(id);
            return "Success";
        }
    }
}
