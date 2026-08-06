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
    }
}