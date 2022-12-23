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
    public class DivisionService : IDivisionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DivisionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<SingleResponse<DivisionDc>> CreateAsync(DivisionDc requestObj)
        {
            var response = new SingleResponse<DivisionDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsDivisionExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblDivision = _mapper.Map<Division>(requestObj);
                    _context.Division.Add(tblDivision);
                    await _context.SaveChangesAsync();
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
                var division = await _context.Division.FindAsync(id);
                _context.Division.Remove(division);
                _context.SaveChanges();
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

        public ListResponse<DivisionDc> GetAllSync()
        {
            var response = new ListResponse<DivisionDc>();
            try
            {

                response.Data = _context.Division.Select(x => new DivisionDc
                {
                    DivisionId = x.DivisionId,
                    DivisionName = x.Dname,
                    DivisionDesc = x.Ddesc,
                    TenantId = x.TenantId
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

        public async Task<SingleResponse<DivisionDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<DivisionDc>();
            try
            {
                response.Data = await _context.Division.Select(x => new DivisionDc
                {
                    DivisionId = x.DivisionId,
                    DivisionName = x.Dname,
                    DivisionDesc = x.Ddesc,
                    TenantId = x.TenantId
                }).FirstOrDefaultAsync(x => x.DivisionId == id);

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

        public async Task<SingleResponse<DivisionDc>> UpdateAsync(DivisionDc divisionDc)
        {
            var response = new SingleResponse<DivisionDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsDivisionExist(divisionDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var division = await _context.Division.FindAsync(divisionDc.DivisionId);
                    division.Dname = divisionDc.DivisionName;
                    division.Ddesc = division.Ddesc;
                    _context.Division.Update(division);
                    await _context.SaveChangesAsync();
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

        private List<KeyValuePair<string, string>> IsDivisionExist(DivisionDc divisionDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (divisionDc.DivisionId > 0)
                {
                    IsExists = _context.Division.Where(x => x.Dname == divisionDc.DivisionName && x.DivisionId != divisionDc.DivisionId).Any();
                }
                else
                {
                    IsExists = _context.Division.Where(x => x.Dname == divisionDc.DivisionName).Any();
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
                foreach (Division d in await _context.Division.ToListAsync())
                {
                    divisions.Add(d.Dname+"-" + d.DivisionId);
                }

            }
            catch (Exception ex)
            {

            }
            return divisions;
        }
    }
}
