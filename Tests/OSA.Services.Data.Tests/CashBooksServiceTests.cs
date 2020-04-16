namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using OSA.Common;
    using OSA.Data.Models;
    using OSA.Services.Data.Interfaces;
    using OSA.Web.Controllers;
    using OSA.Web.ViewModels.CashBooks.Input_Models;
    using Xunit;

    public class CashBooksServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private const string StockName = "sugar";
        private ICashBooksService icbs;

        [Fact]
        public async Task CashBookExistAsyncReturnsCashBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.CashBookExistAsync(startDate, endDate, 1);
            Assert.Equal(cashBook, result);
        }

        [Fact]
        public async Task CashBookExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.CashBookExistAsync(startDate, endDate, 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task DeleteAsyncReturnsCashBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.DeleteAsync(1);
            Assert.Equal(cashBook, result);
        }

        [Fact]
        public async Task DeleteAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.DeleteAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetCashBookByIdAsyncReturnsCashBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.GetCashBookByIdAsync(1);
            Assert.Equal(cashBook, result);
        }

        [Fact]
        public async Task GetCashBookByIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.GetCashBookByIdAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetCashBooksByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.GetCashBooksByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetMonthlyExpenseBookReturnsExpenseBook()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
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
            var result = await this.icbs.GetMonthlyExpenseBook(startDate, endDate, 1);
            Assert.Equal(expenseBook, result);
        }

        [Fact]
        public async Task GetMonthlyExpenseBookReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBook = new ExpenseBook
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                TotalExternalCost = 20.00M,
                TotalSalaryCost = 20.00M,
                TotalBookValue = 20.00M,
                Profit = 100.00M,
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var result = await this.icbs.GetMonthlyExpenseBook(startDate, endDate, 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task TotalSumStockCostReturnsCorrectSum()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var purchase = new Purchase
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalQuantity = 20.00M,
                TotalPrice = 30.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.Purchases.AddAsync(purchase);
            await context.SaveChangesAsync();
            var result = this.icbs.TotalSumStockCost(startDate, endDate, 1);
            Assert.Equal("30.00", result.ToString());
        }

        [Fact]
        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBook = new CashBook
            {
                Id = 1,
                CreatedOn = startDate,
                TotalInvoicePricesCost = 200.00M,
                TotalSalaryCost = 200.00M,
                TotalStockExternalCost = 50.00M,
                TotalProfit = 100.00M,
                Saldo = 100.00M,
                OwnFunds = 0.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.CashBooks.AddAsync(cashBook);
            await context.SaveChangesAsync();
            Assert.Equal("1", context.CashBooks.Count().ToString());
        }

        [Fact]

        public async Task AddPartTwoReturnsCorrectModel()
        {
            var moqCashBookService = new Mock<ICashBooksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.icbs = new CashBooksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new CashBookController(moqCashBookService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var cashBookModel = new CreateCashBookInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                Saldo = 20.00M,
                OwnFunds = 20.00M,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };

            var result = await controller.Add(cashBookModel, StartDate, EndDate);
            var view = controller.View(cashBookModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
