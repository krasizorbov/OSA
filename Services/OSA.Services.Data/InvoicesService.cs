namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using OSA.Common;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class InvoicesService : IInvoicesService
    {
        private readonly IDeletableEntityRepository<Invoice> invoiceRepository;
        private readonly ApplicationDbContext context;

        public InvoicesService(IDeletableEntityRepository<Invoice> invoiceRepository, ApplicationDbContext context)
        {
            this.invoiceRepository = invoiceRepository;
            this.context = context;
        }

        public async Task AddAsync(string invoiceNumber, string date, int supplierId, int companyId)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                SupplierId = supplierId,
                CompanyId = companyId,
            };

            await this.invoiceRepository.AddAsync(invoice);
            await this.invoiceRepository.SaveChangesAsync();
        }

        public async Task<ICollection<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId)
        {
            var invoices = await this.context.Invoices
                .Where(x => x.CompanyId == companyId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.InvoiceNumber })
                .ToListAsync();
            return invoices;
        }

        public async Task<string> GetInvoiceNumberByInvoiceIdAsync(int invoiceId)
        {
            var invoiceNumber = await this.invoiceRepository.All().Where(x => x.Id == invoiceId).Select(x => x.InvoiceNumber).FirstOrDefaultAsync();

            return invoiceNumber;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var invoices = await this.invoiceRepository.All().Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId).ToListAsync();

            return invoices;
        }

        public async Task<string> InvoiceExistAsync(string invoiceNumber, int companyId)
        {
            var number = await this.context.Invoices
                .Where(x => x.CompanyId == companyId && x.InvoiceNumber == invoiceNumber)
                .Select(x => x.InvoiceNumber)
                .FirstOrDefaultAsync();

            return number;
        }
    }
}
