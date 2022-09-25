using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Core.DataContactsEntities
{
    public class ParentDc
    {
        public int ParentId { get; set; }

        public string TenantId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DOB { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public bool? Status { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string LastLoginIp { get; set; }
    }
}
