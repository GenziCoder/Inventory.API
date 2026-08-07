namespace Inventory.API.DTOs.Report
{
    public class StockLedgerDto
    {
        public DateTime Date { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public string ReferenceNumber { get; set; } = string.Empty;

        public int StockIn { get; set; }

        public int StockOut { get; set; }

        public int Balance { get; set; }
    }
}