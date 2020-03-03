namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class SupplierService : ISupplierService
    {
        private readonly IDeletableEntityRepository<Supplier> supplierRepository;

        public SupplierService(IDeletableEntityRepository<Supplier> supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task AddAsync(string name, string bulstat)
        {
            var supplier = new Supplier
            {
                Name = name,
                Bulstat = bulstat,
            };

            await this.supplierRepository.AddAsync(supplier);
            await this.supplierRepository.SaveChangesAsync();
        }
    }
}
