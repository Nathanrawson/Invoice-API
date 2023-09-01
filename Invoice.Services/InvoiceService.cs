using Invoice.Service.Interfaces;
using Invoice.Services.Factories;

namespace Invoice.Services
{
    public class InvoiceService: IInvoiceService
    {
        public Task<Stream> Create(DomainModels.Invoice invoice)
        {
            return Task.FromResult(new InvoiceDocumentFactory().CreateInvoiceDocument(invoice));
        }
    }
}
