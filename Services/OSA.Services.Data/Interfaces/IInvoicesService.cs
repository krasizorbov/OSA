namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Data.Models;

    public interface IInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, int supplierId, int companyId);

        Task<ICollection<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId);

        Task<string> InvoiceExistAsync(string invoiceNumber, int companyId);

        Task<IEnumerable<Invoice>> GetInvoicesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
