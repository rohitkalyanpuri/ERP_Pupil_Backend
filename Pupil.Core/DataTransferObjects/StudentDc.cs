using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Core.DataTransferObjects
{
    public class StudentDc
    {
        public int StudentId { get; set; }
        public string TenantId { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string Mobile { get; set; }

        public int? ParentId { get; set; }

        public DateTime? DateOfJoin { get; set; }

        public bool? Status { get; set; }
    }
}
