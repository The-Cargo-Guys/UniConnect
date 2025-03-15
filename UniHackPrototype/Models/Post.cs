using MyAspNetVueApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHackPrototype.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; } = null!;

        public Guid CommunityId { get; set; }

        [ForeignKey(nameof(CommunityId))]
        public Community Community { get; set; } = null!;
    }
}
