using Inventory.API.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Inventory.API.Documents
{
    public class ProductReportDocument : IDocument
    {
        private readonly List<Product> _products;

        public ProductReportDocument(List<Product> products)
        {
            _products = products;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(25);

                page.Header()
                    .Text("Products Report")
                    .FontSize(20)
                    .Bold();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Code").Bold();
                        header.Cell().Text("Name").Bold();
                        header.Cell().Text("Stock").Bold();
                        header.Cell().Text("Price").Bold();
                    });

                    foreach (var product in _products)
                    {
                        table.Cell().Text(product.ProductCode);
                        table.Cell().Text(product.Name);
                        table.Cell().Text(product.StockQuantity.ToString());
                        table.Cell().Text(product.SellingPrice.ToString("C"));
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }
    }
}