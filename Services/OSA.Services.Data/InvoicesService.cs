namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class InvoicesService : IInvoicesService
    {
        private const string DateFormat = "dd/MM/yyyy";
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
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                SupplierId = supplierId,
                CompanyId = companyId,
            };

            await this.invoiceRepository.AddAsync(invoice);
            await this.invoiceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId)
        {
            var invoices = await this.context.Invoices
                .Where(x => x.CompanyId == companyId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.InvoiceNumber })
                .ToListAsync();
            return invoices;
        }

        public bool InvoiceExist(string invoiceNumber, int companyId)
        {
            if (this.context.Invoices.Where(x => x.CompanyId == companyId).Any(x => x.InvoiceNumber == invoiceNumber))
            {
                return true;
            }

            return false;
        }
    }
}
