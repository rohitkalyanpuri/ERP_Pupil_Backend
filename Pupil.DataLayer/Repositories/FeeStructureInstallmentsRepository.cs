using Pupil.Model;

namespace Pupil.DataLayer
{
    public class FeeStructureInstallmentsRepository : GenericRepository<FeeStructureInstallment>
    {
        public FeeStructureInstallmentsRepository(DBContext context)
           : base(context)
        {
        }
    }
}
