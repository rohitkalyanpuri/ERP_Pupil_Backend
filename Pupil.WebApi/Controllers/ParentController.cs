using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/parent")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly IParentService _service;

        public ParentController(IParentService service)
        {
            _service = service;
        }

        [HttpGet,Route("getparentslist")]
        public IActionResult Get()
        {
            var parents =  _service.GetAllSync();
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
