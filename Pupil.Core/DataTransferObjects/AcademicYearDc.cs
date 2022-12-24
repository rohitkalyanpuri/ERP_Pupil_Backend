using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Core.DataTransferObjects
{
    public class AcademicYearDc
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
