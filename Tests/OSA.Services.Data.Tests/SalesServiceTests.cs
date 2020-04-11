namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using OSA.Common;
    using OSA.Data.Models;
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
    }
}
