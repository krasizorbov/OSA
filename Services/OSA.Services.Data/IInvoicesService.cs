namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, int supplierId, int companyId);

        Task<IEnumerable<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId);

        bool InvoiceExist(string invoiceNumber, int companyId);
    }
}
