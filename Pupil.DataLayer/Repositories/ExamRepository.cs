using Pupil.Model;

namespace Pupil.DataLayer
{
    public class ExamRepository : GenericRepository<ExamType>
    {
        public ExamRepository(DBContext context)
            : base(context)
        {
        }
    }
}
