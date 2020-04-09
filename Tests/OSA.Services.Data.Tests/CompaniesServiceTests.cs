namespace OSA.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;
    using Xunit;

    public class CompaniesServiceTests
    {
        private readonly IUsersService us;
        private ICompaniesService cs;

        [Fact]
        public async Task CompanyExistAsyncReturnsCompanyName()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.cs = new CompaniesService(this.us, context);
            var userId = Guid.NewGuid().ToString();
            var name = "ET Oazis";
            var company = new Company
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = name,
                Bulstat = "123456789",
                UserId = userId,
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            var result = await this.cs.CompanyExistAsync(name, userId);
            Assert.Equal(name, result);
        }

        [Fact]
        public async Task CompanyExistAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.cs = new CompaniesService(this.us, context);
            var userId = Guid.NewGuid().ToString();
            var name = "ET Oazis";
            var company = new Company
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = name,
                Bulstat = "123456789",
                UserId = userId,
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            var result = await this.cs.CompanyExistAsync("Krasi 12", userId);
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetCompanyNameByIdAsyncReturnsCompanyName()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.cs = new CompaniesService(this.us, context);
            var userId = Guid.NewGuid().ToString();
            var name = "ET Oazis";
            var company = new Company
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = name,
                Bulstat = "123456789",
                UserId = userId,
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            var result = await this.cs.GetCompanyNameByIdAsync(1);
            Assert.Equal(name, result);
        }

        [Fact]
        public async Task GetCompanyNameByIdAsyncReturnsNull()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.cs = new CompaniesService(this.us, context);
            var userId = Guid.NewGuid().ToString();
            var name = "ET Oazis";
            var company = new Company
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = name,
                Bulstat = "123456789",
                UserId = userId,
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            var result = await this.cs.GetCompanyNameByIdAsync(2);
            Assert.True(result == null);
        }
    }
}
