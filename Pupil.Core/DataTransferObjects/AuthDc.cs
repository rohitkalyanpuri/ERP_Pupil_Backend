using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Core.DataTransferObjects
{
    public class AuthDc
    {
        public bool IsAuthenticated { get; set; }=false;

        public string UserName { get; set; }

        public string Password { get; set; }

        public int AuthId { get; set; }

        public string UserType { get; set; }

        public string Message { get; set; }
    }
}
