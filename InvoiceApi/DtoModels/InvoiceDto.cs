using System.ComponentModel.DataAnnotations;

namespace Invoice.Api.DtoModels
{
    public class InvoiceDto
    {
        [MaxLength(50)]
        public string CompanyName { get; set; }

        [MaxLength(255)]
        public string CompanySubHeader { get; set; }

        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        [MaxLength(50)]
        public string OrderNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public AddressDto ClientAddress { get; set; }

        public AddressDto CompanyAddress { get; set; }

        public ContactDto CompanyContact { get; set; }

        public BankingDetailsDto BankingDetails { get; set; }

        public List<InvoiceLineItemDto> LineItems { get; set; }
    }
}
