namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IInvoicesService
    {
        Task AddAsync(string invoiceNumber, string date, int supplierId, int companyId);

        Task<ICollection<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId);

        Task<string> InvoiceExistAsync(string invoiceNumber, int companyId);
    }
}
