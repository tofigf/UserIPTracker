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

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string ip)
        {
            var users = await _userService.SearchUsersByIpAsync(ip);
            return Ok(users);
        }

        [HttpGet("{userId}/ips")]
        public async Task<IActionResult> GetUserIps(long userId)
        {
            var ips = await _userService.GetUserIpAddressesAsync(userId);
            return Ok(ips);
        }

        [HttpGet("{userId}/last")]
        public async Task<IActionResult> GetLastConnection(long userId)
        {
            var lastConnection = await _userService.GetLastConnectionAsync(userId);

            if (lastConnection == null)
            {
                return NotFound(new { message = "No connection history found for this user." });
            }

            return Ok(lastConnection);
        }
    }
}
