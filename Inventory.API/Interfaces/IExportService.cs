namespace Inventory.API.Interfaces
{
    public interface IExportService
    {
        // Excel sheet
        Task<byte[]> ExportProductsAsync();
        Task<byte[]> ExportCustomersAsync();

        Task<byte[]> ExportSuppliersAsync();

        Task<byte[]> ExportSalesAsync();

        Task<byte[]> ExportPurchasesAsync();

        // Pdf shee

        Task<byte[]> ExportProductsPdfAsync();

        //Task<byte[]> ExportCustomersPdfAsync();

        //Task<byte[]> ExportSalesPdfAsync();

        //Task<byte[]> ExportPurchasesPdfAsync();
    }
}