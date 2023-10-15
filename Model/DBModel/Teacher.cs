using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblTeacher")]
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Fname { get; set; }

        public string Lname { get; set; }

        public DateTime? DOB { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public bool? Status { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string LastLoginIp { get; set; }
    }
}
