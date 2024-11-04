using Microsoft.AspNetCore.Mvc;
using RapidPay.services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly UserService _userService;

    public AuthController(JwtTokenService jwtTokenService, UserService userService)
    {
        _jwtTokenService = jwtTokenService;
        _userService = userService;
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _userService.RegisterUser(request.Username, request.Password);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.ValidateUser(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = _jwtTokenService.GenerateToken(user.Username);
        return Ok(new { Token = token });
    }
}
