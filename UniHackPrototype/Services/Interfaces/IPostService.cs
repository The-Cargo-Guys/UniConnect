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
		List<Post> GetPostsByTag(Tag tag);
		bool CreatePost(string title, string content, IEnumerable<Tag> tags, User author, Community community);
		bool UpdatePost(Guid id, string? title, string? content, IEnumerable<Tag>? tags);
		bool DeletePost(Guid id);
		bool AddPostTag(Guid id, Tag tag);
		bool RemovePostTag(Guid id, Tag tag);
	}
}