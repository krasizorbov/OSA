namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OSA.Data;
    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class AvailableStocksService : IAvailableStocksService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<AvailableStock> availableStockRepository;
        private readonly ApplicationDbContext context;

        public AvailableStocksService(IDeletableEntityRepository<AvailableStock> availableStockRepository, ApplicationDbContext context)
        {
            this.availableStockRepository = availableStockRepository;
            this.context = context;
        }

        public async Task AddAsync(string startDate, string endDate, string date, int companyId)
        {
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);
        }

        public Task<BookValue> GetCurrentBookValueForStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> GetCurrentPurchasedStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Sell> GetCurrentSoldStockNameAsync(DateTime startDate, DateTime endDate, string stockName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetPurchasedStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetSoldStockNamesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
