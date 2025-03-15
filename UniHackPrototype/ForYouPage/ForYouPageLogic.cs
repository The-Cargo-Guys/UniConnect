using MyAspNetVueApp.Models;
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
        public void GetForYouPage(User user)
        {
            List<string> UserTags = user.Tags.ToList();
            List<Post> ForYouPosts = _postServic
        }
    }
}
