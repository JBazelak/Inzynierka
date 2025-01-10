using Inzynierka.UI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.UI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterContractorDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _authService.RegisterAsync(registerDto);
                return Ok("Registration successful.");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginContractorDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var contractor = await _authService.LoginAsync(loginDto);

                HttpContext.Session.SetString("UserId", contractor.Id.ToString());
                HttpContext.Session.SetString("UserName", contractor.FirstName);

                Console.WriteLine($"UserId: {HttpContext.Session.GetString("UserId")}");
                Console.WriteLine($"UserName: {HttpContext.Session.GetString("UserName")}");
                return Ok(new { Message = "Login successful." });
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
