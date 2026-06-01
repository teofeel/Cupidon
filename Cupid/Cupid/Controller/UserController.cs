using Cupid.Models;
using Cupid.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cupid.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("block")]
        public IActionResult BlockUser([FromBody] BlockRequest request)
        {
            _userService.BlockUser(request.Current, request.Target);
            return Ok(new { message = "User blocked" });
        }

        [HttpPost("unbusy")]
        public IActionResult UnbusyUser([FromBody] string username)
        {
            _userService.SetUserBusy(username, false);
            return Ok(new { message = "User is now available" });
        }
    }
}
