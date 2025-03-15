using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using UniHack.Services.Interfaces;
using UniHack.Models;

namespace UniHack.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public CommentController(ICommentService commentService, IUserService userService, IPostService postService)
        {
            _commentService = commentService;
            _userService = userService;
            _postService = postService;
        }

        [HttpGet("post/{postId}")]
        public IActionResult GetCommentsByPost(Guid postId)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            var comments = _commentService.GetCommentsByPost(postId);
            return Ok(comments);
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            if (comment == null || string.IsNullOrWhiteSpace(comment.Content))
            {
                return BadRequest("Comment content is required.");
            }

            var post = _postService.GetPostById(comment.Id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            var success = _commentService.CreateComment(comment.Content, comment.Author, comment.Id);
            if (!success)
            {
                return StatusCode(500, "Could not create comment.");
            }

            return Ok(new { message = "Comment created successfully." });
        }
    }
}
