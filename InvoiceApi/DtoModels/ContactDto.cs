using System.ComponentModel.DataAnnotations;

namespace Invoice.Api.DtoModels
{
    public class ContactDto
    {
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
