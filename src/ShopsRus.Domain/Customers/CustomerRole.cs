using ShopsRus.Domain.Core;

namespace ShopsRus.Domain.Customers
{
    public class CustomerRole : Entity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}