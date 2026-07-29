using Inventory.API.Data;
using Inventory.API.DTOs.Purchase;
using Inventory.API.Entities;
using Inventory.API.Exceptions;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseRepository _repository;

        public PurchaseService(
            ApplicationDbContext context,
            IPurchaseRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<bool> CreatePurchaseAsync(CreatePurchaseDto dto)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var supplier =
                    await _repository.GetSupplierAsync(dto.SupplierId);

                if (supplier == null)
                    throw new NotFoundException($"Supplier with Id {dto.SupplierId} was not found.");//return false;


                var purchase = new Purchase
                {
                    PurchaseNumber = $"PUR-{DateTime.Now:yyyyMMddHHmmss}",
                    SupplierId = dto.SupplierId,
                    PurchaseDate = dto.PurchaseDate,
                    Remarks = dto.Remarks
                };

                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var product =
                        await _repository.GetProductAsync(item.ProductId);

                    if (product == null)
                        throw new NotFoundException($"Product with Id {item.ProductId} was not found.");

                    product.StockQuantity += item.Quantity;

                    purchase.PurchaseDetails.Add(new PurchaseDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Quantity * item.UnitPrice
                    });

                    total += item.Quantity * item.UnitPrice;
                }

                purchase.TotalAmount = total;

                await _repository.AddPurchaseAsync(purchase);

                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}