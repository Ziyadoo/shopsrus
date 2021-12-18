using ShopsRus.Domain.Core;
using ShopsRus.Domain.Customers;

namespace ShopsRus.Domain.Discounts
{
    public class DiscountCustomerRole: Entity
    {
        public int DiscountId { get; set; }
        public int CustomerRoleId { get; set; }
        public bool IsCustomerRoleIncluded { get; set; }
        
        public virtual CustomerRole CustomerRole { get; set; }
    }
}