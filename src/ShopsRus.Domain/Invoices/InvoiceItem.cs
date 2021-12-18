using ShopsRus.Domain.Core;
using ShopsRus.Domain.Products;

namespace ShopsRus.Domain.Invoices
{
    public class InvoiceItem : Entity
    {
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        
        public virtual Product Product { get; set; }
    }
}