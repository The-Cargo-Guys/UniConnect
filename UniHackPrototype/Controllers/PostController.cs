using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using UniHack.Models;

namespace UniHack.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly ISocietyService _societyService;
        private readonly ICourseService _courseService;

        public PostController(IPostService postService, IUserService userService, ISocietyService societyService, ICourseService courseService)
        {
            _postService = postService;
            _userService = userService;
            _societyService = societyService;
            _courseService = courseService;
        }

        [HttpPost("add-post")]
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

            Community? community;
            if (request.CommunityType == Enums.CommunityType.Course)
            {
                community = _courseService.GetCourseById(request.CommunityId);
            }
            else
            {
                community = _societyService.GetSocietyById(request.CommunityId);
            }

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

        [HttpPost("add-upvote/{id}")]
        public IActionResult AddUpvote(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Post ID is required.");
            }
            if (!_postService.AddUpvote(id))
            {
                return BadRequest("Upvote failed.");
            }
            return Ok(new { message = "Upvote added successfully." });
        }

        [HttpPost("remove-upvote/{id}")]
        public IActionResult RemoveUpvote(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Post ID is required.");
            }
            if (!_postService.RemoveUpvote(id))
            {
                return BadRequest("Upvote removal failed.");
            }
            return Ok(new { message = "Upvote removed successfully." });
        }
    }
}
