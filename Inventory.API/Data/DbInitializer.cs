using Inventory.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            await SeedUsers(context);
            await SeedCategories(context);
            await SeedSuppliers(context);
            await SeedCustomers(context);
            await SeedProducts(context);
        }

        private static async Task SeedUsers(ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
                return;

            var admin = new User
            {
                FirstName = "System ",
                LastName = "Administrator",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                IsActive = true
            };

            context.Users.Add(admin);

            await context.SaveChangesAsync();
        }

        private static async Task SeedCategories(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync())
                return;

            context.Categories.AddRange(
                new Category
                {
                    Name = "Electronics",
                    Description = "Electronic Products",
                    IsActive = true
                },
                new Category
                {
                    Name = "Accessories",
                    Description = "Computer Accessories",
                    IsActive = true
                },
                new Category
                {
                    Name = "Stationery",
                    Description = "Office Stationery",
                    IsActive = true
                });

            await context.SaveChangesAsync();
        }

        private static async Task SeedSuppliers(ApplicationDbContext context)
        {
            if (await context.Suppliers.AnyAsync())
                return;

            context.Suppliers.AddRange(
                new Supplier
                {
                    SupplierCode = "SUP001",
                    CompanyName = "Dell India",
                    ContactPerson = "Rohit Sharma",
                    Email = "sales@dell.com",
                    Phone = "9876543210",
                    Address = "Bangalore",
                    City = "Bangalore",
                    State = "Karnataka",
                    Country = "India",
                    PostalCode = "560001",
                    IsActive = true
                },
                new Supplier
                {
                    SupplierCode = "SUP002",
                    CompanyName = "HP India",
                    ContactPerson = "Ankit Verma",
                    Email = "sales@hp.com",
                    Phone = "9999999999",
                    Address = "Mumbai",
                    City = "Mumbai",
                    State = "Maharashtra",
                    Country = "India",
                    PostalCode = "400001",
                    IsActive = true
                });

            await context.SaveChangesAsync();
        }

        private static async Task SeedCustomers(ApplicationDbContext context)
        {
            if (await context.Customers.AnyAsync())
                return;

            context.Customers.AddRange(
                new Customer
                {
                    CustomerCode = "CUST001",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    Phone = "9876543210",
                    Address = "Mumbai",
                    City = "Mumbai",
                    State = "Maharashtra",
                    Country = "India",
                    PostalCode = "400001",
                    IsActive = true
                },
                new Customer
                {
                    CustomerCode = "CUST002",
                    FirstName = "Rahul",
                    LastName = "Sharma",
                    Email = "rahul@example.com",
                    Phone = "9999999999",
                    Address = "Delhi",
                    City = "Delhi",
                    State = "Delhi",
                    Country = "India",
                    PostalCode = "110001",
                    IsActive = true
                });

            await context.SaveChangesAsync();
        }

        private static async Task SeedProducts(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync())
                return;

            var electronics = await context.Categories
                .FirstAsync(x => x.Name == "Electronics");

            var accessories = await context.Categories
                .FirstAsync(x => x.Name == "Accessories");

            context.Products.AddRange(
                new Product
                {
                    ProductCode = "PROD001",
                    Name = "Dell Laptop",
                    Description = "Dell Latitude 5440",
                    PurchasePrice = 65000,
                    SellingPrice = 72000,
                    StockQuantity = 0,
                    MinimumStock = 5,
                    Barcode = "111111111",
                    CategoryId = electronics.Id,
                    IsActive = true
                },
                new Product
                {
                    ProductCode = "PROD002",
                    Name = "Dell Mouse",
                    Description = "Wireless Mouse",
                    PurchasePrice = 700,
                    SellingPrice = 900,
                    StockQuantity = 0,
                    MinimumStock = 10,
                    Barcode = "222222222",
                    CategoryId = accessories.Id,
                    IsActive = true
                });

            await context.SaveChangesAsync();
        }
    }
}