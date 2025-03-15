using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHackPrototype.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
