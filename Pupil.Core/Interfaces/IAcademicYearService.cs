using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IAcademicYearService
    {
        Task<SingleResponse<AcademicYearDc>> CreateAsync(AcademicYearDc academicYearDc);
        ListResponse<AcademicYearDc> GetAllSync();
        Task<SingleResponse<AcademicYearDc>> GetByIdAsync(int id);
        Task<Response> Delete(int id);

        Task<SingleResponse<AcademicYearDc>> UpdateAsync(AcademicYearDc academicYearDc);

        Task<List<string>> GetAcademicYearForExcel();
    }
}
