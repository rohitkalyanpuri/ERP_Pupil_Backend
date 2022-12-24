using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IEnrollmentService
    {
        Task<SingleResponse<EnrollmentDc>> CreateAsync(EnrollmentDc enrollmentDc);

        Task<SingleResponse<EnrollmentDc>> UpdateAsync(EnrollmentDc enrollmentDc);

        Task<Response> Delete(int id);
    }
}
