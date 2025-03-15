using UniHack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniHack.Models;

namespace UniHack.Repositories
{
	public interface IPostRepository
	{
		Task<List<Post>> GetAllAsync();
		Task<Post?> GetByIdAsync(Guid id);
		Task<List<Post>> GetByCommunityIdAsync(Guid communityId);
		Task<List<Post>> GetByAuthorIdAsync(Guid authorId);
		Task<List<Post>> GetByTagAsync(Tag tag);
		Task<bool> AddAsync(Post post);
		Task<bool> UpdateAsync(Post post);
		Task<bool> DeleteAsync(Guid id);
	}
}