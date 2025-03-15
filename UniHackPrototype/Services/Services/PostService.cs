using UniHack.Repositories;
using UniHackPrototype.Models;

namespace UniHack.Services.Services
{
	public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postServiceRepository)
        {
            _postRepository = postServiceRepository;
        }

        public List<Post> GetForYouPage(List<string> userTags)
        {
            List<Post> ForYouPosts = _postRepository
                .GetAllAsync()
                .Result
                .Where(post => post.Tags.Any(tag => userTags.Contains(tag)))
                .ToList();
                .OrderByDescending()
            return ForYouPosts;
        }
    }
}
