using MyAspNetVueApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHack.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
