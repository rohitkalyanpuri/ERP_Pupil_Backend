using Pupil.Model;

namespace Pupil.DataLayer
{
    public class FeeStructureClassRepository : GenericRepository<FeeStructureClass>
    {
        public FeeStructureClassRepository(DBContext context)
           : base(context)
        {
        }
    }
}
