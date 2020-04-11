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
    public class ExpenseBooksServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
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
            var result = await this.iebs.DeleteAsync(2);
            Assert.True(result == null);
        }
    }
}
