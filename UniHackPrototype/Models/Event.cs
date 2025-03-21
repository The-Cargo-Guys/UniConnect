﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHack.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePathBanner { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        [ForeignKey(nameof(Society))]
        public Guid SocietyId { get; set; }

        [InverseProperty(nameof(Society.Events))]
        public Society? Society { get; set; }
    }
}
