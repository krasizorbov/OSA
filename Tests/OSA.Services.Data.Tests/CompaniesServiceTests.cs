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

        [Fact]
        public async Task AddAsyncReturnsCorrectCount()
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
            Assert.Equal("1", context.Companies.Count().ToString());
        }

        [Fact]
        public async Task AddAsyncReturnsAnArgumentExeption()
        {
            var context = InitializeContext.CreateContextForInMemory();
            this.cs = new CompaniesService(this.us, context);
            var expectedErrorMessage = "User with the given ID doesn't exist!";
            var userId = Guid.NewGuid().ToString();
            var name = "ET Oazis";
            var company = new Company
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Name = name,
                Bulstat = "123456789",
                UserId = "kdgaskadasuhwqqpojaknad",
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                this.cs.AddAsync(company.Name, company.Bulstat, company.UserId));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }
    }
}
