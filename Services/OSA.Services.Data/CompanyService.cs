namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class CompanyService : ICompanyService
    {
        private readonly IDeletableEntityRepository<Company> companyRepository;

        public CompanyService(IDeletableEntityRepository<Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task AddAsync(string name, string bulstat)
        {
            var company = new Company
            {
                Name = name,
                Bulstat = bulstat,
            };

            await this.companyRepository.AddAsync(company);
            await this.companyRepository.SaveChangesAsync();
        }
    }
}
