namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class CashBooksService : ICashBooksService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<CashBook> cashBooksRepository;
        private readonly ApplicationDbContext context;

        public CashBooksService(IDeletableEntityRepository<CashBook> cashBooksRepository, ApplicationDbContext context)
        {
            this.cashBooksRepository = cashBooksRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);

            var totalStockCost = this.TotalSumStockCostAsync(start_Date, end_Date, companyId);
            var expenseBook = await this.GetMonthlyExpenseBook(start_Date, end_Date, companyId);

            var cashBook = new CashBook
            {
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                TotalInvoicePricesCost = totalStockCost,
                TotalSalaryCost = expenseBook.TotalSalaryCost,
                TotalStockExternalCost = expenseBook.TotalStockExternalCost,
                TotalProfit = expenseBook.TotalProfit,
                CompanyId = companyId,
            };
            await this.cashBooksRepository.AddAsync(cashBook);
            await this.cashBooksRepository.SaveChangesAsync();
        }

        public async Task<ExpenseBook> GetMonthlyExpenseBook(DateTime startDate, DateTime endDate, int id)
        {
            var expenseBook = await this.context.ExpenseBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .FirstOrDefaultAsync();

            return expenseBook;
        }

        public decimal TotalSumStockCostAsync(DateTime startDate, DateTime endDate, int id)
        {
            var stockCost = this.context.Stocks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Sum(x => x.Price);

            return stockCost;
        }
    }
}
