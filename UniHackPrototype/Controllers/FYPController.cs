using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Models;
using System.Security.Claims;
using UniHack.ForYouPageNamespace;
using UniHack.Services.Interfaces;
using UniHack.Models;

namespace UniHack.Controllers
{
	[ApiController]
	[Route("api/for-you")]
	public class FYPController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IPostService _postService;
		private readonly ISocietyService _societyService;
		private readonly ICourseService _courseService;
		private readonly IForYouPageLogic _forYouPageLogic;

		public FYPController(
			IUserService userService,
			IPostService postService,
			ISocietyService societyService,
			ICourseService courseService,
            IForYouPageLogic forYouPageLogic)
		{
			_userService = userService;
			_postService = postService;
			_societyService = societyService;
			_courseService = courseService;
            _forYouPageLogic = forYouPageLogic;
        }

		[HttpGet("GetFyp/{userId}")]
		public IActionResult GetForYouPosts(Guid userId)
		{
			var user = _userService.GetUserById(userId);
			if (user == null)
			{
				return NotFound("User not found");
			}

            List<Post> posts = _forYouPageLogic.GetForYouPage(user);

			return Ok(posts);
		}

		[HttpGet("posts")]
		public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("societies")]
		public IActionResult GetRecommendedSocieties()
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


			return Ok("societies");
		}

		[HttpGet("courses")]
		public IActionResult GetRecommendedCourses()
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

			//stub to get courses

			return Ok("courses");
		}
	}
}