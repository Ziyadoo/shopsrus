using ShopsRus.Domain.Core;
using ShopsRus.Domain.Products;

namespace ShopsRus.Domain.Discounts
{
    public class DiscountCategory: Entity
    {
        public int DiscountId { get; set; }
        public int CategoryId { get; set; }
        public bool IsCategoryIncluded { get; set; }
        
        public virtual Category Category { get; set; }
    }
}