using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IStudentService
    {
        Task<SingleResponse<StudentDc>> CreateAsync(StudentDc studentDc);
        ListResponse<StudentDc> GetAllSync();
        Task<SingleResponse<StudentDc>> GetByIdAsync(int id);
        Task<Response> Delete(int id);

        Task<SingleResponse<StudentDc>> UpdateAsync(StudentDc studentDc);
    }
}
