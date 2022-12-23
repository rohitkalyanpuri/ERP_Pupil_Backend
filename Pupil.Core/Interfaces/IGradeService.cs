using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IGradeService
    {
        Task<SingleResponse<GradeDc>> CreateAsync(GradeDc parentDc);
        ListResponse<GradeDc> GetAllSync();
        Task<SingleResponse<GradeDc>> GetByIdAsync(int id);
        Task<Response> Delete(int id);

        Task<SingleResponse<GradeDc>> UpdateAsync(GradeDc parentDc);

        Task<List<string>> GetGradesForExcel();
    }
}
