using Inventory.API.DTOs.Dashboard;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardSummaryDto>> GetDashboardSummaryAsync();
        Task<ApiResponse<RecentTransactionDto>> GetRecentTransactionsAsync(int count = 5);
        Task<ApiResponse<List<MonthlyChartDto>>> GetMonthlyChartAsync();
        Task<ApiResponse<List<TopSellingProductDto>>> GetTopSellingProductsAsync(int count = 10);
        Task<ApiResponse<List<TopPurchasedProductDto>>> GetTopPurchasedProductsAsync(int count = 10);

    }
}