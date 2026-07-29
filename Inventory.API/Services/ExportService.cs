using ClosedXML.Excel;
using Inventory.API.Data;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Inventory.API.Documents;
using QuestPDF.Fluent;

namespace Inventory.API.Services
{
    public class ExportService : IExportService
    {
        private readonly ApplicationDbContext _context;

        public ExportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportProductsAsync()
        {
            var products = await _context.Products
                .Include(x => x.Category)
               // .Include(x => x.Supplier)
                .OrderBy(x => x.Name)
                .ToListAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Products");

            // Header
            worksheet.Cell(1, 1).Value = "Product Code";
            worksheet.Cell(1, 2).Value = "Product Name";
            worksheet.Cell(1, 3).Value = "Category";
           // worksheet.Cell(1, 4).Value = "Supplier";
            worksheet.Cell(1, 4).Value = "Purchase Price";
            worksheet.Cell(1, 5).Value = "Selling Price";
            worksheet.Cell(1, 6).Value = "Stock";
            worksheet.Cell(1, 7).Value = "Minimum Stock";

            var row = 2;

            foreach (var product in products)
            {
                worksheet.Cell(row, 1).Value = product.ProductCode;
                worksheet.Cell(row, 2).Value = product.Name;
                worksheet.Cell(row, 3).Value = product.Category.Name;
               // worksheet.Cell(row, 4).Value = product.Supplier.CompanyName;
                worksheet.Cell(row, 4).Value = product.PurchasePrice;
                worksheet.Cell(row, 5).Value = product.SellingPrice;
                worksheet.Cell(row, 6).Value = product.StockQuantity;
                worksheet.Cell(row, 7).Value = product.MinimumStock;

                row++;
            }

            // worksheet.Columns().AdjustToContents();
            FormatWorksheet(worksheet, "A1:G1");

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        public async Task<byte[]> ExportCustomersAsync()
        {
            var customers = await _context.Customers
                .OrderBy(x => x.FirstName)
                .ToListAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Customers");

            worksheet.Cell(1, 1).Value = "Customer Code";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Phone";
            worksheet.Cell(1, 5).Value = "City";
            worksheet.Cell(1, 6).Value = "State";
            worksheet.Cell(1, 7).Value = "Country";
            worksheet.Cell(1, 8).Value = "Status";

            int row = 2;

            foreach (var customer in customers)
            {
                worksheet.Cell(row, 1).Value = customer.CustomerCode;
                worksheet.Cell(row, 2).Value = $"{customer.FirstName} {customer.LastName}";
                worksheet.Cell(row, 3).Value = customer.Email;
                worksheet.Cell(row, 4).Value = customer.Phone;
                worksheet.Cell(row, 5).Value = customer.City;
                worksheet.Cell(row, 6).Value = customer.State;
                worksheet.Cell(row, 7).Value = customer.Country;
                worksheet.Cell(row, 8).Value = customer.IsActive ? "Active" : "Inactive";

                row++;
            }

            FormatWorksheet(worksheet, "A1:H1");

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        public async Task<byte[]> ExportSuppliersAsync()
        {
            var suppliers = await _context.Suppliers
                .OrderBy(x => x.CompanyName)
                .ToListAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Suppliers");

            worksheet.Cell(1, 1).Value = "Supplier Code";
            worksheet.Cell(1, 2).Value = "Company";
            worksheet.Cell(1, 3).Value = "Contact Person";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Phone";
            worksheet.Cell(1, 6).Value = "City";
            worksheet.Cell(1, 7).Value = "Country";

            int row = 2;

            foreach (var supplier in suppliers)
            {
                worksheet.Cell(row, 1).Value = supplier.SupplierCode;
                worksheet.Cell(row, 2).Value = supplier.CompanyName;
                worksheet.Cell(row, 3).Value = supplier.ContactPerson;
                worksheet.Cell(row, 4).Value = supplier.Email;
                worksheet.Cell(row, 5).Value = supplier.Phone;
                worksheet.Cell(row, 6).Value = supplier.City;
                worksheet.Cell(row, 7).Value = supplier.Country;

                row++;
            }

            FormatWorksheet(worksheet, "A1:G1");

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        public async Task<byte[]> ExportSalesAsync()
        {
            var sales = await _context.Sales
                .Include(x => x.Customer)
                .OrderByDescending(x => x.SaleDate)
                .ToListAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Sales");

            worksheet.Cell(1, 1).Value = "Invoice";
            worksheet.Cell(1, 2).Value = "Date";
            worksheet.Cell(1, 3).Value = "Customer";
            worksheet.Cell(1, 4).Value = "Total Amount";
            worksheet.Cell(1, 5).Value = "Remarks";

            int row = 2;

            foreach (var sale in sales)
            {
                worksheet.Cell(row, 1).Value = sale.InvoiceNumber;
                worksheet.Cell(row, 2).Value = sale.SaleDate;
                worksheet.Cell(row, 3).Value = $"{sale.Customer.FirstName} {sale.Customer.LastName}";
                worksheet.Cell(row, 4).Value = sale.TotalAmount;
                worksheet.Cell(row, 5).Value = sale.Remarks;

                row++;
            }

            worksheet.Column(2).Style.DateFormat.Format = "dd-MM-yyyy";

            FormatWorksheet(worksheet, "A1:E1");

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        public async Task<byte[]> ExportPurchasesAsync()
        {
            var purchases = await _context.Purchases
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Purchases");

            worksheet.Cell(1, 1).Value = "Purchase No";
            worksheet.Cell(1, 2).Value = "Date";
            worksheet.Cell(1, 3).Value = "Supplier";
            worksheet.Cell(1, 4).Value = "Total Amount";
            worksheet.Cell(1, 5).Value = "Remarks";

            int row = 2;

            foreach (var purchase in purchases)
            {
                worksheet.Cell(row, 1).Value = purchase.PurchaseNumber;
                worksheet.Cell(row, 2).Value = purchase.PurchaseDate;
                worksheet.Cell(row, 3).Value = purchase.Supplier.CompanyName;
                worksheet.Cell(row, 4).Value = purchase.TotalAmount;
                worksheet.Cell(row, 5).Value = purchase.Remarks;

                row++;
            }

            worksheet.Column(2).Style.DateFormat.Format = "dd-MM-yyyy";

            FormatWorksheet(worksheet, "A1:E1");

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        // PDF related method
        public async Task<byte[]> ExportProductsPdfAsync()
        {
            var products = await _context.Products
                .OrderBy(x => x.Name)
                .ToListAsync();

            var document = new ProductReportDocument(products);

            return document.GeneratePdf();
        }

        private static void FormatWorksheet(IXLWorksheet worksheet, string headerRange)
        {
            var header = worksheet.Range(headerRange);

            header.Style.Font.Bold = true;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            header.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();

            worksheet.SheetView.FreezeRows(1);
        }
    }
}