using Pupil.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthDc> AuthenticateUser(AuthDc authDc);
    }
}
