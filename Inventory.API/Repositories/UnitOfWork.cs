using Inventory.API.Data;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IDbContextTransaction? _transaction;

        public ICategoryRepository Categories { get; }

        public IProductRepository Products { get; }

        public ISupplierRepository Suppliers { get; }

        public IPurchaseRepository Purchases { get; }
        public ICustomerRepository Customers { get; }
        public ISaleRepository Sales { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            ICategoryRepository categories,
            IProductRepository products,
            ISupplierRepository suppliers,
            IPurchaseRepository purchases,
            ICustomerRepository customers,
            ISaleRepository sales)
        {
            _context = context;

            Categories = categories;
            Products = products;
            Suppliers = suppliers;
            Purchases = purchases;
            Customers = customers;
            Sales = sales;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}