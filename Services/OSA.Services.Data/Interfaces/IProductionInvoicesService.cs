namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    public interface IProductionInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, decimal materialCost, decimal externalCost, int companyId);

        Task<string> InvoiceExistAsync(string invoiceNumber, int companyId);
    }
}
