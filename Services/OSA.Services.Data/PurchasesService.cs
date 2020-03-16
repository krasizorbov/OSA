namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class PurchasesService : IPurchasesService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<Purchase> purchaseRepository;
        private readonly ApplicationDbContext context;
        private IEnumerable<string> stockNamesForCurrentMonth;
        private IEnumerable<string> stockNamesForPreviousMonth;
        private decimal quantityPurchased = 0;
        private decimal quantitySold = 0;
        private decimal totalQuantity = 0;
        private decimal totalPrice = 0;

        public PurchasesService(IDeletableEntityRepository<Purchase> purchaseRepository, ApplicationDbContext context)
        {
            this.purchaseRepository = purchaseRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);

            this.stockNamesForCurrentMonth = await this.GetStockNamesForCurrentMonthByCompanyIdAsync(start_Date, end_Date, companyId);
            this.stockNamesForPreviousMonth = await this.GetStockNamesForPrevoiusMonthByCompanyIdAsync(start_Date, end_Date, companyId);

            List<string> stockNamesCM = this.stockNamesForCurrentMonth.ToList();
            List<string> stockNamesPM = this.stockNamesForPreviousMonth.ToList();

            stockNamesCM.AddRange(stockNamesPM);

            var stockNames = stockNamesCM.Distinct().ToList();

            foreach (var name in stockNames)
            {
                this.quantitySold = await this.QuantitySoldAsync(name, start_Date, end_Date, companyId);
                this.quantityPurchased = await this.QuantityPurchasedAsync(name, start_Date, end_Date, companyId);

                decimal quantityAvailable = 0;

                if (this.quantityPurchased < this.quantitySold)
                {
                    Console.WriteLine("The quantity sold is bigger than the quantity purchased");
                    continue;
                }
                else
                {
                    quantityAvailable = this.quantityPurchased - this.quantitySold;
                }

                this.totalQuantity = this.TotalQuantity(name, start_Date, end_Date, companyId);
                this.totalQuantity += quantityAvailable;
                this.totalPrice = this.TotalPrice(name, start_Date, end_Date, companyId);

                var purchase = new Purchase
                {
                    StockName = name,
                    TotalQuantity = this.totalQuantity,
                    TotalPrice = this.totalPrice,
                    Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                    CompanyId = companyId,
                };

                await this.purchaseRepository.AddAsync(purchase);
                await this.purchaseRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            this.stockNamesForCurrentMonth = await this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
            .Select(x => x.Name)
            .Distinct()
            .ToListAsync();
            return this.stockNamesForCurrentMonth;
        }

        public async Task<IEnumerable<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            this.stockNamesForPreviousMonth = await this.context.Stocks
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= endDate.AddMonths(-1) && x.CompanyId == id)
            .Select(x => x.Name)
            .Distinct()
            .ToListAsync();
            return this.stockNamesForPreviousMonth;
        }

        public async Task<decimal> QuantityPurchasedAsync(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.quantityPurchased = await this.context.Purchases
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= endDate.AddMonths(-1) && x.StockName == stockName && x.CompanyId == id)
            .Select(x => x.TotalQuantity)
            .FirstOrDefaultAsync();
            return this.quantityPurchased;
        }

        public async Task<decimal> QuantitySoldAsync(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.quantitySold = await this.context.Sells
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= endDate.AddMonths(-1) && x.StockName == stockName && x.CompanyId == id)
            .Select(x => x.TotalQuantity)
            .FirstOrDefaultAsync();
            return this.quantitySold;
        }

        public decimal TotalPrice(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.totalPrice = this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.Name == stockName && x.CompanyId == id)
            .Sum(p => p.Price); // Total Price

            return this.totalPrice;
        }

        public decimal TotalQuantity(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.totalQuantity = this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.Name == stockName && x.CompanyId == id)
            .Sum(q => q.Quantity);

            return this.totalQuantity;
        }
    }
}
