using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pupil.Core.Constans;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Entities;
using Pupil.Core.Enums;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<SingleResponse<StudentDc>> CreateAsync(StudentDc studentDc)
        {
            var response = new SingleResponse<StudentDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsStudentExist(studentDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblStudent = _mapper.Map<Student>(studentDc);
                    _context.Student.Add(tblStudent);
                    await _context.SaveChangesAsync();
                    studentDc.StudentId = tblStudent.StudentId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Student Successfully Created!";
                }
                response.Data = studentDc;

            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
            }
            return response;
        }

        public async Task<Response> Delete(int id)
        {
            var response = new Response();
            try
            {
                var student = await _context.Student.FindAsync(id);
                _context.Student.Remove(student);
                _context.SaveChanges();
                response.Status = StatusCode.NoContent;
                response.Message = "Successfully Deleted!";
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
            }
            return response;
        }

        public ListResponse<StudentDc> GetAllSync()
        {
            var response = new ListResponse<StudentDc>();
            try
            {
                response.Data = _context.Student.Select(x => new StudentDc
                {
                    StudentId = x.StudentId,
                    FirstName = x.Fname,
                    LastName = x.Lname,
                    Mobile = x.Mobile,
                    DOB = x.DOB,
                    Status = x.Status,
                    Password = x.Password,
                    ParentId = x.ParentId,
                    DateOfJoin=x.DateOfJoin
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = null;
            }

            return response;
        }

        public async Task<SingleResponse<StudentDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<StudentDc>();
            try
            {
                response.Data = await _context.Student.Select(x => new StudentDc
                {
                    StudentId = x.StudentId,
                    FirstName = x.Fname,
                    LastName = x.Lname,
                    Mobile = x.Mobile,
                    DOB = x.DOB,
                    Status = x.Status,
                    Password = x.Password,
                    ParentId = x.ParentId,
                    DateOfJoin = x.DateOfJoin
                }).FirstOrDefaultAsync(x => x.StudentId == id);
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = null;

            }
            return response;
        }

        public async Task<SingleResponse<StudentDc>> UpdateAsync(StudentDc studentDc)
        {
            var response = new SingleResponse<StudentDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsStudentExist(studentDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var student = await _context.Student.FindAsync(studentDc.StudentId);
                    student.Fname = studentDc.FirstName;
                    student.Lname = studentDc.LastName;
                    student.Mobile = studentDc.Mobile;
                    student.Password = student.Password;
                    student.DOB = studentDc.DOB;
                    student.Status = studentDc.Status;
                    student.DateOfJoin = studentDc.DateOfJoin;
                    student.ParentId = studentDc.ParentId;
                    _context.Student.Update(student);
                    _context.SaveChanges();
                    response.Status = StatusCode.Ok;
                    response.Message = "Student Successfully Updated!";
                }
                response.Data = studentDc;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = null;
            }
            return response;
        }

        public List<KeyValuePair<string, string>> IsStudentExist(StudentDc studentDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if(studentDc.StudentId > 0)
                {
                    IsExists = _context.Student.Where(x => x.Mobile == studentDc.Mobile && x.DOB == studentDc.DOB 
                    && x.Fname == studentDc.FirstName && x.Lname == studentDc.LastName 
                    && x.ParentId == studentDc.ParentId && x.StudentId != studentDc.StudentId).Any();
                }
                else
                {
                    IsExists = _context.Student.Where(x => x.Phone == studentDc.Mobile && x.DOB == studentDc.DOB
                    && x.Fname == studentDc.FirstName && x.Lname == studentDc.LastName
                    && x.ParentId == studentDc.ParentId ).Any();
                }
                
                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "Student already exists in the system.");
                    responses.Add(mesgStr);
                }
            }
            catch (Exception ex)
            {
                
            }

            return responses;
        }
    }
}
