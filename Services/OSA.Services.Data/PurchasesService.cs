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
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class PurchasesService : IPurchasesService
    {
        private readonly IDeletableEntityRepository<Purchase> purchaseRepository;
        private readonly ApplicationDbContext context;
        private List<string> stockNamesForCurrentMonth;
        private List<string> stockNamesForPreviousMonth;
        private List<string> stockNamesList;
        private decimal quantityPurchased = 0;
        private decimal quantitySold = 0;
        private decimal totalQuantity = 0;
        private decimal totalPrice = 0;

        public PurchasesService(IDeletableEntityRepository<Purchase> purchaseRepository, ApplicationDbContext context)
        {
            this.purchaseRepository = purchaseRepository;
            this.context = context;
            this.stockNamesList = new List<string>();
        }

        public async Task AddAsync(string startDate, string endDate, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            this.stockNamesForCurrentMonth = await this.GetStockNamesForCurrentMonthByCompanyIdAsync(start_Date, end_Date, companyId);
            this.stockNamesForPreviousMonth = await this.GetStockNamesForPrevoiusMonthByCompanyIdAsync(start_Date, end_Date, companyId);
            this.stockNamesList.AddRange(this.stockNamesForCurrentMonth);
            this.stockNamesList.AddRange(this.stockNamesForPreviousMonth);
            var stockNames = this.stockNamesList.Distinct();

            foreach (var name in stockNames)
            {
                decimal quantityAvailable = 0;
                if (this.stockNamesForCurrentMonth.Contains(name) && this.stockNamesForPreviousMonth.Contains(name))
                {
                    this.quantitySold = await this.QuantitySoldAsync(name, start_Date, end_Date, companyId);
                    this.quantityPurchased = await this.QuantityPurchasedAsync(name, start_Date, end_Date, companyId);
                    quantityAvailable = this.quantityPurchased - this.quantitySold;
                    this.totalQuantity = this.TotalQuantity(name, start_Date, end_Date, companyId);
                    this.totalQuantity += quantityAvailable;
                    var availableStockMoney = await this.GetAvailableStockForPreviousMonthByCompanyIdAsync(start_Date, end_Date, name, companyId);
                    this.totalPrice = this.TotalPrice(name, start_Date, end_Date, companyId);
                    this.totalPrice += availableStockMoney;
                    var purchase = new Purchase
                    {
                        StockName = name,
                        TotalQuantity = this.totalQuantity,
                        TotalPrice = this.totalPrice,
                        Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };

                    await this.purchaseRepository.AddAsync(purchase);
                }

                if (!this.stockNamesForCurrentMonth.Contains(name) && this.stockNamesForPreviousMonth.Contains(name))
                {
                    this.quantitySold = await this.QuantitySoldAsync(name, start_Date, end_Date, companyId);
                    this.quantityPurchased = await this.QuantityPurchasedAsync(name, start_Date, end_Date, companyId);
                    quantityAvailable = this.quantityPurchased - this.quantitySold;
                    this.totalQuantity = 0;
                    this.totalQuantity += quantityAvailable;
                    var availableStockMoney = await this.GetAvailableStockForPreviousMonthByCompanyIdAsync(start_Date, end_Date, name, companyId);
                    this.totalPrice = 0;
                    this.totalPrice += availableStockMoney;
                    var purchase = new Purchase
                    {
                        StockName = name,
                        TotalQuantity = this.totalQuantity,
                        TotalPrice = this.totalPrice,
                        Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };

                    await this.purchaseRepository.AddAsync(purchase);
                }

                if (this.stockNamesForCurrentMonth.Contains(name) && !this.stockNamesForPreviousMonth.Contains(name))
                {
                    this.totalQuantity = this.TotalQuantity(name, start_Date, end_Date, companyId);
                    var availableStockMoney = await this.GetAvailableStockForPreviousMonthByCompanyIdAsync(start_Date, end_Date, name, companyId);
                    this.totalPrice = this.TotalPrice(name, start_Date, end_Date, companyId);
                    this.totalPrice += availableStockMoney;
                    var purchase = new Purchase
                    {
                        StockName = name,
                        TotalQuantity = this.totalQuantity,
                        TotalPrice = this.totalPrice,
                        Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };

                    await this.purchaseRepository.AddAsync(purchase);
                }

                await this.purchaseRepository.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetStockNamesAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            this.stockNamesForCurrentMonth = await this.GetStockNamesForCurrentMonthByCompanyIdAsync(startDate, endDate, companyId);
            this.stockNamesForPreviousMonth = await this.GetStockNamesForPrevoiusMonthByCompanyIdAsync(startDate, endDate, companyId);
            this.stockNamesForCurrentMonth.AddRange(this.stockNamesForPreviousMonth);
            var stockNames = this.stockNamesForCurrentMonth.Distinct();
            return stockNames.ToList();
        }

        public async Task<decimal> GetAvailableStockForPreviousMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, string name, int companyId)
        {
            var availableStockMoney = await this.context.AvailableStocks
                .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= startDate.AddDays(-1) && x.CompanyId == companyId && x.StockName == name && x.IsDeleted == false)
                .Select(x => x.RemainingPrice)
                .FirstOrDefaultAsync();

            return availableStockMoney;
        }

        public async Task<List<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            this.stockNamesForCurrentMonth = await this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
            .Select(x => x.Name)
            .Distinct()
            .ToListAsync();
            return this.stockNamesForCurrentMonth;
        }

        public async Task<List<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            this.stockNamesForPreviousMonth = await this.context.Purchases
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= startDate.AddDays(-1) && x.CompanyId == id && x.IsDeleted == false)
            .Select(x => x.StockName)
            .Distinct()
            .ToListAsync();
            return this.stockNamesForPreviousMonth;
        }

        public async Task<decimal> QuantityPurchasedAsync(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.quantityPurchased = await this.context.Purchases
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= startDate.AddDays(-1) && x.StockName == stockName && x.CompanyId == id && x.IsDeleted == false)
            .Select(x => x.TotalQuantity)
            .FirstOrDefaultAsync();
            return this.quantityPurchased;
        }

        public async Task<decimal> QuantitySoldAsync(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.quantitySold = await this.context.Sales
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= startDate.AddDays(-1) && x.StockName == stockName && x.CompanyId == id && x.IsDeleted == false)
            .Select(x => x.TotalPurchaseQuantity)
            .FirstOrDefaultAsync();
            return this.quantitySold;
        }

        public decimal TotalPrice(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.totalPrice = this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.Name == stockName && x.CompanyId == id && x.IsDeleted == false)
            .Sum(p => p.Price);

            return this.totalPrice;
        }

        public decimal TotalQuantity(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            this.totalQuantity = this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.Name == stockName && x.CompanyId == id && x.IsDeleted == false)
            .Sum(q => q.Quantity);

            return this.totalQuantity;
        }

        public async Task<List<string>> PurchaseExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var stockNames = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .Select(x => x.StockName)
                .ToListAsync();

            return stockNames;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var purchases = await this.purchaseRepository.All()
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return purchases;
        }

        public async Task<List<Purchase>> DeleteAsync(List<int> ids)
        {
            var purchases = new List<Purchase>();
            foreach (var id in ids)
            {
                var purchase = await this.purchaseRepository.All().Where(x => x.Id == id).FirstOrDefaultAsync();
                if (purchase != null)
                {
                    purchases.Add(purchase);
                    this.purchaseRepository.Delete(purchase);
                }
            }

            await this.purchaseRepository.SaveChangesAsync();
            return purchases;
        }
    }
}
