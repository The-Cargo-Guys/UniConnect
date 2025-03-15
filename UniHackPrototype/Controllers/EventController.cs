using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniHack.Models;
using UniHack.Services.Interfaces;

namespace UniHack.Controllers
{
	[ApiController]
	[Route("api/events")]
	public class EventController : ControllerBase
	{
		private readonly IEventService _eventService;
		private readonly ISocietyService _societyService;
		private readonly IUserService _userService;

		public EventController(
			IEventService eventService,
			ISocietyService societyService,
			IUserService userService)
		{
			_eventService = eventService;
			_societyService = societyService;
			_userService = userService;
		}

		[HttpGet]
		public IActionResult GetAllEvents()
		{
			var events = _eventService.GetAllEvents();
			return Ok(events);
		}

		[HttpGet("/get-event/{id}")]
		public IActionResult GetEventById(Guid id)
		{
			var eventItem = _eventService.GetEventById(id);
			if (eventItem == null)
			{
				return NotFound("Event not found");
			}

			return Ok(eventItem);
		}

		[HttpGet("society/{societyId}")]
		public IActionResult GetEventsBySociety(Guid societyId)
		{
			var society = _societyService.GetSocietyById(societyId);
			if (society == null)
			{
				return NotFound("Society not found");
			}

			var events = _eventService.GetEventsBySociety(societyId);
			return Ok(events);
		}

		[HttpGet("upcoming")]
		public IActionResult GetUpcomingEvents([FromQuery] int days = 30)
		{
			var events = _eventService.GetUpcomingEvents(days);
			return Ok(events);
		}

		[HttpPost]
		public IActionResult CreateEvent([FromBody] Event request)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Name) || request.Date <= DateTime.UtcNow)
			{
				return BadRequest("Name is required and event date must be in the future");
			}

			var society = _societyService.GetSocietyById(request.SocietyId);
			if (society == null)
			{
				return NotFound("Society not found");
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !society.Members.Any(m => m.Id == Guid.Parse(userId)))
			{
				return Forbid("Only society members can create events");
			}

			bool result = _eventService.CreateEvent(
				request.Name,
				request.Description ?? string.Empty,
				request.ImagePathBanner ?? string.Empty,
				request.Date,
				request.SocietyId
			);

			if (!result)
			{
				return StatusCode(500, "Failed to create event");
			}

			return StatusCode(201, "Event created successfully");
		}

		[HttpPut("update/{id}")]
		public IActionResult UpdateEvent(Guid id, [FromBody] Event request)
		{
			var eventItem = _eventService.GetEventById(id);
			if (eventItem == null)
			{
				return NotFound("Event not found");
			}

			var societies = _societyService.GetAllSocieties();
			var society = societies.FirstOrDefault(s => s.Events.Any(e => e.Id == id));

			if (society == null)
			{
				return NotFound("Society not found for this event");
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !society.Members.Any(m => m.Id == Guid.Parse(userId)))
			{
				return Forbid("Only society members can update events");
			}

			bool result = _eventService.UpdateEvent(
				id,
				request.Name,
				request.Description,
				request.ImagePathBanner,
				request.Date
			);

			if (!result)
			{
				return StatusCode(500, "Failed to update event");
			}

			return Ok("Event updated successfully");
		}

		[HttpDelete("delete/{id}")]
		public IActionResult DeleteEvent(Guid id)
		{
			var eventItem = _eventService.GetEventById(id);
			if (eventItem == null)
			{
				return NotFound("Event not found");
			}

			var societies = _societyService.GetAllSocieties();
			var society = societies.FirstOrDefault(s => s.Events.Any(e => e.Id == id));

			if (society == null)
			{
				return NotFound("Society not found for this event");
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !society.Members.Any(m => m.Id == Guid.Parse(userId)))
			{
				return Forbid("Only society members can delete events");
			}

			bool result = _eventService.DeleteEvent(id);
			if (!result)
			{
				return StatusCode(500, "Failed to delete event");
			}

			return Ok("Event deleted successfully");
		}
	}
}