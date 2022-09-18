using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblExam")]
    public class Exam
    {
        public int ExamId { get; set; }

        public string TenantId { get; set; }

        public int ExamTypeId { get; set; }

        public string Ename { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
