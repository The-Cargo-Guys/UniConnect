using UniHack.Repositories;
using UniHack.Services.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Services.Services
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;

		public CommentService(ICommentRepository commentRepository)
		{
			_commentRepository = commentRepository;
		}

		public Comment? GetCommentById(Guid id)
		{
			return _commentRepository.GetByIdAsync(id).Result;
		}

		public List<Comment> GetCommentsByPost(Guid postId)
		{
			return _commentRepository.GetByPostIdAsync(postId).Result;
		}

		public List<Comment> GetCommentsByAuthor(Guid authorId)
		{
			return _commentRepository.GetByAuthorIdAsync(authorId).Result;
		}

		public bool CreateComment(string content, User author, Guid postId)
		{
			if (string.IsNullOrWhiteSpace(content) || author == null)
			{
				return false;
			}

			var comment = new Comment
			{
				Id = Guid.NewGuid(),
				Content = content,
				Author = author
			};

			return _commentRepository.AddAsync(comment, postId).Result;
		}

		public bool UpdateComment(Guid id, string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return false;
			}

			var comment = _commentRepository.GetByIdAsync(id).Result;
			if (comment == null)
			{
				return false;
			}

			comment.Content = content;
			return _commentRepository.UpdateAsync(comment).Result;
		}

		public bool DeleteComment(Guid id)
		{
			return _commentRepository.DeleteAsync(id).Result;
		}
	}
}