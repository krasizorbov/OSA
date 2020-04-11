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

    public class ExpenseBooksService : IExpenseBooksService
    {
        private readonly ApplicationDbContext context;

        public ExpenseBooksService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoices = await this.GetAllProductionInvoicesByMonthAsync(start_Date, end_Date, companyId);
            var sales = await this.GetAllSalesByMonthAsync(start_Date, end_Date, companyId);

            var expenseBook = new ExpenseBook
            {
                TotalExternalCost = productionInvoices.Sum(x => x.ExternalCost),
                TotalSalaryCost = productionInvoices.Sum(x => x.Salary),
                TotalBookValue = sales.Sum(x => x.BookValue),
                Profit = sales.Sum(x => x.TotalPrice),
                Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };
            await this.context.AddAsync(expenseBook);
            await this.context.SaveChangesAsync();
        }

        public async Task<ExpenseBook> DeleteAsync(int id)
        {
            var cashBook = await this.context.ExpenseBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (cashBook != null)
            {
                cashBook.IsDeleted = true;
                cashBook.DeletedOn = DateTime.UtcNow;
                await this.context.SaveChangesAsync();
            }

            return cashBook;
        }

        public async Task<ExpenseBook> ExpenseBookExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var expenseBook = await this.context.ExpenseBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .Select(x => x)
                .FirstOrDefaultAsync();

            return expenseBook;
        }

        public async Task<List<ProductionInvoice>> GetAllProductionInvoicesByMonthAsync(DateTime startDate, DateTime endDate, int id)
        {
            var productionInvoices = await this.context.ProductionInvoices
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
                .Select(x => x)
                .ToListAsync();

            return productionInvoices;
        }

        public async Task<List<Sale>> GetAllSalesByMonthAsync(DateTime startDate, DateTime endDate, int id)
        {
            var sales = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
                .Select(x => x)
                .ToListAsync();

            return sales;
        }

        public async Task<ExpenseBook> GetExpenseBookByIdAsync(int id)
        {
            var expenseBook = await this.context.ExpenseBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            return expenseBook;
        }

        public async Task<IEnumerable<ExpenseBook>> GetExpenseBooksByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var expenseBooks = await this.context.ExpenseBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return expenseBooks;
        }

        public async Task<AvailableStock> GetMonthlyAvailableStockByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var availableStock = await this.context.AvailableStocks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return availableStock;
        }
    }
}
