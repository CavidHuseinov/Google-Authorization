using Auth.Business.Helpers.DTOs.UserDto;
using Auth.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterDto dto)
        {
            await _service.Register(dto);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginDto dto)
        {
            var token = await _service.Login(dto);
            return Ok(token);
        }
    }
}
