using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblAttendance")]
    public class Attendance
    {
        public Guid Id { get;set; }

        public DateTime AtndDate { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int GradeId { get; set; }
        public int DivisionId { get; set; }

        public bool Staus { get; set; }

        public string Remark { get; set; }=String.Empty;
    }
}
