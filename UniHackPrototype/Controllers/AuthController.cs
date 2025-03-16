using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyAspNetVueApp.Data;
using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;
using Microsoft.AspNetCore.Cors;

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
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Missing email or password." });
        }

        var user = await _authService.RegisterUser(request.Name, request.Email, request.Password, request.PhoneNumber);
        if (user == null)
        {
            return BadRequest(new { message = "Email already in use." });
        }

        return Ok(new { userId = user.Id.ToString() }); // ✅ Returns only userId as a string
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Missing email or password." });
        }

        var user = _authService.AuthenticateUser(request.Email, request.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        return Ok(new { userId = user.ToString() }); // ✅ Returns only userId as a string
    }
}

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
