namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class SellsService : ISellsService
    {
        private readonly IDeletableEntityRepository<Sell> sellRepository;

        public SellsService(IDeletableEntityRepository<Sell> sellRepository)
        {
            this.sellRepository = sellRepository;
        }

        public async Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId)
        {
            var sell = new Sell
            {
                StockName = stockName,
                TotalPrice = totalPrice,
                ProfitPercent = profitPercent,
                Date = DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };

            await this.sellRepository.AddAsync(sell);
            await this.sellRepository.SaveChangesAsync();
        }
    }
}
