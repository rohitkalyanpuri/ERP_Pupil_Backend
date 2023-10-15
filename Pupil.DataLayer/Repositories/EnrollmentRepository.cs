using Pupil.Model;

namespace Pupil.DataLayer
{
    public class EnrollmentRepository : GenericRepository<Enrollment>
    {
        public EnrollmentRepository(DBContext context)
            : base(context)
        {
            
        }
    }
}
