namespace OSA.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using OSA.Data.Common.Repositories;
    using OSA.Data.Models;

    public class PurchasesService : IPurchasesService
    {
        private readonly IDeletableEntityRepository<Purchase> purchaseRepository;

        public PurchasesService(IDeletableEntityRepository<Purchase> purchaseRepository)
        {
            this.purchaseRepository = purchaseRepository;
        }

        public async Task AddAsync(string stockName, decimal totalQuantity, decimal totalPrice, string date, int companyId)
        {
            var purchase = new Purchase
            {
                StockName = stockName,
                TotalQuantity = totalQuantity,
                TotalPrice = totalPrice,
                Date = DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                CompanyId = companyId,
            };
            await this.purchaseRepository.AddAsync(purchase);
            await this.purchaseRepository.SaveChangesAsync();
        }
    }
}
