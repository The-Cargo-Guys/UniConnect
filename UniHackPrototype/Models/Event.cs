using System.ComponentModel.DataAnnotations;

namespace UniHackPrototype.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePathBanner { get; set; } = string.Empty;
        public DateTime Date { get; set; }

    }
}
