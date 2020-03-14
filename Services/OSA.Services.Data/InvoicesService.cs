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
        private readonly IDeletableEntityRepository<Invoice> invoiceRepository;
        private readonly ISuppliersService suppliersService;
        private readonly ApplicationDbContext context;

        public InvoicesService(IDeletableEntityRepository<Invoice> invoiceRepository, ISuppliersService suppliersService, ApplicationDbContext context)
        {
            this.invoiceRepository = invoiceRepository;
            this.suppliersService = suppliersService;
            this.context = context;
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

        public async Task<IEnumerable<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId)
        {
            var invoices = await this.context.Invoices
                .Where(x => x.CompanyId == companyId)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.InvoiceNumber })
                .ToListAsync();
            return invoices;
        }
    }
}
