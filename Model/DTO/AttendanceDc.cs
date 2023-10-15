using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Model
{
    public class AttendanceDc
    {
        public Guid Id { get; set; }

        public DateTime AtndDate { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int GradeId { get; set; }
        public int DivisionId { get; set; }

        public bool Staus { get; set; }

        public string Remark { get; set; } = String.Empty;

    }
}
