using UniHack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UniHack.Repositories.Interfaces;
using UniHack.Services.Interfaces;
using UniHack.Services.Notifications;
using UniHack.Repositories;

namespace UniHack.Services.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;
		private readonly ISocietyRepository _societyRepository;

		public EventService(IEventRepository eventRepository, ISocietyRepository societyRepository)
		{
			_eventRepository = eventRepository;
			_societyRepository = societyRepository;
		}

		public List<Event> GetAllEvents()
		{
			return _eventRepository.GetAllAsync().Result;
		}

		public Event? GetEventById(Guid id)
		{
			return _eventRepository.GetByIdAsync(id).Result;
		}

		public List<Event> GetEventsBySociety(Guid societyId)
		{
			return _eventRepository.GetBySocietyIdAsync(societyId).Result;
		}

		public List<Event> GetUpcomingEvents(int days = 30)
		{
			var fromDate = DateTime.UtcNow;
			var toDate = fromDate.AddDays(days);
			return _eventRepository.GetUpcomingAsync(fromDate).Result
				.Where(e => e.Date <= toDate)
				.OrderBy(e => e.Date)
				.ToList();
		}

		public bool CreateEvent(string name, string description, string imagePath, DateTime eventDate, Guid societyId)
		{
			if (string.IsNullOrWhiteSpace(name) || eventDate <= DateTime.UtcNow)
			{
				return false;
			}

			var society = _societyRepository.GetByIdAsync(societyId).Result;
			if (society == null)
			{
				return false;
			}

			var newEvent = new Event
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = description ?? string.Empty,
				ImagePathBanner = imagePath ?? string.Empty,
				Date = eventDate
			};

			society.Events.Add(newEvent);

			var result = _eventRepository.AddAsync(newEvent).Result;

			if (result)
			{
				ScheduleEventNotifications(newEvent.Id);
			}

			return result;
		}

		public bool UpdateEvent(Guid id, string? name, string? description, string? imagePath, DateTime? eventDate)
		{
			var eventItem = _eventRepository.GetByIdAsync(id).Result;
			if (eventItem == null)
			{
				return false;
			}

			if (name != null) eventItem.Name = name;
			if (description != null) eventItem.Description = description;
			if (imagePath != null) eventItem.ImagePathBanner = imagePath;
			if (eventDate != null && eventDate > DateTime.UtcNow) eventItem.Date = eventDate.Value;

			return _eventRepository.UpdateAsync(eventItem).Result;
		}

		public bool DeleteEvent(Guid id)
		{
			CancelEventNotifications(id);
			return _eventRepository.DeleteAsync(id).Result;
		}

		public bool ScheduleEventNotifications(Guid eventId, bool dayBefore = true, bool hourBefore = true)
		{
			var eventItem = _eventRepository.GetByIdAsync(eventId).Result;
			if (eventItem == null)
			{
				return false;
			}

			// In a real implementation, we might store this in a notifications table
			// For now, we're relying on the background service to check and send at the appropriate times

			return true;
		}

		public bool CancelEventNotifications(Guid eventId)
		{
			// In a real implementation, we might mark notifications as cancelled in the database
			// For this implementation, we just return true

			return true;
		}
	}
}