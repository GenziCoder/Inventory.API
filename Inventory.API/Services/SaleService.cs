using AutoMapper;
using Inventory.API.Common;
using Inventory.API.Data;
using Inventory.API.DTOs.Sale;
using Inventory.API.Entities;
using Inventory.API.Exceptions;
using Inventory.API.Interfaces;

namespace Inventory.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SaleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<SaleDto>> GetAllAsync(string? search, int pageNumber, int pageSize)
        {
            var sales = await _unitOfWork.Sales.GetAllAsync(search, pageNumber, pageSize);
            return new PagedResponse<SaleDto>(
                sales.Data.Select(x => new SaleDto
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    CustomerId = x.CustomerId,
                    CustomerName = $"{x.Customer.FirstName} {x.Customer.LastName}",
                    SaleDate = x.SaleDate,
                    TotalAmount = x.TotalAmount,
                    Remarks = x.Remarks,
                    Items = x.SaleDetails.Select(x => new SaleItemDto
                    { 
                       ProductId=x.ProductId,
                       ProductName=x.Product.Name,
                       Quantity=x.Quantity,
                       UnitPrice=x.UnitPrice,
                       TotalPrice=x.TotalPrice
                    }).ToList()
                }).ToList(),

                sales.TotalRecords,
                sales.PageNumber,
                sales.PageSize
            );
        }

        public async Task<SaleDto?> GetByIdAsync(int id)
        {
            var sale = await _unitOfWork.Sales.GetByIdAsync(id);

            if (sale == null)
                return null;

            return new SaleDto
            {
                Id = sale.Id,
                InvoiceNumber = sale.InvoiceNumber,
                CustomerId = sale.CustomerId,
                CustomerName = $"{sale.Customer.FirstName} {sale.Customer.LastName}",
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount,
                Remarks = sale.Remarks,

                Items = sale.SaleDetails.Select(x => new SaleItemDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice
                }).ToList()
            };
        }

        public async Task<bool> CreateSaleAsync(CreateSaleDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var customer =
                    await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId);

                if (customer == null)
                    throw new NotFoundException(
                        $"Customer with Id {dto.CustomerId} was not found.");

                var sale = new Sale
                {
                    InvoiceNumber = $"SAL-{DateTime.Now:yyyyMMddHHmmss}",
                    CustomerId = dto.CustomerId,
                    SaleDate = dto.SaleDate,
                    Remarks = dto.Remarks
                };

                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var product =
                        await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                    if (product == null)
                        throw new NotFoundException(
                            $"Product with Id {item.ProductId} was not found.");

                    if (product.StockQuantity < item.Quantity)
                        throw new BadRequestException(
                            $"{product.Name} has only {product.StockQuantity} item(s) available.");

                    product.StockQuantity -= item.Quantity;

                    _unitOfWork.Products.Update(product);

                    sale.SaleDetails.Add(new SaleDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Quantity * item.UnitPrice
                    });

                    total += item.Quantity * item.UnitPrice;
                }

                sale.TotalAmount = total;

                await _unitOfWork.Sales.AddAsync(sale);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateSaleAsync(int id, UpdateSaleDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var sale = await _unitOfWork.Sales.GetByIdAsync(id);

                if (sale == null)
                    throw new NotFoundException($"Sale with Id {id} was not found.");

                var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId);

                if (customer == null)
                    throw new NotFoundException($"Customer with Id {dto.CustomerId} was not found.");

                // Restore previous stock
                foreach (var detail in sale.SaleDetails)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);

                    if (product != null)
                    {
                        product.StockQuantity += detail.Quantity;
                        _unitOfWork.Products.Update(product);
                    }
                }

                // Remove old details
                //sale.SaleDetails.Clear();
                if (sale.SaleDetails.Any())
                {
                    await _unitOfWork.Sales.RemoveSaleDetailsAsync(sale.SaleDetails);
                    sale.SaleDetails.Clear();
                }

                // Update header
                sale.CustomerId = dto.CustomerId;
                sale.SaleDate = dto.SaleDate;
                sale.Remarks = dto.Remarks;

                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                    if (product == null)
                        throw new NotFoundException($"Product with Id {item.ProductId} was not found.");

                    if (product.StockQuantity < item.Quantity)
                        throw new BadRequestException(
                            $"{product.Name} has only {product.StockQuantity} item(s) available.");

                    product.StockQuantity -= item.Quantity;

                    _unitOfWork.Products.Update(product);

                    sale.SaleDetails.Add(new SaleDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Quantity * item.UnitPrice
                    });

                    total += item.Quantity * item.UnitPrice;
                }

                sale.TotalAmount = total;

                await _unitOfWork.Sales.UpdateAsync(sale);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteSaleAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var sale = await _unitOfWork.Sales.GetByIdAsync(id);

                if (sale == null)
                    throw new NotFoundException($"Sale with Id {id} was not found.");

                // Restore Stock
                foreach (var detail in sale.SaleDetails)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);

                    if (product == null)
                        continue;

                    product.StockQuantity += detail.Quantity;

                    _unitOfWork.Products.Update(product);
                }

                await _unitOfWork.Sales.DeleteAsync(sale);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}