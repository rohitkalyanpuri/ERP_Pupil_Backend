using Pupil.Model;

namespace Pupil.DataLayer
{
    public class FeeTypeRepository : GenericRepository<FeeTypes>
    {
        public FeeTypeRepository(DBContext context)
           : base(context)
        {
        }
    }
}
