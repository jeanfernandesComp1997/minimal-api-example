namespace Minimal.Api.Models
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
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