using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Core.DataTransferObjects
{
    public class EnrollmentDc
    {
        public int GradeId { get; set; }

        public int AcademicId { get; set; }

        public int StudentId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool Cancelled { get; set; }

        public string CancellationReason { get; set; }

        public string TenantId { get; set; }
    }
}
