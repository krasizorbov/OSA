namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class ExpenseBooksService : IExpenseBooksService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<ExpenseBook> expenseBooksRepository;
        private readonly ApplicationDbContext context;

        public ExpenseBooksService(IDeletableEntityRepository<ExpenseBook> expenseBooksRepository, ApplicationDbContext context)
        {
            this.expenseBooksRepository = expenseBooksRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);

            var productionInvoices = await this.GetAllProductionInvoicesByMonthAsync(start_Date, end_Date, companyId);
            var receipts = await this.GetAllReceiptsByMonthAsync(start_Date, end_Date, companyId);
            var sales = await this.GetAllSalesByMonthAsync(start_Date, end_Date, companyId);
            var bookValues = await this.GetAllBookValuesByMonthAsync(start_Date, end_Date, companyId);

            var expenseBook = new ExpenseBook
            {
                TotalStockCost = productionInvoices.Sum(x => x.StockCost),
                TotalExternalCost = productionInvoices.Sum(x => x.ExternalCost),
                TotalSalaryCost = receipts.Sum(x => x.Salary),
                TotalBookValue = bookValues.Sum(x => x.Price),
                Profit = sales.Sum(x => x.TotalPrice),
                Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };
            await this.expenseBooksRepository.AddAsync(expenseBook);
            await this.expenseBooksRepository.SaveChangesAsync();
        }

        public async Task<ExpenseBook> ExpenseBookExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var expenseBook = await this.context.ExpenseBooks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId)
                .Select(x => x)
                .FirstOrDefaultAsync();

            return expenseBook;
        }

        public async Task<List<BookValue>> GetAllBookValuesByMonthAsync(DateTime startDate, DateTime endDate, int id)
        {
            var bookValues = await this.context.BookValues
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .ToListAsync();

            return bookValues;
        }

        public async Task<List<ProductionInvoice>> GetAllProductionInvoicesByMonthAsync(DateTime startDate, DateTime endDate, int id)
        {
            var productionInvoices = await this.context.ProductionInvoices
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .ToListAsync();

            return productionInvoices;
        }

        public async Task<List<Receipt>> GetAllReceiptsByMonthAsync(DateTime startDate, DateTime endDate, int id)
        {
            var receipts = await this.context.Receipts
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .ToListAsync();

            return receipts;
        }

        public async Task<List<Sale>> GetAllSalesByMonthAsync(DateTime startDate, DateTime endDate, int id)
        {
            var sells = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .ToListAsync();

            return sells;
        }
    }
}
