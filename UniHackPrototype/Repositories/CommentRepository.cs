using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Data;
using UniHackPrototype.Models;

namespace UniHack.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly AppDbContext _context;

		public CommentRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Comment>> GetAllAsync()
		{
			return await _context.Comments
				.Include(c => c.Author)
				.ToListAsync();
		}

		public async Task<Comment?> GetByIdAsync(Guid id)
		{
			return await _context.Comments
				.Include(c => c.Author)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<List<Comment>> GetByPostIdAsync(Guid postId)
		{
			// Since comments are stored in the Post navigation property, we need to find the post first
			var post = await _context.Posts
				.Include(p => p.Comments)
					.ThenInclude(c => c.Author)
				.FirstOrDefaultAsync(p => p.Id == postId);

			return post?.Comments.ToList() ?? new List<Comment>();
		}

		public async Task<List<Comment>> GetByAuthorIdAsync(Guid authorId)
		{
			return await _context.Comments
				.Include(c => c.Author)
				.Where(c => c.Author.Id == authorId)
				.ToListAsync();
		}

		public async Task<bool> AddAsync(Comment comment, Guid postId)
		{
			var post = await _context.Posts.FindAsync(postId);
			if (post == null)
				return false;

			post.Comments.Add(comment);
			await _context.Comments.AddAsync(comment);
			return await SaveChangesAsync();
		}

		public async Task<bool> UpdateAsync(Comment comment)
		{
			_context.Comments.Update(comment);
			return await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var comment = await GetByIdAsync(id);
			if (comment == null)
				return false;

			_context.Comments.Remove(comment);
			return await SaveChangesAsync();
		}

		private async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}