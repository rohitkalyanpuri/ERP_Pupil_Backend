using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblAcademicYear")]
    public class AcademicYear
    {
        public int AcademicId { get; set; }

        public string Description { get; set; }

        public DateTime YearStartDate { get; set; }

        public DateTime YearEndDate { get; set; }

        public DateTime? VacationStartDate { get; set; }

        public DateTime? VacationEndDate { get; set; }

        public string TenantId { get; set; }

    }
}
