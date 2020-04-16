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
    using OSA.Web.ViewModels.AvailableStocks.Input_Models;
    using OSA.Web.ViewModels.AvailableStocks.View_Models;
    using Xunit;

    public class AvailableStocksServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private const string StockName = "sugar";
        private IAvailableStocksService iass;

        [Fact]
        public async Task AvailableStockExistAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.AvailableStockExistAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task DeleteAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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
            var availableStocksIds = new List<int> { 1 };
            var result = await this.iass.DeleteAsync(availableStocksIds);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetAvailableStockForPreviousMonthByCompanyIdAsyncReturnsAvailableStock()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetAvailableStockForPreviousMonthByCompanyIdAsync(startDate, endDate, StockName, 1);
            Assert.Equal(availableStock, result);
        }

        [Fact]
        public async Task GetAvailableStockForPreviousMonthByCompanyIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetAvailableStockForPreviousMonthByCompanyIdAsync(startDate, endDate, StockName, 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetAvailableStocksForCurrentMonthByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetAvailableStocksForCurrentMonthByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetAvailableStocksForPreviousMonthByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetAvailableStocksForPreviousMonthByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetCurrentBookValueForStockNameAsyncReturnsCorrectValue()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetCurrentBookValueForStockNameAsync(startDate, endDate, StockName, 1);
            Assert.Equal("166.67", result.ToString("F"));
        }

        [Fact]
        public async Task GetCurrentPurchasedStockNameAsyncReturnsCorrectPurchase()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetCurrentPurchasedStockNameAsync(startDate, endDate, StockName, 1);
            Assert.Equal(purchase, result);
        }

        [Fact]
        public async Task GetCurrentSoldStockNameAsyncReturnsCorrectSale()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetCurrentSoldStockNameAsync(startDate, endDate, StockName, 1);
            Assert.Equal(sale, result);
        }

        [Fact]
        public async Task GetCurrentSoldStockNameAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                StockName = StockName,
                TotalPrice = 200.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var result = await this.iass.GetCurrentSoldStockNameAsync(startDate, endDate, StockName, 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetPurchasedStockNamesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetPurchasedStockNamesByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetSoldStockNamesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
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
            var result = await this.iass.GetSoldStockNamesByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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
            Assert.Equal("1", context.AvailableStocks.Count().ToString());
        }

        [Fact]

        public async Task AddReturnsCorrectModel()
        {
            var moqAvailableStockService = new Mock<IAvailableStocksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new AvailableStockController(moqAvailableStockService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var availableStockModel = new CreateAvailableStockInputModel
            {
                StockName = StockName,
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };

            var result = await controller.Add(availableStockModel, StartDate, EndDate);
            var view = controller.View(availableStockModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddReturnsModelStateDateTimeFormatError()
        {
            var moqAvailableStockService = new Mock<IAvailableStocksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var controller = new AvailableStockController(moqAvailableStockService.Object, moqCompanyService.Object, moqDateTimeService.Object);

            var availableStockModel = new CreateAvailableStockInputModel
            {
                StockName = StockName,
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/01/2020")).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/31/2020")).Returns(false);
            var result = await controller.Add(availableStockModel, StartDate, EndDate);
            var view = controller.View(availableStockModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task AddReturnsAvailableStockExistError()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqAvailableStockService = new Mock<IAvailableStocksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Available stock for the month already done!";
            var controller = new AvailableStockController(moqAvailableStockService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var availableStockModel = new CreateAvailableStockInputModel
            {
                StockName = StockName,
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("31/01/2020")).Returns(true);
            var moqStockNames = moqAvailableStockService.Setup(x => x.AvailableStockExistAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<string> { StockName }));
            var moqPurchased = moqAvailableStockService.Setup(x => x.GetPurchasedStockNamesByCompanyIdAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<string> { StockName }));
            var moqSold = moqAvailableStockService.Setup(x => x.GetSoldStockNamesByCompanyIdAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<string> { StockName }));
            var result = await controller.Add(availableStockModel, StartDate, EndDate);
            var view = controller.View(availableStockModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddReturnsAvailableStockNoSaleNoPurchaseError()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqAvailableStockService = new Mock<IAvailableStocksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Register a sale and (or) a purchase please!";
            var controller = new AvailableStockController(moqAvailableStockService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var availableStockModel = new CreateAvailableStockInputModel
            {
                StockName = StockName,
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime("31/01/2020")).Returns(true);
            var moqStockNames = moqAvailableStockService.Setup(x => x.AvailableStockExistAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<string>()));
            var moqPurchased = moqAvailableStockService.Setup(x => x.GetPurchasedStockNamesByCompanyIdAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<string>()));
            var moqSold = moqAvailableStockService.Setup(x => x.GetSoldStockNamesByCompanyIdAsync(startDate, endDate, 1))
                .Returns(Task.FromResult(new List<string>()));
            var result = await controller.Add(availableStockModel, StartDate, EndDate);
            var view = controller.View(availableStockModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task GetCompanyReturnsModelStateDateTimeFormatError()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqAvailableStockService = new Mock<IAvailableStocksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var controller = new AvailableStockController(moqAvailableStockService.Object, moqCompanyService.Object, moqDateTimeService.Object);

            var availableStockModel = new ShowAvailableStockByCompanyInputModel
            {
                StartDate = "13/01/2020",
                EndDate = "01/31/2020",
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime(availableStockModel.StartDate)).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime(availableStockModel.EndDate)).Returns(false);
 
            var result = await controller.GetCompany(availableStockModel, StartDate, EndDate);
            var view = controller.View(availableStockModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetAvailableStockReturnsCorrectBindingModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqAvailableStockService = new Mock<IAvailableStocksService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iass = new AvailableStocksService(context);
            var controller = new AvailableStockController(moqAvailableStockService.Object, moqCompanyService.Object, moqDateTimeService.Object);

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

            var availableStockModel = new AvailableStockBindingViewModel
            {
                Name = "Ivan Petrov",
                AvailableStocks = new List<AvailableStock> { availableStock },
            };
            var result = await controller.GetAvailableStock(1, StockName, StartDate, EndDate);
            var view = controller.View(availableStock) as ViewResult;
            var actual = controller.ModelState;
            Assert.Equal("Ivan Petrov", availableStockModel.Name);
            Assert.Equal("1", availableStockModel.AvailableStocks.Select(x => x.Id).ElementAt(0).ToString());
            Assert.Equal("1/1/2020 12:00:00 AM", availableStockModel.AvailableStocks.Select(x => x.Date).ElementAt(0).ToString());
            Assert.Equal("30.00", availableStockModel.AvailableStocks.Select(x => x.TotalPurchasedPrice).ElementAt(0).ToString());
            Assert.Equal("20.00", availableStockModel.AvailableStocks.Select(x => x.TotalPurchasedAmount).ElementAt(0).ToString());
            Assert.Equal("1.50", availableStockModel.AvailableStocks.Select(x => x.AveragePrice).ElementAt(0).ToString());
            Assert.Equal("1", availableStockModel.AvailableStocks.Select(x => x.CompanyId).ElementAt(0).ToString());
        }
    }
}
