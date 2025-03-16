using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UniHack.Services.Interfaces;
using MyAspNetVueApp.Models;
using UniHack.Models;

namespace UniHack.Controllers
{
	[ApiController]
	[Route("api/search")]
	public class SearchController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ISocietyService _societyService;
		private readonly ICourseService _courseService;

		public SearchController(
			IUserService userService,
			ISocietyService societyService,
			ICourseService courseService)
		{
			_userService = userService;
			_societyService = societyService;
			_courseService = courseService;
		}

		[HttpGet("{searchName}")]
		public IActionResult Search(string searchName, [FromQuery] SearchType type = SearchType.All)
		{
			if (string.IsNullOrWhiteSpace(searchName))
			{
				return BadRequest("Search query cannot be empty");
			}

			// Normalize search name to lowercase for case-insensitive search
			searchName = searchName.Trim().ToLower();

			var results = new List<object>();

			// Search based on type
			switch (type)
			{
				case SearchType.Users:
					results.AddRange(SearchUsers(searchName));
					break;
				case SearchType.Societies:
					results.AddRange(SearchSocieties(searchName));
					break;
				case SearchType.Courses:
					results.AddRange(SearchCourses(searchName));
					break;
				case SearchType.All:
				default:
					results.AddRange(SearchUsers(searchName));
					results.AddRange(SearchSocieties(searchName));
					results.AddRange(SearchCourses(searchName));
					break;
			}

			results = results.Take(10).ToList();

			return Ok(new
			{
				TotalCount = results.Count,
				Results = results
			});
		}

		private List<object> SearchUsers(string searchName)
		{
			return _userService.GetAllUsers()
				.Where(u =>
					u.Name.ToLower().Contains(searchName) ||
					u.Email.ToLower().Contains(searchName) ||
					u.University.ToLower().Contains(searchName) ||
					u.Degree.ToLower().Contains(searchName))
				.Select(u => new
				{
					Id = u.Id,
					Type = "User",
					Name = u.Name,
					Email = u.Email,
					University = u.University,
					Degree = u.Degree,
					ImagePath = u.ImagePath
				})
				.Cast<object>()
				.ToList();
		}

		private List<object> SearchSocieties(string searchName)
		{
			return _societyService.GetAllSocieties()
				.Where(s =>
					s.Name.ToLower().Contains(searchName) ||
					s.Description.ToLower().Contains(searchName) ||
					s.Tags.Any(t => t.Value.ToLower().Contains(searchName)))
				.Select(s => new
				{
					Id = s.Id,
					Type = "Society",
					Name = s.Name,
					Description = s.Description,
					ImagePath = s.ImagePathBanner,
					MemberCount = s.Members.Count,
					Tags = s.Tags.Select(t => t.Value)
				})
				.Cast<object>()
				.ToList();
		}

		private List<object> SearchCourses(string searchName)
		{
			return _courseService.GetAllCourses()
				.Where(c =>
					c.Name.ToLower().Contains(searchName) ||
					c.Description.ToLower().Contains(searchName) ||
					c.Tags.Any(t => t.Value.ToLower().Contains(searchName)))
				.Select(c => new
				{
					Id = c.Id,
					Type = "Course",
					Name = c.Name,
					Description = c.Description,
					ImagePath = c.ImagePathBanner,
					MemberCount = c.Members.Count,
					Tags = c.Tags.Select(t => t.Value)
				})
				.Cast<object>()
				.ToList();
		}
	}

	public enum SearchType
	{
		All,
		Users,
		Societies,
		Courses
	}
}