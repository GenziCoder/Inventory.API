namespace Inventory.API.DTOs.Dashboard
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}