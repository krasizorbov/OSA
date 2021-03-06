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

    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext context;

        public SalesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId)
        {
            var start_Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var averagePrice = await this.GetAveragePrice(start_Date, stockName, companyId);
            var sale = new Sale
            {
                StockName = stockName,
                TotalPrice = totalPrice,
                ProfitPercent = profitPercent,
                AveragePrice = averagePrice.ToString(),
                Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };

            await this.context.AddAsync(sale);
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

        public async Task<List<Sale>> DeleteAsync(List<int> ids)
        {
            var sales = new List<Sale>();
            foreach (var id in ids)
            {
                var sale = await this.context.Sales.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (sale != null)
                {
                    sales.Add(sale);
                    sale.IsDeleted = true;
                    sale.DeletedOn = DateTime.UtcNow;
                }
            }

            await this.context.SaveChangesAsync();
            return sales;
        }

        public async Task<decimal> GetAveragePrice(DateTime startDate, string stockName, int companyId)
        {
            var averagePrice = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.CompanyId == companyId && x.StockName == stockName && x.IsDeleted == false)
                .Select(x => x.AveragePrice)
                .FirstOrDefaultAsync();

            return averagePrice;
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            var sale = await this.context.Sales.Where(x => x.Id == id).FirstOrDefaultAsync();
            return sale;
        }

        public async Task<ICollection<Sale>> GetSalesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var sales = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return sales;
        }

        public async Task<string> GetStockNameBySaleIdAsync(int id)
        {
            var stockName = await this.context.Sales.Where(x => x.Id == id).Select(n => n.StockName).FirstOrDefaultAsync();
            return stockName;
        }

        public async Task<decimal> GetTotalPurchasedQuantity(DateTime startDate, string stockName, int companyId)
        {
            var totalQuantity = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.CompanyId == companyId && x.StockName == stockName && x.IsDeleted == false)
                .Select(x => x.TotalQuantity)
                .FirstOrDefaultAsync();

            return totalQuantity;
        }

        public async Task<bool> IsBigger(decimal totalPrice, int profitPercent, DateTime startDate, string stockName, int companyId)
        {
            var averagePrice = await this.GetAveragePrice(startDate, stockName, companyId);
            var sale = new Sale
            {
                StockName = stockName,
                TotalPrice = totalPrice,
                ProfitPercent = profitPercent,
                AveragePrice = averagePrice.ToString(),
            };
            var totalQuantity = await this.GetTotalPurchasedQuantity(startDate, stockName, companyId);
            if (sale.TotalPurchaseQuantity > totalQuantity)
            {
                return true;
            }

            return false;
        }

        public async Task<string> SaleExistAsync(DateTime startDate, string stockName, int companyId)
        {
            var name = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.StockName == stockName && x.CompanyId == companyId && x.IsDeleted == false)
                .Select(x => x.StockName)
                .FirstOrDefaultAsync();

            return name;
        }

        public async Task UpdateSaleAsync(int id, decimal price, int profitPercent, DateTime date)
        {
            var sale = await this.GetSaleByIdAsync(id);
            sale.TotalPrice = price;
            sale.ProfitPercent = profitPercent;
            sale.Date = date;
            await this.context.SaveChangesAsync();
        }
    }
}
