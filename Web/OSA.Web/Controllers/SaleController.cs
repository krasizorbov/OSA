namespace OSA.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Sales.Input_Models;
    using OSA.Web.ViewModels.Sales.View_Models;

    public class SaleController : BaseController
    {
        private const string SaleErrorMessage = "Monthly sale for the current stock is already done!";
        private readonly ISalesService salesService;
        private readonly ICompaniesService companiesService;
        private readonly IStocksService stocksService;

        public SaleController(ISalesService salesService, ICompaniesService companiesService, IStocksService stocksService)
        {
            this.salesService = salesService;
            this.companiesService = companiesService;
            this.stocksService = stocksService;
        }

        [Authorize]
        public async Task<IActionResult> AddPartOne()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            if (companyNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.CompanyErrorMessage);
            }

            var model = new CreateSaleInputModelOne
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPartOne(CreateSaleInputModelOne saleInputModelOne)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("AddPartTwo", "Sale", new { id = saleInputModelOne.CompanyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);

            if (stockNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.StockErrorMessage);
            }

            var model = new CreateSaleInputModelTwo
            {
                StockNames = stockNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartTwo(CreateSaleInputModelTwo saleInputModel, string startDate, string endDate, string stockName, int id)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var saleExist = await this.salesService.SaleExistAsync(start_Date, end_Date, stockName, id);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (saleExist != null)
            {
                var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);

                this.SetFlash(FlashMessageType.Error, SaleErrorMessage);

                var model = new CreateSaleInputModelTwo
                {
                    StockNames = stockNames,
                };
                return this.View(model);
            }

            await this.salesService.AddAsync(
                saleInputModel.StockName,
                saleInputModel.TotalPrice,
                saleInputModel.ProfitPercent,
                saleInputModel.EndDate,
                id);
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

            var model = new ShowSaleByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowSaleByCompanyInputModel inputModel)
        {
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(inputModel.CompanyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetSale", "Sale", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetSale(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var sales = await this.salesService.GetSalesByCompanyIdAsync(start_Date, end_Date, id);

            var model = new SaleBindingViewModel
            {
                Name = name,
                Sales = sales,
            };

            return this.View(model);
        }
    }
}
