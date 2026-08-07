using Inventory.API.Common;
using Inventory.API.DTOs.Report;

namespace Inventory.API.Interfaces
{
    public interface IReportRepository
    {
        Task<PagedResponse<SalesReportDto>> GetSalesReportAsync(SalesReportFilterDto filter);
        Task<PagedResponse<PurchaseReportDto>> GetPurchaseReportAsync(PurchaseReportFilterDto filter);
        Task<PagedResponse<StockReportDto>> GetStockReportAsync(StockReportFilterDto filter);
        Task<List<StockLedgerDto>> GetStockLedgerAsync(int productId);
        Task<PagedResponse<ProfitReportDto>> GetProfitReportAsync(ProfitReportFilterDto filter);

        Task<PagedResponse<CustomerSalesReportDto>> GetCustomerSalesReportAsync(CustomerSalesReportFilterDto filter);

        Task<PagedResponse<SupplierPurchaseReportDto>> GetSupplierPurchaseReportAsync(SupplierPurchaseReportFilterDto filter);



    }
}