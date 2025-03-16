using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;
using System;

namespace UniHack.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ Get User by ID (No Authentication Needed)
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.PhoneNumber,
                user.ImagePath,
                user.Bio,
                user.University,
                user.Degree,
                Tags = user.Tags.Select(t => new { t.Id, t.Value }), // ✅ Ensure tags are returned as an array
                user.IsAdmin
            });
        }
    }
}
