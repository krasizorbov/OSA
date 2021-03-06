﻿namespace OSA.Services.Data
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

    public class StocksService : IStocksService
    {
        private readonly ApplicationDbContext context;

        public StocksService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string name, decimal quantity, decimal price, string date, int invoiceId, int companyId)
        {
            var stock = new Stock
            {
                Name = name.ToLower(),
                Quantity = quantity,
                Price = price,
                Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                InvoiceId = invoiceId,
                CompanyId = companyId,
            };

            await this.context.AddAsync(stock);
            await this.context.SaveChangesAsync();
        }

        public bool CompanyIdExists(int id)
        {
            if (this.context.Companies.Any(x => x.Id == id))
            {
                return true;
            }

            return false;
        }

        public async Task<List<Stock>> DeleteAsync(List<int> ids)
        {
            var stocks = new List<Stock>();
            foreach (var id in ids)
            {
                var stock = await this.context.Stocks.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (stock != null)
                {
                    stocks.Add(stock);
                    stock.IsDeleted = true;
                    stock.DeletedOn = DateTime.UtcNow;
                }
            }

            await this.context.SaveChangesAsync();
            return stocks;
        }

        public async Task<Stock> GetStockById(int id)
        {
            var stock = await this.context.Stocks.Where(x => x.Id == id).FirstOrDefaultAsync();
            return stock;
        }

        public async Task<List<string>> GetStockNamesByCompanyIdAsync(int companyId)
        {
            var stockNames = await this.context.Stocks
               .Where(x => x.CompanyId == companyId && x.IsDeleted == false)
               .Select(x => x.Name)
               .Distinct()
               .OrderBy(x => x)
               .ToListAsync();

            return stockNames;
        }

        public async Task<ICollection<Stock>> GetStocksByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var stocks = await this.context.Stocks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return stocks;
        }

        public async Task UpdateStock(int id, string name, decimal price, decimal quantity, DateTime date)
        {
            var stock = await this.GetStockById(id);
            stock.Name = name;
            stock.Price = price;
            stock.Quantity = quantity;
            stock.Date = date;
            await this.context.SaveChangesAsync();
        }
    }
}
