namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> supplierRepository;
        private readonly ApplicationDbContext context;
        private readonly ICompaniesService companiesService;

        public SuppliersService(IDeletableEntityRepository<Supplier> supplierRepository, ApplicationDbContext context, ICompaniesService companiesService)
        {
            this.supplierRepository = supplierRepository;
            this.context = context;
            this.companiesService = companiesService;
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

        public async Task<List<SelectListItem>> GetAllSuppliersByCompanyIdAsync(int companyId)
        {
            var supplierNames = Task.Run(() => this.context.Suppliers
                .Where(x => x.CompanyId == companyId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name })
                .ToList());
            var result = await supplierNames;

            return result;
        }
    }
}
