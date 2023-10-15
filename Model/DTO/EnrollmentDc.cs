using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Model
{
    public class EnrollmentDc
    {
        public int GradeId { get; set; }

        public int AcademicId { get; set; }

        public int StudentId { get; set; }

        public int? DivisionId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public bool Cancelled { get; set; }

        public string CancellationReason { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;
        public string Academic { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public Guid Id { get; set; }
    }
}
