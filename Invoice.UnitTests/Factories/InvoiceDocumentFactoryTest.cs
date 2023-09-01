using Invoice.DomainModels;
using Invoice.Services.Factories;

namespace Invoice.Services.UnitTests.Factories
{
    public class InvoiceDocumentFactoryTest
    {
        [Fact]
        public void CreateInvoiceDocument_ShouldReturnValidStream_GivenValidInvoice()
        {
            // Arrange
            var factory = new InvoiceDocumentFactory();
            var invoice = GetDummyInvoice();

            // Act
            var result = factory.CreateInvoiceDocument(invoice);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0, "The stream is empty");
        }

        [Fact]
        public void CreateInvoiceDocument_ShouldReturnPdfWithPages_GivenValidInvoice()
        {
            // Arrange
            var factory = new InvoiceDocumentFactory();
            var invoice = GetDummyInvoice();

            // Act
            var resultStream = factory.CreateInvoiceDocument(invoice);
            var pdf = PdfSharpCore.Pdf.IO.PdfReader.Open(resultStream, PdfSharpCore.Pdf.IO.PdfDocumentOpenMode.ReadOnly);

            // Assert
            Assert.True(pdf.PageCount > 0, "The PDF does not contain any pages");
        }

        private DomainModels.Invoice GetDummyInvoice()
        {
            return new DomainModels.Invoice
            {
                CompanyName = "TechSolutions Ltd.",
                CompanySubHeader = "Technology Innovators",
                InvoiceNumber = "INV-12345",
                OrderNumber = "ORD-54321",
                InvoiceDate = DateTime.Parse("2023-09-01T17:00:30.939Z"),
                PaymentDate = DateTime.Parse("2023-10-01T17:00:30.939Z"),
                ClientAddress = new Address
                {
                    Name = "John Doe",
                    Line1 = "123 Client St.",
                    Line2 = "Suite 45",
                    City = "ClientCity",
                    County = "ClientCounty",
                    Country = "ClientLand",
                    Postcode = "CL1234"
                },
                CompanyAddress = new Address
                {
                    Name = "TechSolutions Ltd.",
                    Line1 = "456 Tech Ave.",
                    Line2 = "Floor 7",
                    City = "TechCity",
                    County = "TechCounty",
                    Country = "TechLand",
                    Postcode = "TC5678"
                },
                CompanyContact = new Contact
                {
                    PhoneNumber = "+123-456-7890",
                    Email = "contact@techsolutions.com"
                },
                BankingDetails = new BankingDetails
                {
                    BankName = "TechBank",
                    AccountName = "TechSolutions Ltd. Account",
                    RoutingNumber = 123456789,
                    AccountNumber = 987654321
                },
                LineItems = new List<InvoiceLineItem>
                {
                    new InvoiceLineItem
                    {
                        Description = "Software Development",
                        Date = DateTime.Parse("2023-09-01T17:00:30.939Z"),
                        Rate = 50,
                        RateTotal = 1000,
                        Total = 1000
                    },
                    new InvoiceLineItem
                    {
                        Description = "Consultancy",
                        Date = DateTime.Parse("2023-08-25T12:30:20.123Z"),
                        Rate = 70,
                        RateTotal = 210,
                        Total = 210
                    }
                }
            };
        }
    }
}