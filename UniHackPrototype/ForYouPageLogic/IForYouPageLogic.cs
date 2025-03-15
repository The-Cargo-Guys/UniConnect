using MyAspNetVueApp.Models;
using UniHack.Models;

namespace UniHack.ForYouPageNamespace
{
    public interface IForYouPageLogic
    {
        public List<Post> GetForYouPage(User user);
    }
}
