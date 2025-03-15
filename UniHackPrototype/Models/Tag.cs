using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHack.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        public string Value { get; set; } = string.Empty;
    }
}
