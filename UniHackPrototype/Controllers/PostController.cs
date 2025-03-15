using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using UniHackPrototype.Models;

namespace UniHack.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly ISocietyService _societyService;

        public PostController(IPostService postService, IUserService userService, ISocietyService societyService)
        {
            _postService = postService;
            _userService = userService;
            _societyService = societyService;
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] Post request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Title and content are required.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }

            var author = _userService.GetUserById(userGuid);
            if (author == null)
            {
                return NotFound("Author not found.");
            }

            var community = _societyService.GetSocietyById(request.CommunityId);
            if (community == null)
            {
                return NotFound("Community not found.");
            }

            var success = _postService.CreatePost(request.Title, request.Content, request.Tags, author, community);

            if (!success)
            {
                return StatusCode(500, "Could not create post.");
            }

            return Ok(new { message = "Post created successfully." });
        }
    }
}
