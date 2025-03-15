using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniHack.Services.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Controllers
{
    [ApiController]
    [Route("api/societies")]
    public class SocietyController : ControllerBase
    {
        private readonly ISocietyService _societyService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public SocietyController(
            ISocietyService societyService, 
            IUserService userService, 
            IPostService postService)
        {
            _societyService = societyService;
            _userService = userService;
            _postService = postService;
        }

        [HttpGet]
        public IActionResult GetAllSocieties()
        {
            var societies = _societyService.GetAllSocieties();
            return Ok(societies);
        }

        [HttpGet("{id}")]
        public IActionResult GetSocietyById(Guid id)
        {
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }
            
            return Ok(society);
        }

        [HttpGet("search")]
        public IActionResult SearchSocieties([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name is required for search");
            }
            
            var societies = _societyService.GetSocietiesByName(name);
            return Ok(societies);
        }

        [HttpGet("by-tag")]
        public IActionResult GetSocietiesByTag([FromQuery] Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Value))
            {
                return BadRequest("Tag is required");
            }
            
            var societies = _societyService.GetSocietiesByTag(tag);
            return Ok(societies);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateSociety([FromBody] CreateSocietyModel model)
        {
            // Validate model
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest("Name is required");
            }

            // Create society
            bool result = _societyService.CreateSociety(
                model.Name,
                model.Description ?? string.Empty,
                model.ImagePath ?? string.Empty,
				model.Tags ?? []
            );

            if (!result)
            {
                return StatusCode(500, "Failed to create society");
            }

            return StatusCode(201, "Society created successfully");
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateSociety(Guid id, [FromBody] UpdateSocietyModel model)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            // TODO: Add permission check (only admins/moderators should be able to update)

            // Update society
            bool result = _societyService.UpdateSociety(
                id,
                model.Name,
                model.Description,
                model.ImagePath,
                model.Tags
            );

            if (!result)
            {
                return StatusCode(500, "Failed to update society");
            }

            return Ok("Society updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSociety(Guid id)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            // Delete society
            bool result = _societyService.DeleteSociety(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete society");
            }

            return Ok("Society deleted successfully");
        }

        [HttpPost("{id}/members/{userId}")]
        [Authorize]
        public IActionResult AddMember(Guid id, Guid userId)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            // Check if user exists
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Verify user is adding themselves or is admin
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !Guid.TryParse(currentUserId, out var currentUserGuid) || 
                (currentUserGuid != userId && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            // Add member
            bool result = _societyService.AddMemberToSociety(id, userId);
            if (!result)
            {
                return StatusCode(500, "Failed to add member to society");
            }

            return Ok("Member added to society successfully");
        }

        [HttpDelete("{id}/members/{userId}")]
        [Authorize]
        public IActionResult RemoveMember(Guid id, Guid userId)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            // Check if user exists
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Verify user is removing themselves or is admin
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !Guid.TryParse(currentUserId, out var currentUserGuid) || 
                (currentUserGuid != userId && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            // Remove member
            bool result = _societyService.RemoveMemberFromSociety(id, userId);
            if (!result)
            {
                return StatusCode(500, "Failed to remove member from society");
            }

            return Ok("Member removed from society successfully");
        }

        [HttpPost("{id}/tags")]
        [Authorize]
        public IActionResult AddSocietyTag(Guid id, [FromBody] Tag model)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            if (string.IsNullOrWhiteSpace(model.Tag.Value))
            {
                return BadRequest("Tag is required");
            }

            // TODO: Add permission check (only admins/moderators should be able to add tags)

            // Add tag
            bool result = _societyService.AddTag(id, model.Tag);
            if (!result)
            {
                return StatusCode(500, "Failed to add tag");
            }

            return Ok("Tag added successfully");
        }

        [HttpDelete("{id}/tags")]
        [Authorize]
        public IActionResult RemoveSocietyTag(Guid id, [FromQuery] Tag tag)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            if (string.IsNullOrWhiteSpace(tag.Value))
            {
                return BadRequest("Tag is required");
            }

            // TODO: Add permission check (only admins/moderators should be able to remove tags)

            // Remove tag
            bool result = _societyService.RemoveTag(id, tag);
            if (!result)
            {
                return StatusCode(500, "Failed to remove tag");
            }

            return Ok("Tag removed successfully");
        }

        [HttpGet("{id}/posts")]
        public IActionResult GetSocietyPosts(Guid id)
        {
            // Check if society exists
            var society = _societyService.GetSocietyById(id);
            if (society == null)
            {
                return NotFound("Society not found");
            }

            // Get posts
            var posts = _postService.GetPostsByCommunity(id);
            return Ok(posts);
        }
    }
}