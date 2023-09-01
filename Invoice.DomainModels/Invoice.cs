namespace Invoice.DomainModels
{
    public class Invoice
    {
        public string CompanyName { get; set; }

        public string CompanySubHeader { get; set; }

        public string InvoiceNumber { get; set; }

        public string OrderNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public Address ClientAddress { get; set; }

        public Address CompanyAddress { get; set; }

        public Contact CompanyContact { get; set; }

        public BankingDetails BankingDetails { get; set; }

        public List<InvoiceLineItem> LineItems { get; set; }

        public string AdditionalNote { get; set; }
    }
}
