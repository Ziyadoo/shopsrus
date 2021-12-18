using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopsRus.Domain.Core;
using ShopsRus.Domain.Customers;
using ShopsRus.Domain.Discounts;
using ShopsRus.Domain.Products;

namespace ShopsRus.Application.Discounts
{
    public class DiscountCalculatorService : IDiscountCalculatorService
    {
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;

        public DiscountCalculatorService(IRepository<Discount> discountRepository,
            IRepository<Customer> customerRepository,
            IRepository<Product> productRepository)
        {
            _discountRepository = discountRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<decimal> CalculateApplicableDiscount(OrderDto order)
        {
            var discounts = await _discountRepository.GetList();
            if (!discounts.Any())
            {
                return 0;
            }

            var customer = await _customerRepository.GetAsync(order.CustomerId);
            var products = new List<Product>();
            foreach (var item in order.Items)
            {
                products.Add(await _productRepository.GetAsync(item.ProductId));
            }

            var dic = new Dictionary<Discount, decimal>();
            foreach (var discount in discounts)
            {
                var discountAmount = ApplyDiscountOnOrder(discount, products, customer, order);
                dic.Add(discount, discountAmount);
            }

            
            var percentageDiscount = 0m;
            var otherDiscounts = 0m;
            if (dic.Any(x => x.Key.UsePercentage & x.Value > 0))
            {
                //Apply on one percentage discount
                percentageDiscount = dic.Where(x => x.Key.UsePercentage & x.Value > 0).Min(x => x.Value);
            }

            if (dic.Any(x => !x.Key.UsePercentage))
            {
                otherDiscounts = dic.Where(x => !x.Key.UsePercentage).Sum(x => x.Value);
            }

            return percentageDiscount + otherDiscounts;
        }

        private decimal ApplyDiscountOnOrder(Discount discount, IList<Product> products, Customer customer,
            OrderDto order)
        {
            //Check if must ignore the customer
            if (discount.CustomerRoles != null)
            {
                if (discount.CustomerRoles.Any(x =>
                    x.CustomerRoleId == customer.CustomerRoleId && !x.IsCustomerRoleIncluded))
                {
                    return 0.0m;
                }
            }

            var includedProducts = new List<Product>();
            var includedCategories = discount.Categories?.Where(x => x.IsCategoryIncluded).ToList();
            var excludedCategories = discount.Categories?.Where(x => !x.IsCategoryIncluded).ToList();

            //Only add products from allowed categories
            if (includedCategories != null && includedCategories.Any())
            {
                foreach (var includedCategory in includedCategories)
                {
                    includedProducts.AddRange(products.Where(x => x.CategoryId == includedCategory.CategoryId));
                }
            }

            if (excludedCategories != null && excludedCategories.Any())
            {
                foreach (var product in products)
                {
                    if (!excludedCategories.Any(x => x.CategoryId == product.CategoryId))
                    {
                        includedProducts.Add(product);
                    }
                }
            }

            if ((includedCategories == null || !includedCategories.Any()) && (excludedCategories == null || !excludedCategories.Any()))
            {
                includedProducts.AddRange(products);
            }

            if (!includedProducts.Any())
            {
                return 0m;
            }

            var totalAmount = 0.0m;
            foreach (var includedProduct in includedProducts)
            {
                var quantity = order.Items.First(x => x.ProductId == includedProduct.Id).Quantity;
                totalAmount += includedProduct.Price * quantity;
            }

            switch (discount.Type)
            {
                case DiscountType.PerCustomerRole:
                    return ApplyDiscountOnOrderPerCustomerRole(totalAmount, discount, customer);
                case DiscountType.PerCustomerCreationDate:
                    return ApplyDiscountOnOrderPerCustomerCreationDate(totalAmount, discount, customer);
                case DiscountType.BySplitAmount:
                    return discount.SplitAmount == null ? 0 : ApplyDiscountOnOrderBySplitAmount(totalAmount, discount.Amount, discount.SplitAmount.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private decimal ApplyDiscountOnOrderBySplitAmount(decimal totalAmount, decimal discountAmount, decimal splitAmount)
        {
            return ((int)(totalAmount / splitAmount) * discountAmount);
        }

        private decimal ApplyDiscountOnOrderPerCustomerCreationDate(decimal totalAmount, Discount discount, Customer customer)
        {
            var customerSinceInDays = (DateTime.Now - customer.CreationTime).Days;
            if (discount.CustomerCreationDays != null && customerSinceInDays >= discount.CustomerCreationDays.Value)
            {
                return totalAmount * discount.Amount;
            }

            return 0;
        }

        private decimal ApplyDiscountOnOrderPerCustomerRole(decimal totalAmount, Discount discount, Customer customer)
        {
            if (discount.CustomerRoles != null && discount.CustomerRoles.Any(x => x.IsCustomerRoleIncluded && customer.CustomerRoleId == x.CustomerRoleId))
            {
                return totalAmount * discount.Amount;
            }

            return 0;
        }
    }
}