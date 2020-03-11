namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInvoicesService
    {
        Task AddAsync(string invoiceNumber, DateTime date, int supplierId, int companyId);
    }
}
