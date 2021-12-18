using System;
using System.Collections.Generic;
using ShopsRus.Domain.Core;

namespace ShopsRus.Domain.Invoices
{
    public class Invoice: Entity, IHaveCreationTime
    {
        public int CustomerId { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public IList<InvoiceItem> Items { get; set; }
    }
}