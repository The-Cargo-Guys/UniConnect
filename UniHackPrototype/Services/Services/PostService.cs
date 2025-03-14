namespace UniHack.Services.Services
{
	public class PostService : IPostService
    {
        private readonly IPostService _postService;
        public PostService(IPostService postService)
        {
            _postService = postService;
        }
    }
}
