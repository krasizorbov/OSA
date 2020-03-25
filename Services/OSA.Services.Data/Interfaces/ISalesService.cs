namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface ISalesService
    {
        Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId);

        Task<string> SaleExistAsync(string stockName, int companyId);

        Task<IEnumerable<Sale>> GetSalesByCompanyIdAsync(int companyId);
    }
}
