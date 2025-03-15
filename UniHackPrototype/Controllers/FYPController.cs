using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Models;
using System.Security.Claims;
using UniHack.ForYouPageLogic;
using UniHack.Services.Interfaces;

namespace UniHack.Controllers
{
	[ApiController]
	[Route("api/foryou")]
	[Authorize]
	public class FYPController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IPostService _postService;
		private readonly ISocietyService _societyService;
		private readonly ICourseService _courseService;

		public FYPController(
			IUserService userService,
			IPostService postService,
			ISocietyService societyService,
			ICourseService courseService)
		{
			_userService = userService;
			_postService = postService;
			_societyService = societyService;
			_courseService = courseService;
		}

		[HttpGet]
		public IActionResult GetForYouPosts()
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

			//_forYouPageLogic.GetForYouPage(user);

			var posts = _postService.GetAllPosts().OrderByDescending(p => p.Author.University == user.University).ToList();

			return Ok(posts);
		}

		[HttpGet]
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

		[HttpGet]
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