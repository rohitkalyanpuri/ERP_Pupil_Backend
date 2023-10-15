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
    public class AcademicYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AcademicYearService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<AcademicYearDc>> CreateAsync(AcademicYearDc requestObj)
        {
            var response = new SingleResponse<AcademicYearDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsAcademicYearExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblAcademicYear = _mapper.Map<AcademicYear>(requestObj);
                    _unitOfWork.AcademicYearRepository.Insert(tblAcademicYear);
                    await _unitOfWork.SaveChangesAsync();
                    requestObj.AcademicId = tblAcademicYear.AcademicId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Academic Year Successfully Created!";
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
                var item = await _unitOfWork.AcademicYearRepository.GetByIdAsync(id);
                _unitOfWork.AcademicYearRepository.Delete(item);
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

        public async Task<List<string>> GetAcademicYearForExcel()
        {
            List<string> academicYears = new List<string>();
            try
            {
                foreach (AcademicYear y in await _unitOfWork.AcademicYearRepository.GetAllAsync())
                {
                    academicYears.Add(y.YearStartDate.Year + " To " + y.YearEndDate.Year + "-" + y.AcademicId);
                }

            }
            catch (Exception ex)
            {

            }
            return academicYears;
        }

        public async Task<ListResponse<AcademicYearDc>> GetAllASync()
        {
            var response = new ListResponse<AcademicYearDc>();
            try
            {

                response.Data = await _unitOfWork.AcademicYearRepository.GetWithProjectionAsync(x => new AcademicYearDc
                {
                    AcademicId = x.AcademicId,
                    Description = x.Description,
                    YearStartDate = x.YearStartDate,
                    YearEndDate = x.YearEndDate,
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<AcademicYearDc>();
            }

            return response;
        }

        public async Task<ListResponse<BindToDropDownDc>> GetBindAsync()
        {
            var response = new ListResponse<BindToDropDownDc>();
            try
            {

                response.Data = await _unitOfWork.AcademicYearRepository.GetWithProjectionAsync(x => new BindToDropDownDc
                {
                    Id = x.AcademicId,
                    Name = x.Description,
                    
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

        public async Task<SingleResponse<AcademicYearDc>> GetByIdAsync(int id)
        {

            var response = new SingleResponse<AcademicYearDc>();
            try
            {
                response.Data = (await _unitOfWork.AcademicYearRepository.GetWithProjectionAsync(x => new AcademicYearDc
                {
                    AcademicId = x.AcademicId,
                    Description = x.Description,
                    YearStartDate = x.YearStartDate,
                    YearEndDate = x.YearEndDate,
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate
                },x => x.AcademicId == id)).Single();
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new AcademicYearDc();
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        public async Task<SingleResponse<AcademicYearDc>> UpdateAsync(AcademicYearDc academicYearDc)
        {
            var response = new SingleResponse<AcademicYearDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsAcademicYearExist(academicYearDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var item = await _unitOfWork.AcademicYearRepository.GetByIdAsync(academicYearDc.AcademicId);
                    item.Description = academicYearDc.Description;
                    item.YearStartDate = academicYearDc.YearStartDate;
                    item.YearEndDate = academicYearDc.YearEndDate;
                    item.VacationStartDate = academicYearDc.VacationStartDate;
                    item.VacationEndDate = academicYearDc.VacationEndDate;
                    _unitOfWork.AcademicYearRepository.Update(item);
                    await _unitOfWork.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Academic Year Successfully Updated!";
                }
                response.Data = academicYearDc;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = academicYearDc;
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        private async Task<List<KeyValuePair<string, string>>> IsAcademicYearExist(AcademicYearDc academicYearDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (academicYearDc.AcademicId > 0)
                {
                    IsExists = (await _unitOfWork.AcademicYearRepository.GetSearchAsync(x => x.YearStartDate == academicYearDc.YearStartDate && x.YearEndDate == academicYearDc.YearEndDate && x.AcademicId != academicYearDc.AcademicId)).Any();
                }
                else
                {
                    IsExists = (await _unitOfWork.AcademicYearRepository.GetSearchAsync(x => x.YearStartDate == academicYearDc.YearStartDate && x.YearEndDate == academicYearDc.YearEndDate)).Any();
                }

                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "AcademicYear already exists in the system.");
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
