namespace OSA.Services.Data.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using OSA.Common;
    using OSA.Data.Models;
    using Xunit;

    public class ProductionInvoicesServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private IProductionInvoicesService ipis;

        [Fact]
        public async Task GetProductionInvoicesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoice = new ProductionInvoice
            {
                Id = 1,
                CreatedOn = startDate,
                InvoiceNumber = "1",
                ExternalCost = 20.00M,
                Salary = 120.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.SaveChangesAsync();
            var result = await this.ipis.GetProductionInvoicesByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task InvoiceExistAsyncReturnsProductionInvoice()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoice = new ProductionInvoice
            {
                Id = 1,
                CreatedOn = startDate,
                InvoiceNumber = "1",
                ExternalCost = 20.00M,
                Salary = 120.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.SaveChangesAsync();
            var result = await this.ipis.InvoiceExistAsync(productionInvoice.InvoiceNumber, 1);
            Assert.Equal(productionInvoice.InvoiceNumber, result);
        }

        [Fact]
        public async Task InvoiceExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoice = new ProductionInvoice
            {
                Id = 1,
                CreatedOn = startDate,
                InvoiceNumber = "1",
                ExternalCost = 20.00M,
                Salary = 120.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.SaveChangesAsync();
            var result = await this.ipis.InvoiceExistAsync("2", 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoice = new ProductionInvoice
            {
                Id = 1,
                CreatedOn = startDate,
                InvoiceNumber = "1",
                ExternalCost = 20.00M,
                Salary = 120.00M,
                Date = startDate,
                CompanyId = 1,
            };

            await context.ProductionInvoices.AddAsync(productionInvoice);
            await context.SaveChangesAsync();
            Assert.Equal("1", context.ProductionInvoices.Count().ToString());
        }
    }
}
