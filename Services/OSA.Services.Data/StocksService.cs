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

    public class StocksService : IStocksService
    {
        private readonly IDeletableEntityRepository<Stock> stockRepository;
        private readonly ApplicationDbContext context;

        public StocksService(IDeletableEntityRepository<Stock> stockRepository, ApplicationDbContext context)
        {
            this.stockRepository = stockRepository;
            this.context = context;
        }

        public async Task AddAsync(string name, decimal quantity, decimal price, string date, int invoiceId, int companyId)
        {
            var stock = new Stock
            {
                Name = name,
                Quantity = quantity,
                Price = price,
                Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                InvoiceId = invoiceId,
                CompanyId = companyId,
            };

            await this.stockRepository.AddAsync(stock);
            await this.stockRepository.SaveChangesAsync();
        }

        public async Task<List<string>> GetStockNamesByCompanyIdAsync(int companyId)
        {
            var stockNames = await this.context.Stocks
               .Where(x => x.CompanyId == companyId)
               .Select(x => x.Name)
               .Distinct()
               .ToListAsync();

            return stockNames;
        }

        //public async Task<List<SelectListItem>> GetStockNamesByCompanyIdAsync(int companyId)
        //{
        //    var stockNames = await this.context.Stocks
        //        .Where(x => x.CompanyId == companyId)
        //        .Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name })
        //        .ToListAsync();
        //    return stockNames;
        //}
    }
}
