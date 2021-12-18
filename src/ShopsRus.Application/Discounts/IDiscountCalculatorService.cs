using System.Threading.Tasks;

namespace ShopsRus.Application.Discounts
{
    public interface IDiscountCalculatorService
    {
        Task<decimal> CalculateApplicableDiscount(OrderDto order);
    }
}