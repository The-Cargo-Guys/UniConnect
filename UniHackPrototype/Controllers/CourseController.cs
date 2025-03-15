using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniHack.Services.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public CourseController(
            ICourseService courseService, 
            IUserService userService, 
            IPostService postService)
        {
            _courseService = courseService;
            _userService = userService;
            _postService = postService;
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourseById(Guid id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }
            
            return Ok(course);
        }

        [HttpGet("search")]
        public IActionResult SearchCourses([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required for search");
            }
            
            var courses = _courseService.GetCoursesByName(name);
            return Ok(courses);
        }

        [HttpGet("by-tag")]
        public IActionResult GetCoursesByTag([FromQuery] Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Value))
            {
                return BadRequest("Tag is required");
            }
            
            var courses = _courseService.GetCoursesByTag(tag);
            return Ok(courses);
        }

        [HttpGet("by-member/{memberId}")]
        public IActionResult GetCoursesByMember(Guid memberId)
        {
            var courses = _courseService.GetCoursesByMember(memberId);
            return Ok(courses);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCourse([FromBody] Course model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest("Name is required");
            }

            bool result = _courseService.CreateCourse(
                model.Name,
                model.Description ?? string.Empty,
                model.ImagePathBanner ?? string.Empty,
				model.Tags ?? []
            );

            if (!result)
            {
                return StatusCode(500, "Failed to create course");
            }

            return StatusCode(201, "Course created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCourse(Guid id, [FromBody] Course model)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

			bool result = _courseService.UpdateCourse(
                id,
                model.Name,
                model.Description,
                model.ImagePathBanner,
                model.Tags
            );

            if (!result)
            {
                return StatusCode(500, "Failed to update course");
            }

            return Ok("Course updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(Guid id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            bool result = _courseService.DeleteCourse(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete course");
            }

            return Ok("Course deleted successfully");
        }

        [HttpPost("{id}/members/{userId}")]
        [Authorize]
        public IActionResult AddMember(Guid id, Guid userId)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !Guid.TryParse(currentUserId, out var currentUserGuid) || 
                (currentUserGuid != userId && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            bool result = _courseService.AddMemberToCourse(id, userId);
            if (!result)
            {
                return StatusCode(500, "Failed to add member to course");
            }

            return Ok("Member added to course successfully");
        }

        [HttpDelete("{id}/members/{userId}")]
        [Authorize]
        public IActionResult RemoveMember(Guid id, Guid userId)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !Guid.TryParse(currentUserId, out var currentUserGuid) || 
                (currentUserGuid != userId && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            bool result = _courseService.RemoveMemberFromCourse(id, userId);
            if (!result)
            {
                return StatusCode(500, "Failed to remove member from course");
            }

            return Ok("Member removed from course successfully");
        }

        [HttpPost("{id}/tags")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCourseTag(Guid id, [FromBody] T model)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            if (string.IsNullOrWhiteSpace(model.Tag.Value))
            {
                return BadRequest("Tag is required");
            }

            bool result = _courseService.AddCourseTag(id, model.Tag);
            if (!result)
            {
                return StatusCode(500, "Failed to add tag");
            }

            return Ok("Tag added successfully");
        }

        [HttpDelete("{id}/tags")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveCourseTag(Guid id, [FromQuery] Tag tag)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            if (string.IsNullOrWhiteSpace(tag.Value))
            {
                return BadRequest("Tag is required");
            }

            bool result = _courseService.RemoveCommunityTag(id, tag);
            if (!result)
            {
                return StatusCode(500, "Failed to remove tag");
            }

            return Ok("Tag removed successfully");
        }

        [HttpGet("{id}/posts")]
        public IActionResult GetCoursePosts(Guid id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var posts = _postService.GetPostsByCommunity(id);
            return Ok(posts);
        }
    }
}