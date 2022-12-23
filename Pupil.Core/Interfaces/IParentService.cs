using Pupil.Core.DataContactsEntities;
using Pupil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IParentService
    {
        Task<SingleResponse<ParentDc>> CreateAsync(ParentDc parentDc);
        ListResponse<ParentDc> GetAllSync();
        Task<SingleResponse<ParentDc>> GetByIdAsync(int id);
        Task<Response> Delete(int id);

        Task<SingleResponse<ParentDc>> UpdateAsync(ParentDc parentDc);

        Task<ListResponse<ParentDc>> ImportParents(IEnumerable<ParentDc> requestObj);

        Task<List<string>> GetParentsForExcel();
    }
}
