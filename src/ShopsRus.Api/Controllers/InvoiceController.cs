using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ShopsRus.Application;
using ShopsRus.Application.Invoices;
using ShopsRus.Domain.Invoices;

namespace ShopsRus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<Invoice> GenerateInvoice(OrderDto orderDto)
        {
            return await _invoiceService.GenerateInvoice(orderDto);
        }
    }
}
