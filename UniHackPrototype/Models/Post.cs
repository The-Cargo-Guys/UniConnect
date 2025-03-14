using MyAspNetVueApp.Models;
using System.ComponentModel.DataAnnotations;

namespace UniHackPrototype.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ICollection<string> Tags { get; set; } = new List<string>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public required User Author { get; set; }
        public required Community Community { get; set; }
    }
}
