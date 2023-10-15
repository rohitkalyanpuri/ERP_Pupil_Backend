using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblEnrollment")]
    public class Enrollment
    {
        public Guid Id { get; set; }
        public int GradeId { get; set; }
        public int? DivisionId { get; set; }

        public int AcademicId { get; set; }

        public int StudentId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool Cancelled { get; set; }

        public string? CancellationReason { get; set; }
    }
}
