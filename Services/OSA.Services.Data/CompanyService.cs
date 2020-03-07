namespace OSA.Services.Data
{
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
    }
}
