using MyAspNetVueApp.Models;

namespace UniHackPrototype.Models
{
    public class Post
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ICollection<string> Tags { get; set; } = new List<string>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public User Author { get; set; }
        public Community Community { get; set; }
        public 
    }
}
