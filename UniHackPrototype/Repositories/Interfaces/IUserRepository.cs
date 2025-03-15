using MyAspNetVueApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniHack.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<List<User>> GetAllAsync();
		Task<User?> GetByIdAsync(Guid id);
		Task<User?> GetByEmailAsync(string email);
		Task<bool> AddAsync(User user);
		Task<bool> UpdateAsync(User user);
		Task<bool> DeleteAsync(Guid id);
	}
}