using Pupil.Core.Options;

namespace Pupil.Core.Interfaces
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();

        public string GetConnectionString();

        public Tenant GetTenant();
    }
}