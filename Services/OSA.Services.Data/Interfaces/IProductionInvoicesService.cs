namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IProductionInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, decimal salary, decimal externalCost, int companyId);

        Task<string> InvoiceExistAsync(string invoiceNumber, int companyId);

        Task<ICollection<ProductionInvoice>> GetProductionInvoicesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<ProductionInvoice> DeleteAsync(int id);

        Task<ProductionInvoice> GetProductionInvoiceByIdAsync(int id);

        Task UpdateProductionInvoiceAsync(int id, string invoiceNumber, decimal salary, decimal externalCost, DateTime date);
    }
}
