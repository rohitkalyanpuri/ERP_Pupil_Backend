using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using Pupil.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _service;
        private readonly EnrollmentService _enrollmentService;
        public StudentController(StudentService service, EnrollmentService enrollmentService)
        {
            _service = service;
            _enrollmentService = enrollmentService;
        }

        [HttpGet, Route("getstudentslist")]
        public async Task<IActionResult> Get()
        {
            var response =  await _service.GetAllASync();
            return Ok(response);
        }

        [HttpGet, Route("getstudentbyid/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
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

        [HttpPost, Route("importstudent")]
        public async Task<IActionResult> ImportStudent(IEnumerable<StudentImportDc> studentImportDc)
        {
            return Ok(
                await _service.ImportStudents(studentImportDc)
                );
        }

        [HttpGet, Route("getenrollments/{id:int}")]
        public async Task<IActionResult> GetEnrollments(int id)
        {
            var response = await _enrollmentService.GetEnrollments(id);
            return Ok(response);
        }

        [HttpPost, Route("saveenrollment")]
        public async Task<IActionResult> SaveEnrollment(EnrollmentDc enrollmentDc)
        {
            return Ok(
                await _enrollmentService.CreateAsync(enrollmentDc)
                );
        }

        [HttpDelete, Route("deleteenrollment/{id:Guid}")]
        public async Task<IActionResult> DeleteEnrollment(Guid id)
        {
            return Ok(await _enrollmentService.Delete(id));
        }
    }
}
