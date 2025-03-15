using System.ComponentModel.DataAnnotations.Schema;

namespace UniHack.Models
{
    public class Society : Community
    {
        [InverseProperty(nameof(Event.Society))]
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
