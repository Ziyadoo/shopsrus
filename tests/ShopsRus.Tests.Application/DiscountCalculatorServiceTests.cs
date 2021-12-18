using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ShopsRus.Application;
using ShopsRus.Application.Discounts;
using ShopsRus.Application.Invoices;
using ShopsRus.Domain.Core;
using ShopsRus.Domain.Invoices;
using ShopsRus.Domain.Products;
using ShopsRus.EntityFramework;
using ShopsRus.EntityFramework.Repositories;

namespace ShopsRus.Tests.Application
{
    public class DiscountCalculatorServiceTests
    {
        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<ShopsRusDbContext>(opt => opt.UseInMemoryDatabase("ShopsRusDb"));
            _serviceCollection.AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkInMemoryRepository<,>));
            _serviceCollection.AddTransient(typeof(IRepository<>), typeof(EntityFrameworkInMemoryRepository<>));
            _serviceCollection.AddTransient<IDiscountCalculatorService, DiscountCalculatorService>();
            _serviceCollection.AddTransient<IInvoiceService, InvoiceService>();
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        [Test]
        public async Task Discount_For_Employee_30_Percent()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 2,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto{ProductId = 2, Quantity = 1});

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);
            
            Assert.AreEqual(22.5m, discount);
        }

        [Test]
        public async Task Discount_For_Employee_30_Percent_And_5_For_Each_100()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 2,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 1, Quantity = 1 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(35.0m, discount);
        }


        [Test]
        public async Task Discount_For_Affiliate_10_Percent()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 3,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 2, Quantity = 1 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(7.5m, discount);
        }

        [Test]
        public async Task Discount_For_Customer_Over_2_Years_5_Percent()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 5,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 2, Quantity = 1 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(3.75m, discount);
        }

        [Test]
        public async Task Discount_5_For_Each_100()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 4,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 1, Quantity = 1 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(5.0m, discount);
        }

        [Test]
        public async Task Execlude_Grocery_From_Percentage_1()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 2,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 6, Quantity = 200 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(5.0m, discount);
        }

        [Test]
        public async Task Execlude_Grocery_From_Percentage_2()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 2,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 6, Quantity = 10 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(0.0m, discount);
        }


        [Test]
        public async Task Ensure_Only_One_Percentage_Discount_Applied()
        {
            //Arrange
            var discountCalculatorService = _serviceProvider.GetRequiredService<IDiscountCalculatorService>();
            var order = new OrderDto
            {
                CustomerId = 6,
                Items = new List<OrderItemDto>()
            };
            order.Items.Add(new OrderItemDto { ProductId = 2, Quantity = 1 });

            //Act
            var discount = await discountCalculatorService.CalculateApplicableDiscount(order);

            Assert.AreEqual(3.75m, discount);
        }

    }
}