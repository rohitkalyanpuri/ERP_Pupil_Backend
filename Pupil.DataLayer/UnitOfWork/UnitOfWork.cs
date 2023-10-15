using Microsoft.Extensions.Logging;
using Pupil.DataLayer;
namespace Pupil.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        public ParentRepository ParentRepository { get;private set; }
        public AuthenticationRepository AuthenticationRepository { get; private set; }

        public AcademicYearRepository AcademicYearRepository { get; private set; }

        public DivisionRepository DivisionRepository { get; private set; }

        public EnrollmentRepository EnrollmentRepository { get; private set; }

        public ExamRepository ExamRepository { get; private set; }

        public GradeRepository GradeRepository { get; private set; }
        public StudentRepository StudentRepository { get; private set; }
        public CourseRepository CourseRepository { get; private set; }
        public AttendanceRepository AttendanceRepository { get; private set; }
        public FeeTypeRepository FeeTypeRepository  { get; private set; }
        public FeeStructureRepository FeeStructureRepository   { get; private set; }
        public FeeStructureClassRepository FeeStructureClassRepository    { get; private set; }
        public FeeStructureInstallmentsRepository FeeStructureInstallmentsRepository { get; private set; }
        public UnitOfWork(DBContext context)
        {
            _context = context;
            ParentRepository = new ParentRepository(_context);
            AuthenticationRepository = new AuthenticationRepository(_context);
            AcademicYearRepository = new AcademicYearRepository(_context);
            DivisionRepository = new DivisionRepository(_context);
            EnrollmentRepository = new EnrollmentRepository(_context);
            ExamRepository = new ExamRepository(_context);
            GradeRepository = new GradeRepository(_context);
            StudentRepository = new StudentRepository(_context);
            CourseRepository = new CourseRepository(_context);
            AttendanceRepository = new AttendanceRepository(_context);
            FeeTypeRepository = new FeeTypeRepository(_context);
            FeeStructureRepository = new FeeStructureRepository(_context);
            FeeStructureClassRepository = new FeeStructureClassRepository(_context);
            FeeStructureInstallmentsRepository = new FeeStructureInstallmentsRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public  async Task Run(Func<Task> functionToCall)
        {
            var trans = _context.Database.BeginTransaction();
            bool inTransaction = true;
            try
            {
                await functionToCall();
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                inTransaction = false;
                throw;
            }
            finally
            {
                if (inTransaction)
                    await trans.CommitAsync();
            }
        }
    }
}
