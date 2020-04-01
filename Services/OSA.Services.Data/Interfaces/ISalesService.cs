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

        Task<IEnumerable<Sale>> GetSalesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> GetAveragePrice(DateTime startDate, string stockName, int companyId);

        Task<decimal> GetTotalPurchasedQuantity(DateTime startDate, string stockName, int companyId);

        Task<string> PurchasedStockExist(DateTime startDate, string stockName, int companyId);

        bool IsBigger();
    }
}
