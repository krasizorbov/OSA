namespace OSA.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class CashBooksService : ICashBooksService
    {
        private readonly IDeletableEntityRepository<CashBook> cashBooksRepository;
        private readonly ApplicationDbContext context;

        public CashBooksService(IDeletableEntityRepository<CashBook> cashBooksRepository, ApplicationDbContext context)
        {
            this.cashBooksRepository = cashBooksRepository;
            this.context = context;
        }

        public Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpenseBook> GetMonthlyExpenseBook(DateTime startDate, DateTime endDate, int id)
        {
            var expenseBook = await this.context.ExpenseBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .FirstOrDefaultAsync();

            return expenseBook;
        }

        public async Task<decimal> TotalSumStockCostAsync(DateTime startDate, DateTime endDate, int id)
        {
            var stockCost = await this.context.Stocks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Sum(x => x.Price);

            return stockCost;
        }
    }
}
