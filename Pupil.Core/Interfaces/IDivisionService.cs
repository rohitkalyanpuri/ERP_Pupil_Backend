using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IDivisionService
    {
        Task<SingleResponse<DivisionDc>> CreateAsync(DivisionDc divisionDc);
        ListResponse<DivisionDc> GetAllSync();
        Task<SingleResponse<DivisionDc>> GetByIdAsync(int id);
        Task<Response> Delete(int id);

        Task<SingleResponse<DivisionDc>> UpdateAsync(DivisionDc divisionDc);

        Task<List<string>> GetDivisionsForExcel();
    }
}
