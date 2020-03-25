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

    public class BookValuesService : IBookValuesService
    {
        private readonly IDeletableEntityRepository<BookValue> bookValueRepository;
        private readonly ApplicationDbContext context;

        public BookValuesService(IDeletableEntityRepository<BookValue> bookValueRepository, ApplicationDbContext context)
        {
            this.bookValueRepository = bookValueRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var monthlySales = await this.GetMonthlySalesAsync(start_Date, end_Date, companyId);
            decimal bookvalue = 0;

            // Consider making a Dictionary<int, string> from "purchasedStockAveragePrice" for easy validation in the Controller later
            foreach (var sale in monthlySales.OrderBy(x => x.StockName))
            {
                var purchasedStockAveragePrice = await this.GetStockMonthlyAveragePriceAsync(sale.StockName, start_Date, end_Date, companyId);

                if (purchasedStockAveragePrice == 0)
                {
                    bookvalue = 0;
                    //Console.WriteLine("No satch material has been purchased this month!");
                }
                else
                {
                    bookvalue = sale.TotalPurchasePrice;
                    //Console.WriteLine($"Material name : {sell.StockName} with Book Value {bookvalue:F2} lv.");
                }

                var bookValue = new BookValue
                {
                    Price = bookvalue,
                    StockName = sale.StockName,
                    Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture),
                    CompanyId = companyId,
                };

                await this.bookValueRepository.AddAsync(bookValue);
                await this.bookValueRepository.SaveChangesAsync();
            }
        }

        public async Task<List<string>> BookValueExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var stockNames = await this.context.BookValues
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId)
                .Select(x => x.StockName)
                .ToListAsync();

            return stockNames;
        }

        public async Task<IEnumerable<BookValue>> GetBookValuesByCompanyIdAsync(int companyId)
        {
            var bookValues = await this.bookValueRepository.All().Where(x => x.CompanyId == companyId).ToListAsync();

            return bookValues;
        }

        public async Task<List<Sale>> GetMonthlySalesAsync(DateTime startDate, DateTime endDate, int id)
        {
            var monthlySale = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .ToListAsync();

            return monthlySale;
        }

        public async Task<decimal> GetStockMonthlyAveragePriceAsync(string stockName, DateTime startDate, DateTime endDate, int id)
        {
            var purchasedStockAveragePrice = await this.context.Purchases
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == id)
                .Select(x => x.AveragePrice)
                .FirstOrDefaultAsync();

            return purchasedStockAveragePrice;
        }
    }
}
