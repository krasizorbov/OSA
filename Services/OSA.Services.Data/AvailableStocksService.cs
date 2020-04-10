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

    public class AvailableStocksService : IAvailableStocksService
    {
        private readonly ApplicationDbContext context;
        private List<string> purchasedStockNamesForCurrentMonth;
        private List<string> soldStockNamesForCurrentMonth;
        private List<string> stockNamesList;

        public AvailableStocksService(ApplicationDbContext context)
        {
            this.context = context;
            this.stockNamesList = new List<string>();
        }

        public async Task AddAsync(string startDate, string endDate, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            this.purchasedStockNamesForCurrentMonth = await this.GetPurchasedStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);
            this.soldStockNamesForCurrentMonth = await this.GetSoldStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);

            this.stockNamesList.AddRange(this.purchasedStockNamesForCurrentMonth);
            this.stockNamesList.AddRange(this.soldStockNamesForCurrentMonth);
            var stockNames = this.stockNamesList.Distinct();

            if (stockNames.Count() == 0)
            {
                var availableStocksForPreviousMonth = await this.GetAvailableStocksForPreviousMonthByCompanyIdAsync(start_Date, end_Date, companyId);
                foreach (var availableStock in availableStocksForPreviousMonth)
                {
                    var availableStockFromPreviousMonth = new AvailableStock
                    {
                        StockName = availableStock.StockName,
                        TotalPurchasedAmount = availableStock.TotalPurchasedAmount,
                        TotalPurchasedPrice = availableStock.TotalPurchasedPrice,
                        TotalSoldPrice = availableStock.TotalSoldPrice,
                        BookValue = availableStock.BookValue,
                        AveragePrice = availableStock.AveragePrice,
                        Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };
                    await this.context.AddAsync(availableStockFromPreviousMonth);
                }

                await this.context.SaveChangesAsync();
            }

            foreach (var name in stockNames.OrderBy(x => x))
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
                        AveragePrice = currentPurchasedStock.AveragePrice.ToString(),
                        Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };
                    await this.context.AddAsync(availableStock);
                }
                else if (!this.purchasedStockNamesForCurrentMonth.Contains(name) && this.soldStockNamesForCurrentMonth.Contains(name)) // No satch stock in Purchases
                {
                    var availableStockName = await this.GetAvailableStockForPreviousMonthByCompanyIdAsync(start_Date, end_Date, name, companyId);
                    if (availableStockName != null)
                    {
                        var availableStock = new AvailableStock
                        {
                            StockName = availableStockName.StockName,
                            TotalPurchasedAmount = availableStockName.TotalPurchasedAmount,
                            TotalPurchasedPrice = availableStockName.TotalPurchasedPrice,
                            TotalSoldPrice = currentSoldStock.TotalPrice,
                            BookValue = availableStockName.BookValue,
                            AveragePrice = availableStockName.AveragePrice,
                            Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                            CompanyId = companyId,
                        };
                        await this.context.AddAsync(availableStock);
                    }
                }
                else if (this.purchasedStockNamesForCurrentMonth.Contains(name) && !this.soldStockNamesForCurrentMonth.Contains(name)) // No satch stock in Sales
                {
                    var availableStock = new AvailableStock
                    {
                        StockName = name,
                        TotalPurchasedAmount = currentPurchasedStock.TotalQuantity,
                        TotalPurchasedPrice = currentPurchasedStock.TotalPrice,
                        TotalSoldPrice = 0,
                        BookValue = 0,
                        AveragePrice = currentPurchasedStock.AveragePrice.ToString(),
                        Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                        CompanyId = companyId,
                    };
                    await this.context.AddAsync(availableStock);
                }

                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> AvailableStockExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var stockNames = await this.context.AvailableStocks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .Select(x => x.StockName)
                .ToListAsync();

            return stockNames;
        }

        public async Task<List<AvailableStock>> DeleteAsync(List<int> ids)
        {
            var availableStocks = new List<AvailableStock>();
            foreach (var id in ids)
            {
                var availableStock = await this.context.AvailableStocks.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (availableStock != null)
                {
                    availableStocks.Add(availableStock);
                    availableStock.IsDeleted = true;
                    availableStock.DeletedOn = DateTime.UtcNow;
                }
            }

            await this.context.SaveChangesAsync();
            return availableStocks;
        }

        public async Task<AvailableStock> GetAvailableStockForPreviousMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, string name, int companyId)
        {
            var availableStockName = await this.context.AvailableStocks
                .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= startDate.AddDays(-1) && x.CompanyId == companyId && x.StockName == name && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return availableStockName;
        }

        public async Task<IEnumerable<AvailableStock>> GetAvailableStocksForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var availableStocks = await this.context.AvailableStocks
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return availableStocks;
        }

        public async Task<List<AvailableStock>> GetAvailableStocksForPreviousMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var availableStocks = await this.context.AvailableStocks
                .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= startDate.AddDays(-1) && x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return availableStocks;
        }

        public async Task<decimal> GetCurrentBookValueForStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int id)
        {
            var currentBookValueForStockName = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id && x.IsDeleted == false)
                .Select(x => x.BookValue)
                .FirstOrDefaultAsync();

            return currentBookValueForStockName;
        }

        public async Task<Purchase> GetCurrentPurchasedStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int id)
        {
            var currentPurchasedStockName = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return currentPurchasedStockName;
        }

        public async Task<Sale> GetCurrentSoldStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int id)
        {
            var currentSoldStockName = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return currentSoldStockName;
        }

        public async Task<List<string>> GetPurchasedStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            var purchasedStockNames = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
                .Select(x => x.StockName)
                .ToListAsync();

            return purchasedStockNames;
        }

        public async Task<List<string>> GetSoldStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            var soldStockNames = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id && x.IsDeleted == false)
                .Select(x => x.StockName)
                .ToListAsync();

            return soldStockNames;
        }
    }
}
