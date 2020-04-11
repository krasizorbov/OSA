using OSA.Common;
using OSA.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OSA.Services.Data.Tests
{
    public class CashBooksServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
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
            var result = await this.icbs.DeleteAsync(1);
            Assert.Equal(cashBook, result);
        }

        [Fact]
        public async Task CDeleteAsyncReturnsNull()
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
            var result = await this.icbs.DeleteAsync(2);
            Assert.True(result == null);
        }
    }
}
