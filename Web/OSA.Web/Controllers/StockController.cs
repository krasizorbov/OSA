namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Stocks.Input_Models;
    using OSA.Web.ViewModels.Stocks.View_Models;

    public class StockController : BaseController
    {
        private readonly IStocksService stocksService;
        private readonly ICompaniesService companiesService;
        private readonly IInvoicesService invoicesService;

        public StockController(IStocksService stocksService, ICompaniesService companiesService, IInvoicesService invoicesService)
        {
            this.stocksService = stocksService;
            this.invoicesService = invoicesService;
            this.companiesService = companiesService;
        }

        [Authorize]
        public async Task<IActionResult> AddPartOne()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            if (companyNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.CompanyErrorMessage);
            }

            var model = new CreateStockInputModelOne
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPartOne(CreateStockInputModelOne stockInputModelOne)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var companyId = stockInputModelOne.CompanyId;
            return this.RedirectToAction("AddPartTwo", "Stock", new { id = companyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var invoices = await this.invoicesService.GetAllInvoicesByCompanyIdAsync(id);

            if (invoices.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.InvoiceErrorMessage);
            }

            var model = new CreateStockInputModelTwo
            {
                Invoices = invoices,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartTwo(CreateStockInputModelTwo stockInputModelTwo, int id)
        {
            var companyId = id;
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.stocksService.AddAsync(
                stockInputModelTwo.Name,
                stockInputModelTwo.Quantity,
                stockInputModelTwo.Price,
                stockInputModelTwo.Date,
                stockInputModelTwo.InvoiceId,
                companyId);
            this.TempData["message"] = GlobalConstants.SuccessfullyRegistered;
            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> GetCompany()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            if (companyNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.CompanyErrorMessage);
            }

            var model = new ShowStockByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowStockByCompanyInputModel inputModel)
        {
            var companyId = inputModel.CompanyId;
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(companyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetStock", "Stock", new { id = companyId, name = companyName });
        }

        [Authorize]
        public async Task<IActionResult> GetStock(int id, string name)
        {
            var stocks = await this.stocksService.GetStocksByCompanyIdAsync(id);

            var model = new StockBindingViewModel
            {
                Name = name,
                Stocks = stocks,
            };

            return this.View(model);
        }
    }
}
