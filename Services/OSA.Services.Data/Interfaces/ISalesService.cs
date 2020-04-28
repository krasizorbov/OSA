namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface ISalesService
    {
        Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId);

        Task<string> SaleExistAsync(DateTime startDate, string stockName, int companyId);

        Task<ICollection<Sale>> GetSalesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> GetAveragePrice(DateTime startDate, string stockName, int companyId);

        Task<decimal> GetTotalPurchasedQuantity(DateTime startDate, string stockName, int companyId);

        Task<bool> IsBigger(decimal totalPrice, int profitPercent, DateTime startDate, string stockName, int companyId);

        Task<List<Sale>> DeleteAsync(List<int> ids);

        bool CompanyIdExists(int id);

        Task<Sale> GetSaleByIdAsync(int id);

        Task UpdateSaleAsync(int id, decimal price, int profitPercent, DateTime date);

        Task<string> GetStockNameBySaleIdAsync(int id);
    }
}
