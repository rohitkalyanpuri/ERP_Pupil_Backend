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
    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GradeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<SingleResponse<GradeDc>> CreateAsync(GradeDc requestObj)
        {
            var response = new SingleResponse<GradeDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsGradeExist(requestObj);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var tblGrade = _mapper.Map<Grade>(requestObj);
                    _context.Grade.Add(tblGrade);
                    await _context.SaveChangesAsync();
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
                var parent = await _context.Parent.FindAsync(id);
                _context.Parent.Remove(parent);
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

        public ListResponse<GradeDc> GetAllSync()
        {
            var response = new ListResponse<GradeDc>();
            try
            {

                response.Data = _context.Grade.Select(x => new GradeDc
                {
                    GradeId = x.GradeId,
                    Gname = x.Gname,
                    Gdesc = x.Gdesc,
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

        public async Task<SingleResponse<GradeDc>> GetByIdAsync(int id)
        {
            var response = new SingleResponse<GradeDc>();
            try
            {
                response.Data = await _context.Grade.Select(x => new GradeDc
                {
                    GradeId = x.GradeId,
                    Gname = x.Gname,
                    Gdesc = x.Gdesc,
                    TenantId = x.TenantId
                }).FirstOrDefaultAsync(x => x.GradeId == id);
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

        public async Task<SingleResponse<GradeDc>> UpdateAsync(GradeDc gradeDc)
        {
            var response = new SingleResponse<GradeDc>();
            try
            {
                List<KeyValuePair<string, string>> responses = IsGradeExist(gradeDc);
                if (responses.Count > 0)
                {
                    response.Status = StatusCode.AlreadyExists;
                    response.Message = responses[0].Value;
                }
                else
                {
                    var grade = await _context.Grade.FindAsync(gradeDc.GradeId);
                    grade.Gname = gradeDc.Gname;
                    grade.Gdesc = gradeDc.Gdesc;
                    _context.Grade.Update(grade);
                    await _context.SaveChangesAsync();
                    response.Status = StatusCode.Ok;
                    response.Message = "Grade Successfully Updated!";
                }
                response.Data = gradeDc;
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

        private List<KeyValuePair<string, string>> IsGradeExist(GradeDc gradeDc)
        {
            List<KeyValuePair<string, string>> responses = new List<KeyValuePair<string, string>>();
            bool IsExists = false;
            try
            {
                if (gradeDc.GradeId > 0)
                {
                    IsExists = _context.Grade.Where(x => x.Gname == gradeDc.Gname && x.GradeId != gradeDc.GradeId).Any();
                }
                else
                {
                    IsExists = _context.Grade.Where(x => x.Gname == gradeDc.Gname).Any();
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
    }
}
