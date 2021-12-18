using System.Threading.Tasks;
using ShopsRus.Domain.Invoices;

namespace ShopsRus.Application.Invoices
{
    public interface IInvoiceService
    {
        Task<Invoice> GenerateInvoice(OrderDto order);
    }
}