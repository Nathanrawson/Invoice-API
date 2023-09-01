namespace Invoice.DomainModels
{
    public class BankingDetails
    {
        public string BankName { get; set; }

        public string AccountName { get; set; }

        public int RoutingNumber { get; set; }

        public int AccountNumber { get; set; }
    }
}
