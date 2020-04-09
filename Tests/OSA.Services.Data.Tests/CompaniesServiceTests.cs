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
        private readonly CompaniesService cs;
        private readonly Mock<IDeletableEntityRepository<Company>> companiesServiceMock = new Mock<IDeletableEntityRepository<Company>>();
        private readonly Mock<IUsersService> usersServiceMock = new Mock<IUsersService>();


        public CompaniesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            var context = new ApplicationDbContext(options);
            this.cs = new CompaniesService(this.companiesServiceMock.Object, this.usersServiceMock.Object, context);
        }

        [Fact]
        public async Task CompanyExistAsyncReturnCompanyName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            var context = new ApplicationDbContext(options);

            string name = "ET Oazis";
            string id = Guid.NewGuid().ToString();
            var mock = new Mock<ICompaniesService>();

            mock.Setup(x => x.CompanyExistAsync(name, id)).Returns(Task.FromResult(name));

            var result = await this.cs.CompanyExistAsync(name, id);
            Assert.Equal(name, result);
        }

        [Fact]
        public async Task GetCompaniesByUserIdAsyncReturnsListOfNames()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            var context = new ApplicationDbContext(options);
            string name = "ET Oazis";
            string bulstat = "123456789";
            string id = Guid.NewGuid().ToString();
            await this.cs.AddAsync(name, bulstat, id);
            var result = await this.cs.GetCompaniesByUserIdAsync();
            List<Company> newList = result.ToList();
            Assert.Equal(1, newList.Count);

        }
    }
}
