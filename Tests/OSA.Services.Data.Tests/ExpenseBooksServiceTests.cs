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
    using OSA.Web.ViewModels.ExpenseBooks.Input_Models;
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

        [Fact]

        public async Task AddReturnsCorrectModel()
        {
            var moqExpenseBookService = new Mock<IExpenseBooksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new ExpenseBookController(moqExpenseBookService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var expenseBookModel = new CreateExpenseBookInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };

            var result = await controller.Add(expenseBookModel, StartDate, EndDate);
            var view = controller.View(expenseBookModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddReturnsModelStateDateTimeFormatError()
        {
            var moqExpenseBookService = new Mock<IExpenseBooksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var controller = new ExpenseBookController(moqExpenseBookService.Object, moqCompanyService.Object, moqDateTimeService.Object);

            var expenseBookModel = new CreateExpenseBookInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/01/2020")).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/31/2020")).Returns(false);
            var result = await controller.Add(expenseBookModel, StartDate, EndDate);
            var view = controller.View(expenseBookModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task AddReturnsExpenseBookErrorMessage()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqExpenseBookService = new Mock<IExpenseBooksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Expense book for the month already done!";
            var controller = new ExpenseBookController(moqExpenseBookService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };
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
            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var expenseBookModel = new CreateExpenseBookInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("31/01/2020")).Returns(true);
            moqExpenseBookService.Setup(x => x.ExpenseBookExistAsync(startDate, endDate, 1)).Returns(Task.FromResult(expenseBook));
            moqExpenseBookService.Setup(x => x.GetAllProductionInvoicesByMonthAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<ProductionInvoice> { productionInvoice }));
            moqExpenseBookService.Setup(x => x.GetMonthlyAvailableStockByCompanyIdAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(availableStock));
            var result = await controller.Add(expenseBookModel, StartDate, EndDate);
            var view = controller.View(expenseBookModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddReturnsProductionInvoiceErrorMessage()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqExpenseBookService = new Mock<IExpenseBooksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Please register a production invoice before proceeding!";
            var controller = new ExpenseBookController(moqExpenseBookService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };
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
            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var expenseBookModel = new CreateExpenseBookInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("31/01/2020")).Returns(true);
            moqExpenseBookService.Setup(x => x.ExpenseBookExistAsync(startDate, endDate, 2)).Returns(Task.FromResult(expenseBook));
            moqExpenseBookService.Setup(x => x.GetAllProductionInvoicesByMonthAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<ProductionInvoice>()));
            moqExpenseBookService.Setup(x => x.GetMonthlyAvailableStockByCompanyIdAsync(startDate, endDate, 2))
                .Returns(Task.FromResult(availableStock));
            var result = await controller.Add(expenseBookModel, StartDate, EndDate);
            var view = controller.View(expenseBookModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddReturnsAvailableStockErrorMessage()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqExpenseBookService = new Mock<IExpenseBooksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iebs = new ExpenseBooksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "There is no Monthly Available Stock! Please register!";
            var controller = new ExpenseBookController(moqExpenseBookService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };
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
            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.ExpenseBooks.AddAsync(expenseBook);
            await context.SaveChangesAsync();
            var expenseBookModel = new CreateExpenseBookInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("31/01/2020")).Returns(true);
            moqExpenseBookService.Setup(x => x.ExpenseBookExistAsync(startDate, endDate, 2)).Returns(Task.FromResult(expenseBook));
            moqExpenseBookService.Setup(x => x.GetAllProductionInvoicesByMonthAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<ProductionInvoice> { productionInvoice }));
            moqExpenseBookService.Setup(x => x.GetMonthlyAvailableStockByCompanyIdAsync(startDate, endDate, 2))
                .Returns(Task.FromResult(availableStock));
            var result = await controller.Add(expenseBookModel, StartDate, EndDate);
            var view = controller.View(expenseBookModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
