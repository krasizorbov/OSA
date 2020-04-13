namespace OSA.Services.Data.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using OSA.Common;
    using OSA.Data.Models;
    using Xunit;

    public class ExpenseBooksServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private const string StockName = "sugar";
        private IExpenseBooksService iebs;

        [Fact]
        public async Task DeleteAsyncReturnsExpenseBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.DeleteAsync(1);
            Assert.Equal(expenseBook, result);
        }

        [Fact]
        public async Task DeleteAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.DeleteAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task ExpenseBookExistAsyncReturnsExpenseBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.ExpenseBookExistAsync(startDate, endDate, 1);
            Assert.Equal(expenseBook, result);
        }

        [Fact]
        public async Task ExpenseBookExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.ExpenseBookExistAsync(startDate, endDate, 2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetAllProductionInvoicesByMonthAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoice = new ProductionInvoice
            {
                Id = 1,
                CreatedOn = startDate,
                InvoiceNumber = "1",
                ExternalCost = 20.00M,
                Salary = 120.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetAllProductionInvoicesByMonthAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetAllSalesByMonthAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 200.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetAllSalesByMonthAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetExpenseBookByIdAsyncReturnsExpenseBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetExpenseBookByIdAsync(1);
            Assert.Equal(expenseBook, result);
        }

        [Fact]
        public async Task GetExpenseBookByIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetExpenseBookByIdAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetExpenseBooksByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetExpenseBooksByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetMonthlyAvailableStockByCompanyIdAsyncReturnsAvailableStock()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var availableStock = new AvailableStock
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPurchasedAmount = 20.00M,
                TotalPurchasedPrice = 30.00M,
                BookValue = 20.00M,
                AveragePrice = "1.50",
                TotalSoldPrice = 35.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.AvailableStocks.AddAsync(availableStock);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetMonthlyAvailableStockByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal(availableStock, result);
        }

        [Fact]
        public async Task GetMonthlyAvailableStockByCompanyIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var availableStock = new AvailableStock
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                StockName = StockName,
                TotalPurchasedAmount = 20.00M,
                TotalPurchasedPrice = 30.00M,
                BookValue = 20.00M,
                AveragePrice = "1.50",
                TotalSoldPrice = 35.00M,
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.AvailableStocks.AddAsync(availableStock);
            await context.SaveChangesAsync();
            var result = await this.iebs.GetMonthlyAvailableStockByCompanyIdAsync(startDate, endDate, 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            Assert.Equal("1", context.ExpenseBooks.Count().ToString());
        }
    }
}
