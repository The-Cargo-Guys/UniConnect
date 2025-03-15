using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHackPrototype.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;

        [MaxLength(100)]
        public string University { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Degree { get; set; } = string.Empty;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public ICollection<Community> Communities { get; set; } = new List<Community>();

        public bool IsAdmin { get; set; } = false;
    }
}
