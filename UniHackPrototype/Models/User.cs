using System.ComponentModel.DataAnnotations;

namespace MyAspNetVueApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
