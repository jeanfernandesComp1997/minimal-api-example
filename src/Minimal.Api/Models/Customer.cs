using System.ComponentModel.DataAnnotations;

namespace Minimal.Api.Models
{
    public class Customer
    {
        [Required]
        public Guid Id { get; private set; }
        [Required, MinLength(2)]
        public string Name { get; private set; }
        [Required, MinLength(21)]
        public string Document { get; private set; }
        public bool Active { get; private set; }

        public Customer(Guid id, string name, string document, bool active)
        {
            Id = id;
            Name = name;
            Document = document;
            Active = active;
        }
    }
}