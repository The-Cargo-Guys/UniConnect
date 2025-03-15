using System;
using System.ComponentModel.DataAnnotations;

namespace UniHackPrototype.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Value { get; set; } = string.Empty;
    }
}
