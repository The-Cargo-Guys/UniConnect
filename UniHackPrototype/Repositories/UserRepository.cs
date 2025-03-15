using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniHack.Repositories.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<User>> GetAllAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<User?> GetByIdAsync(Guid id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<bool> AddAsync(User user)
		{
			await _context.Users.AddAsync(user);
			return await SaveChangesAsync();
		}

		public async Task<bool> UpdateAsync(User user)
		{
			_context.Users.Update(user);
			return await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var user = await GetByIdAsync(id);
			if (user == null)
				return false;

			_context.Users.Remove(user);
			return await SaveChangesAsync();
		}

		private async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}