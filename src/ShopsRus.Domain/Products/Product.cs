using ShopsRus.Domain.Core;

namespace ShopsRus.Domain.Products
{
    public class Product: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public decimal Price { get; set; }
    }
}