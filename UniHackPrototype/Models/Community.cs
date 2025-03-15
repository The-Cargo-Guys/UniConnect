using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniHackPrototype.Models
{
    public class Community
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImagePathBanner { get; set; } = string.Empty;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public ICollection<User> Members { get; set; } = new List<User>();
    }
}
