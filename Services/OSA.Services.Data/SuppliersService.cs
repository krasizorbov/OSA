namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> supplierRepository;
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;

        public SuppliersService(IDeletableEntityRepository<Supplier> supplierRepository, ApplicationDbContext context, IUsersService usersService)
        {
            this.supplierRepository = supplierRepository;
            this.context = context;
            this.usersService = usersService;
        }

        public async Task AddAsync(string name, string bulstat, int companyId)
        {
            var supplier = new Supplier
            {
                Name = name,
                Bulstat = bulstat,
                CompanyId = companyId,
            };

            await this.supplierRepository.AddAsync(supplier);
            await this.supplierRepository.SaveChangesAsync();
        }
    }
}
