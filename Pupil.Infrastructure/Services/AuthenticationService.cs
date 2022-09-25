using Microsoft.EntityFrameworkCore;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuthDc> AuthenticateUser(AuthDc authDc)
        {
            try
            {
                var auth = await _context.Authentication.SingleOrDefaultAsync(x => x.Usename == authDc.UserName && x.Password == authDc.Password);
                authDc.Password = string.Empty;
                if(auth != null)
                {
                    if (auth.IsLocked)
                    {
                        authDc.IsAuthenticated = false;
                        authDc.Message = "Your account has been deactivated. Please contact system admin";
                    }
                    else
                    {
                        authDc.IsAuthenticated = true;
                        authDc.Message = "Success";
                        authDc.UserType = auth.UserType;
                    } 
                }
                else
                {
                    authDc.IsAuthenticated = false;
                    authDc.Message = "Wrong username or password!";
                }   
            }
            catch(Exception ex)
            {
                authDc.Password = string.Empty;
                authDc.IsAuthenticated = false;
                authDc.Message = "System Error!";
            }
            return authDc;
        } 
    }
}
