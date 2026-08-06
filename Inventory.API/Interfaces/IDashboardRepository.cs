using Inventory.API.DTOs.Dashboard;

namespace Inventory.API.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    }
}