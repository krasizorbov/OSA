namespace OSA.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using OSA.Data.Models;
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
    }
}
