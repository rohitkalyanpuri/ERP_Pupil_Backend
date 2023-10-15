using Pupil.Model;

namespace Pupil.DataLayer
{
    public class ParentRepository : GenericRepository<Parent>
    {
        public ParentRepository(DBContext context)
            : base(context)
        {
        }
    }
}
