using MyAspNetVueApp.Models;
using System;
using System.Collections.Generic;
using UniHackPrototype.Models;

namespace UniHack.Services.Interfaces
{
	public interface IPostService
	{
		List<Post> GetAllPosts();
		Post? GetPostById(Guid id);
		List<Post> GetPostsByCommunity(Guid communityId);
		List<Post> GetPostsByAuthor(Guid authorId);
		List<Post> GetPostsByTag(string tag);
		bool CreatePost(string title, string content, IEnumerable<string> tags, User author, Community community);
		bool UpdatePost(Guid id, string? title, string? content, IEnumerable<string>? tags);
		bool DeletePost(Guid id);
		bool AddPostTag(Guid id, string tag);
		bool RemovePostTag(Guid id, string tag);
	}
}