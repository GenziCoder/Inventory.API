using Inventory.API.Common;
using Inventory.API.DTOs.Dashboard;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Inventory.API.Repositories;

namespace Inventory.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<DashboardSummaryDto>> GetDashboardSummaryAsync()
        {
            var summary = await _repository.GetDashboardSummaryAsync();

            return ApiResponse<DashboardSummaryDto>.SuccessResponse(
                summary,
                "Dashboard summary");
        }

        public async Task<ApiResponse<RecentTransactionDto>> GetRecentTransactionsAsync(int count = 5)
        {
            var data = await _repository.GetRecentTransactionsAsync(count);

            return ApiResponse<RecentTransactionDto>.SuccessResponse(
                data,
                "Recent transactions");
        }

        public async Task<ApiResponse<List<MonthlyChartDto>>> GetMonthlyChartAsync()
        {
            var data = await _repository.GetMonthlyChartAsync();

            return ApiResponse<List<MonthlyChartDto>>
                .SuccessResponse(data, "Monthly chart data");
        }
        public async Task<ApiResponse<List<TopSellingProductDto>>> GetTopSellingProductsAsync(int count = 10)
        {
            var products = await _repository.GetTopSellingProductsAsync(count);

            return ApiResponse<List<TopSellingProductDto>>
                .SuccessResponse(products, "Top selling products");
        }
        public async Task<ApiResponse<List<TopPurchasedProductDto>>> GetTopPurchasedProductsAsync(int count = 10)
        {
            var data = await _repository
                .GetTopPurchasedProductsAsync(count);

            return ApiResponse<List<TopPurchasedProductDto>>
                .SuccessResponse(data, "Top purchased products");
        }
    }
}