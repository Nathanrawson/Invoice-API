namespace Invoice.DomainModels
{
    public class InvoiceLineItem
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Rate { get; set; }

        public decimal RateTotal { get; set; }

        public decimal Total { get; set; }

        public string CompanyName { get; set; }
    }
}
