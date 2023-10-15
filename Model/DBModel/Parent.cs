using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblParent")]
    public class Parent
    {
        public int ParentId { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string Fname { get; set; } = string.Empty;

        public string Lname { get; set; } = string.Empty;

        public DateTime? DOB { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public bool? Status { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string? LastLoginIp { get; set; }
    }
}
