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
    public class DivisionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DivisionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<DivisionDc>> CreateAsync(DivisionDc requestObj)
        {
            var response = new SingleResponse<DivisionDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsDivisionExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblDivision = _mapper.Map<Division>(requestObj);
                    _unitOfWork.DivisionRepository.Insert(tblDivision);
                    await _unitOfWork.SaveChangesAsync();
                    requestObj.DivisionId = tblDivision.DivisionId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Division Successfully Created!";
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
                var division = await _unitOfWork.DivisionRepository.GetByIdAsync(id);
                _unitOfWork.DivisionRepository.Delete(division);
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

        public async Task<ListResponse<DivisionDc>> GetAllAsync()
        {
            var response = new ListResponse<DivisionDc>();
            try
            {

                response.Data = await _unitOfWork.DivisionRepository.GetWithProjectionAsync(x => new DivisionDc
                {
                    DivisionId = x.DivisionId,
                    DivisionName = x.Dname,
                    DivisionDesc = x.Ddesc
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<DivisionDc>();
            }

            return response;
        }

        public async Task<SingleResponse<DivisionDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<DivisionDc>();
            try
            {
                response.Data = (await _unitOfWork.DivisionRepository.GetWithProjectionAsync(x => new DivisionDc
                {
                    DivisionId = x.DivisionId,
                    DivisionName = x.Dname,
                    DivisionDesc = x.Ddesc
                },x => x.DivisionId == id)).Single();

                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new DivisionDc();
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        public async Task<SingleResponse<DivisionDc>> UpdateAsync(DivisionDc divisionDc)
        {
            var response = new SingleResponse<DivisionDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsDivisionExist(divisionDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var division = await _unitOfWork.DivisionRepository.GetByIdAsync(divisionDc.DivisionId);
                    division.Dname = divisionDc.DivisionName;
                    division.Ddesc = division.Ddesc;
                    _unitOfWork.DivisionRepository.Update(division);
                    await _unitOfWork.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Division Successfully Updated!";
                }
                response.Data = divisionDc;
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

        private async Task<List<KeyValuePair<string, string>>> IsDivisionExist(DivisionDc divisionDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (divisionDc.DivisionId > 0)
                {
                    IsExists = (await _unitOfWork.DivisionRepository.GetSearchAsync(x => x.Dname == divisionDc.DivisionName && x.DivisionId != divisionDc.DivisionId)).Any();
                }
                else
                {
                    IsExists = (await _unitOfWork.DivisionRepository.GetSearchAsync(x => x.Dname == divisionDc.DivisionName)).Any();
                }

                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "Division already exists in the system.");
                    responses.Add(mesgStr);
                }
            }
            catch (Exception ex)
            {

            }

            return responses;
        }

        public async Task<List<string>> GetDivisionsForExcel()
        {
            List<string> divisions = new List<string>();
            try
            {
                foreach (Division d in await _unitOfWork.DivisionRepository.GetAllAsync())
                {
                    divisions.Add(d.Dname + "-" + d.DivisionId);
                }

            }
            catch (Exception ex)
            {

            }
            return divisions;
        }

        public async Task<ListResponse<BindToDropDownDc>> GetDivisonsToBind()
        {
            var response = new ListResponse<BindToDropDownDc>();
            try
            {
                response.Data = await _unitOfWork.DivisionRepository.GetWithProjectionAsync(x => new BindToDropDownDc
                {
                    Id = x.DivisionId,
                    Name = x.Dname
                });
                response.Status = StatusCode.Ok;
            }
            catch(Exception ex)
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
