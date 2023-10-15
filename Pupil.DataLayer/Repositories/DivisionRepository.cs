using Pupil.Model;

namespace Pupil.DataLayer
{
    public class DivisionRepository : GenericRepository<Division>
    {
        public  DivisionRepository(DBContext context)
            : base(context)
        {
        }
    }
}
