using UniHack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniHack.Repositories.Interfaces
{
	public interface IEventRepository
	{
		Task<List<Event>> GetAllAsync();
		Task<Event?> GetByIdAsync(Guid id);
		Task<List<Event>> GetBySocietyIdAsync(Guid societyId);
		Task<List<Event>> GetUpcomingAsync(DateTime fromDate);
		Task<bool> AddAsync(Event eventItem);
		Task<bool> UpdateAsync(Event eventItem);
		Task<bool> DeleteAsync(Guid id);
	}
}