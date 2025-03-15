using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHackPrototype.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        public string Value
        {
            get
            {
				return Value;
			}
            set
            {
				value = value.ToLower();
			}
        }
    }
}
