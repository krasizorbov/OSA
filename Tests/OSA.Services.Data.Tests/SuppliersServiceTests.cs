namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using OSA.Data.Models;
    using OSA.Web.Controllers;
    using OSA.Web.ViewModels.Suppliers.Input_Models;
    using Xunit;

    public class SuppliersServiceTests
    {
        private ISuppliersService ss;

        [Fact]
        public async Task GetAllSuppliersByCompanyIdAsync()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Ivan Petrov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var result = await this.ss.GetAllSuppliersByCompanyIdAsync(1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]

        public async Task GetSupplierNameBySupplierIdAsyncReturnsSuppliersName()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Ivan Petrov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var result = await this.ss.GetSupplierNameBySupplierIdAsync(1);
            Assert.Equal("Ivan Petrov", result);
        }

        [Theory]
        [InlineData(null)]

        public async Task GetSupplierNameBySupplierIdAsyncReturnsNull(string name)
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = name,
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var result = await this.ss.GetSupplierNameBySupplierIdAsync(1);
            Assert.True(result == null);
        }

        [Fact]

        public async Task GetSuppliersByCompanyIdAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Ivan Petrov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var result = await this.ss.GetSuppliersByCompanyIdAsync(1);
            Assert.Equal("1", result.Count().ToString());
        }

        [Fact]

        public async Task SupplierExistAsyncReturnsSupplierName()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Ivan Petrov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var result = await this.ss.SupplierExistAsync("Ivan Petrov", 1);
            Assert.Equal("Ivan Petrov", result);
        }

        [Fact]

        public async Task SupplierExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Petar Ivanov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var result = await this.ss.SupplierExistAsync("Ivan Petrov", 1);
            Assert.True(result == null);
        }

        [Fact]

        public async Task AddAsyncReturnsCorrectCount()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);

            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Petar Ivanov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            Assert.Equal("1", context.Suppliers.Count().ToString());
        }

        [Fact]

        public async Task AddAsyncReturnsAnArgumentExeption()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);
            var expectedErrorMessage = "Company with the given ID doesn't exist!";
            var companyId = 123;
            var supplier = new Supplier
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Petar Ivanov",
                Bulstat = "123456789",
                CompanyId = 1,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                this.ss.AddAsync(supplier.Name, supplier.Bulstat, companyId));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]

        public async Task CreateSupplierInputModelReturnsCorrectModel()
        {
            var moqCompany = new Mock<ICompaniesService>();
            var moqSupplier = new Mock<ISuppliersService>();
            var moqUser = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            this.ss = new SuppliersService(context);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var expected = tempData["message"] = "Data has been registered successfully!";
            var controller = new SupplierController(moqSupplier.Object, moqCompany.Object)
            {
                TempData = tempData,
            };
            var userId = Guid.NewGuid().ToString();
            var mogUserId = moqUser.Setup(x => x.GetCurrentUserId()).Returns(userId);
            var company = new Company
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = "Ivan petrov",
                Bulstat = "123456789",
                UserId = userId,
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            var supplier = new CreateSupplierInputModel
            {
                Bulstat = "123456789",
                Name = "Peter Ivanov",
                CompanyId = 1,
                CompanyNames = new List<SelectListItem> { new SelectListItem { Value = "1", Text = "Ivan Petrov", } },
            };

            var result = await controller.Add(supplier);
            var view = controller.View(supplier) as ViewResult;
            var actual = controller.TempData;
            Assert.Equal(expected, actual.Values.ElementAt(0));
            
        }
    }
}
