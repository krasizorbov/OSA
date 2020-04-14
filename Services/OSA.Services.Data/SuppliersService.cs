namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Models;

    public class SuppliersService : ISuppliersService
    {
        public const string InvalidCompanyIdErrorMessage = "Company with the given ID doesn't exist!";
        private readonly ApplicationDbContext context;

        public SuppliersService(ApplicationDbContext context)
        {
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
            await this.context.AddAsync(supplier);
            await this.context.SaveChangesAsync();
        }

        public async Task<ICollection<SelectListItem>> GetAllSuppliersByCompanyIdAsync(int companyId)
        {
            var supplierNames = await this.context.Suppliers
                .Where(x => x.CompanyId == companyId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name })
                .ToListAsync();
            return supplierNames;
        }

        public async Task<string> GetSupplierNameBySupplierIdAsync(int supplierId)
        {
            var supplierName = await this.context.Suppliers.Where(x => x.Id == supplierId).Select(x => x.Name).FirstOrDefaultAsync();

            return supplierName;
        }

        public async Task<List<Supplier>> GetSuppliersByCompanyIdAsync(int companyId)
        {
            var suppliers = await this.context.Suppliers.Where(x => x.CompanyId == companyId).ToListAsync();

            return suppliers;
        }

        public async Task<string> SupplierExistAsync(string supplierName, int companyId)
        {
            var name = await this.context.Suppliers
                .Where(x => x.CompanyId == companyId && x.Name == supplierName)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            return name;
        }
    }
}
