namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Common;
    using OSA.Data;
    using OSA.Data.Models;

    public class CashBooksService : ICashBooksService
    {
        private readonly ApplicationDbContext context;

        public CashBooksService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, decimal saldo, decimal ownFunds, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var totalStockCost = this.TotalSumStockCost(start_Date, end_Date, companyId);
            var expenseBook = await this.GetMonthlyExpenseBook(start_Date, end_Date, companyId);

            var cashBook = new CashBook
            {
                TotalInvoicePricesCost = totalStockCost - saldo,
                TotalSalaryCost = expenseBook.TotalSalaryCost,
                TotalStockExternalCost = expenseBook.TotalExternalCost,
                TotalProfit = expenseBook.Profit,
                Saldo = saldo,
                OwnFunds = ownFunds,
                Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };
            await this.context.AddAsync(cashBook);
            await this.context.SaveChangesAsync();
        }

        public async Task<CashBook> CashBookExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var cashBook = await this.context.CashBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .Select(x => x)
                .FirstOrDefaultAsync();

            return cashBook;
        }

        public async Task<CashBook> DeleteAsync(int id)
        {
            var cashBook = await this.context.CashBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (cashBook != null)
            {
                cashBook.IsDeleted = true;
                cashBook.DeletedOn = DateTime.UtcNow;
                await this.context.SaveChangesAsync();
            }

            return cashBook;
        }

        public async Task<CashBook> GetCashBookByIdAsync(int id)
        {
            var cashBook = await this.context.CashBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            return cashBook;
        }

        public async Task<IEnumerable<CashBook>> GetCashBooksByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var cashBooks = await this.context.CashBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return cashBooks;
        }

        public async Task<ExpenseBook> GetMonthlyExpenseBook(DateTime startDate, DateTime endDate, int id)
        {
            var expenseBook = await this.context.ExpenseBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
                .Select(x => x)
                .FirstOrDefaultAsync();

            return expenseBook;
        }

        public decimal TotalSumStockCost(DateTime startDate, DateTime endDate, int id)
        {
            var stockCost = this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
                .Sum(x => x.TotalPrice);

            return stockCost;
        }
    }
}
