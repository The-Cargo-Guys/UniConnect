using MyAspNetVueApp.Models;
using System.ComponentModel.DataAnnotations;

namespace UniHackPrototype.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public required User Author { get; set; }
    }
}
