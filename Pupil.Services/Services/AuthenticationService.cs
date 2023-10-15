using AutoMapper;
using Pupil.DataLayer;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Pupil.Services
{
    public class PupilAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PupilAuthenticationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthDc> AuthenticateUser(AuthDc authDc)
        {
            try
            {
                var auth = (await _unitOfWork.AuthenticationRepository.GetSearchAsync(x => x.Usename == authDc.UserName && x.Password == authDc.Password)).FirstOrDefault();
                authDc.Password = string.Empty;
                if (auth != null)
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
                        authDc.UserId = auth.UserId;
                        authDc.RoleType = auth.RoleType;
                    }
                }
                else
                {
                    authDc.IsAuthenticated = false;
                    authDc.Message = "Wrong username or password!";
                }
            }
            catch (Exception ex)
            {
                authDc.Password = string.Empty;
                authDc.IsAuthenticated = false;
                authDc.Error = ex.Message;
                authDc.Message = "System Error!";
            }
            return authDc;
        }
    }
}
