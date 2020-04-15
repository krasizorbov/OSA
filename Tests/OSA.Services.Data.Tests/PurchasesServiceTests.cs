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
    using OSA.Web.ViewModels.Purchases.Input_Models;
    using Xunit;

    public class PurchasesServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private const string StockName = "sugar";
        private IPurchasesService ips;

        [Fact]
        public async Task GetAvailableStockForPreviousMonthByCompanyIdAsyncReturnsCorrectRemainingMoney()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.GetAvailableStockForPreviousMonthByCompanyIdAsync(startDate, endDate, StockName, 1);
            Assert.Equal("10.00", result.ToString("F"));
        }

        [Fact]
        public async Task GetAvailableStockForPreviousMonthByCompanyIdAsyncReturnsZero()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.GetAvailableStockForPreviousMonthByCompanyIdAsync(startDate, endDate, StockName, 1);
            Assert.Equal("0.00", result.ToString("F"));
        }

        [Fact]
        public async Task GetStockNamesForCurrentMonthByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var stock1 = new Stock
            {
                Id = 1,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock2 = new Stock
            {
                Id = 2,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock3 = new Stock
            {
                Id = 3,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            await context.Stocks.AddAsync(stock1);
            await context.Stocks.AddAsync(stock2);
            await context.Stocks.AddAsync(stock3);
            await context.SaveChangesAsync();
            var result = await this.ips.GetStockNamesForCurrentMonthByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]

        public async Task GetStockNamesForPrevoiusMonthByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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
            var result = await this.ips.GetStockNamesForPrevoiusMonthByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]

        public async Task QuantityPurchasedAsyncReturnsCorrectTotalQuantity()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

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
            var result = await this.ips.QuantityPurchasedAsync(StockName, startDate, endDate, 1);
            Assert.Equal("20.00", result.ToString());
        }

        [Fact]

        public async Task QuantityPurchasedAsyncReturnsZero()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.QuantityPurchasedAsync(StockName, startDate, endDate, 1);
            Assert.True(result == 0.00M);
        }

        [Fact]

        public async Task QuantitySoldAsyncReturnsCorrectTotalQuantity()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.QuantitySoldAsync(StockName, startDate, endDate, 1);
            Assert.Equal("111.11", result.ToString("F"));
        }

        [Fact]

        public async Task QuantitySoldAsyncReturnsZero()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.QuantitySoldAsync(StockName, startDate, endDate, 1);
            Assert.True(result == 0.00M);
        }

        [Fact]
        public async Task TotalPriceReturnsCorrectSum()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var stock1 = new Stock
            {
                Id = 1,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock2 = new Stock
            {
                Id = 2,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock3 = new Stock
            {
                Id = 3,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            await context.Stocks.AddAsync(stock1);
            await context.Stocks.AddAsync(stock2);
            await context.Stocks.AddAsync(stock3);
            await context.SaveChangesAsync();
            var result = this.ips.TotalPrice(StockName, startDate, endDate, 1);
            Assert.Equal("90.00", result.ToString());
        }

        [Fact]
        public async Task TotalQuantityReturnsCorrectSum()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var stock1 = new Stock
            {
                Id = 1,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock2 = new Stock
            {
                Id = 2,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            var stock3 = new Stock
            {
                Id = 3,
                CreatedOn = startDate,
                Name = StockName,
                Quantity = 20.00M,
                Price = 30.00M,
                Date = startDate,
                InvoiceId = 1,
                CompanyId = 1,
            };

            await context.Stocks.AddAsync(stock1);
            await context.Stocks.AddAsync(stock2);
            await context.Stocks.AddAsync(stock3);
            await context.SaveChangesAsync();
            var result = this.ips.TotalQuantity(StockName, startDate, endDate, 1);
            Assert.Equal("60.00", result.ToString());
        }

        [Fact]

        public async Task PurchaseExistAsyncReturnsCorrectStockName()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.PurchaseExistAsync(startDate, endDate, 1);
            Assert.Equal(StockName, result[0]);
        }

        [Fact]

        public async Task PurchaseExistAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.PurchaseExistAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]

        public async Task GetPurchasesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            var result = await this.ips.GetPurchasesByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]

        public async Task DeleteAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var purchase1 = new Purchase
            {
                Id = 1,
                CreatedOn = startDate,
                StockName = StockName,
                TotalQuantity = 20.00M,
                TotalPrice = 30.00M,
                Date = startDate,
                CompanyId = 1,
            };

            var purchase2 = new Purchase
            {
                Id = 2,
                CreatedOn = startDate,
                StockName = StockName,
                TotalQuantity = 20.00M,
                TotalPrice = 30.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.Purchases.AddAsync(purchase1);
            await context.Purchases.AddAsync(purchase2);
            await context.SaveChangesAsync();
            var purchaseIds = new List<int> { 1, 2 };
            var result = await this.ips.DeleteAsync(purchaseIds);
            Assert.Equal("2", result.Count().ToString());
        }

        [Fact]

        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
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
            Assert.Equal("1", context.Purchases.Count().ToString());
        }

        [Fact]

        public async Task AddReturnsCorrectModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqPurchaseService = new Mock<IPurchasesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ips = new PurchasesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new PurchaseController(moqPurchaseService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var purchase = new CreatePurchaseInputModel
            {
                StartDate = StartDate,
                EndDate = EndDate,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };

            var result = await controller.Add(purchase, StartDate, EndDate);
            var view = controller.View(purchase) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
