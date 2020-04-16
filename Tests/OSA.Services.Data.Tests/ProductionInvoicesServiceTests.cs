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
    using OSA.Web.ViewModels.ProductionInvoices.Input_Models;
    using OSA.Web.ViewModels.ProductionInvoices.View_Models;
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

        [Fact]

        public async Task AddPartTwoReturnsCorrectModel()
        {
            var moqProductionInvoiceService = new Mock<IProductionInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new ProductionInvoiceController(moqProductionInvoiceService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var productionInvoiceModel = new CreateProductionInvoiceInputModel
            {
                InvoiceNumber = "1234",
                ExternalCost = 200.00M,
                Salary = 600.00M,
                Date = StartDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };

            var result = await controller.Add(productionInvoiceModel, StartDate);
            var view = controller.View(productionInvoiceModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task AddReturnsModelStateDateTimeFormatError()
        {
            var moqProductionInvoiceService = new Mock<IProductionInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var controller = new ProductionInvoiceController(moqProductionInvoiceService.Object, moqCompanyService.Object, moqDateTimeService.Object);

            var productionInvoiceModel = new CreateProductionInvoiceInputModel
            {
                InvoiceNumber = "1234",
                ExternalCost = 200.00M,
                Salary = 600.00M,
                Date = StartDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("13/31/2020")).Returns(false);
            var result = await controller.Add(productionInvoiceModel, StartDate);
            var view = controller.View(productionInvoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task AddReturnsproductionInvoiceExistError()
        {
            var moqProductionInvoiceService = new Mock<IProductionInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "1234  already exists! Please enter a new invoice number.";
            var controller = new ProductionInvoiceController(moqProductionInvoiceService.Object, moqCompanyService.Object, moqDateTimeService.Object)
            {
                TempData = tempData,
            };

            var productionInvoiceModel = new CreateProductionInvoiceInputModel
            {
                InvoiceNumber = "1234",
                ExternalCost = 200.00M,
                Salary = 600.00M,
                Date = StartDate,
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime("01/01/2020")).Returns(true);
            var moqInvoiceExists = moqProductionInvoiceService.Setup(x => x.InvoiceExistAsync("1234", 1)).Returns(Task.FromResult("1234"));
            var result = await controller.Add(productionInvoiceModel, StartDate);
            var view = controller.View(productionInvoiceModel) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
        }

        [Fact]

        public async Task GetCompanyReturnsModelStateDateTimeFormatError()
        {
            var moqProductionInvoiceService = new Mock<IProductionInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "1234  already exists! Please enter a new invoice number.";
            var controller = new ProductionInvoiceController(moqProductionInvoiceService.Object, moqCompanyService.Object, moqDateTimeService.Object);
            var productionInvoiceModel = new ShowProductionInvoiceByCompanyInputModel
            {
                StartDate = "13/01/2020",
                EndDate = "01/31/2020",
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };
            var moqStartDate = moqDateTimeService.Setup(x => x.IsValidDateTime(productionInvoiceModel.StartDate)).Returns(false);
            var moqEndDate = moqDateTimeService.Setup(x => x.IsValidDateTime(productionInvoiceModel.EndDate)).Returns(false);
            var result = await controller.GetCompany(productionInvoiceModel, StartDate, EndDate);
            var view = controller.View(productionInvoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.True(actual.IsValid == false);
        }

        [Fact]

        public async Task GetStockReturnsCorrectBindingModel()
        {
            var startDate = DateTime.ParseExact(StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(EndDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var moqProductionInvoiceService = new Mock<IProductionInvoicesService>();
            var moqCompanyService = new Mock<ICompaniesService>();
            var moqDateTimeService = new Mock<IDateTimeValidationService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ipis = new ProductionInvoicesService(context);
            var controller = new ProductionInvoiceController(moqProductionInvoiceService.Object, moqCompanyService.Object, moqDateTimeService.Object);
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

            var productionInvoiceModel = new ProductionInvoiceBindingViewModel
            {
                Name = "Ivan Petrov",
                ProductionInvoices = new List<ProductionInvoice> { productionInvoice },
            };
            var result = await controller.GetProductionInvoice(1, "Sugar", StartDate, EndDate);
            var view = controller.View(productionInvoiceModel) as ViewResult;
            var actual = controller.ModelState;
            Assert.Equal("Ivan Petrov", productionInvoiceModel.Name);
            Assert.Equal("1", productionInvoiceModel.ProductionInvoices.Select(x => x.Id).ElementAt(0).ToString());
            Assert.Equal("1/1/2020 12:00:00 AM", productionInvoiceModel.ProductionInvoices.Select(x => x.Date).ElementAt(0).ToString());
            Assert.Equal("1", productionInvoiceModel.ProductionInvoices.Select(x => x.InvoiceNumber).ElementAt(0).ToString());
            Assert.Equal("20.00", productionInvoiceModel.ProductionInvoices.Select(x => x.ExternalCost).ElementAt(0).ToString());
            Assert.Equal("120.00", productionInvoiceModel.ProductionInvoices.Select(x => x.Salary).ElementAt(0).ToString());
            Assert.Equal("1", productionInvoiceModel.ProductionInvoices.Select(x => x.CompanyId).ElementAt(0).ToString());
        }
    }
}
