namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class ProductionInvoicesService : IProductionInvoicesService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<ProductionInvoice> productionInvoicesRepository;

        public ProductionInvoicesService(IDeletableEntityRepository<ProductionInvoice> productionInvoicesRepository)
        {
            this.productionInvoicesRepository = productionInvoicesRepository;
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
    }
}
