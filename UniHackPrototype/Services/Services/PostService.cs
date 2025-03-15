using MyAspNetVueApp.Models;
using UniHack.Repositories;
using UniHack.Services.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Services
{
	public class PostService : IPostService
	{
		private readonly IPostRepository _postRepository;

		public PostService(IPostRepository postRepository)
		{
			_postRepository = postRepository;
		}

		public List<Post> GetAllPosts()
		{
			return _postRepository.GetAllAsync().Result;
		}

		public Post? GetPostById(Guid id)
		{
			return _postRepository.GetByIdAsync(id).Result;
		}

		public List<Post> GetPostsByCommunity(Guid communityId)
		{
			return _postRepository.GetByCommunityIdAsync(communityId).Result;
		}

		public List<Post> GetPostsByAuthor(Guid authorId)
		{
			return _postRepository.GetByAuthorIdAsync(authorId).Result;
		}

		public List<Post> GetPostsByTag(Tag tag)
		{
			return _postRepository.GetByTagAsync(tag).Result;
		}

		public bool CreatePost(string title, string content, IEnumerable<Tag> tags, User author, Community community)
		{
			if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content) || author == null || community == null)
			{
				return false;
			}

			var post = new Post
			{
				Id = Guid.NewGuid(),
				Title = title,
				Content = content,
				Tags = [.. tags],
				Author = author,
				Community = community,
				Comments = []
			};

			return _postRepository.AddAsync(post).Result;
		}

		public bool UpdatePost(Guid id, string? title, string? content, IEnumerable<Tag>? tags)
		{
			var post = _postRepository.GetByIdAsync(id).Result;
			if (post == null)
			{
				return false;
			}

			if (title != null) post.Title = title;
			if (content != null) post.Content = content;
			if (tags != null) post.Tags = [.. tags];

			return _postRepository.UpdateAsync(post).Result;
		}

		public bool DeletePost(Guid id)
		{
			return _postRepository.DeleteAsync(id).Result;
		}

		public bool AddPostTag(Guid id, Tag tag)
		{
			var post = _postRepository.GetByIdAsync(id).Result;
			if (post == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (!post.Tags.Contains(tag))
			{
				post.Tags.Add(tag);
				return _postRepository.UpdateAsync(post).Result;
			}

			return true;
		}

		public bool RemovePostTag(Guid id, Tag tag)
		{
			var post = _postRepository.GetByIdAsync(id).Result;
			if (post == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (post.Tags.Contains(tag))
			{
				post.Tags.Remove(tag);
				return _postRepository.UpdateAsync(post).Result;
			}

			return true;
		}

        public List<Post> GetPostsByTags(List<Tag> Tags)
        {
            return _postRepository
            .GetAllAsync()
            .Result
            .Where(p => p.Tags.Any(t => p.Tags.Any(tag => tag.Value == t.Value)))
            .OrderByDescending(p => p.CreatedAt)
            .ThenBy(p => p.Upvotes)
            .ToList();
        }
    }
}