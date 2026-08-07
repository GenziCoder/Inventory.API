using Inventory.API.Common;
using Inventory.API.DTOs.Report;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface IReportService
    {
        Task<PagedResponse<SalesReportDto>> GetSalesReportAsync(SalesReportFilterDto filter);
        Task<PagedResponse<PurchaseReportDto>> GetPurchaseReportAsync(PurchaseReportFilterDto filter);
        Task<PagedResponse<StockReportDto>> GetStockReportAsync(StockReportFilterDto filter);

        Task<ApiResponse<List<StockLedgerDto>>> GetStockLedgerAsync(int productId);

        Task<PagedResponse<ProfitReportDto>> GetProfitReportAsync(ProfitReportFilterDto filter);
        Task<PagedResponse<CustomerSalesReportDto>> GetCustomerSalesReportAsync(CustomerSalesReportFilterDto filter);

        Task<PagedResponse<SupplierPurchaseReportDto>> GetSupplierPurchaseReportAsync(SupplierPurchaseReportFilterDto filter);






    }
}