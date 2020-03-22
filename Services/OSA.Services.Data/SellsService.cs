namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class SellsService : ISellsService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<Sell> sellRepository;
        private readonly ApplicationDbContext context;

        public SellsService(IDeletableEntityRepository<Sell> sellRepository, ApplicationDbContext context)
        {
            this.sellRepository = sellRepository;
            this.context = context;
        }

        public async Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId)
        {
            var sell = new Sell
            {
                StockName = stockName,
                TotalPrice = totalPrice,
                ProfitPercent = profitPercent,
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };
            await this.sellRepository.AddAsync(sell);
            await this.sellRepository.SaveChangesAsync();
        }

        public async Task<string> SaleExistAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var stockName = await this.context.Sells
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId)
                .Select(x => x.StockName)
                .FirstOrDefaultAsync();

            return stockName;
        }
    }
}
