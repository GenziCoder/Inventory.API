using Inventory.API.Common;
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

        public async Task<PagedResponse<PurchaseDto>> GetAllAsync(string? search,int pageNumber, int pageSize)
        {
            var purchases = await _repository.GetAllAsync(search,pageNumber,pageSize);

            return new PagedResponse<PurchaseDto>
            (
                 purchases.Data.Select(x => new PurchaseDto
                {
                    Id = x.Id,
                    PurchaseNumber = x.PurchaseNumber,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.CompanyName,
                    PurchaseDate = x.PurchaseDate,
                    TotalAmount = x.TotalAmount,
                    Remarks = x.Remarks,
                    Items= (x.PurchaseDetails??Enumerable.Empty<PurchaseDetail>()).Select(d => new PurchaseItemDto
                    {
                        ProductId = d.ProductId,
                        ProductName = d.Product.Name,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                    }).ToList()
                 }).ToList(),

                purchases.TotalRecords,
                purchases.PageNumber,
               purchases.PageSize
            );
        }

        public async Task<PurchaseDto?> GetByIdAsync(int id)
        {
            var purchase = await _repository.GetByIdAsync(id);

            if (purchase == null)
                return null;

            return new PurchaseDto
            {
                Id = purchase.Id,
                PurchaseNumber = purchase.PurchaseNumber,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier.CompanyName,
                PurchaseDate = purchase.PurchaseDate,
                TotalAmount = purchase.TotalAmount,
                Remarks = purchase.Remarks,
                Items = (purchase.PurchaseDetails ?? Enumerable.Empty<PurchaseDetail>()).Select(d => new PurchaseItemDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.Name,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList()
            };
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


        public async Task<bool> UpdatePurchaseAsync(int id, UpdatePurchaseDto dto)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var purchase = await _repository.GetByIdAsync(id);

                if (purchase == null)
                    throw new NotFoundException($"Purchase with Id {id} was not found.");

                var supplier = await _repository.GetSupplierAsync(dto.SupplierId);

                if (supplier == null)
                    throw new NotFoundException($"Supplier with Id {id} was not found.");

                // Reverse previous stock
                foreach (var detail in purchase.PurchaseDetails ?? Enumerable.Empty<PurchaseDetail>())
                {
                    var product = await _repository.GetProductAsync(detail.ProductId);

                    if (product != null)
                        product.StockQuantity -= detail.Quantity;
                }

                // Remove old purchase details

                // await _repository.RemovePurchaseDetailsAsync(purchase.PurchaseDetails);

                if (purchase.PurchaseDetails != null && purchase.PurchaseDetails.Any())
                {
                    await _repository.RemovePurchaseDetailsAsync(purchase.PurchaseDetails);
                    purchase.PurchaseDetails.Clear();
                }

                purchase.SupplierId = dto.SupplierId;
                purchase.PurchaseDate = dto.PurchaseDate;
                purchase.Remarks = dto.Remarks;

                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var product = await _repository.GetProductAsync(item.ProductId);

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

                await _repository.UpdatePurchaseAsync(purchase);

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

        public async Task<bool> DeletePurchaseAsync(int id)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var purchase = await _repository.GetByIdAsync(id);

                if (purchase == null)
                    throw new NotFoundException($"Purchase with Id {id} was not found.");

                // Reverse stock
                foreach (var detail in purchase.PurchaseDetails ?? Enumerable.Empty<PurchaseDetail>())
                {
                    var product = await _repository.GetProductAsync(detail.ProductId);

                    if (product == null)
                        continue;

                    product.StockQuantity -= detail.Quantity;
                }

                await _repository.DeletePurchaseAsync(purchase);

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