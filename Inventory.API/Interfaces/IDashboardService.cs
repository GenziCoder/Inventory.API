using Inventory.API.DTOs.Dashboard;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardSummaryDto>> GetDashboardSummaryAsync();
    }
}