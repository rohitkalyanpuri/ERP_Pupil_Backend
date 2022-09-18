using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblAttendance")]
    public class Attendance
    {
        public DateTime AtndDate { get; set; }

        public string TenantId { get; set; }

        public int StudentId { get; set; }

        public bool Staus { get; set; }

        public string Remark { get; set; }
    }
}
