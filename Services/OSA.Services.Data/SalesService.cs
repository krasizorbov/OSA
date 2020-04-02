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
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class SalesService : ISalesService
    {
        public const string InvalidSaleErrorMessage = "Invalid Sale!";
        private readonly IDeletableEntityRepository<Sale> saleRepository;
        private readonly ApplicationDbContext context;

        public SalesService(IDeletableEntityRepository<Sale> saleRepository, ApplicationDbContext context)
        {
            this.saleRepository = saleRepository;
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

            await this.saleRepository.AddAsync(sale);
            await this.saleRepository.SaveChangesAsync();
        }

        public async Task<decimal> GetAveragePrice(DateTime startDate, string stockName, int companyId)
        {
            var averagePrice = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.CompanyId == companyId && x.StockName == stockName)
                .Select(x => x.AveragePrice)
                .FirstOrDefaultAsync();

            return averagePrice;
        }

        public async Task<IEnumerable<Sale>> GetSalesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var sales = await this.saleRepository.All().Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId).ToListAsync();

            return sales;
        }

        public async Task<decimal> GetTotalPurchasedQuantity(DateTime startDate, string stockName, int companyId)
        {
            var totalQuantity = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.CompanyId == companyId && x.StockName == stockName)
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

        public async Task<string> PurchasedStockExist(DateTime startDate, string stockName, int companyId)
        {
            var purchasedStockName = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.CompanyId == companyId && x.StockName == stockName)
                .Select(x => x.StockName)
                .FirstOrDefaultAsync();

            return purchasedStockName;
        }

        public async Task<string> SaleExistAsync(DateTime startDate, string stockName, int companyId)
        {
            var name = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= startDate.AddMonths(1).AddDays(-1) && x.StockName == stockName && x.CompanyId == companyId)
                .Select(x => x.StockName)
                .FirstOrDefaultAsync();

            return name;
        }
    }
}
