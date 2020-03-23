namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class CompaniesService : ICompaniesService
    {
        public const string InvalidUserIdErrorMessage = "User with the given ID doesn't exist!";
        private readonly IDeletableEntityRepository<Company> companyRepository;
        private readonly IUsersService usersService;
        private readonly ApplicationDbContext context;

        public CompaniesService(IDeletableEntityRepository<Company> companyRepository, IUsersService usersService, ApplicationDbContext context)
        {
            this.companyRepository = companyRepository;
            this.usersService = usersService;
            this.context = context;
        }

        public async Task AddAsync(string name, string bulstat, string userId)
        {
            if (!this.context.Users.Any(x => x.Id == userId))
            {
                throw new ArgumentException(InvalidUserIdErrorMessage);
            }

            var company = new Company
            {
                Name = name,
                Bulstat = bulstat,
                UserId = userId,
            };
            await this.companyRepository.AddAsync(company);
            await this.companyRepository.SaveChangesAsync();
        }

        public async Task<string> CompanyExistAsync(string companyName, string userId)
        {
            var name = await this.context.Companies
                .Where(x => x.UserId == userId && x.Name == companyName)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            return name;
        }

        public async Task<IEnumerable<Company>> GetCompaniesByUserIdAsync()
        {
            var userId = this.usersService.GetCurrentUserId();
            var companies = await this.companyRepository.All().Where(x => x.UserId == userId).ToListAsync();

            return companies;
        }

        async Task<ICollection<SelectListItem>> ICompaniesService.GetAllCompaniesByUserIdAsync()
        {
            var userId = this.usersService.GetCurrentUserId();

            var companyNames = await this.context.Companies
                .Where(x => x.UserId == userId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name })
                .ToListAsync();
            return companyNames;
        }
    }
}
