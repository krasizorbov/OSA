namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IAvailableStocksService
    {
        Task AddAsync(string startDate, string endDate, string date, int companyId);

        Task<IEnumerable<string>> GetPurchasedStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<IEnumerable<string>> GetSoldStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<Purchase> GetCurrentPurchasedStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId);

        Task<Sell> GetCurrentSoldStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId);

        Task<decimal> GetCurrentBookValueForStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId);
    }
}
