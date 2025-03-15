using MyAspNetVueApp.Models;
using UniHackPrototype.Models;

namespace UniHack.ForYouPageNamespace
{
    public interface IForYouPageLogic
    {
        public List<Post> GetForYouPage(User user);
    }
}
