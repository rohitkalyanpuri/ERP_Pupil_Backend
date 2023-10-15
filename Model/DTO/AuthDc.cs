using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pupil.Model
{
    public class AuthDc
    {
        public bool IsAuthenticated { get; set; }=false;
        
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int AuthId { get; set; }

        public string UserType { get; set; }=string.Empty;

        public string Message { get; set; } = string.Empty;

        public int? UserId { get; set; }

        public string RoleType { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
