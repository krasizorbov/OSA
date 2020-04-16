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
    using OSA.Web.ViewModels.Sales.Input_Models;
    using OSA.Web.ViewModels.Sales.View_Models;
    using Xunit;

    public class SalesServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private const string StockName = "sugar";
        private ISalesService iss;

        [Fact]

        public async Task DeleteAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sale1 = new Sale
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

            var sale2 = new Sale
            {
                Id = 2,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 200.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale1);
            await context.Sales.AddAsync(sale2);
            await context.SaveChangesAsync();
            var salesIds = new List<int> { 1, 2 };
            var result = await this.iss.DeleteAsync(salesIds);
            Assert.Equal("2", result.Count().ToString());
        }

        [Fact]

        public async Task GetAveragePriceReturnsCorrectAveragePrice()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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
            var result = await this.iss.GetAveragePrice(startDate, StockName, 1);
            Assert.Equal("1.5", result.ToString());
        }

        [Fact]

        public async Task GetAveragePriceReturnsZero()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var purchase = new Purchase
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                StockName = StockName,
                TotalQuantity = 20.00M,
                TotalPrice = 30.00M,
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.Purchases.AddAsync(purchase);
            await context.SaveChangesAsync();
            var result = await this.iss.GetAveragePrice(startDate, StockName, 1);
            Assert.Equal("0", result.ToString());
        }

        [Fact]

        public async Task GetSalesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var sale1 = new Sale
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

            var sale2 = new Sale
            {
                Id = 2,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 200.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale1);
            await context.Sales.AddAsync(sale2);
            await context.SaveChangesAsync();
            var result = await this.iss.GetSalesByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("2", result.Count().ToString());
        }

        [Fact]

        public async Task GetTotalPurchasedQuantityReturnsCorrectPurchasedQuantity()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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
            var result = await this.iss.GetTotalPurchasedQuantity(startDate, StockName, 1);
            Assert.Equal("20.00", result.ToString());
        }

        [Fact]

        public async Task GetTotalPurchasedQuantityReturnsZero()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var purchase = new Purchase
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                StockName = StockName,
                TotalQuantity = 20.00M,
                TotalPrice = 30.00M,
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.Purchases.AddAsync(purchase);
            await context.SaveChangesAsync();
            var result = await this.iss.GetTotalPurchasedQuantity(startDate, StockName, 1);
            Assert.Equal("0", result.ToString());
        }

        [Fact]

        public async Task IsBiggerReturnsTrue()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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

            await context.Purchases.AddAsync(purchase);
            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var result = await this.iss.IsBigger(sale.TotalPrice, sale.ProfitPercent, startDate, StockName, 1);
            Assert.Equal("True", result.ToString());
        }

        [Fact]

        public async Task IsBiggerReturnsFalse()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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

            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Purchases.AddAsync(purchase);
            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var result = await this.iss.IsBigger(sale.TotalPrice, sale.ProfitPercent, startDate, StockName, 1);
            Assert.Equal("False", result.ToString());
        }

        [Fact]

        public async Task SaleExistAsyncReturnsCorrectStockName()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var result = await this.iss.SaleExistAsync(startDate, StockName, 1);
            Assert.Equal(StockName, result);
        }

        [Fact]

        public async Task SaleExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate.AddDays(-10),
                StockName = StockName,
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate.AddDays(-10),
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var result = await this.iss.SaleExistAsync(startDate, StockName, 1);
            Assert.True(result == null);
        }

        [Fact]

        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            Assert.Equal("1", context.Sales.Count().ToString());
        }

        [Fact]

        public async Task AddPartTwoReturnsCorrectModel()
        {
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var saleModel = new CreateSaleInputModelTwo
            {
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                StartDate = StartDate,
                StockNames = new List<string> { "sugar" },
            };

            var result = await controller.AddPartTwo(saleModel, StartDate, StockName, 1);
            var view = controller.View(saleModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddPartTwoReturnsModelStateDateTimeFormatError()
        {
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object);

            var saleModel = new CreateSaleInputModelTwo
            {
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                StartDate = StartDate,
                StockNames = new List<string> { "sugar" },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/31/2020")).Returns(false);
            var result = await controller.AddPartTwo(saleModel, StartDate, StockName, 1);
            var view = controller.View(saleModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task AddPartTwoReturnsSaleExistError()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Monthly sale for the current stock is already done!";
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var saleModel = new CreateSaleInputModelTwo
            {
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                StartDate = StartDate,
                StockNames = new List<string> { "sugar" },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqSaleExist = moqSaleService.Setup(x => x.SaleExistAsync(startDate, "sugar", 1)).Returns(Task.FromResult("sugar"));
            var result = await controller.AddPartTwo(saleModel, StartDate, StockName, 1);
            var view = controller.View(saleModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddPartTwoReturnsSaleIsBiggerError()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Monthly sale for the current stock is already done!";
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };
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
            var saleModel = new CreateSaleInputModelTwo
            {
                TotalPrice = 200.00M,
                ProfitPercent = 120,
                StartDate = StartDate,
                StockNames = new List<string> { "sugar" },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqSaleExist = moqSaleService.Setup(x => x.SaleExistAsync(startDate, string.Empty, 1)).Returns(Task.FromResult("sugar"));
            var moqIsBigger = moqSaleService.Setup(x => x.IsBigger(200.00M, 120, startDate, "sugar", 1)).Returns(Task.FromResult(true));
            var result = await controller.AddPartTwo(saleModel, StartDate, StockName, 1);
            var view = controller.View(saleModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task GetCompanyReturnsModelStateDateTimeFormatError()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object);
            var saleModel = new ShowSaleByCompanyInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime(saleModel.StartDate)).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime(saleModel.EndDate)).Returns(false);
            var result = await controller.GetCompany(saleModel, StartDate, EndDate);
            var view = controller.View(saleModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetStockReturnsCorrectBindingModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object);
            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var saleModel = new SaleBindingViewModel
            {
                Name = "Ivan Petrov",
                Sales = new List<Sale> { sale },
            };
            var result = await controller.GetSale(1, "sugar", StartDate, EndDate);
            var view = controller.View(saleModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.Equal("Ivan Petrov", saleModel.Name);
            Assert.Equal("1", saleModel.Sales.Select(x => x.Id).ElementAt(0).ToString());
            Assert.Equal("1/1/2020 12:00:00 AM", saleModel.Sales.Select(x => x.Date).ElementAt(0).ToString());
            Assert.Equal("20.00", saleModel.Sales.Select(x => x.TotalPrice).ElementAt(0).ToString());
            Assert.Equal("120", saleModel.Sales.Select(x => x.ProfitPercent).ElementAt(0).ToString());
            Assert.Equal("1.5", saleModel.Sales.Select(x => x.AveragePrice).ElementAt(0).ToString());
            Assert.Equal("1", saleModel.Sales.Select(x => x.CompanyId).ElementAt(0).ToString());
        }

        [Fact]

        public async Task DeleteReturnsCorrectModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqSaleService = new Mock<ISalesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqStockService = new Mock<IStocksService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new SalesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been deleted successfully!";
            var controller = new SaleController(moqSaleService.Object, moqCompanyService.Object, moqStockService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };
            var sale = new Sale
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalPrice = 20.00M,
                ProfitPercent = 120,
                AveragePrice = "1.5",
                Date = startDate,
                CompanyId = 1,
            };

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
            var listIds = new List<int> { 1 };
            moqSaleService.Setup(x => x.DeleteAsync(new List<int> { 1 })).Returns(Task.FromResult(new List<Sale> { sale }));
            var result = await controller.Delete(listIds);
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
