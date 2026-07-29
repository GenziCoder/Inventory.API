using Inventory.API.DTOs.Dashboard;

namespace Inventory.API.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync();
    }
}