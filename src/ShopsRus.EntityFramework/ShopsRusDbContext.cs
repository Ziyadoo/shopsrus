using System;
using Microsoft.EntityFrameworkCore;
using ShopsRus.Domain.Customers;
using ShopsRus.Domain.Discounts;
using ShopsRus.Domain.Invoices;
using ShopsRus.Domain.Products;

namespace ShopsRus.EntityFramework
{
    public class ShopsRusDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerRole> CustomerRoles { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountCategory> DiscountCategories { get; set; }
        public DbSet<DiscountCustomerRole> DiscountCustomerRoles { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public ShopsRusDbContext(DbContextOptions options) : base(options)
        {
            Seed();
        }

        public void Seed()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            
            SeedCustomers();
            SeedProducts();
            SeedDiscount();
            SaveChanges();
        }

        private void SeedCustomers()
        {
            //Seed Customer Roles 
            CustomerRoles.Add(new CustomerRole { Id = 1, Name = "customer", DisplayName = "Customer" });
            CustomerRoles.Add(new CustomerRole { Id = 2, Name = "staff", DisplayName = "Staff" });
            CustomerRoles.Add(new CustomerRole { Id = 3, Name = "affiliate", DisplayName = "Affiliate" });

            //Seed Customer
            Customers.Add(new Customer
            {
                Id = 1, 
                UserName = "Customer1", 
                Password = "123", 
                FullName = "Customer One", 
                CustomerRoleId = 1, 
                CreationTime = DateTime.Now
            });

            Customers.Add(new Customer
            {
                Id = 2,
                UserName = "Staff1", 
                Password = "123",
                FullName = "Staff One",
                CustomerRoleId = 2, 
                CreationTime = DateTime.Now
            });

            Customers.Add(new Customer
                {
                    Id = 3,
                    UserName = "Affiliate1",
                    Password = "123",
                    FullName = "Affiliate One",
                    CustomerRoleId = 3,
                    CreationTime = DateTime.Now
                });

            Customers.Add(new Customer
                {
                    Id = 4,
                    UserName = "Customer2",
                    Password = "123",
                    FullName = "Customer Two",
                    CustomerRoleId = 1,
                    CreationTime = DateTime.Now
                });

            Customers.Add(new Customer
                {
                    Id = 5,
                    UserName = "Customer3",
                    Password = "123",
                    FullName = "Customer Three",
                    CustomerRoleId = 1,
                    CreationTime = new DateTime(2018, 1, 1)
                });

            Customers.Add(new Customer
            {
                Id = 6,
                UserName = "Customer4",
                Password = "123",
                FullName = "Customer Four",
                CustomerRoleId = 3,
                CreationTime = new DateTime(2018, 1, 1)
            });
        }

        private void SeedProducts()
        {
            //Seed Categories
            Categories.Add(new Category { Id = 1, Name = "Electronics", Description = "All electronics appliances" });
            Categories.Add(new Category { Id = 2, Name = "Clothes", Description = "Clothes category" });
            Categories.Add(new Category { Id = 3, Name = "Grocery", Description = "Vegetables, fruits, ..." });

            //Seed product
            //1- Electronics products
            Products.Add(new Product { Id = 1, Name = "TV", CategoryId = 1, Price = 100.0m });
            Products.Add(new Product { Id = 2, Name = "Mobile", CategoryId = 1, Price = 75.0m });
            Products.Add(new Product { Id = 3, Name = "Laptop", CategoryId = 1, Price = 150.0m });
            //2- Clothes products
            Products.Add(new Product { Id = 4, Name = "Pant", CategoryId = 2, Price = 10.0m });
            Products.Add(new Product { Id = 5, Name = "Skirt", CategoryId = 2, Price = 7.5m });
            //3- Grocery products
            Products.Add(new Product { Id = 6, Name = "Tomato", CategoryId = 3, Price = 0.75m });
            Products.Add(new Product { Id = 7, Name = "Cucumber", CategoryId = 3, Price = 0.6m });
        }

        private void SeedDiscount()
        {
            //Discount rule for "If the user is an employee of the store, he gets a 30% discount"
            Discounts.Add(new Discount
            {
                Id = 1,
                Name = "If the user is an employee of the store, he gets a 30% discount",
                UsePercentage = true,
                Amount = 0.3m,
                Type = DiscountType.PerCustomerRole
            });
            DiscountCustomerRoles.Add(new DiscountCustomerRole { Id = 1, DiscountId = 1, CustomerRoleId = 2, IsCustomerRoleIncluded = true });
            //To exclude "Grocery"  
            DiscountCategories.Add(new DiscountCategory { Id = 1, DiscountId = 1, CategoryId = 3, IsCategoryIncluded = false });

            //Discount rule for "If the user is an affiliate of the store, he gets a 10% discount"
            Discounts.Add(new Discount
            {
                Id = 2,
                Name = "If the user is an affiliate of the store, he gets a 10% discount",
                UsePercentage = true,
                Amount = 0.1m,
                Type = DiscountType.PerCustomerRole
            });
            DiscountCustomerRoles.Add(new DiscountCustomerRole { Id = 2, DiscountId = 2, CustomerRoleId = 3, IsCustomerRoleIncluded = true });
            //To exclude "Grocery" 
            DiscountCategories.Add(new DiscountCategory { Id = 2, DiscountId = 2, CategoryId = 3, IsCategoryIncluded = false });

            //Discount rule for "If the user has been a customer for over 2 years, he gets a 5% discount."
            Discounts.Add(new Discount
            {
                Id = 3,
                Name = "If the user has been a customer for over 2 years, he gets a 5% discount.",
                UsePercentage = true,
                Amount = 0.05m,
                CustomerCreationDays = 365 * 2,
                Type = DiscountType.PerCustomerCreationDate,
            });
            //To exclude "Grocery" 
            DiscountCategories.Add(new DiscountCategory { Id = 3, DiscountId = 3, CategoryId = 3, IsCategoryIncluded = false });
            
            //Discount rule for "For every $100 on the bill, there would be a $ 5 discount"
            Discounts.Add(new Discount
            {
                Id = 4,
                Name = "For every $100 on the bill, there would be a $ 5 discount",
                Amount = 5.0m,
                Type = DiscountType.BySplitAmount,
                SplitAmount = 100.0m,
                UsePercentage = false
            });

        }
    }
}