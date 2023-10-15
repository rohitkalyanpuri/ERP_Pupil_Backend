using Pupil.Model;

namespace Pupil.DataLayer
{
    public class CourseRepository : GenericRepository<Course>
    {
        public CourseRepository(DBContext context)
            : base(context)
        {
        }
    }
}
