using System.Collections.Generic;

namespace ShopsRus.Application
{
    public class OrderDto
    {
        public int CustomerId { get; set; }
        public IList<OrderItemDto> Items { get; set; }
    }
}