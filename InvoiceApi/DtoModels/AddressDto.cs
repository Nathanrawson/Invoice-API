using System.ComponentModel.DataAnnotations;

namespace Invoice.Api.DtoModels
{
    public class AddressDto
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Line1 { get; set; }

        [MaxLength(50)]
        public string Line2 { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string County { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(10)]
        public string Postcode { get; set; }
    }
}
