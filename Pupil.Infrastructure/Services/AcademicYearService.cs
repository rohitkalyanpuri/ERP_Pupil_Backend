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
    public class AcademicYearService : IAcademicYearService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AcademicYearService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<SingleResponse<AcademicYearDc>> CreateAsync(AcademicYearDc requestObj)
        {
            var response = new SingleResponse<AcademicYearDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsAcademicYearExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblAcademicYear = _mapper.Map<AcademicYear>(requestObj);
                    _context.AcademicYear.Add(tblAcademicYear);
                    await _context.SaveChangesAsync();
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
                var item = await _context.AcademicYear.FindAsync(id);
                _context.AcademicYear.Remove(item);
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

        public async Task<List<string>> GetAcademicYearForExcel()
        {
            List<string> academicYears = new List<string>();
            try
            {
                foreach (AcademicYear y in await _context.AcademicYear.ToListAsync())
                {
                    academicYears.Add(y.YearStartDate.Year + " To " + y.YearEndDate.Year+"-"+y.AcademicId);
                }

            }
            catch (Exception ex)
            {

            }
            return academicYears;
        }

        public ListResponse<AcademicYearDc> GetAllSync()
        {
            var response = new ListResponse<AcademicYearDc>();
            try
            {

                response.Data = _context.AcademicYear.Select(x => new AcademicYearDc
                {
                    AcademicId = x.AcademicId,
                    Description = x.Description,
                    YearStartDate = x.YearStartDate,
                    YearEndDate = x.YearEndDate,
                    VacationStartDate=x.VacationStartDate,
                    VacationEndDate=x.VacationEndDate,
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

        public async Task<SingleResponse<AcademicYearDc>> GetByIdAsync(int id)
        {

            var response = new SingleResponse<AcademicYearDc>();
            try
            {
                response.Data = await _context.AcademicYear.Select(x => new AcademicYearDc
                {
                    AcademicId = x.AcademicId,
                    Description = x.Description,
                    YearStartDate = x.YearStartDate,
                    YearEndDate = x.YearEndDate,
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate,
                    TenantId = x.TenantId
                }).FirstOrDefaultAsync(x => x.AcademicId == id);
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

        public async Task<SingleResponse<AcademicYearDc>> UpdateAsync(AcademicYearDc academicYearDc)
        {
            var response = new SingleResponse<AcademicYearDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsAcademicYearExist(academicYearDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var item = await _context.AcademicYear.FindAsync(academicYearDc.AcademicId);
                    item.Description = academicYearDc.Description;
                    item.YearStartDate = academicYearDc.YearStartDate;
                    item.YearEndDate = academicYearDc.YearEndDate;
                    item.VacationStartDate = academicYearDc.VacationStartDate;
                    item.VacationEndDate = academicYearDc.VacationEndDate;
                    _context.AcademicYear.Update(item);
                    await _context.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Academic Year Successfully Updated!";
                }
                response.Data = academicYearDc;
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

        private List<KeyValuePair<string, string>> IsAcademicYearExist(AcademicYearDc academicYearDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (academicYearDc.AcademicId > 0)
                {
                    IsExists = _context.AcademicYear.Where(x => x.YearStartDate == academicYearDc.YearStartDate && x.YearEndDate== academicYearDc.YearEndDate && x.AcademicId != academicYearDc.AcademicId).Any();
                }
                else
                {
                    IsExists = _context.AcademicYear.Where(x => x.YearStartDate == academicYearDc.YearStartDate && x.YearEndDate == academicYearDc.YearEndDate).Any();
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
