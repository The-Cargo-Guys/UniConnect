using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniHack.Data;
using UniHack.Models;
using UniHack.Repositories;
using UniHack.Repositories.Interfaces;

namespace UniHack.Services.Notifications
{
	public class EventNotificationScheduler : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<EventNotificationScheduler> _logger;
		private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);

		public EventNotificationScheduler(
			IServiceProvider serviceProvider,
			ILogger<EventNotificationScheduler> logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Event notification scheduler service is starting.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await ProcessPendingNotificationsAsync();
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error processing event notifications");
				}

				await Task.Delay(_checkInterval, stoppingToken);
			}

			_logger.LogInformation("Event notification scheduler service is stopping.");
		}

		private async Task ProcessPendingNotificationsAsync()
		{
			using var scope = _serviceProvider.CreateScope();
			var eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();

			var now = DateTime.UtcNow;
			var events = await eventRepository.GetUpcomingAsync(now.AddDays(-1));

			foreach (var eventItem in events)
			{
				try
				{
					var timeUntilEvent = eventItem.Date - now;

					if (timeUntilEvent.TotalHours > 23 && timeUntilEvent.TotalHours < 24)
					{
						await SendEventNotificationAsync(eventItem, NotificationType.OneDayBefore);
						_logger.LogInformation($"Sent one-day notification for event: {eventItem.Name}");
					}
					else if (timeUntilEvent.TotalMinutes > 59 && timeUntilEvent.TotalMinutes < 60)
					{
						await SendEventNotificationAsync(eventItem, NotificationType.OneHourBefore);
						_logger.LogInformation($"Sent one-hour notification for event: {eventItem.Name}");
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Error processing notification for event {eventItem.Id}");
				}
			}
		}

		private async Task SendEventNotificationAsync(Event eventItem, NotificationType notificationType)
		{
			using var scope = _serviceProvider.CreateScope();
			var societyRepository = scope.ServiceProvider.GetRequiredService<ISocietyRepository>();
			var societies = await societyRepository.GetAllAsync();

			var society = societies.FirstOrDefault(s => s.Events.Any(e => e.Id == eventItem.Id));
			if (society == null)
			{
				_logger.LogWarning($"Could not find society for event {eventItem.Id}");
				return;
			}

			string eventDateFormatted = eventItem.Date.ToLocalTime().ToString("dddd, MMMM d, yyyy 'at' h:mm tt");

			foreach (var member in society.Members)
			{
				bool isDayBefore = notificationType == NotificationType.OneDayBefore;

				EmailSender.SendEventNotification(
					member.Email,
					member.Name,
					eventItem.Name,
					eventDateFormatted,
					eventItem.Description,
					isDayBefore
				);
			}
		}

		private enum NotificationType
		{
			OneDayBefore,
			OneHourBefore
		}
	}
}