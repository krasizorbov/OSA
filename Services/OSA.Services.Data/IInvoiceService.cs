namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInvoiceService
    {
        Task AddAsync(string invoiceNumber, DateTime date, int supplierId, int companyId);

        Task<IEnumerable<string>> GetAllSupplierNames();
    }
}
