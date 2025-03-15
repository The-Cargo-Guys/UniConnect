using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniHack.Models;

namespace UniHack.Repositories
{
	public class SocietyRepository : ISocietyRepository
	{
		private readonly AppDbContext _context;

		public SocietyRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Society>> GetAllAsync()
		{
			return await _context.Societies
				.Include(s => s.Members)
				.ToListAsync();
		}

		public async Task<Society?> GetByIdAsync(Guid id)
		{
			return await _context.Societies
				.Include(s => s.Members)
				.FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task<List<Society>> GetByNameAsync(string name)
		{
			return await _context.Societies
				.Include(s => s.Members)
				.Where(s => s.Name.Contains(name))
				.ToListAsync();
		}

		public async Task<List<Society>> GetByTagAsync(Tag tag)
		{
			return await _context.Societies
				.Include(s => s.Members)
				.Where(s => s.Tags.Contains(tag))
				.ToListAsync();
		}

		public async Task<List<Society>> GetByMemberIdAsync(Guid memberId)
		{
			return await _context.Societies
				.Include(s => s.Members)
				.Where(s => s.Members.Any(m => m.Id == memberId))
				.ToListAsync();
		}

		public async Task<bool> AddAsync(Society society)
		{
			await _context.Societies.AddAsync(society);
			return await SaveChangesAsync();
		}

		public async Task<bool> UpdateAsync(Society society)
		{
			_context.Societies.Update(society);
			return await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var society = await GetByIdAsync(id);
			if (society == null)
				return false;

			_context.Societies.Remove(society);
			return await SaveChangesAsync();
		}

		public async Task<bool> AddMemberAsync(Guid societyId, Guid userId)
		{
			var society = await GetByIdAsync(societyId);
			var user = await _context.Users.FindAsync(userId);

			if (society == null || user == null)
				return false;

			society.Members.Add(user);
			return await SaveChangesAsync();
		}

		public async Task<bool> RemoveMemberAsync(Guid societyId, Guid userId)
		{
			var society = await GetByIdAsync(societyId);

			if (society == null)
				return false;

			var user = society.Members.FirstOrDefault(m => m.Id == userId);

			if (user == null)
				return false;

			society.Members.Remove(user);
			return await SaveChangesAsync();
		}

		private async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}