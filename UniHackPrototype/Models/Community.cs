﻿using MyAspNetVueApp.Models;
using System.ComponentModel.DataAnnotations;
using UniHack.Enums;
using UniHack.Models;

namespace UniHack.Models
{
    public class Community
    {
        [Key]
        public Guid Id { get; set; }
        public CommunityType communityType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePathBanner { get; set; } = string.Empty;
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<User> Members { get; set; } = new List<User>();
        public DateTime CreatedAt { get; set; }
    }
}
