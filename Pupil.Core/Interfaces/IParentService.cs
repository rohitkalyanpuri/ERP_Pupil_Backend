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
        Task<ParentDc> CreateAsync(ParentDc parentDc);
        IEnumerable<ParentDc> GetAllSync();
        Task<ExamType> GetByIdAsync(int id);
        Task Delete(int id);

        Task<ParentDc> UpdateAsync(ParentDc parentDc);
    }
}
