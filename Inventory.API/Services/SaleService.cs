using Inventory.API.Data;
using Inventory.API.DTOs.Sale;
using Inventory.API.Entities;
using Inventory.API.Exceptions;
using Inventory.API.Interfaces;

namespace Inventory.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISaleRepository _repository;

        public SaleService(
            ApplicationDbContext context,
            ISaleRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<bool> CreateSaleAsync(CreateSaleDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var sale = new Sale
                {
                     
                    InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}",
                    CustomerId = dto.CustomerId,
                    //CustomerName = dto.CustomerName,
                    //CustomerPhone = dto.CustomerPhone,
                    SaleDate = dto.SaleDate,
                    Remarks = dto.Remarks
                };

                decimal totalAmount = 0;

                foreach (var item in dto.Items)
                {
                    var product = await _repository.GetProductAsync(item.ProductId);

                    if (product == null)
                        throw new NotFoundException($"Product with Id {item.ProductId} was not found.");

                    if (product.StockQuantity < item.Quantity)
                        throw new BusinessException($"Insufficient stock for product '{product.Name}'. Available quantity: {product.StockQuantity}.");


                    product.StockQuantity -= item.Quantity;

                    sale.SaleDetails.Add(new SaleDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Quantity * item.UnitPrice
                    });

                    totalAmount += item.Quantity * item.UnitPrice;
                }

                sale.TotalAmount = totalAmount;

                await _repository.AddSaleAsync(sale);

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