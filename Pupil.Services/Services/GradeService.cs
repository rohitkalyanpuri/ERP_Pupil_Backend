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
    public class GradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GradeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<GradeDc>> CreateAsync(GradeDc requestObj)
        {
            var response = new SingleResponse<GradeDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsGradeExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblGrade = _mapper.Map<Grade>(requestObj);
                    _unitOfWork.GradeRepository.Insert(tblGrade);
                    await _unitOfWork.SaveChangesAsync();
                    requestObj.GradeId = tblGrade.GradeId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Grade Successfully Created!";
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
                var grade = await _unitOfWork.GradeRepository.GetByIdAsync(id);
                _unitOfWork.GradeRepository.Delete(grade);
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

        public async Task<ListResponse<GradeDc>> GetAllAsync()
        {
            var response = new ListResponse<GradeDc>();
            try
            {

                response.Data = await _unitOfWork.GradeRepository.GetWithProjectionAsync(x => new GradeDc
                {
                    GradeId = x.GradeId,
                    Gname = x.Gname,
                    Gdesc = x.Gdesc
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<GradeDc>();
            }

            return response;
        }

        public async Task<ListResponse<BindToDropDownDc>> GetBindGradeASync()
        {
            var response = new ListResponse<BindToDropDownDc>();
            try
            {

                response.Data = await _unitOfWork.GradeRepository.GetWithProjectionAsync(x => new BindToDropDownDc
                {
                    Id = x.GradeId,
                    Name = x.Gname
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

        public async Task<SingleResponse<GradeDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<GradeDc>();
            try
            {
                response.Data = (await _unitOfWork.GradeRepository.GetWithProjectionAsync(x => new GradeDc
                {
                    GradeId = x.GradeId,
                    Gname = x.Gname,
                    Gdesc = x.Gdesc
                },x => x.GradeId == id)).Single();

                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new GradeDc();
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        public async Task<SingleResponse<GradeDc>> UpdateAsync(GradeDc gradeDc)
        {
            var response = new SingleResponse<GradeDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsGradeExist(gradeDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var grade = await _unitOfWork.GradeRepository.GetByIdAsync(gradeDc.GradeId);
                    grade.Gname = gradeDc.Gname;
                    grade.Gdesc = gradeDc.Gdesc;
                    _unitOfWork.GradeRepository.Update(grade);
                    await _unitOfWork.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Grade Successfully Updated!";
                }
                response.Data = gradeDc;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = gradeDc;
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        private async Task<List<KeyValuePair<string, string>>> IsGradeExist(GradeDc gradeDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (gradeDc.GradeId > 0)
                {
                    IsExists =(await _unitOfWork.GradeRepository.GetSearchAsync(x => x.Gname == gradeDc.Gname && x.GradeId != gradeDc.GradeId)).Any();
                }
                else
                {
                    IsExists = (await _unitOfWork.GradeRepository.GetSearchAsync(x => x.Gname == gradeDc.Gname)).Any();
                }

                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "Grade already exists in the system.");
                    responses.Add(mesgStr);
                }
            }
            catch (Exception ex)
            {

            }

            return responses;
        }

        public async Task<List<string>> GetGradesForExcel()
        {
            List<string> grades = new List<string>();
            try
            {
                foreach (Grade g in await _unitOfWork.GradeRepository.GetAllAsync())
                {
                    grades.Add(g.Gname + "-" + g.GradeId);
                }

            }
            catch (Exception ex)
            {

            }
            return grades;
        }
    }
}
