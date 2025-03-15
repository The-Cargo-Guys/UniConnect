using MyAspNetVueApp.Models;
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
        public required User Author { get; set; }
    }
}
