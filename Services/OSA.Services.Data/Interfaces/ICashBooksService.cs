namespace OSA.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface ICashBooksService
    {
        Task AddAsync(string startDate, string endDate, int companyId);

        decimal TotalSumStockCostAsync(DateTime startDate, DateTime endDate, int companyId);

        Task<ExpenseBook> GetMonthlyExpenseBook(DateTime startDate, DateTime endDate, int companyId);

        Task<CashBook> CashBookExistAsync(DateTime startDate, DateTime endDate, int companyId);
    }
}
