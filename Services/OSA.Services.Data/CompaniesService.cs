namespace OSA.Services.Data
{
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
            var company = new Company
            {
                Name = name,
                Bulstat = bulstat,
                UserId = userId,
            };
            await this.companyRepository.AddAsync(company);
            await this.companyRepository.SaveChangesAsync();
        }

        public bool CompanyExist(string companyName, string userId)
        {
            if (this.context.Companies.Where(x => x.UserId == userId).Any(x => x.Name == companyName))
            {
                return true;
            }

            return false;
        }

        async Task<IEnumerable<SelectListItem>> ICompaniesService.GetAllCompaniesByUserIdAsync()
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
