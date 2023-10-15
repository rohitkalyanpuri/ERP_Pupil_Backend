using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pupil.Model;
using System.Threading.Tasks;

namespace Pupil.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Pupil.Services.PupilAuthenticationService _service;

        public AuthController(Pupil.Services.PupilAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody]AuthDc request)
        {
            return Ok(await _service.AuthenticateUser(request));
        }
    }
}
