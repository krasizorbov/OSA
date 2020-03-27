namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IProductionInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, decimal materialCost, decimal externalCost, int companyId);

        Task<string> InvoiceExistAsync(string invoiceNumber, int companyId);

        Task<IEnumerable<ProductionInvoice>> GetProductionInvoicesByCompanyIdAsync(int companyId);
    }
}
