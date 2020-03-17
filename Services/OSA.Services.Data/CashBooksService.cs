namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public class CashBooksService : ICashBooksService
    {
        public Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ExpenseBook> GetMonthlyExpenseBook(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> TotalSumStockCostAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
