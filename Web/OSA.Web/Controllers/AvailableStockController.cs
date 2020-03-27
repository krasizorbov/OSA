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
    using OSA.Web.ViewModels.AvailableStocks.Input_Models;
    using OSA.Web.ViewModels.AvailableStocks.View_Models;

    public class AvailableStockController : BaseController
    {
        private const string AvailableStockErrorMessage = "There is no sales for the current month! Please register a sale!";
        private const string AvailableStockExistMessage = "Available stock for the month already done!";

        private readonly IAvailableStocksService availableStocksService;
        private readonly ICompaniesService companiesService;

        public AvailableStockController(IAvailableStocksService availableStocksService, ICompaniesService companiesService)
        {
            this.availableStocksService = availableStocksService;
            this.companiesService = companiesService;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            if (companyNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.CompanyErrorMessage);
            }

            var model = new CreateAvailableStockInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateAvailableStockInputModel availableStockInputModel, string startDate, string endDate, int id)
        {
            var companyId = availableStockInputModel.CompanyId;

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var soldStockNamesForCurrentMonth = await this.availableStocksService.GetSoldStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);
            var stockNames = await this.availableStocksService.AvailableStockExistAsync(start_Date, end_Date, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (soldStockNamesForCurrentMonth.Count == 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateAvailableStockInputModel
                {
                    CompanyNames = companyNames,
                };
                this.SetFlash(FlashMessageType.Error, AvailableStockErrorMessage);
                return this.View(model);
            }

            if (stockNames.Count != 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateAvailableStockInputModel
                {
                    CompanyNames = companyNames,
                };
                this.SetFlash(FlashMessageType.Error, AvailableStockExistMessage);
                return this.View(model);
            }

            await this.availableStocksService.AddAsync(
                availableStockInputModel.StartDate,
                availableStockInputModel.EndDate,
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

            var model = new ShowAvailableStockByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowAvailableStockByCompanyInputModel inputModel)
        {
            var companyId = inputModel.CompanyId;
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(companyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetAvailableStock", "AvailableStock", new { id = companyId, name = companyName });
        }

        [Authorize]
        public async Task<IActionResult> GetAvailableStock(int id, string name)
        {
            var availableStocks = await this.availableStocksService.GetAvailableStocksByCompanyIdAsync(id);

            var model = new AvailableStockBindingViewModel
            {
                Name = name,
                AvailableStocks = availableStocks,
            };

            return this.View(model);
        }
    }
}