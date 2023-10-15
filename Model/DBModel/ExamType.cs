using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{

    [Table("tblExamType")]
    public class ExamType 
    {
        public int ExamTypeId { get; set; }

        public string Tname { get; set; }

        public string Tdesc { get; set; }
    }
}
