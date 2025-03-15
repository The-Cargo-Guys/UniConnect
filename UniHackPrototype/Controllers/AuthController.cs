using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UniHack.Services.Interfaces;
using UniHackPrototype.Data;
using UniHackPrototype.Models;

namespace UniHack.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;
		private readonly EmailVerification _emailVerification;

		public AuthController(IUserService userService, IConfiguration configuration, EmailVerification emailVerification)
		{
			_userService = userService;
			_configuration = configuration;
			_emailVerification = emailVerification;
		}

		[HttpPost("register")]
		public IActionResult Register([FromBody] User user)
		{
			if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
			{
				return BadRequest("Email and password are required");
			}

			if (!user.Email.EndsWith("@uts.edu.au") && !user.Email.EndsWith("@student.uts.edu.au"))
			{
				return BadRequest("Only UTS email addresses are allowed");
			}

			var existingUser = _userService.GetUserByEmail(user.Email);
			if (existingUser != null)
			{
				return Conflict("User with this email already exists");
			}

			bool result = _userService.CreateUser(user);
			if (!result)
			{
				return StatusCode(500, "Failed to register user");
			}

			_emailVerification.VerifyAndSendEmail(user.Email);

			return Ok("User registered successfully. Please verify your email.");
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel model)
		{
			if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
			{
				return BadRequest("Email and password are required");
			}

			if (!_userService.AuthenticateUser(model.Email, model.Password))
			{
				return Unauthorized("Invalid email or password");
			}

			var user = _userService.GetUserByEmail(model.Email);

			if (user is null)
			{
				throw new NullReferenceException("user is null");
			}
			var token = GenerateJwtToken(user);

			return Ok(new { token });
		}

		[Authorize]
		[HttpGet("profile")]
		public IActionResult GetProfile()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
			{
				return Unauthorized();
			}

			var user = _userService.GetUserById(userGuid);
			if (user == null)
			{
				return NotFound("User not found");
			}

			user.Password = "";

			return Ok(user);
		}

		[Authorize]
		[HttpPut("profile")]
		public IActionResult UpdateProfile([FromBody] UpdateProfileModel model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
			{
				return Unauthorized();
			}

			bool result = _userService.UpdateUserProfile(
				userGuid,
				model.Name,
				model.Bio,
				model.University,
				model.Degree,
				model.ImagePath,
				model.Tags
			);

			if (!result)
			{
				return StatusCode(500, "Failed to update profile");
			}

			return Ok("Profile updated successfully");
		}

		[Authorize]
		[HttpPost("change-password")]
		public IActionResult ChangePassword([FromBody] ChangePasswordModel model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
			{
				return Unauthorized();
			}

			if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
			{
				return BadRequest("Current password and new password are required");
			}

			bool result = _userService.UpdateUserPassword(userGuid, model.CurrentPassword, model.NewPassword);
			if (!result)
			{
				return BadRequest("Current password is incorrect");
			}

			return Ok("Password changed successfully");
		}

		private string GenerateJwtToken(User user)
		{
			var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Name, user.Name)
			};

			if (user.IsAdmin)
			{
				claims.Add(new Claim(ClaimTypes.Role, "Admin"));
			}

			var issuer = _configuration["Jwt:Issuer"] ?? "DefaultIssuer";
			var audience = _configuration["Jwt:Audience"] ?? "DefaultAudience";

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.Now.AddHours(3),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

	public class LoginModel
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}

	public class UpdateProfileModel
	{
		public string? Name { get; set; }
		public string? Bio { get; set; }
		public string? University { get; set; }
		public string? Degree { get; set; }
		public string? ImagePath { get; set; }
		public List<Tag>? Tags { get; set; }
	}

	public class ChangePasswordModel
	{
		public string CurrentPassword { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
	}
}