namespace UniHackPrototype.Models
{
    public class Society : Community
    {
        public ICollection<Event> events { get; set; } = new List<Event>();
    }
}
