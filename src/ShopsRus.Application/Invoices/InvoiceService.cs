using System.Collections.Generic;
using System.Threading.Tasks;
using ShopsRus.Application.Discounts;
using ShopsRus.Domain.Core;
using ShopsRus.Domain.Invoices;
using ShopsRus.Domain.Products;

namespace ShopsRus.Application.Invoices
{
    public class InvoiceService: IInvoiceService
    {
        private readonly IDiscountCalculatorService _discountCalculatorService;
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Product> _productRepository;

        public InvoiceService(IDiscountCalculatorService discountCalculatorService, IRepository<Invoice> invoiceRepository, IRepository<Product> productRepository)
        {
            _discountCalculatorService = discountCalculatorService;
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
        }

        public async Task<Invoice>  GenerateInvoice(OrderDto order)
        {
            var discount = await _discountCalculatorService.CalculateApplicableDiscount(order);
            var invoiceItems = new List<InvoiceItem>();
            var totalPrice = 0m;
            foreach (var orderItemDto in order.Items)
            {
                var unitPrice = (await _productRepository.GetAsync(orderItemDto.ProductId)).Price;
                var total = unitPrice * orderItemDto.Quantity;
                invoiceItems.Add(new InvoiceItem
                {
                    ItemId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Total = total,
                    UnitPrice = unitPrice
                });

                totalPrice += total;
            }

            var invoice = new Invoice
            {
                Items = invoiceItems,
                Total = totalPrice,
                CustomerId = order.CustomerId,
                Discount = discount,
                TotalAfterDiscount = totalPrice - discount
            };

            await _invoiceRepository.InsertAsync(invoice);
            return invoice;
        }
    }
}