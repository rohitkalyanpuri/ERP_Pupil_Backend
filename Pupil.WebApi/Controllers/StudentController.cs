using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.Interfaces;
using System.Threading.Tasks;
using System;
using Pupil.Core.DataTransferObjects;

namespace Pupil.WebApi.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet, Route("getstudentslist")]
        public IActionResult Get()
        {
            var response = _service.GetAllSync();
            return Ok(response);
        }

        [HttpPost, Route("addstudent")]
        public async Task<IActionResult> Add(StudentDc studentDc)
        {
            var response = await _service.CreateAsync(studentDc);
            return Ok(response);
        }

        [HttpPut, Route("editstudent")]
        public async Task<IActionResult> Update(StudentDc studentDc)
        {
            var response = await _service.UpdateAsync(studentDc);
            return Ok(response);
        }

        [HttpDelete, Route("deletestudent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
