namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IStocksService
    {
        Task AddAsync(string name, decimal quantity, decimal price, string date, int invoiceId, int companyId);

        //Task<List<SelectListItem>> GetStockNamesByCompanyIdAsync(int companyId);

        Task<List<string>> GetStockNamesByCompanyIdAsync(int companyId);
    }
}
