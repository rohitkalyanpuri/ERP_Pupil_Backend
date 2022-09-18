using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblCourse")]
    public class Course
    {
        public int CourseId { get; set; }

        public string TenantId { get; set; }

        public string Cname { get; set; }

        public string Cdesc { get; set; }

        public string GradeId { get; set; }
    }
}
