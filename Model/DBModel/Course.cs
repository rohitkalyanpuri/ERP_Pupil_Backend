using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblCourse")]
    public class Course
    {
        public int CourseId { get; set; }
        public string Cname { get; set; } = String.Empty;

        public string Cdesc { get; set; } = String.Empty;

    }
}
