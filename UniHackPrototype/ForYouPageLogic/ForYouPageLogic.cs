using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;
using UniHack.Services.Services;
using UniHack.Models;

namespace UniHack.ForYouPageNamespace
{
    public class ForYouPageLogic : IForYouPageLogic
    {
        private readonly IPostService _postService;
        public ForYouPageLogic(IPostService postService)
        {
            _postService = postService;
        }
        public List<Post> GetForYouPage(User user)
        {
            List<Post> taggedPosts = _postService.GetPostsByTags(user.Tags.ToList());

            if (taggedPosts.Count >= 50)
            {
                return taggedPosts.Take(50).ToList();
            }

            List<Post> allPosts = _postService.GetAllPosts();

            List<Post> remainingPosts = allPosts.Except(taggedPosts).ToList();

            int remainingCount = 50 - taggedPosts.Count;
            List<Post> additionalPosts = remainingPosts.Take(remainingCount).ToList();

            return taggedPosts.Concat(additionalPosts).ToList();
        }
    }
}
