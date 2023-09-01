using Invoice.DomainModels;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;

namespace Invoice.Services.Factories
{
    public class InvoiceDocumentFactory
    {
        public Stream CreateInvoiceDocument(DomainModels.Invoice invoice)
        {
            var document = new Document();

            DefineStyles(document);

            var section = document.AddSection();
            section.PageSetup.TopMargin = "2.5cm";
            section.PageSetup.BottomMargin = "2.5cm";
            section.PageSetup.LeftMargin = "2.5cm";
            section.PageSetup.RightMargin = "2.5cm";

            CreateHeader(section, invoice);
            CreateFooter(section, invoice);

            PopulateInvoiceContent(section, invoice);

            var renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();

            var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            stream.Position = 0;

            return stream;
        }

        private void CreateHeader(Section section, DomainModels.Invoice invoice)
        {
            var header = section.Headers.Primary.AddParagraph();
            header.AddFormattedText(invoice.CompanyName, TextFormat.Bold);
            header.AddLineBreak();
            var subHeader = header.AddFormattedText(invoice.CompanySubHeader);
            subHeader.Font.Size = 10;
        }

        private void CreateFooter(Section section, DomainModels.Invoice invoice)
        {
            var footer = section.Footers.Primary;

            var companyNameFooter = footer.AddParagraph();
            companyNameFooter.AddText(invoice.CompanyName);
            companyNameFooter.Format.Alignment = ParagraphAlignment.Left;

            var companyContactAndAddressFooter = footer.AddParagraph();
            companyContactAndAddressFooter.AddText($"{invoice.CompanyContact.PhoneNumber} | {invoice.CompanyContact.Email}");
            companyContactAndAddressFooter.AddLineBreak();
            AddAddressToParagraph(companyContactAndAddressFooter, invoice.CompanyAddress);
            companyContactAndAddressFooter.Format.Alignment = ParagraphAlignment.Right;
        }

        private void DefineStyles(Document document)
        {
            // Define a style for headers
            Style style = document.Styles["Header"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 20;
            style.Font.Bold = true;
            style.ParagraphFormat.SpaceAfter = 6;

            // Define a style for footers
            style = document.Styles["Footer"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 10;

            // Define a style for table headers
            style = document.Styles.AddStyle("TableHeader", "Normal");
            style.Font.Name = "Tahoma";
            style.Font.Bold = true;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.SpaceAfter = 6;

            // Define a style for the table content
            style = document.Styles.AddStyle("TableContent", "Normal");
            style.Font.Name = "Tahoma";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.SpaceAfter = 6;
        }

        private void PopulateInvoiceContent(Section section, DomainModels.Invoice invoice)
        {
            section.AddSpace("1cm");
            
            var table = section.AddTable();
            table.AddColumn("8cm");  // Left column: Client Address
            table.AddColumn("8cm");  // Right column: Invoice Details

            var row = table.AddRow();
            // Left column: "Billed to:" and client address
            AddAddressToParagraph(row.Cells[0].AddParagraph(), invoice.ClientAddress, "Billed to:");

            // Right column: Invoice details
            var invoiceDetails = row.Cells[1].AddParagraph();
            invoiceDetails.Format.Alignment = ParagraphAlignment.Right;
            invoiceDetails.AddFormattedText($"Invoice Number: {invoice.InvoiceNumber}", TextFormat.Bold);
            invoiceDetails.AddLineBreak();
            invoiceDetails.AddText($"Order Number: {invoice.OrderNumber}");
            invoiceDetails.AddLineBreak();
            invoiceDetails.AddText($"Invoice Date: {invoice.InvoiceDate?.ToShortDateString()}");
            invoiceDetails.AddLineBreak();
            invoiceDetails.AddText($"Payment Date: {invoice.PaymentDate?.ToShortDateString()}");

            section.AddSpace("1cm");
            // Line items table
            var itemsTable = section.AddTable();
            itemsTable.Style = "Table";
            itemsTable.Borders.Color = Colors.Black;
            itemsTable.Borders.Width = 0.25;
            itemsTable.Borders.Left.Width = 0.5;
            itemsTable.Borders.Right.Width = 0.5;
            itemsTable.Rows.LeftIndent = 0;
            itemsTable.AddColumn("3.2cm");  // Description
            itemsTable.AddColumn("3.2cm");  // Date
            itemsTable.AddColumn("3.2cm");  // Ratepadding
            itemsTable.AddColumn("3.2cm");  // RateTotal
            itemsTable.AddColumn("3.2cm");  // Total

            row = itemsTable.AddRow();
            row.Cells[0].AddParagraph("Description").Style = "TableHeader";
            row.Cells[1].AddParagraph("Date").Style = "TableHeader";
            row.Cells[2].AddParagraph("Rate").Style = "TableHeader";
            row.Cells[3].AddParagraph("Rate Total").Style = "TableHeader";
            row.Cells[4].AddParagraph("Total").Style = "TableHeader";
            decimal totalAmount = 0;
            foreach (var lineItem in invoice.LineItems)
            {
                totalAmount += lineItem.Total;
                row = itemsTable.AddRow();
                row.Cells[0].AddParagraph(lineItem.Description);
                row.Cells[1].AddParagraph(lineItem.Date.ToShortDateString());
                row.Cells[2].AddParagraph(lineItem.Rate.ToString("C"));
                row.Cells[3].AddParagraph(lineItem.RateTotal.ToString("C"));
                row.Cells[4].AddParagraph(lineItem.Total.ToString("C"));
            }

            var totalParagraph = section.AddParagraph();
            totalParagraph.AddFormattedText($"Total: {totalAmount.ToString("C")}", TextFormat.Bold);
            totalParagraph.Format.SpaceBefore = "1cm";

            if (invoice.BankingDetails == null)
                return;
            var bankDetailsParagraph = section.AddParagraph();
            bankDetailsParagraph.AddFormattedText($"Bank: {invoice.BankingDetails.BankName}");
            bankDetailsParagraph.AddLineBreak();
            bankDetailsParagraph.AddFormattedText($"Account Name: {invoice.BankingDetails.AccountName}");
            bankDetailsParagraph.AddLineBreak();
            bankDetailsParagraph.AddFormattedText($"Routing Number: {invoice.BankingDetails.RoutingNumber}");
            bankDetailsParagraph.AddLineBreak();
            bankDetailsParagraph.AddFormattedText($"Account Number: {invoice.BankingDetails.AccountNumber}");
            bankDetailsParagraph.Format.SpaceBefore = "2cm";
        }

        private void AddAddressToParagraph(Paragraph paragraph, Address address, string prefix = null)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                paragraph.AddText(prefix);
                paragraph.AddLineBreak();
            }
            paragraph.AddText(address.Name);
            paragraph.AddLineBreak();
            paragraph.AddText(address.Line1);
            paragraph.AddLineBreak();
            if (!string.IsNullOrEmpty(address.Line2))
            {
                paragraph.AddText(address.Line2);
                paragraph.AddLineBreak();
            }
            paragraph.AddText(address.City);
            paragraph.AddLineBreak();
            paragraph.AddText(address.County);
            paragraph.AddLineBreak();
            paragraph.AddText(address.Country);
            paragraph.AddLineBreak();
            paragraph.AddText(address.Postcode);
        }
    }

    public static class PdfExtensions
    {
        public static void AddSpace(this Section section, string space)
        {
            var spacer = section.AddParagraph();
            spacer.Format.SpaceAfter = space;
        }
    }
}
