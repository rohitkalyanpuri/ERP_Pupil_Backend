
using Pupil.Model;
namespace Pupil.DataLayer
{
    public class AuthenticationRepository : GenericRepository<Authentication>
    {
        public AuthenticationRepository(DBContext context)
            : base(context)
        {
        }
    }
}
