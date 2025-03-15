using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyAspNetVueApp.Data;
using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;

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
        var user = await _authService.RegisterUser(request.Name, request.Email, request.Password, request.PhoneNumber);
        if (user == null)
        {
            return BadRequest("Email already in use.");
        }

        return Ok(new { message = "User registered successfully." });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var token = _authService.AuthenticateUser(request.Email, request.Password);
        if (token == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(new { token });
    }
}

public class RegisterRequest
{
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}
