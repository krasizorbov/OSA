namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
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
    }
}
