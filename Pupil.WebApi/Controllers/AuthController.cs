using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Entities;
using Pupil.Core.Interfaces;
using System.Threading.Tasks;

namespace Pupil.WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(AuthDc request)
        {
            return Ok(await _service.AuthenticateUser(request));
        }
    }
}
