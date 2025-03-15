using System.Collections.Generic;
using System.Threading.Tasks;
using UniHack.Models;

namespace UniHack.Repositories
{
	public interface ISocietyRepository
	{
		Task<List<Society>> GetAllAsync();
		Task<Society?> GetByIdAsync(Guid id);
		Task<List<Society>> GetByNameAsync(string name);
		Task<List<Society>> GetByTagAsync(Tag tag);
		Task<List<Society>> GetByMemberIdAsync(Guid memberId);
		Task<bool> AddAsync(Society society);
		Task<bool> UpdateAsync(Society society);
		Task<bool> DeleteAsync(Guid id);
		Task<bool> AddMemberAsync(Guid societyId, Guid userId);
		Task<bool> RemoveMemberAsync(Guid societyId, Guid userId);
	}
}