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

    public class BooksValueService : IBooksValueService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<BookValue> bookValueRepository;
        private readonly ApplicationDbContext context;

        public BooksValueService(IDeletableEntityRepository<BookValue> bookValueRepository, ApplicationDbContext context)
        {
            this.bookValueRepository = bookValueRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);

            var monthlySells = await this.GetMonthlySellsAsync(start_Date, end_Date, companyId);
            decimal bookvalue = 0;

            foreach (var sell in monthlySells.OrderBy(x => x.StockName))
            {
                var purchasedStockAveragePrice = await this.GetStockMonthlyAveragePriceAsync(sell.StockName, start_Date, end_Date, companyId);

                if (monthlySells.Count == 0)
                {
                    Console.WriteLine($"There is no sells for the month");
                }

                if (purchasedStockAveragePrice == 0)
                {
                    bookvalue = 0;
                    Console.WriteLine("No satch material has been purchased this month!");
                }
                else
                {
                    bookvalue = sell.TotalQuantity * purchasedStockAveragePrice;
                    Console.WriteLine($"Material name : {sell.StockName} with Book Value {bookvalue:F2} lv.");
                }

                var bookValue = new BookValue
                {
                    Price = bookvalue,
                    StockName = sell.StockName,
                    Date = DateTime.ParseExact(date.ToString(), DateFormat, CultureInfo.InvariantCulture),
                    CompanyId = companyId,
                };

                await this.bookValueRepository.AddAsync(bookValue);
                await this.bookValueRepository.SaveChangesAsync();
            }
        }

        public async Task<List<Sell>> GetMonthlySellsAsync(DateTime startDate, DateTime endDate, int id)
        {
            var monthlySell = await this.context.Sells
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
                .Select(x => x)
                .ToListAsync();

            return monthlySell;
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
