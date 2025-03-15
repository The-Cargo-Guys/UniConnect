using UniHack.Models;

namespace UniHack.Models
{
    public class Society : Community
    {
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
