using Pupil.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{

    [Table("tblExamType")]
    public class ExamType : IMustHaveTenant
    {
        public int ExamTypeId { get; set; }

        public string Tname { get; set; }

        public string Tdesc { get; set; }
        public string TenantId { get; set; }
    }
}
