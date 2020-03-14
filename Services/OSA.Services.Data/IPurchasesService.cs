namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPurchasesService
    {
        Task AddAsync(string stockName, string date, int companyId);

        Task<IEnumerable<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(int companyId);

        Task<IEnumerable<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(int companyId);

        Task<decimal> QuantitySold(int companyId);

        Task<decimal> QuantityPurchased(int companyId);

        decimal TotalQuantity(int companyId);

        decimal TotalPrice(int companyId);

        void GetStockName(string name);

        void GetDates(string startDate, string endDate, int companyId);
    }
}
