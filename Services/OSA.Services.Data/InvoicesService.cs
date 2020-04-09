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
    using OSA.Data.Models;

    public class InvoicesService : IInvoicesService
    {
        private readonly ApplicationDbContext context;

        public InvoicesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string invoiceNumber, decimal totalAmount, string date, int supplierId, int companyId)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                TotalAmount = totalAmount,
                Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                SupplierId = supplierId,
                CompanyId = companyId,
            };

            await this.context.AddAsync(invoice);
            await this.context.SaveChangesAsync();
        }

        public async Task<Invoice> DeleteAsync(int id)
        {
            var invoice = await this.context.Invoices.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (invoice != null)
            {
                invoice.IsDeleted = true;
                invoice.DeletedOn = DateTime.UtcNow;
                await this.context.SaveChangesAsync();
            }

            return invoice;
        }

        public async Task<ICollection<SelectListItem>> GetAllInvoicesByCompanyIdAsync(int companyId)
        {
            var invoices = await this.context.Invoices
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false)
                .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.InvoiceNumber })
                .ToListAsync();
            return invoices;
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            var invoice = await this.context.Invoices.Where(x => x.Id == id).FirstOrDefaultAsync();
            return invoice;
        }

        public async Task<string> GetInvoiceNumberByInvoiceIdAsync(int invoiceId)
        {
            var invoiceNumber = await this.context.Invoices
                .Where(x => x.Id == invoiceId && x.IsDeleted == false)
                .Select(x => x.InvoiceNumber)
                .FirstOrDefaultAsync();

            return invoiceNumber;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var invoices = await this.context.Invoices
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return invoices;
        }

        public async Task<string> InvoiceExistAsync(string invoiceNumber, int companyId)
        {
            var number = await this.context.Invoices
                .Where(x => x.CompanyId == companyId && x.InvoiceNumber == invoiceNumber && x.IsDeleted == false)
                .Select(x => x.InvoiceNumber)
                .FirstOrDefaultAsync();

            return number;
        }
    }
}
