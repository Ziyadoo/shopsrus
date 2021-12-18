using ShopsRus.Domain.Core;

namespace ShopsRus.Domain.Products
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}