namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models; 

    public class InvoiceService : IInvoiceService
    {
        private readonly IDeletableEntityRepository<Invoice> invoiceRepository;

        public InvoiceService(IDeletableEntityRepository<Invoice> invoiceRepository)
        {
            this.invoiceRepository = invoiceRepository;
        }

        public async Task AddAsync(string invoiceNumber, DateTime date, int supplierId, int companyId)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                Date = DateTime.ParseExact(date.ToString(), "d/M/yyyy", CultureInfo.InvariantCulture),
                SupplierId = supplierId,
                CompanyId = companyId,
            };

            await this.invoiceRepository.AddAsync(invoice);
            await this.invoiceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetAllSupplierNames()
        {
            return await this.
        }
    }
}
