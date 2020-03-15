namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IBooksValue
    {
        Task AddAsync(decimal price, string stockName, string date, int companyId);

        Task<Sell> GetMonthlySells(string startDate, string endDate, int companyId);

        Task<decimal> GetStockMonthlyAveragePrice(string stockName, string startDate, string endDate, int companyId);
    }
}
