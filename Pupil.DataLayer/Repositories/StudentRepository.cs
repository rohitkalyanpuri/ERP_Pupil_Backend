using Pupil.Model;

namespace Pupil.DataLayer
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(DBContext context)
            : base(context)
        {
        }
    }
}
