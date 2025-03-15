using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Data;
using UniHack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniHack.Repositories.Interfaces;

namespace UniHack.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly AppDbContext _context;

		public EventRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Event>> GetAllAsync()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task<Event?> GetByIdAsync(Guid id)
		{
			return await _context.Events.FindAsync(id);
		}

		public async Task<List<Event>> GetBySocietyIdAsync(Guid societyId)
		{
			var society = await _context.Societies
				.Include(s => s.Events)
				.FirstOrDefaultAsync(s => s.Id == societyId);

			return society?.Events.ToList() ?? new List<Event>();
		}

		public async Task<List<Event>> GetUpcomingAsync(DateTime fromDate)
		{
			return await _context.Events
				.Where(e => e.Date > fromDate)
				.OrderBy(e => e.Date)
				.ToListAsync();
		}

		public async Task<bool> AddAsync(Event eventItem)
		{
			await _context.Events.AddAsync(eventItem);
			return await SaveChangesAsync();
		}

		public async Task<bool> UpdateAsync(Event eventItem)
		{
			_context.Events.Update(eventItem);
			return await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var eventItem = await GetByIdAsync(id);
			if (eventItem == null)
				return false;

			_context.Events.Remove(eventItem);
			return await SaveChangesAsync();
		}

		private async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}