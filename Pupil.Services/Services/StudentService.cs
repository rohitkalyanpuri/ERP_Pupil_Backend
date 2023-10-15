using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pupil.DataLayer;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Pupil.Services
{
    public class StudentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<StudentDc>> CreateAsync(StudentDc studentDc)
        {
            var response = new SingleResponse<StudentDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsStudentExist(studentDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblStudent = _mapper.Map<Student>(studentDc);
                    _unitOfWork.StudentRepository.Insert(tblStudent);
                    await _unitOfWork.SaveChangesAsync();
                    studentDc.StudentId = tblStudent.StudentId;


                }
                response.Status = StatusCode.Ok;
                response.Message = "Student Successfully Created!";
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
                var student = await _unitOfWork.StudentRepository.GetByIdAsync(id);
                _unitOfWork.StudentRepository.Delete(student);
               await _unitOfWork.SaveChangesAsync();
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

        public async Task<ListResponse<StudentDc>> GetAllASync()
        {
            var response = new ListResponse<StudentDc>();
            try
            {
                response.Data = await _unitOfWork.StudentRepository.GetWithProjectionAsync(x => new StudentDc
                {
                    StudentId = x.StudentId,
                    FirstName = x.Fname,
                    LastName = x.Lname,
                    Mobile = x.Mobile,
                    DOB = x.DOB,
                    Status = x.Status,
                    Password = x.Password,
                    ParentId = x.ParentId,
                    DateOfJoin = x.DateOfJoin,
                    Email=x.Email
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<StudentDc>();
            }

            return response;
        }

        public async Task<SingleResponse<StudentDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<StudentDc>();
            try
            {
                response.Data = (await _unitOfWork.StudentRepository.GetWithProjectionAsync(x => new StudentDc
                {
                    StudentId = x.StudentId,
                    FirstName = x.Fname,
                    LastName = x.Lname,
                    Mobile = x.Mobile,
                    DOB = x.DOB,
                    Status = x.Status,
                    Password = x.Password,
                    ParentId = x.ParentId,
                    DateOfJoin = x.DateOfJoin,
                    Email = x.Email,
                },x => x.StudentId == id)).Single();
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new StudentDc();

            }
            return response;
        }

        public async Task<SingleResponse<StudentDc>> UpdateAsync(StudentDc studentDc)
        {
            var response = new SingleResponse<StudentDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsStudentExist(studentDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentDc.StudentId);
                    student.Fname = studentDc.FirstName;
                    student.Lname = studentDc.LastName;
                    student.Mobile = studentDc.Mobile;
                    student.Password = student.Password;
                    student.DOB = studentDc.DOB;
                    student.Status = studentDc.Status;
                    student.DateOfJoin = studentDc.DateOfJoin;
                    student.ParentId = studentDc.ParentId;
                    student.Email = studentDc.Email;
                    _unitOfWork.StudentRepository.Update(student);
                    await _unitOfWork.SaveChangesAsync();
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

        private async Task<List<KeyValuePair<string, string>>> IsStudentExist(StudentDc studentDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (studentDc.StudentId > 0)
                {
                    IsExists = (await _unitOfWork.StudentRepository.GetSearchAsync(x => x.Mobile == studentDc.Mobile && x.DOB == studentDc.DOB
                    && x.Fname == studentDc.FirstName && x.Lname == studentDc.LastName
                    && x.ParentId == studentDc.ParentId && x.StudentId != studentDc.StudentId)).Any();
                }
                else
                {
                    IsExists = (await _unitOfWork.StudentRepository.GetSearchAsync(x => x.Phone == studentDc.Mobile && x.DOB == studentDc.DOB
                    && x.Fname == studentDc.FirstName && x.Lname == studentDc.LastName
                    && x.ParentId == studentDc.ParentId)).Any();
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

        private async Task<int> GetStudentId(StudentDc studentDc)
        {
            int responses = 0;
            Student? student = (await _unitOfWork.StudentRepository.GetSearchAsync(x => x.Phone == studentDc.Mobile && x.DOB == studentDc.DOB
                    && x.Fname == studentDc.FirstName && x.Lname == studentDc.LastName
                    && x.ParentId == studentDc.ParentId)).FirstOrDefault();

            if (student != null)
                responses = student.StudentId;

            return responses;
        }

        public async Task<ListResponse<StudentImportDc>> ImportStudents(IEnumerable<StudentImportDc> requestObj)
        {
            var response = new ListResponse<StudentImportDc>();
            try
            {
                await _unitOfWork.Run(async () =>
                {
                    foreach (var item in requestObj)
                    {
                        var tblStudent = ConvertStudentImportToStudent(item);
                        int studentId = await GetStudentId(new StudentDc() { StudentId = 0, FirstName = tblStudent.Fname, LastName = tblStudent.Lname, DOB = tblStudent.DOB, Mobile = tblStudent.Mobile, ParentId = tblStudent.ParentId });
                        if (studentId > 0)
                        {
                            item.StudentId = studentId;
                        }
                        else
                        {
                            _unitOfWork.StudentRepository.Insert(tblStudent);
                            await _unitOfWork.SaveChangesAsync();
                            item.StudentId = tblStudent.StudentId;
                        }
                        await AddUpdateEnrollmentFromStudentImport(GetEnrollmentFromStudentImport(item));
                    }
                });
                
               
                response.Message = "Imported Successfully!";
                response.Status = StatusCode.Ok;
               
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = null;
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        private Student ConvertStudentImportToStudent(StudentImportDc studentImportDc)
        {
            Student student = new Student();
            student.Fname = studentImportDc.FirstName;
            student.Lname = studentImportDc.LastName;
            student.DOB = studentImportDc.DOB;
            student.Mobile = studentImportDc.Mobile;
            student.ParentId = studentImportDc.Parent.GetIdAfterSpilt("-", 1);
            //student.DateOfJoin = GeneralHelper.GetValidDate(studentImportDc.EnrollmentDate);
            return student;
        }

        private Enrollment GetEnrollmentFromStudentImport(StudentImportDc studentImportDc)
        {
            Enrollment enrollment = new Enrollment();
            enrollment.GradeId = studentImportDc.Grade.GetIdAfterSpilt("-", 1);
            enrollment.DivisionId = studentImportDc.Division.GetIdAfterSpilt("-", 1);
            enrollment.AcademicId = studentImportDc.AcademicYear.GetIdAfterSpilt("-", 1);
            enrollment.StudentId = studentImportDc.StudentId;
            enrollment.EnrollmentDate = GeneralHelper.GetValidDate(studentImportDc.EnrollmentDate)!.Value;
            return enrollment;
        }

        private async Task<bool> IsStudentEnrollmentExists(int studentId, int gradeId, DateTime enrollment)
        {
            var studentEnrollment = (await _unitOfWork.EnrollmentRepository.GetSearchAsync(x => x.StudentId == studentId && x.GradeId == gradeId)).FirstOrDefault();
            if (studentEnrollment != null)
                return true;
            else
                return false;
        }

        private async Task AddUpdateEnrollmentFromStudentImport(Enrollment enrollment)
        {
            try
            {
                var studentEnrollment = (await _unitOfWork.EnrollmentRepository.GetSearchAsync(x => x.StudentId == enrollment.StudentId && x.GradeId == enrollment.GradeId)).FirstOrDefault();
                if (studentEnrollment != null)
                {
                    studentEnrollment.AcademicId = enrollment.AcademicId;
                    studentEnrollment.DivisionId = enrollment.DivisionId;
                    studentEnrollment.EnrollmentDate = enrollment.EnrollmentDate;
                    _unitOfWork.EnrollmentRepository.Update(studentEnrollment);
                }
                else
                {
                    studentEnrollment = new Enrollment();
                    studentEnrollment.AcademicId = enrollment.AcademicId;
                    studentEnrollment.EnrollmentDate = enrollment.EnrollmentDate;
                    studentEnrollment.GradeId = enrollment.GradeId;
                    studentEnrollment.DivisionId = enrollment.DivisionId;
                    studentEnrollment.StudentId = enrollment.StudentId;
                    studentEnrollment.Cancelled = false;
                    _unitOfWork.EnrollmentRepository.Insert(studentEnrollment);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
