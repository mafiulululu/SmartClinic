namespace BLL.Services
{
    public interface IInvoicePdfService
    {
        Task<byte[]?> GenerateInvoicePdfAsync(int invoiceId);
    }
}