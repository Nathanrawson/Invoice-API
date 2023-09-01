using AutoMapper;
using Invoice.Api.DtoModels;
using Invoice.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public InvoiceController(IInvoiceService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InvoiceDto invoice)
        {
            DomainModels.Invoice mappedInvoice = _mapper.Map<DomainModels.Invoice>(invoice);
            Stream pdfStream = await _invoiceService.Create(mappedInvoice);
            return new FileStreamResult(pdfStream, "application/pdf")
            {
                FileDownloadName = $"{mappedInvoice.CompanyName}_Invoice_{mappedInvoice.InvoiceNumber}.pdf"
            };
        }
    }
}
