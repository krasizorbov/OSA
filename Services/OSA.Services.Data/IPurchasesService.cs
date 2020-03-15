namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPurchasesService
    {
        Task AddAsync(string stockName, string startDate, string endDate, string date, int companyId);

        Task<IEnumerable<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<IEnumerable<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> QuantitySold(string stockName, DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> QuantityPurchased(string stockName, DateTime startDate, DateTime endDate, int companyId);

        decimal TotalQuantity(string stockName, DateTime startDate, DateTime endDate, int companyId);

        decimal TotalPrice(string stockName, DateTime startDate, DateTime endDate, int companyId);
    }
}
