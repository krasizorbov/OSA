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

    public class AvailableStocksService : IAvailableStocksService
    {
        private readonly IDeletableEntityRepository<AvailableStock> availableStockRepository;
        private readonly ApplicationDbContext context;
        private List<string> purchasedStockNamesForCurrentMonth;
        private List<string> soldStockNamesForCurrentMonth;

        public AvailableStocksService(IDeletableEntityRepository<AvailableStock> availableStockRepository, ApplicationDbContext context)
        {
            this.availableStockRepository = availableStockRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            this.purchasedStockNamesForCurrentMonth = await this.GetPurchasedStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);
            this.soldStockNamesForCurrentMonth = await this.GetSoldStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);

            this.purchasedStockNamesForCurrentMonth.AddRange(this.soldStockNamesForCurrentMonth);

            var stockNamesList = this.purchasedStockNamesForCurrentMonth.Distinct();

            foreach (var name in stockNamesList.OrderBy(x => x))
            {
                var currentPurchasedStock = await this.GetCurrentPurchasedStockNameAsync(start_Date, end_Date, name, companyId);
                var currentSoldStock = await this.GetCurrentSoldStockNameAsync(start_Date, end_Date, name, companyId);
                var currentBookValue = await this.GetCurrentBookValueForStockNameAsync(start_Date, end_Date, name, companyId);

                if (this.purchasedStockNamesForCurrentMonth.Contains(name) && this.soldStockNamesForCurrentMonth.Contains(name)) // Best scenario equal stock amount on both sides
                {
                    var availableStock = new AvailableStock
                    {
                        StockName = name,
                        TotalPurchasedAmount = currentPurchasedStock.TotalQuantity,
                        TotalPurchasedPrice = currentPurchasedStock.TotalPrice,
                        TotalSoldPrice = currentSoldStock.TotalPrice,
                        BookValue = currentBookValue,
                        AveragePrice = currentPurchasedStock.AveragePrice,
                        Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };
                    await this.availableStockRepository.AddAsync(availableStock);
                }
                else if (!this.purchasedStockNamesForCurrentMonth.Contains(name) && this.soldStockNamesForCurrentMonth.Contains(name)) // No satch stock in Purchases
                {
                    var availableStock = new AvailableStock
                    {
                        StockName = name,
                        TotalPurchasedAmount = 0,
                        TotalPurchasedPrice = 0,
                        TotalSoldPrice = currentSoldStock.TotalPrice,
                        BookValue = currentBookValue,
                        AveragePrice = currentPurchasedStock.AveragePrice,
                        Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };
                    await this.availableStockRepository.AddAsync(availableStock);
                }
                else if (this.purchasedStockNamesForCurrentMonth.Contains(name) && !this.soldStockNamesForCurrentMonth.Contains(name)) // No satch stock in Sells
                {
                    var availableStock = new AvailableStock
                    {
                        StockName = name,
                        TotalPurchasedAmount = currentPurchasedStock.TotalQuantity,
                        TotalPurchasedPrice = currentPurchasedStock.TotalPrice,
                        TotalSoldPrice = 0,
                        BookValue = 0,
                        AveragePrice = currentPurchasedStock.AveragePrice,
                        Date = DateTime.ParseExact(date, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };
                    await this.availableStockRepository.AddAsync(availableStock);
                }

                await this.availableStockRepository.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetCurrentBookValueForStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int id)
        {
            var currentBookValueForStockName = await this.context.BookValues
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id)
                .Select(x => x.Price)
                .FirstOrDefaultAsync();

            return currentBookValueForStockName;
        }

        public async Task<Purchase> GetCurrentPurchasedStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int id)
        {
            var currentPurchasedStockName = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id)
                .FirstOrDefaultAsync();

            return currentPurchasedStockName;
        }

        public async Task<Sell> GetCurrentSoldStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int id)
        {
            var currentSoldStockName = await this.context.Sells
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id)
                .FirstOrDefaultAsync();

            return currentSoldStockName;
        }

        public async Task<List<string>> GetPurchasedStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            var purchasedStockNames = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x.StockName)
                .ToListAsync();

            return purchasedStockNames;
        }

        public async Task<List<string>> GetSoldStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            var soldStockNames = await this.context.Sells
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x.StockName)
                .ToListAsync();

            return soldStockNames;
        }
    }
}
