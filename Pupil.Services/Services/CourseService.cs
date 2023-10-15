using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pupil.DataLayer;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Services
{
    public class CourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<CourseDc>> CreateAsync(CourseDc requestObj)
        {
            var response = new SingleResponse<CourseDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsCourseExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblCourse = _mapper.Map<Course>(requestObj);
                    _unitOfWork.CourseRepository.Insert(tblCourse);
                    await _unitOfWork.SaveChangesAsync();
                    requestObj.CourseId = tblCourse.CourseId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Course Successfully Created!";
                }
                response.Data = requestObj;
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
                var Course = await _unitOfWork.CourseRepository.GetByIdAsync(id);
                _unitOfWork.CourseRepository.Delete(Course);
                await _unitOfWork.SaveChangesAsync();
                response.Status = StatusCode.NoContent;
                response.Message = "Successfully Deleted!";
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        public async Task<ListResponse<CourseDc>> GetAllAsync()
        {
            var response = new ListResponse<CourseDc>();
            try
            {

                response.Data = await _unitOfWork.CourseRepository.GetWithProjectionAsync(x => new CourseDc
                {
                    CourseId = x.CourseId,
                    Name = x.Cname,
                    Description = x.Cdesc
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<CourseDc>();
            }

            return response;
        }

        public async Task<SingleResponse<CourseDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<CourseDc>();
            try
            {
                response.Data = (await _unitOfWork.CourseRepository.GetWithProjectionAsync(x => new CourseDc
                {
                    CourseId = x.CourseId,
                    Name = x.Cname,
                    Description = x.Cdesc
                }, x => x.CourseId == id)).Single();

                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new CourseDc();
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        public async Task<SingleResponse<CourseDc>> UpdateAsync(CourseDc CourseDc)
        {
            var response = new SingleResponse<CourseDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsCourseExist(CourseDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var Course = await _unitOfWork.CourseRepository.GetByIdAsync(CourseDc.CourseId);
                    Course.Cname = CourseDc.Name;
                    Course.Cdesc = Course.Cdesc;
                    _unitOfWork.CourseRepository.Update(Course);
                    await _unitOfWork.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Course Successfully Updated!";
                }
                response.Data = CourseDc;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new CourseDc();
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        private async Task<List<KeyValuePair<string, string>>> IsCourseExist(CourseDc CourseDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (CourseDc.CourseId > 0)
                {
                    IsExists = (await _unitOfWork.CourseRepository.GetSearchAsync(x => x.Cname == CourseDc.Name && x.CourseId != CourseDc.CourseId)).Any();
                }
                else
                {
                    IsExists = (await _unitOfWork.CourseRepository.GetSearchAsync(x => x.Cname == CourseDc.Name)).Any();
                }

                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "Course already exists in the system.");
                    responses.Add(mesgStr);
                }
            }
            catch (Exception ex)
            {

            }

            return responses;
        }

        public async Task<List<string>> GetCoursesForExcel()
        {
            List<string> Courses = new List<string>();
            try
            {
                foreach (Course d in await _unitOfWork.CourseRepository.GetAllAsync())
                {
                    Courses.Add(d.Cname + "-" + d.CourseId);
                }

            }
            catch (Exception ex)
            {

            }
            return Courses;
        }

        public async Task<ListResponse<BindToDropDownDc>> GetCoursesToBind()
        {
            var response = new ListResponse<BindToDropDownDc>();
            try
            {
                response.Data = await _unitOfWork.CourseRepository.GetWithProjectionAsync(x => new BindToDropDownDc
                {
                    Id = x.CourseId,
                    Name = x.Cname
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<BindToDropDownDc>();
            }
            return response;
        }
    }
}

