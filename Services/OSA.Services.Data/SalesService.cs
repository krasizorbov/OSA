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

    public class SalesService : ISalesService
    {
        private const string DateFormat = "dd/MM/yyyy";
        private readonly IDeletableEntityRepository<Sale> saleRepository;
        private readonly ApplicationDbContext context;

        public SalesService(IDeletableEntityRepository<Sale> saleRepository, ApplicationDbContext context)
        {
            this.saleRepository = saleRepository;
            this.context = context;
        }

        public async Task AddAsync(string stockName, decimal totalPrice, int profitPercent, string date, int companyId)
        {
            var sale = new Sale
            {
                StockName = stockName,
                TotalPrice = totalPrice,
                ProfitPercent = profitPercent,
                Date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };
            await this.saleRepository.AddAsync(sale);
            await this.saleRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sale>> GetSalesByCompanyIdAsync(DateTime startDate, DateTime endDate, int companyId)
        {
            var sales = await this.saleRepository.All().Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == companyId).ToListAsync();

            return sales;
        }

        public async Task<string> SaleExistAsync(DateTime startDate, DateTime endDate, string stockName, int companyId)
        {
            var name = await this.context.Sales
                .Where(x => x.Date >= startDate && x.Date <= endDate && x.StockName == stockName && x.CompanyId == companyId)
                .Select(x => x.StockName)
                .FirstOrDefaultAsync();

            return name;
        }
    }
}
