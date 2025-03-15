using System;
using System.Collections.Generic;
using UniHackPrototype.Models;

namespace UniHack.Services.Interfaces
{
	public interface ICommentService
	{
		Comment? GetCommentById(Guid id);
		List<Comment> GetCommentsByPost(Guid postId);
		List<Comment> GetCommentsByAuthor(Guid authorId);
		bool CreateComment(string content, User author, Guid postId);
		bool UpdateComment(Guid id, string content);
		bool DeleteComment(Guid id);
	}
}