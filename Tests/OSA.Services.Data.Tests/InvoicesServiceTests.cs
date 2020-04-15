namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
    using OSA.Web.ViewModels.Invoices.View_Models;
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

        [Fact]

        public async Task AddPartTwoReturnsModelStateErrorWrongDateFormat()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqSupplierService = new Mock<ISuppliersService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var controller = new InvoiceController(moqInvoiceService.Object, moqCompanyService.Object, moqSupplierService.Object, moqDateTimeService.Object);

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
                Date = "01/31/2020",
                TotalAmount = 20,
                SupplierNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Peter Ivanov", } },
            };
            var moqDate = moqDateTimeService.Setup(x => x.IsValidDateTime(invoiceModel.Date)).Returns(false);
            var result = await controller.AddPartTwo(invoiceModel, supplier.CompanyId, invoiceModel.Date);
            var view = controller.View(invoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task AddPartTwoReturnsModelStateErrorInvoiceExists()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqSupplierService = new Mock<ISuppliersService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var controller = new InvoiceController(moqInvoiceService.Object, moqCompanyService.Object, moqSupplierService.Object, moqDateTimeService.Object);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsDeleted = false,
                Name = "Petar Ivanov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            var invoice = new Invoice
            {
                Id = 1,
                CreatedOn = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                InvoiceNumber = "1",
                Date = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                SupplierId = 1,
                CompanyId = 1,
                TotalAmount = 20,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            var moqInvoice = moqInvoiceService.Setup(x => x.InvoiceExistAsync(invoice.InvoiceNumber, invoice.CompanyId))
                .Returns(Task.FromResult(invoice.InvoiceNumber));
            var moqDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var invoiceModel = new CreateInvoiceInputModelTwo
            {
                InvoiceNumber = "1",
                Date = StartDate,
                TotalAmount = 20,
                SupplierNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Peter Ivanov", } },
            };

            await controller.AddPartTwo(invoiceModel, supplier.CompanyId, invoiceModel.Date);
            var view = controller.View(invoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetCompanyReturnsModelStateErrorWrongDateFormat()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqSupplierService = new Mock<ISuppliersService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var controller = new InvoiceController(moqInvoiceService.Object, moqCompanyService.Object, moqSupplierService.Object, moqDateTimeService.Object);

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
            var invoiceModel = new ShowInvoiceByCompanyInputModel
            {
                StartDate = "13/01/2020",
                EndDate = "01/31/2020",
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime(invoiceModel.StartDate)).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime(invoiceModel.EndDate)).Returns(false);
            var result = await controller.GetCompany(invoiceModel, invoiceModel.StartDate, invoiceModel.EndDate);
            var view = controller.View(invoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetInvoiceReturnsCorrectBindingModel()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqSupplierService = new Mock<ISuppliersService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var controller = new InvoiceController(moqInvoiceService.Object, moqCompanyService.Object, moqSupplierService.Object, moqDateTimeService.Object);

            var invoice = new Invoice
            {
                Id = 1,
                CreatedOn = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                InvoiceNumber = "1",
                Date = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                SupplierId = 1,
                CompanyId = 1,
                TotalAmount = 20,
            };

            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            var invoiceModel = new InvoiceBindingViewModel
            {
                Name = "Ivan Petrov",
                Invoices = new List<Invoice> { invoice },
            };

            var result = await controller.GetInvoice(1, "Ivan Petrov", StartDate, EndDate);
            var view = controller.View(invoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.Equal("Ivan Petrov", invoiceModel.Name);
            Assert.Equal("1", invoiceModel.Invoices.Select(x => x.Id).ElementAt(0).ToString());
            Assert.Equal("1/1/2020 12:00:00 AM", invoiceModel.Invoices.Select(x => x.Date).ElementAt(0).ToString());
            Assert.Equal("1", invoiceModel.Invoices.Select(x => x.InvoiceNumber).ElementAt(0).ToString());
            Assert.Equal("1", invoiceModel.Invoices.Select(x => x.SupplierId).ElementAt(0).ToString());
            Assert.Equal("1", invoiceModel.Invoices.Select(x => x.CompanyId).ElementAt(0).ToString());
            Assert.Equal("20", invoiceModel.Invoices.Select(x => x.TotalAmount).ElementAt(0).ToString());
        }

        [Fact]

        public async Task DeleteReturnsCorrectModel()
        {
            var moqInvoiceService = new Mock<IInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqSupplierService = new Mock<ISuppliersService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.iis = new InvoicesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been deleted successfully!";
            var controller = new InvoiceController(moqInvoiceService.Object, moqCompanyService.Object, moqSupplierService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var invoice = new Invoice
            {
                Id = 1,
                CreatedOn = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                InvoiceNumber = "1",
                Date = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                SupplierId = 1,
                CompanyId = 1,
                TotalAmount = 20,
            };
            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            moqInvoiceService.Setup(x => x.GetInvoiceByIdAsync(invoice.Id)).Returns(Task.FromResult(invoice));
            moqInvoiceService.Setup(x => x.DeleteAsync(invoice.Id)).Returns(Task.FromResult(invoice));
            var result = await controller.Delete(invoice.Id);
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }
    }
}
