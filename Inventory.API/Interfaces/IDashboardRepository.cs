using Inventory.API.DTOs.Dashboard;

namespace Inventory.API.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<RecentTransactionDto> GetRecentTransactionsAsync(int count = 5);
        Task<List<MonthlyChartDto>> GetMonthlyChartAsync();
        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int count = 10);
        Task<List<TopPurchasedProductDto>> GetTopPurchasedProductsAsync(int count = 10);

    }
}