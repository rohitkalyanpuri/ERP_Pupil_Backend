using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Model
{
    public class StudentDc
    {
        public int StudentId { get; set; }
        public string? Password { get; set; }

        public string FirstName { get; set; }=String.Empty;

        public string LastName { get; set; }=String.Empty;

        public DateTime DOB { get; set; }=DateTime.Now;

        public string? Mobile { get; set; }

        public int? ParentId { get; set; }

        public DateTime? DateOfJoin { get; set; }

        public bool? Status { get; set; }

        public string? Email { get; set; }
    }
}
