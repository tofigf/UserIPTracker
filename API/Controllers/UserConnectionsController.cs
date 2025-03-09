using Application.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user-connections")]
    public class UserConnectionsController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserConnectionsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> ConnectUser([FromBody] ConnectUserRequest request)
        {
            await _userService.AddConnectionAsync(request.UserId, request.IpAddress);
            return Ok();
        }
    }
}
