namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public class BooksValueService : IBooksValueService
    {
        public Task AddAsync(decimal price, string stockName, string date, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Sell> GetMonthlySells(string startDate, string endDate, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetStockMonthlyAveragePrice(string stockName, string startDate, string endDate, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
