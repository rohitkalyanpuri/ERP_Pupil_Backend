using Pupil.Model;

namespace Pupil.DataLayer
{
    public class AcademicYearRepository : GenericRepository<AcademicYear>
    {
        public AcademicYearRepository(DBContext context)
            : base(context)
        {
        }
    }
}
