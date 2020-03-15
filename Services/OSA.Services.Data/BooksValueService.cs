namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class BooksValueService : IBooksValueService
    {
        private readonly IDeletableEntityRepository<BookValue> bookValueRepository;
        private readonly ApplicationDbContext context;
        private decimal price = 0;

        public BooksValueService(IDeletableEntityRepository<BookValue> bookValueRepository, ApplicationDbContext context)
        {
            this.bookValueRepository = bookValueRepository;
            this.context = context;
        }

        public Task AddAsync(decimal price, string stockName, string date, int companyId)
        {
            throw new NotImplementedException();
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
