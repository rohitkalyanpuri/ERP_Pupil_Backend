using Pupil.Model;

namespace Pupil.DataLayer
{
    public class AttendanceRepository : GenericRepository<Attendance>
    {
        public AttendanceRepository(DBContext context)
            : base(context)
        {
        }
    }
}
