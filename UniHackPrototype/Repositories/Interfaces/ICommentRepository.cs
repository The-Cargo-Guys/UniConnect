using UniHackPrototype.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniHack.Repositories
{
	public interface ICommentRepository
	{
		Task<List<Comment>> GetAllAsync();
		Task<Comment?> GetByIdAsync(Guid id);
		Task<List<Comment>> GetByPostIdAsync(Guid postId);
		Task<List<Comment>> GetByAuthorIdAsync(Guid authorId);
		Task<bool> AddAsync(Comment comment, Guid postId);
		Task<bool> UpdateAsync(Comment comment);
		Task<bool> DeleteAsync(Guid id);
	}
}