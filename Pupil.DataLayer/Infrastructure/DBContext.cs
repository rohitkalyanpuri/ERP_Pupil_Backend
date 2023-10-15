using Microsoft.EntityFrameworkCore;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.DataLayer
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<ExamType> ExamType { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<Student> Student { get; set; }

        public DbSet<Grade> Grade { get; set; }
        public DbSet<Authentication> Authentication { get; set; }

        public DbSet<Division> Division { get; set; }

        public DbSet<AcademicYear> AcademicYear { get; set; }

        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Course> Course { get; set; }

        public DbSet<FeeTypes> FeeTypes { get; set; }
        public DbSet<FeeStructure> FeeStructure { get; set; }
        public DbSet<FeeStructureClass> FeeStructureClasses { get; set; }
        public DbSet<FeeStructureInstallment> FeeStructureInstallments{ get; set; }

        // On model creating function will provide us with the ability to manage the tables properties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Authentication>(eb => { eb.HasKey(k => k.AuthId);  });
            modelBuilder.Entity<ExamType>(eb => { eb.HasKey(k => k.ExamTypeId);  });
            modelBuilder.Entity<ClassRoom>(eb => { eb.HasKey(k => k.ClassRoomId);  });
            modelBuilder.Entity<Course>(eb => { eb.HasKey(k => k.CourseId);  });
            modelBuilder.Entity<Exam>(eb => { eb.HasKey(k => k.ExamId);  });
            modelBuilder.Entity<Fee>(eb => { eb.HasKey(k => k.FeeId);  });
            modelBuilder.Entity<FeeMode>(eb => { eb.HasKey(k => k.FeeModeId);  });
           
            modelBuilder.Entity<Grade>(eb => { eb.HasKey(k => k.GradeId);  });
            modelBuilder.Entity<Parent>(eb => { eb.HasKey(k => k.ParentId);  });
            modelBuilder.Entity<Student>(eb => { eb.HasKey(k => k.StudentId);  });
            modelBuilder.Entity<Teacher>(eb => { eb.HasKey(k => k.TeacherId);  });
            modelBuilder.Entity<Attendance>(eb => { eb.HasKey(k=>k.Id);  });
            modelBuilder.Entity<ClassRoomStudent>(eb => { eb.HasNoKey();  });
            modelBuilder.Entity<ExamResult>(eb => { eb.HasNoKey();  });
            modelBuilder.Entity<Division>(eb => { eb.HasKey(k => k.DivisionId);  });
            modelBuilder.Entity<AcademicYear>(eb => { eb.HasKey(k => k.AcademicId);  });
            modelBuilder.Entity<Enrollment>(eb => { eb.HasKey(k=>k.Id);  });

            modelBuilder.Entity<FeeTypes>(eb => { eb.HasKey(k => k.Id); });
            modelBuilder.Entity<FeeStructure>(eb => { eb.HasKey(k => k.FeeStructureId); });
            modelBuilder.Entity<FeeStructureClass>(eb => { eb.HasKey(k => k.FeeStructureClassId); });
            modelBuilder.Entity<FeeStructureInstallment>(eb => { eb.HasKey(k => k.InstallmentId); });
        }
    }
}
