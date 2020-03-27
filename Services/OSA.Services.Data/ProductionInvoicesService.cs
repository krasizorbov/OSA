namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class ProductionInvoicesService : IProductionInvoicesService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<ProductionInvoice> productionInvoicesRepository;
        private readonly ApplicationDbContext context;

        public ProductionInvoicesService(IDeletableEntityRepository<ProductionInvoice> productionInvoicesRepository, ApplicationDbContext context)
        {
            this.productionInvoicesRepository = productionInvoicesRepository;
            this.context = context;
        }

        public async Task AddAsync(string invoiceNumber, string date, decimal materialCost, decimal externalCost, int companyId)
        {
            var productionInvoice = new ProductionInvoice
            {
                InvoiceNumber = invoiceNumber,
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                StockCost = materialCost,
                ExternalCost = externalCost,
                CompanyId = companyId,
            };
            await this.productionInvoicesRepository.AddAsync(productionInvoice);
            await this.productionInvoicesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductionInvoice>> GetProductionInvoicesByCompanyIdAsync(int companyId)
        {
            var productionInvoices = await this.productionInvoicesRepository.All().Where(x => x.CompanyId == companyId).ToListAsync();

            return productionInvoices;
        }

        public async Task<string> InvoiceExistAsync(string invoiceNumber, int companyId)
        {
            var number = await this.context.ProductionInvoices
                .Where(x => x.CompanyId == companyId && x.InvoiceNumber == invoiceNumber)
                .Select(x => x.InvoiceNumber)
                .FirstOrDefaultAsync();

            return number;
        }
    }
}
