namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Common;
    using OSA.Data;
    using OSA.Data.Models;

    public class ProductionInvoicesService : IProductionInvoicesService
    {
        private readonly ApplicationDbContext context;

        public ProductionInvoicesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string invoiceNumber, string date, decimal salary, decimal externalCost, int companyId)
        {
            var productionInvoice = new ProductionInvoice
            {
                InvoiceNumber = invoiceNumber,
                Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                Salary = salary,
                ExternalCost = externalCost,
                CompanyId = companyId,
            };
            await this.context.AddAsync(productionInvoice);
            await this.context.SaveChangesAsync();
        }

        public async Task<ProductionInvoice> DeleteAsync(int id)
        {
            var productionInvoice = await this.context.ProductionInvoices.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (productionInvoice != null)
            {
                productionInvoice.IsDeleted = true;
                productionInvoice.DeletedOn = DateTime.UtcNow;
                await this.context.SaveChangesAsync();
            }

            return productionInvoice;
        }

        public async Task<ProductionInvoice> GetProductionInvoiceByIdAsync(int id)
        {
            var productionInvoice = await this.context.ProductionInvoices.Where(x => x.Id == id).FirstOrDefaultAsync();
            return productionInvoice;
        }

        public async Task<ICollection<ProductionInvoice>> GetProductionInvoicesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var productionInvoices = await this.context.ProductionInvoices
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId)
                .ToListAsync();

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

        public async Task UpdateProductionInvoiceAsync(int id, string invoiceNumber, decimal salary, decimal externalCost, DateTime date)
        {
            var productionInvoice = await this.GetProductionInvoiceByIdAsync(id);
            productionInvoice.InvoiceNumber = invoiceNumber;
            productionInvoice.Salary = salary;
            productionInvoice.ExternalCost = externalCost;
            productionInvoice.Date = date;
            await this.context.SaveChangesAsync();
        }
    }
}
