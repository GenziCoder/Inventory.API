using Inventory.API.Common;
using Inventory.API.DTOs.Report;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;

namespace Inventory.API.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _salesReportRepository;
        private readonly IReportRepository _purchaseReportRepository;
        private readonly IReportRepository _stockReportRepository;
        private readonly IReportRepository _profitReportRepository;
        private readonly IReportRepository _customerReportRepository;
        private readonly IReportRepository _supplierReportRepository;

        public ReportService(IReportRepository salesReportRepository, 
            IReportRepository purchaseReportRepository,
            IReportRepository stockReportRepository,
            IReportRepository profitReportRepository,
            IReportRepository customerReportRepository,
            IReportRepository supplierReportRepository)
        {
            _salesReportRepository = salesReportRepository;
            _purchaseReportRepository = purchaseReportRepository;
            _stockReportRepository = stockReportRepository;
            _profitReportRepository = profitReportRepository;
            _customerReportRepository = customerReportRepository;
            _supplierReportRepository = supplierReportRepository;
        }

        public async Task<PagedResponse<SalesReportDto>> GetSalesReportAsync(SalesReportFilterDto filter)
        {
            return await _salesReportRepository.GetSalesReportAsync(filter);

        }

        public async Task<PagedResponse<PurchaseReportDto>> GetPurchaseReportAsync(PurchaseReportFilterDto filter)
        {
            return await _purchaseReportRepository.GetPurchaseReportAsync(filter);

        }

        public async Task<PagedResponse<StockReportDto>> GetStockReportAsync(StockReportFilterDto filter)
        {
            return await _stockReportRepository.GetStockReportAsync(filter);

        }

        public async Task<ApiResponse<List<StockLedgerDto>>> GetStockLedgerAsync(int productId)
        {
            var ledger =
                await _stockReportRepository
                    .GetStockLedgerAsync(productId);

            return ApiResponse<List<StockLedgerDto>>
                .SuccessResponse(
                    ledger,
                    "Stock ledger");
        }

        public async Task<PagedResponse<ProfitReportDto>> GetProfitReportAsync(ProfitReportFilterDto filter)
        {
            return await _profitReportRepository.GetProfitReportAsync(filter);

        }

        public async Task<PagedResponse<CustomerSalesReportDto>> GetCustomerSalesReportAsync(CustomerSalesReportFilterDto filter)
        {
            return await _customerReportRepository
                .GetCustomerSalesReportAsync(filter);
        }

        public async Task<PagedResponse<SupplierPurchaseReportDto>> GetSupplierPurchaseReportAsync(SupplierPurchaseReportFilterDto filter)
        {
            return await _supplierReportRepository
                .GetSupplierPurchaseReportAsync(filter);
        }
    }
}