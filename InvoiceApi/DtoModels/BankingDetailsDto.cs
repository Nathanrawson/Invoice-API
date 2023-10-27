using System.ComponentModel.DataAnnotations;

namespace Invoice.Api.DtoModels
{
    public class BankingDetailsDto
    {
        [MaxLength(50)]
        public string BankName { get; set; }

        [MaxLength(50)]
        public string AccountName { get; set; }

        public int RoutingNumber { get; set; }

        public int AccountNumber { get; set; }
    }
}
