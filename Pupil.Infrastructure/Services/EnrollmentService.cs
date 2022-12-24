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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EnrollmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<SingleResponse<EnrollmentDc>> CreateAsync(EnrollmentDc requestObj)
        {
            var response = new SingleResponse<EnrollmentDc>();
            try
            {
                var tblEnrollment = _mapper.Map<Enrollment>(requestObj);
                _context.Enrollment.Add(tblEnrollment);
                await _context.SaveChangesAsync();
                response.Status = StatusCode.Ok;
                response.Message = "Enrollment Successfully Created!";
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
                var item = await _context.Enrollment.FindAsync(id);
                _context.Enrollment.Remove(item);
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

        public async Task<SingleResponse<EnrollmentDc>> UpdateAsync(EnrollmentDc enrollmentDc)
        {
            var response = new SingleResponse<EnrollmentDc>();
            try
            {
                var item = await _context.Enrollment.Where(x => x.StudentId == enrollmentDc.StudentId
                && x.GradeId == enrollmentDc.GradeId).FirstOrDefaultAsync();

                item.AcademicId = enrollmentDc.AcademicId;
                item.EnrollmentDate = enrollmentDc.EnrollmentDate;
                item.Cancelled = enrollmentDc.Cancelled;
                item.CancellationReason = enrollmentDc.CancellationReason;
                _context.Enrollment.Update(item);
                await _context.SaveChangesAsync();
                response.Status = StatusCode.Ok;
                response.Message = "Enrollment Successfully Updated!";
                response.Data = enrollmentDc;
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
    }
}
