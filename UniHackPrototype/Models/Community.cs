using MyAspNetVueApp.Models;
using System.ComponentModel.DataAnnotations;

namespace UniHackPrototype.Models
{
    public class Community
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePathBanner { get; set; } = string.Empty;
        public ICollection<string> Tags { get; set; } = new List<string>();
        public ICollection<User> Members { get; set; } = new List<User>();
    }
}
