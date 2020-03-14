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

        Task<decimal> QuantitySold(int companyId);

        Task<decimal> QuantityPurchased(int companyId);

        decimal TotalQuantity(int companyId);

        decimal TotalPrice(int companyId);

        void GetStockName(string name);

        void GetDates(string startDate, string endDate, int companyId);
    }
}
