using System.Collections.Generic;
using ShopsRus.Domain.Core;

namespace ShopsRus.Domain.Discounts
{
    public class Discount: Entity
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool UsePercentage { get; set; }
        public decimal? SplitAmount { get; set; }
        public DiscountType Type { get; set; }
        public int? CustomerCreationDays { get; set; }
        
        public IList<DiscountCategory> Categories { get; set; }
        public IList<DiscountCustomerRole> CustomerRoles { get; set; }
    }
}