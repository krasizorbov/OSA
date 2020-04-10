namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Common;
    using OSA.Data.Models;
    using Xunit;

    public class PurchasesServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private const string StockName = "sugar";
        private List<string> stockNamesForCurrentMonth;
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
                Name = "Sugar",
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
                Name = "Sugar",
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
                Name = "Sugar",
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
    }
}
