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

    public class SuppliersService : ISuppliersService
    {
        public const string InvalidCompanyIdErrorMessage = "Company with the given ID doesn't exist!";
        private readonly IDeletableEntityRepository<Supplier> supplierRepository;
        private readonly ApplicationDbContext context;

        public SuppliersService(IDeletableEntityRepository<Supplier> supplierRepository, ApplicationDbContext context)
        {
            this.supplierRepository = supplierRepository;
            this.context = context;
        }

        public async Task AddAsync(string name, string bulstat, int companyId)
        {
            if (!this.context.Companies.Any(x => x.Id == companyId))
            {
                throw new ArgumentException(InvalidCompanyIdErrorMessage);
            }

            var supplier = new Supplier
            {
                Name = name,
                Bulstat = bulstat,
                CompanyId = companyId,
            };
            await this.supplierRepository.AddAsync(supplier);
            await this.supplierRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAllSuppliersByCompanyIdAsync(int companyId)
        {
            var supplierNames = await this.context.Suppliers
                .Where(x => x.CompanyId == companyId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name })
                .ToListAsync();
            return supplierNames;
        }

        public bool SupplierExist(string supplierName, int companyId)
        {
            if (this.context.Suppliers.Where(x => x.CompanyId == companyId).Any(x => x.Name == supplierName))
            {
                return true;
            }

            return false;
        }
    }
}
