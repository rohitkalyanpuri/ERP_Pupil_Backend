using AutoMapper;
using Pupil.DataLayer;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Pupil.Services
{
    public class ParentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ParentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<SingleResponse<ParentDc>> CreateAsync(ParentDc parentDc)
        {
            var response = new SingleResponse<ParentDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsParentExist(parentDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblParent = _mapper.Map<Parent>(parentDc);
                    _unitOfWork.ParentRepository.Insert(tblParent);
                    await _unitOfWork.SaveChangesAsync();
                    parentDc.ParentId = tblParent.ParentId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Parent Successfully Created!";
                }
                response.Data = parentDc;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
            }
            return response;
        }



        public async Task<ListResponse<ParentDc>> GetAllAsync()
        {
            var response = new ListResponse<ParentDc>();
            try
            {
               
                response.Data =  await _unitOfWork.ParentRepository.GetWithProjectionAsync(x => new ParentDc
                {
                    FirstName = x.Fname,
                    LastName = x.Lname,
                    Email = x.Email,
                    Phone = x.Phone,
                    Mobile = x.Mobile,
                    LastLoginDate = x.LastLoginDate,
                    LastLoginIp = x.LastLoginIp,
                    DOB = x.DOB,
                    Status = x.Status,
                    Password = x.Password,
                    ParentId = x.ParentId
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

        public async Task<ListResponse<BindToDropDownDc>> GetBindParentAsync()
        {
            var response = new ListResponse<BindToDropDownDc>();
            try
            {

                response.Data = await _unitOfWork.ParentRepository.GetWithProjectionAsync(x => new BindToDropDownDc
                {
                    Id = x.ParentId,
                    Name = x.Fname+" "+x.Lname
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

        public async Task<SingleResponse<ParentDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<ParentDc>();
            try
            {
                response.Data = (await _unitOfWork.ParentRepository.GetWithProjectionAsync(x => new ParentDc
                {
                    FirstName = x.Fname,
                    LastName = x.Lname,
                    Email = x.Email,
                    Phone = x.Phone,
                    Mobile = x.Mobile,
                    LastLoginDate = x.LastLoginDate,
                    LastLoginIp = x.LastLoginIp,
                    DOB = x.DOB,
                    Status = x.Status,
                    Password = x.Password,
                    ParentId = x.ParentId
                },x=>x.ParentId==id)).Single();
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
        public async Task<Response> Delete(int id)
        {
            var response = new Response();
            try
            {
                 _unitOfWork.ParentRepository.Delete(id);
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

        public async Task<SingleResponse<ParentDc>> UpdateAsync(ParentDc parentDc)
        {
            var response = new SingleResponse<ParentDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsParentExist(parentDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var parent = await _unitOfWork.ParentRepository.GetByIdAsync(parentDc.ParentId);
                    parent.Fname = parentDc.FirstName;
                    parent.Lname = parentDc.LastName;
                    parent.Email = parentDc.Email;
                    parent.Mobile = parentDc.Mobile;
                    parent.DOB = parentDc.DOB;
                    _unitOfWork.ParentRepository.Update(parent);
                    await _unitOfWork.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Parent Successfully Updated!";
                }
                response.Data = parentDc;
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

        public List<KeyValuePair<string, string>> IsParentExist(ParentDc parentDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (parentDc.ParentId > 0)
                {
                    if (_unitOfWork.ParentRepository.GetSearchCount(x => x.Mobile == parentDc.Mobile && x.DOB == parentDc.DOB
                    && x.Fname == parentDc.FirstName && x.Lname == parentDc.LastName
                    && x.ParentId != parentDc.ParentId) > 0)
                        IsExists = true;
                }
                else
                {
                    if (_unitOfWork.ParentRepository.GetSearchCount(x => x.Mobile == parentDc.Mobile && x.DOB == parentDc.DOB
                    && x.Fname == parentDc.FirstName && x.Lname == parentDc.LastName) > 0)
                        IsExists = true;
                   
                }

                if (IsExists)
                {
                    KeyValuePair<string, string> mesgStr = new KeyValuePair<string, string>("Key", "Parent already exists in the system.");
                    responses.Add(mesgStr);
                }
            }
            catch (Exception ex)
            {

            }

            return responses;
        }

        public async Task<ListResponse<ParentDc>> ImportParents(IEnumerable<ParentDc> requestObj)
        {
            var response = new ListResponse<ParentDc>();
            List<ParentDc> notInsertedList = new List<ParentDc>();
            List<Parent> parents = new List<Parent>();
            try
            {
                foreach (var item in requestObj)
                {
                    List<KeyValuePair<string, string>> responses = IsParentExist(item);
                    if (responses.Count > 0)
                    {
                        notInsertedList.Add(item);
                    }
                    else
                    {
                        var tblParent = _mapper.Map<Parent>(item);
                        parents.Add(tblParent);
                    }
                }
                if (parents.Count > 0)
                {
                    _unitOfWork.ParentRepository.InsertRange(parents);
                    await _unitOfWork.SaveChangesAsync();
                }

                if (notInsertedList.Count > 0)
                {
                    response.Data = notInsertedList;
                    response.Message = "Few records not inserted because Parent already exists in the system. Please find these records in grid.";
                }
                else
                {
                    response.Data = null;
                    response.Message = "All records inserted successfully!";
                }

                //if (parents.Count > 0 && notInsertedList.Count > 0)
                //    response.Status = StatusCode.PartiallyImported;
                if (parents.Count > 0 && notInsertedList.Count > 0)
                    response.Status = StatusCode.Ok;
                else if (parents.Count > 0 && !notInsertedList.Any())
                    response.Status = StatusCode.Ok;
                else if (!parents.Any())
                    response.Status = StatusCode.NotImported;
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

        public async Task<List<string>> GetParentsForExcel()
        {
            List<string> parents = new List<string>();
            try
            {
                foreach (Parent p in await _unitOfWork.ParentRepository.GetAllAsync())
                {
                    parents.Add(p.Fname + " " + p.Lname + "-" + p.ParentId);
                }

            }
            catch (Exception ex)
            {

            }
            return parents;
        }
    }
}
