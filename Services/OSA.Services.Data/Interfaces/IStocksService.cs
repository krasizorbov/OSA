namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IStocksService
    {
        Task AddAsync(string name, decimal quantity, decimal price, string date, int invoiceId, int companyId);

        Task<List<string>> GetStockNamesByCompanyIdAsync(int companyId);

        Task<IEnumerable<Stock>> GetStocksByCompanyIdAsync(int companyId);
    }
}
