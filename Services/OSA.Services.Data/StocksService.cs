namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class StocksService : IStocksService
    {
        private readonly IDeletableEntityRepository<Stock> stockRepository;

        public StocksService(IDeletableEntityRepository<Stock> stockRepository)
        {
            this.stockRepository = stockRepository;
        }

        public async Task AddAsync(string name, decimal quantity, decimal price, string date, int invoiceId, int companyId)
        {
            var stock = new Stock
            {
                Name = name,
                Quantity = quantity,
                Price = price,
                Date = DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                InvoiceId = invoiceId,
                CompanyId = companyId,
            };

            await this.stockRepository.AddAsync(stock);
            await this.stockRepository.SaveChangesAsync();
        }
    }
}
