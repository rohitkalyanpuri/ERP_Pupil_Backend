using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Model
{
    public class ParentDc
    {
        public int ParentId { get; set; }

        public string? Email { get; set; } 

        public string? Password { get; set; } 

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime? DOB { get; set; } = null;

        public string? Phone { get; set; } 

        public string? Mobile { get; set; } 

        public bool? Status { get; set; }

        public DateTime? LastLoginDate { get; set; } 

        public string? LastLoginIp { get; set; } 
    }
}
