using Pupil.Core.Contracts;
using Pupil.Core.Entities;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pupil.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;

        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetTenant()?.TID;
        }

        //public DbSet<Product> Products { get; set; }
        public DbSet<ExamType> ExamType  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExamType>(eb => { eb.HasKey(k => k.ExamTypeId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<ClassRoom>(eb => { eb.HasKey(k => k.ClassRoomId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Course>(eb => { eb.HasKey(k => k.CourseId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Exam>(eb => { eb.HasKey(k => k.ExamId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Fee>(eb => { eb.HasKey(k => k.FeeId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<FeeMode>(eb => { eb.HasKey(k=>k.FeeModeId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<FeeStructure>(eb => { eb.HasKey(k => k.FeeStructureId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Grade>(eb => { eb.HasKey(k => k.GradeId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Parent>(eb => { eb.HasKey(k => k.ParentId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Student>(eb => { eb.HasKey(k => k.StudentId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Teacher>(eb => { eb.HasKey(k => k.TeacherId); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<Attendance>(eb => { eb.HasNoKey();eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<ClassRoomStudent>(eb => { eb.HasNoKey(); eb.HasQueryFilter(a => a.TenantId == TenantId); });
            modelBuilder.Entity<ExamResult>(eb => { eb.HasNoKey(); eb.HasQueryFilter(a => a.TenantId == TenantId); });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}