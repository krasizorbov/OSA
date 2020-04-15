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
    using OSA.Web.ViewModels.Invoices.Input_Models;
    using Xunit;

    public class InvoicesServiceTests
    {
        private const string StartDate = "01/01/2020";
        private const string EndDate = "31/01/2020";
        private IInvoicesService iis;

        [Fact]
        public async Task DeleteAsyncReturnsInvoice()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.DeleteAsync(1);
            Assert.Equal(invoice, result);
        }

        [Fact]
        public async Task DeleteAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.DeleteAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetAllInvoicesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.GetAllInvoicesByCompanyIdAsync(1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task GetInvoiceByIdAsyncReturnsInvoice()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.GetInvoiceByIdAsync(1);
            Assert.Equal(invoice, result);
        }

        [Fact]
        public async Task GetInvoiceByIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.GetInvoiceByIdAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetInvoiceNumberByInvoiceIdAsyncReturnsInvoiceNumber()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.GetInvoiceNumberByInvoiceIdAsync(1);
            Assert.Equal(invoice.InvoiceNumber, result);
        }

        [Fact]
        public async Task GetInvoiceNumberByInvoiceIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.GetInvoiceNumberByInvoiceIdAsync(2);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetInvoicesByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var invoice = new Invoice
            {
                Id = 1,
                CreatedOn = startDate.AddDays(2),
                InvoiceNumber = "1",
                Date = startDate.AddDays(2),
                SupplierId = 1,
                CompanyId = 1,
                TotalAmount = 20,
            };

            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            var result = await this.iis.GetInvoicesByCompanyIdAsync(startDate, endDate, 1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]
        public async Task InvoiceExistAsyncReturnsInvoiceNumber()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.InvoiceExistAsync("1", 1);
            Assert.Equal(invoice.InvoiceNumber, result.ToString());
        }

        [Fact]
        public async Task InvoiceExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            var result = await this.iis.InvoiceExistAsync("2", 1);
            Assert.True(result == null);
        }

        [Fact]
        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);

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
            Assert.Equal("1", context.Invoices.Count().ToString());
        }

        [Fact]

        public async Task AddPartTwoReturnsCorrectModel()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqSupplierService = new Mock<ISuppliersService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new InvoiceController(moqInvoiceService.Object, moqCompanyService.Object, moqSupplierService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsDeleted = false,
                Name = "Petar Ivanov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var invoiceModel = new CreateInvoiceInputModelTwo
            {
                InvoiceNumber = "123456789",
                Date = StartDate,
                TotalAmount = 20,
                SupplierNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Peter Ivanov", } },
            };

            var result = await controller.AddPartTwo(invoiceModel, supplier.CompanyId, "01/01/2020");
            var view = controller.View(invoiceModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
