namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

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
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                InvoiceNumber = invoiceNumber,
                StockCost = materialCost,
                ExternalCost = externalCost,
                CompanyId = companyId,
            };
            await this.productionInvoicesRepository.AddAsync(productionInvoice);
            await this.productionInvoicesRepository.SaveChangesAsync();
        }
    }
}
