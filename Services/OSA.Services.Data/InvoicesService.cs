namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class InvoicesService : IInvoicesService
    {
        private readonly IDeletableEntityRepository<Invoice> invoiceRepository;
        private readonly ISuppliersService suppliersService;

        public InvoicesService(IDeletableEntityRepository<Invoice> invoiceRepository, ISuppliersService suppliersService)
        {
            this.invoiceRepository = invoiceRepository;
            this.suppliersService = suppliersService;
        }

        public async Task AddAsync(string invoiceNumber, string date, int supplierId, int companyId)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                Date = DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                SupplierId = supplierId,
                CompanyId = companyId,
            };

            await this.invoiceRepository.AddAsync(invoice);
            await this.invoiceRepository.SaveChangesAsync();
        }
    }
}
