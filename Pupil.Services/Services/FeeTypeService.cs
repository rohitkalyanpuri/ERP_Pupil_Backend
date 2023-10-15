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
    public class FeeTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeeTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<SingleResponse<FeeTypesDc>> CreateAsync(FeeTypesDc requestObj)
        {
            var response = new SingleResponse<FeeTypesDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsFeeTypeExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblFeeType = _mapper.Map<FeeTypes>(requestObj);
                    _unitOfWork.FeeTypeRepository.Insert(tblFeeType);
                    await _unitOfWork.SaveChangesAsync();
                    requestObj.Id = tblFeeType.Id;
                    response.Status = StatusCode.Ok;
                    response.Message = "FeeType Successfully Created!";
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
                var feeTypes = await _unitOfWork.FeeTypeRepository.GetByIdAsync(id);
                _unitOfWork.FeeTypeRepository.Delete(feeTypes);
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

        public async Task<ListResponse<FeeTypesDc>> GetAllAsync()
        {
            var response = new ListResponse<FeeTypesDc>();
            try
            {

                response.Data = await _unitOfWork.FeeTypeRepository.GetWithProjectionAsync(x => new FeeTypesDc
                {
                    Id = x.Id,
                    FeeType = x.FeeType
                });
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<FeeTypesDc>();
            }

            return response;
        }

        public async Task<ListResponse<BindToDropDownDc>> GetBindFeeTypeASync()
        {
            var response = new ListResponse<BindToDropDownDc>();
            try
            {

                response.Data = await _unitOfWork.FeeTypeRepository.GetWithProjectionAsync(x => new BindToDropDownDc
                {
                    Id = x.Id,
                    Name = x.FeeType
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

        public async Task<SingleResponse<FeeTypesDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<FeeTypesDc>();
            try
            {
                response.Data = (await _unitOfWork.FeeTypeRepository.GetWithProjectionAsync(x => new FeeTypesDc
                {
                    Id = x.Id,
                    FeeType = x.FeeType
                }, x => x.Id == id)).Single();

                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new FeeTypesDc();
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        public async Task<SingleResponse<FeeTypesDc>> UpdateAsync(FeeTypesDc feeTypeDc)
        {
            var response = new SingleResponse<FeeTypesDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = await IsFeeTypeExist(feeTypeDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var feeTypes = await _unitOfWork.FeeTypeRepository.GetByIdAsync(feeTypeDc.Id);
                    feeTypes.FeeType = feeTypeDc.FeeType;
                    _unitOfWork.FeeTypeRepository.Update(feeTypes);
                    await _unitOfWork.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "FeeType Successfully Updated!";
                }
                response.Data = feeTypeDc;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = feeTypeDc;
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }

        private async Task<List<KeyValuePair<string, string>>> IsFeeTypeExist(FeeTypesDc FeeTypeDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (FeeTypeDc.Id > 0)
                {
                    IsExists = (await _unitOfWork.FeeTypeRepository.GetSearchAsync(x => x.FeeType == FeeTypeDc.FeeType && x.Id != FeeTypeDc.Id)).Any();
                }
                else
                {
                    IsExists = (await _unitOfWork.FeeTypeRepository.GetSearchAsync(x => x.FeeType == FeeTypeDc.FeeType)).Any();
                }

                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "FeeType already exists in the system.");
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
