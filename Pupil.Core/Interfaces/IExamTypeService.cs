using Pupil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IExamTypeService
    {
        Task<ExamType> CreateAsync(string name, string description);

        Task<ExamType> GetByIdAsync(int id);

        Task<IReadOnlyList<ExamType>> GetAllAsync();

        Task Delete(int id);
    }
}
