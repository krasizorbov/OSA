namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using OSA.Data.Models;
    using Xunit;

    public class StocksServiceTests
    {
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
            var startDate = DateTime.ParseExact("01/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact("31/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture);

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
    }
}
