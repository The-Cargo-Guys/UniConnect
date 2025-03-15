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
        public List<Post> GetForYouPage(User user) => _postService.GetPostsByTags(user.Tags.ToList());
    }
}
