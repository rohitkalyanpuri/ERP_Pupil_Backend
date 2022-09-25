using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly IParentService _service;

        public ParentController(IParentService service)
        {
            _service = service;
        }

        [HttpGet,Route("Getparentslist")]
        public IActionResult GetParentsList()
        {
            var productDetails =  _service.GetAllSync();
            return Ok(productDetails);
        }

        [HttpPost, Route("Addparent")]
        public async Task<IActionResult> Addparent(ParentDc parentDc)
        {
            var productDetails = await _service.CreateAsync(parentDc);
            return Ok(productDetails);
        }

        [HttpPut, Route("Editparent")]
        public async Task<IActionResult> Editparent(ParentDc parentDc)
        {
            var productDetails = await _service.UpdateAsync(parentDc);
            return Ok(productDetails);
        }

        [HttpDelete, Route("Deleteparent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return Ok(true);
            }
            catch(Exception ex)
            {
               return BadRequest();
            }
            
        }
    }
}
