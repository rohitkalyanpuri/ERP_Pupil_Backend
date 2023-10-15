using Pupil.Model;

namespace Pupil.DataLayer
{
    public class FeeStructureRepository : GenericRepository<FeeStructure>
    {
        public FeeStructureRepository(DBContext context)
          : base(context)
        {
        }
    }
}
