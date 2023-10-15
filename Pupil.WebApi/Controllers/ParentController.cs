using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/parent")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly ParentService _service;
        public ParentController(ParentService service)
        {
            _service = service;
        }

        [HttpGet, Route("getparentslist")]
        public async Task<IActionResult> Get()
        {
            var parents = await _service.GetAllAsync();
            return Ok(parents);
        }

        [HttpGet, Route("gettobindtodropdown")]
        public async Task<IActionResult> GetBindParentAsync()
        {
            var parents = await _service.GetBindParentAsync();
            return Ok(parents);
        }

        [HttpPost, Route("addparent")]
        public async Task<IActionResult> Add(ParentDc parentDc)
        {
            var productDetails = await _service.CreateAsync(parentDc);
            return Ok(productDetails);
        }

        [HttpPut, Route("editparent")]
        public async Task<IActionResult> Update(ParentDc parentDc)
        {
            var productDetails = await _service.UpdateAsync(parentDc);
            return Ok(productDetails);
        }

        [HttpDelete, Route("deleteparent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));

        }

        [HttpPost, Route("importparent")]
        public async Task<IActionResult> ImportParent(IEnumerable<ParentDc> parentDc)
        {
            return Ok(await _service.ImportParents(parentDc));
        }
    }
}
