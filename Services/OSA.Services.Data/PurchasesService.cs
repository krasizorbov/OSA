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

    public class PurchasesService : IPurchasesService
    {
        private readonly IDeletableEntityRepository<Purchase> purchaseRepository;
        private readonly ApplicationDbContext context;
        private IEnumerable<string> stockNamesForCurrentMonth;
        private IEnumerable<string> stockNamesForPreviousMonth;
        private decimal quantityPurchased = 0;
        private decimal quantitySold = 0;
        private decimal totalQuantity = 0;
        private decimal totalPrice = 0;
        private string stockName;
        private DateTime startDate;
        private DateTime endDate;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        private int id;

        public PurchasesService(IDeletableEntityRepository<Purchase> purchaseRepository, ApplicationDbContext context)
        {
            this.purchaseRepository = purchaseRepository;
            this.context = context;
        }

        public async Task AddAsync(string stockName, string startDate, string endDate, string date, int companyId)
        {
            CultureInfo myCultureInfo = new CultureInfo("bg-BG");
            var sstartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", myCultureInfo);
            var eendDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", myCultureInfo);

            this.stockNamesForCurrentMonth = await this.GetStockNamesForCurrentMonthByCompanyIdAsync(sstartDate, eendDate, this.id);
            this.stockNamesForPreviousMonth = await this.GetStockNamesForPrevoiusMonthByCompanyIdAsync(sstartDate, eendDate, this.id);
            List<string> stockNamesCM = this.stockNamesForCurrentMonth.ToList();
            List<string> stockNamesPM = this.stockNamesForPreviousMonth.ToList();
            stockNamesCM.AddRange(stockNamesPM);
            var stockNames = stockNamesCM.Distinct().ToList();

            foreach (var name in stockNames)
            {
                this.quantitySold = await this.QuantitySold(this.id);
                this.quantityPurchased = await this.QuantityPurchased(this.id);
                decimal quantityAvailable = 0;
                if (this.quantityPurchased < this.quantitySold)
                {
                    Console.WriteLine("The quantity sold is bigger than the quantity purchased");
                    continue;
                }
                else
                {
                    quantityAvailable = this.quantityPurchased - this.quantitySold;
                }

                this.totalQuantity = this.TotalQuantity(this.id);
                this.totalQuantity += quantityAvailable;
                this.totalPrice = this.TotalPrice(this.id);

                var purchase = new Purchase
                {
                    StockName = stockName,
                    TotalQuantity = this.totalQuantity,
                    TotalPrice = this.totalPrice,
                    Date = DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CompanyId = companyId,
                };

                await this.purchaseRepository.AddAsync(purchase);
                await this.purchaseRepository.SaveChangesAsync();
            }
        }

        public void GetDates(string startDate, string endDate, int id)
        {
            this.startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.id = id;
        }

        public void GetStockName(string name)
        {
            this.stockName = name;
        }

        public async Task<IEnumerable<string>> GetStockNamesForCurrentMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            this.stockNamesForCurrentMonth = await this.context.Stocks
            .Where(x => x.Date >= startDate && x.Date <= endDate && x.CompanyId == id)
            .Select(x => x.Name)
            .Distinct()
            .ToListAsync();
            return this.stockNamesForCurrentMonth;
        }

        public async Task<IEnumerable<string>> GetStockNamesForPrevoiusMonthByCompanyIdAsync(DateTime startDate, DateTime endDate, int id)
        {
            this.stockNamesForPreviousMonth = await this.context.Stocks
            .Where(x => x.Date >= startDate.AddMonths(-1) && x.Date <= endDate.AddMonths(-1) && x.CompanyId == id)
            .Select(x => x.Name)
            .Distinct()
            .ToListAsync();
            return this.stockNamesForPreviousMonth;
        }

        public async Task<decimal> QuantityPurchased(int id)
        {
            this.quantityPurchased = await this.context.Purchases
            .Where(x => x.Date >= this.startDate.AddMonths(-1) && x.Date <= this.endDate.AddMonths(-1) && x.StockName == this.stockName && x.CompanyId == id)
            .Select(x => x.TotalQuantity)
            .FirstOrDefaultAsync();
            return this.quantityPurchased;
        }

        public async Task<decimal> QuantitySold(int id)
        {
            this.quantitySold = await this.context.Sells
            .Where(x => x.Date >= this.startDate.AddMonths(-1) && x.Date <= this.endDate.AddMonths(-1) && x.StockName == this.stockName && x.CompanyId == id)
            .Select(x => x.TotalQuantity)
            .FirstOrDefaultAsync();
            return this.quantitySold;
        }

        public decimal TotalPrice(int id)
        {
            this.totalPrice = this.context.Stocks
            .Where(x => x.Date >= this.startDate && x.Date <= this.endDate && x.Name == this.stockName && x.CompanyId == id).Sum(p => p.Price);

            return this.totalPrice;
        }

        public decimal TotalQuantity(int id)
        {
            this.totalQuantity = this.context.Stocks
            .Where(x => x.Date >= this.startDate && x.Date <= this.endDate && x.Name == this.stockName && x.CompanyId == id).Sum(q => q.Quantity);

            return this.totalQuantity;
        }
    }
}
