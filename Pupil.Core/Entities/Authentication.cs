using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblAuthentication")]
    public class Authentication
    {
        public int AuthId { get; set; }

        public string TenantId { get; set; }

        public string Usename { get; set; }

        public string Password { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? Created { get; set; }

        public int? Pin { get; set; }

        public string UserType { get; set; }

        public int? UserId { get; set; }

        public string RoleType { get; set; }
    }
}
