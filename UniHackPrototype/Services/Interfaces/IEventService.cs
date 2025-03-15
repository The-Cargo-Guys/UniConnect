using UniHack.Models;
using System;
using System.Collections.Generic;

namespace UniHack.Services.Interfaces
{
	public interface IEventService
	{
		List<Event> GetAllEvents();
		Event? GetEventById(Guid id);
		List<Event> GetEventsBySociety(Guid societyId);
		List<Event> GetUpcomingEvents(int days = 30);

		bool CreateEvent(string name, string description, string imagePath, DateTime eventDate, Guid societyId);
		bool UpdateEvent(Guid id, string? name, string? description, string? imagePath, DateTime? eventDate);
		bool DeleteEvent(Guid id);

		bool ScheduleEventNotifications(Guid eventId, bool dayBefore = true, bool hourBefore = true);
		bool CancelEventNotifications(Guid eventId);
	}
}