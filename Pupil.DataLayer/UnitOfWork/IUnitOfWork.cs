

namespace Pupil.DataLayer
{
    public interface IUnitOfWork : IDisposable
    {
        ParentRepository ParentRepository { get; }
        AuthenticationRepository AuthenticationRepository { get; }

        AcademicYearRepository AcademicYearRepository { get; }
        DivisionRepository DivisionRepository { get; }
        EnrollmentRepository EnrollmentRepository { get; }
        ExamRepository ExamRepository { get; }
        GradeRepository GradeRepository { get; }
        StudentRepository StudentRepository { get; }
        CourseRepository CourseRepository { get; }
        AttendanceRepository AttendanceRepository { get; }

        FeeTypeRepository FeeTypeRepository { get;  }
        FeeStructureRepository FeeStructureRepository { get;  }
        FeeStructureClassRepository FeeStructureClassRepository { get;  }
        FeeStructureInstallmentsRepository FeeStructureInstallmentsRepository { get;  }
        Task<int> SaveChangesAsync();

        Task Run(Func<Task> functionToCall);
    }
}
