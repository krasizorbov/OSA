namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Purchases.Input_Models;

    public class PurchaseController : BaseController
    {
        private readonly IPurchasesService purchasesService;
        private readonly ICompaniesService companiesService;
        private readonly IStocksService stocksService;
        private readonly IPurchasesService purchaseService;

        public PurchaseController(IPurchasesService purchasesService, ICompaniesService companiesService, IStocksService stocksService, IPurchasesService purchaseService)
        {
            this.purchasesService = purchasesService;
            this.companiesService = companiesService;
            this.stocksService = stocksService;
            this.purchaseService = purchaseService;
        }

        [Authorize]
        public async Task<IActionResult> AddPartOne()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreatePurchaseInputModelOne
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPartOne(CreatePurchaseInputModelOne purchaseInputModelOne, int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var companyId = purchaseInputModelOne.CompanyId;

            return this.RedirectToAction("AddPartTwo", "Purchase", new { id = companyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);

            var model = new CreatePurchaseInputModelTwo
            {
                StockNames = stockNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartTwo(CreatePurchaseInputModelTwo purchaseInputModelTwo, string startDate, string endDate, int id, string stockName)
        {
            var companyId = id;
            this.purchaseService.GetDates(startDate, endDate, companyId);
            this.purchaseService.GetStockName(stockName);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.purchasesService.AddAsync(
                purchaseInputModelTwo.StockName,
                purchaseInputModelTwo.StartDate,
                purchaseInputModelTwo.EndDate,
                purchaseInputModelTwo.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}