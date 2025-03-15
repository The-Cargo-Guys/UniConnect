using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniHack.Models;

namespace MyAspNetVueApp.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public string University { get; set; } = string.Empty;

        public string Degree { get; set; } = string.Empty;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public bool IsAdmin { get; set; } = false;
    }
}
