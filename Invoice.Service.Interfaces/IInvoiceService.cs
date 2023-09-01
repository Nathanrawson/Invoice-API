namespace Invoice.Service.Interfaces
{
    public interface IInvoiceService
    {
        Task<Stream> Create(DomainModels.Invoice invoice);
    }
}
