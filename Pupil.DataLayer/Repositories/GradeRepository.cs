using Pupil.Model;

namespace Pupil.DataLayer
{
    public class GradeRepository : GenericRepository<Grade>
    {
        public GradeRepository(DBContext context)
            : base(context)
        {
        }
    }
}
