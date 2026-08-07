namespace Inventory.API.DTOs.Dashboard
{
    public class RecentTransactionDto
    {
        public List<TransactionDto> RecentPurchases { get; set; } = [];

        public List<TransactionDto> RecentSales { get; set; } = [];
    }
}