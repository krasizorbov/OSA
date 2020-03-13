namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Sells.Input_Models;

    public class SellController : BaseController
    {
        private readonly ISellsService sellsService;
        private readonly ICompaniesService companiesService;
        private readonly IStocksService stocksService;

        public SellController(ISellsService sellsService, ICompaniesService companiesService, IStocksService stocksService)
        {
            this.sellsService = sellsService;
            this.companiesService = companiesService;
            this.stocksService = stocksService;
        }

        [Authorize]
        public async Task<IActionResult> AddPartOne()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreateSellInputModelOne
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPartOne(CreateSellInputModelOne sellInputModelOne)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var companyId = sellInputModelOne.CompanyId;
            return this.RedirectToAction("AddPartTwo", "Sell", new { id = companyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);

            var model = new CreateSellInputModelTwo
            {
                StockNames = stockNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartTwo(CreateSellInputModelTwo sellInputModel, int id)
        {
            var companyId = id;
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.sellsService.AddAsync(
                sellInputModel.StockName,
                sellInputModel.TotalPrice,
                sellInputModel.ProfitPercent,
                sellInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}