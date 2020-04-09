namespace OSA.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using OSA.Data.Models;
    using Xunit;

    public class InvoicesServiceTests
    {
        private IInvoicesService iss;

        [Fact]
        public async Task DeleteAsyncReturnsInvoice()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new InvoicesService(context);

            var invoice = new Invoice
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                InvoiceNumber = "1",
                Date = DateTime.UtcNow,
                SupplierId = 1,
                CompanyId = 1,
                TotalAmount = 20,
            };

            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            var result = await this.iss.DeleteAsync(1);
            Assert.Equal(invoice, result);
        }

        [Fact]
        public async Task DeleteAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iss = new InvoicesService(context);

            var invoice = new Invoice
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                InvoiceNumber = "1",
                Date = DateTime.UtcNow,
                SupplierId = 1,
                CompanyId = 1,
                TotalAmount = 20,
            };

            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            var result = await this.iss.DeleteAsync(2);
            Assert.True(result == null);
        }
    }
}
