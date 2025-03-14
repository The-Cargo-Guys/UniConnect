using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Data;
using UniHackPrototype.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniHack.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly AppDbContext _context;

		public PostRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Post>> GetAllAsync()
		{
			return await _context.Posts
				.Include(p => p.Author)
				.Include(p => p.Community)
				.Include(p => p.Comments)
				.ToListAsync();
		}

		public async Task<Post?> GetByIdAsync(Guid id)
		{
			return await _context.Posts
				.Include(p => p.Author)
				.Include(p => p.Community)
				.Include(p => p.Comments)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<List<Post>> GetByCommunityIdAsync(Guid communityId)
		{
			return await _context.Posts
				.Include(p => p.Author)
				.Include(p => p.Community)
				.Include(p => p.Comments)
				.Where(p => p.Community.Id == communityId)
				.ToListAsync();
		}

		public async Task<List<Post>> GetByAuthorIdAsync(Guid authorId)
		{
			return await _context.Posts
				.Include(p => p.Author)
				.Include(p => p.Community)
				.Include(p => p.Comments)
				.Where(p => p.Author.Id == authorId)
				.ToListAsync();
		}

		public async Task<List<Post>> GetByTagAsync(string tag)
		{
			return await _context.Posts
				.Include(p => p.Author)
				.Include(p => p.Community)
				.Include(p => p.Comments)
				.Where(p => p.Tags.Contains(tag))
				.ToListAsync();
		}

		public async Task<bool> AddAsync(Post post)
		{
			await _context.Posts.AddAsync(post);
			return await SaveChangesAsync();
		}

		public async Task<bool> UpdateAsync(Post post)
		{
			_context.Posts.Update(post);
			return await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var post = await GetByIdAsync(id);
			if (post == null)
				return false;

			_context.Posts.Remove(post);
			return await SaveChangesAsync();
		}

		private async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}