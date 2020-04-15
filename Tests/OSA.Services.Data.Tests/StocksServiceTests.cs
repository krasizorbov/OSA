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
    using OSA.Web.ViewModels.Stocks.Input_Models;
    using OSA.Web.ViewModels.Stocks.View_Models;
    using Xunit;

    public class StocksServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private IStocksService ss;

        [Fact]
        public async Task DeleteAsyncReturnsCorrectStockCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);

            var stock1 = new Stock
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = DateTime.UtcNow,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock2 = new Stock
            {
                Id = 2,
                CreatedOn = DateTime.UtcNow,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = DateTime.UtcNow,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock3 = new Stock
            {
                Id = 3,
                CreatedOn = DateTime.UtcNow,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = DateTime.UtcNow,
                InvoiceId = 1,
                CompanyId = 1,
            };

            await context.Stocks.AddAsync(stock1);
            await context.Stocks.AddAsync(stock2);
            await context.Stocks.AddAsync(stock3);
            await context.SaveChangesAsync();
            var stockIds = new List<int> { 1, 2, 3 };
            var result = await this.ss.DeleteAsync(stockIds);
            Assert.Equal("3", result.Count().ToString());
        }

        [Fact]
        public async Task GetStockNamesByCompanyIdAsyncReturnsCorrectStockCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);

            var stock1 = new Stock
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = DateTime.UtcNow,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock2 = new Stock
            {
                Id = 2,
                CreatedOn = DateTime.UtcNow,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = DateTime.UtcNow,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock3 = new Stock
            {
                Id = 3,
                CreatedOn = DateTime.UtcNow,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = DateTime.UtcNow,
                InvoiceId = 1,
                CompanyId = 1,
            };

            await context.Stocks.AddAsync(stock1);
            await context.Stocks.AddAsync(stock2);
            await context.Stocks.AddAsync(stock3);
            await context.SaveChangesAsync();
            var result = await this.ss.GetStockNamesByCompanyIdAsync(1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetStocksByCompanyIdAsyncReturnsCorrectStockCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var stock1 = new Stock
            {
                Id = 1,
                CreatedOn = startDate.AddDays(2),
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate.AddDays(2),
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock2 = new Stock
            {
                Id = 2,
                CreatedOn = startDate.AddDays(5),
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate.AddDays(5),
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock3 = new Stock
            {
                Id = 3,
                CreatedOn = startDate.AddDays(7),
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate.AddDays(7),
                InvoiceId = 1,
                CompanyId = 1,
            };

            await context.Stocks.AddAsync(stock1);
            await context.Stocks.AddAsync(stock2);
            await context.Stocks.AddAsync(stock3);
            await context.SaveChangesAsync();
            var result = await this.ss.GetStocksByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("3", result.Count().ToString());
        }

        [Fact]

        public async Task AddPartTwoReturnsCorrectModel()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new StockController(moqStockService.Object, moqCompanyService.Object, moqInvoiceService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var stockModel = new CreateStockInputModelTwo
            {
                Name = "sugar",
                Price = 300.00M,
                Quantity = 200.00M,
                Date = StartDate,
                Invoices = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "1", } },
            };

            var result = await controller.AddPartTwo(stockModel, 1, StartDate);
            var view = controller.View(stockModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddPartTwoReturnsModelStateDateTimeFormatError()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);
            var controller = new StockController(moqStockService.Object, moqCompanyService.Object, moqInvoiceService.Object, moqDateTimeService.Object);
            var stockModel = new CreateStockInputModelTwo
            {
                Name = "sugar",
                Price = 300.00M,
                Quantity = 200.00M,
                Date = StartDate,
                Invoices = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "1", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/31/2020")).Returns(false);
            var result = await controller.AddPartTwo(stockModel, 1, StartDate);
            var view = controller.View(stockModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetCompanyReturnsModelStateDateTimeFormatError()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);
            var controller = new StockController(moqStockService.Object, moqCompanyService.Object, moqInvoiceService.Object, moqDateTimeService.Object);
            var stockModel = new ShowStockByCompanyInputModel
            {
                StartDate = "13/01/2020",
                EndDate = "01/31/2020",
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime(stockModel.StartDate)).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime(stockModel.EndDate)).Returns(false);
            var result = await controller.GetCompany(stockModel, StartDate, EndDate);
            var view = controller.View(stockModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetStockReturnsCorrectBindingModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);
            var controller = new StockController(moqStockService.Object, moqCompanyService.Object, moqInvoiceService.Object, moqDateTimeService.Object);
            var stock = new Stock
            {
                Id = 1,
                CreatedOn = startDate,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };
            await context.Stocks.AddAsync(stock);
            await context.SaveChangesAsync();
            var stockModel = new StockBindingViewModel
            {
                Name = "Ivan Petrov",
                Stocks = new List<Stock> { stock },
            };
            var result = await controller.GetStock(1, "Sugar", StartDate, EndDate);
            var view = controller.View(stockModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.Equal("Ivan Petrov", stockModel.Name);
            Assert.Equal("1", stockModel.Stocks.Select(x => x.Id).ElementAt(0).ToString());
            Assert.Equal("1/1/2020 12:00:00 AM", stockModel.Stocks.Select(x => x.Date).ElementAt(0).ToString());
            Assert.Equal("30.00", stockModel.Stocks.Select(x => x.Price).ElementAt(0).ToString());
            Assert.Equal("20.00", stockModel.Stocks.Select(x => x.Quantity).ElementAt(0).ToString());
            Assert.Equal("1", stockModel.Stocks.Select(x => x.InvoiceId).ElementAt(0).ToString());
            Assert.Equal("1", stockModel.Stocks.Select(x => x.CompanyId).ElementAt(0).ToString());
        }

        [Fact]

        public async Task DeleteReturnsCorrectModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new StocksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been deleted successfully!";
            var controller = new StockController(moqStockService.Object, moqCompanyService.Object, moqInvoiceService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var stock = new Stock
            {
                Id = 1,
                CreatedOn = startDate,
                Name = "Sugar",
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };
            await context.Stocks.AddAsync(stock);
            await context.SaveChangesAsync();
            var listIds = new List<int> { 1 };
            moqStockService.Setup(x => x.DeleteAsync(new List<int> { 1 })).Returns(Task.FromResult(new List<Stock> { stock }));
            var result = await controller.Delete(listIds);
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
