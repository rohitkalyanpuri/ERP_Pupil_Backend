using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.Entities;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pupil.Core.Enums;
using Pupil.Core.Constans;
using Pupil.Core.DataTransferObjects;

namespace Pupil.Infrastructure.Services
{
    public class ParentService : IParentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ParentService(ApplicationDbContext context,IMapper mapper)
        {
            _context = context; _mapper = mapper;
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
                    _context.Parent.Add(tblParent);
                    await _context.SaveChangesAsync();
                    parentDc.ParentId = tblParent.ParentId;
                    response.Status = StatusCode.Ok;
                    response.Message = "Parent Successfully Created!";
                }
                response.Data = parentDc;
            }
            catch(Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
            }
            return response;
        }



        public ListResponse<ParentDc> GetAllSync()
        {
            var response = new ListResponse<ParentDc>();
            try
            {
                
                response.Data = _context.Parent.Select(x => new ParentDc
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
            catch(Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error=ex.Message;
                response.Data = null; 
            }
            
            return response;
        }

        public async Task<SingleResponse<ParentDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<ParentDc>();
            try
            {
                response.Data = await _context.Parent.Select(x => new ParentDc
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
                }).FirstOrDefaultAsync(x=>x.ParentId==id);
                response.Status = StatusCode.Ok;
            }
            catch(Exception ex)
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
                var parent = await _context.Parent.FindAsync(id);
                _context.Parent.Remove(parent);
                _context.SaveChanges();
                response.Status = StatusCode.NoContent;
                response.Message = "Successfully Deleted!";
            }
            catch(Exception ex)
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
                else{
                    var parent = await _context.Parent.FindAsync(parentDc.ParentId);
                    parent.Fname = parentDc.FirstName;
                    parent.Lname = parentDc.LastName;
                    parent.Email = parentDc.Email;
                    parent.Mobile = parentDc.Mobile;
                    parent.DOB = parentDc.DOB;
                    _context.Parent.Update(parent);
                    _context.SaveChanges();
                    response.Status = StatusCode.Ok;
                    response.Message = "Parent Successfully Updated!";
                }
                response.Data = parentDc;
            }
            catch(Exception ex)
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
                    IsExists = _context.Parent.Where(x => x.Mobile == parentDc.Mobile && x.DOB == parentDc.DOB
                    && x.Fname == parentDc.FirstName && x.Lname == parentDc.LastName
                    && x.ParentId != parentDc.ParentId).Any();
                }
                else
                {
                    IsExists = _context.Parent.Where(x => x.Mobile == parentDc.Mobile && x.DOB == parentDc.DOB
                    && x.Fname == parentDc.FirstName && x.Lname == parentDc.LastName).Any();
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
                foreach(var item in requestObj)
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
                if(parents.Count > 0)
                {
                    await _context.Parent.AddRangeAsync(parents);
                    _context.SaveChanges();
                }

                if(notInsertedList.Count > 0)
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
            catch(Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = null;
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        } 
    }
}
