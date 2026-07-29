using Inventory.API.Interfaces;

namespace Inventory.API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }

        IProductRepository Products { get; }

        ISupplierRepository Suppliers { get; }

        IPurchaseRepository Purchases { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}