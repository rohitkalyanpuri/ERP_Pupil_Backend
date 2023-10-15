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
    public class EnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<EnrollmentDc>> CreateAsync(EnrollmentDc requestObj)
        {
            var response = new SingleResponse<EnrollmentDc>();
            try
            {
                var tblEnrollment = _mapper.Map<Enrollment>(requestObj);
                tblEnrollment.Id = Guid.NewGuid();
                _unitOfWork.EnrollmentRepository.Insert(tblEnrollment);
                await _unitOfWork.SaveChangesAsync();
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

        public async Task<Response> Delete(Guid id)
        {
            var response = new Response();
            try
            {
                var item = await _unitOfWork.EnrollmentRepository.GetByIdAsync(id);
                _unitOfWork.EnrollmentRepository.Delete(item);
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

        public async Task<SingleResponse<EnrollmentDc>> UpdateAsync(EnrollmentDc enrollmentDc)
        {
            var response = new SingleResponse<EnrollmentDc>();
            try
            {
                var item = (await _unitOfWork.EnrollmentRepository.GetSearchAsync(x => x.StudentId == enrollmentDc.StudentId
                && x.GradeId == enrollmentDc.GradeId)).First();

                item.AcademicId = enrollmentDc.AcademicId;
                item.EnrollmentDate = enrollmentDc.EnrollmentDate;
                item.Cancelled = enrollmentDc.Cancelled;
                item.CancellationReason = enrollmentDc.CancellationReason;
                _unitOfWork.EnrollmentRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
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

        public async Task<ListResponse<EnrollmentDc>> GetEnrollments(int studentId)
        {
            var response = new ListResponse<EnrollmentDc>();
            try
            {

                response.Data = (from er in await _unitOfWork.EnrollmentRepository.GetSearchAsync(x => x.StudentId == studentId)
                                 join gr in await _unitOfWork.GradeRepository.GetSearchAsync() on er.GradeId equals gr.GradeId
                                 join dv in await _unitOfWork.DivisionRepository.GetAllAsync() on er.DivisionId equals dv.DivisionId
                                 join ac in await _unitOfWork.AcademicYearRepository.GetAllAsync() on er.AcademicId equals ac.AcademicId
                                 select new EnrollmentDc() {
                                     Id = er.Id,
                                     StudentId = er.StudentId,
                                     GradeId = gr.GradeId,
                                     AcademicId = er.AcademicId,
                                     DivisionId = er.DivisionId,
                                     EnrollmentDate=er.EnrollmentDate,
                                     Grade= gr.Gname,
                                     Division=dv.Dname,
                                     Academic=ac.Description

                                 }).ToList();
                
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<EnrollmentDc>();
            }

            return response;
        }
    }
}
