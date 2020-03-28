namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IBookValuesService
    {
        Task AddAsync(string startDate, string endDate, int companyId);

        Task<List<Sale>> GetMonthlySalesAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<decimal> GetStockMonthlyAveragePriceAsync(string stockName, DateTime startDate, DateTime endDate, int companyId);

        Task<List<string>> BookValueExistAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<IEnumerable<BookValue>> GetBookValuesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
