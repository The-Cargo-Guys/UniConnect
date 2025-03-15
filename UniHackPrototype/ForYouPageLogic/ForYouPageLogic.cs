using MyAspNetVueApp.Models;
using UniHack.Services.Interfaces;
using UniHack.Services.Services;
using UniHackPrototype.Models;

namespace UniHack.ForYouPage
{
    public class ForYouPageLogic
    {
        private readonly IPostService _postService;
        public ForYouPageLogic(IPostService postService)
        {
            _postService = postService;
        }
        public List<Post> GetForYouPage(User user) => _postService.GetPostsByTags(user.Tags.ToList());
    }
}
