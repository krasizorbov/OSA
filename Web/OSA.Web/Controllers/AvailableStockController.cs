namespace OSA.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Services.Data.Interfaces;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.AvailableStocks.Input_Models;
    using OSA.Web.ViewModels.AvailableStocks.View_Models;

    public class AvailableStockController : BaseController
    {
        private const string AvailableStockExistMessage = "Available stock for the month already done!";
        private const string NoPurchaseNoSaleMessage = "Reister a sale and (or) a purchase please!";

        private readonly IAvailableStocksService availableStocksService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public AvailableStockController(IAvailableStocksService availableStocksService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.availableStocksService = availableStocksService;
            this.companiesService = companiesService;
            this.dateTimeValidationService = dateTimeValidationService;
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
        public async Task<IActionResult> Add(CreateAvailableStockInputModel availableStockInputModel, string startDate, string endDate)
        {
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);
            var isValidEndDate = this.dateTimeValidationService.IsValidDateTime(endDate);

            if (!this.ModelState.IsValid || !isValidStartDate || !isValidEndDate)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(availableStockInputModel.StartDate), availableStockInputModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                if (!isValidEndDate)
                {
                    this.ModelState.AddModelError(nameof(availableStockInputModel.EndDate), availableStockInputModel.EndDate + GlobalConstants.InvalidDateTime);
                }

                var model = new CreateAvailableStockInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var stockNames = await this.availableStocksService.AvailableStockExistAsync(start_Date, end_Date, availableStockInputModel.CompanyId);
            var purchasedStockNamesForCurrentMonth = await this.availableStocksService.GetPurchasedStockNamesByCompanyIdAsync(start_Date, end_Date, availableStockInputModel.CompanyId);
            var soldStockNamesForCurrentMonth = await this.availableStocksService.GetSoldStockNamesByCompanyIdAsync(start_Date, end_Date, availableStockInputModel.CompanyId);

            if (stockNames.Count != 0 || (purchasedStockNamesForCurrentMonth.Count == 0 && soldStockNamesForCurrentMonth.Count == 0))
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateAvailableStockInputModel
                {
                    CompanyNames = companyNames,
                };

                if (stockNames.Count != 0)
                {
                    this.SetFlash(FlashMessageType.Error, AvailableStockExistMessage);
                }

                if (purchasedStockNamesForCurrentMonth.Count == 0 && soldStockNamesForCurrentMonth.Count == 0)
                {
                    this.SetFlash(FlashMessageType.Error, NoPurchaseNoSaleMessage);
                }

                return this.View(model);
            }

            await this.availableStocksService.AddAsync(
                availableStockInputModel.StartDate,
                availableStockInputModel.EndDate,
                availableStockInputModel.CompanyId);

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
        public async Task<IActionResult> GetCompany(ShowAvailableStockByCompanyInputModel inputModel, string startDate, string endDate)
        {
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(inputModel.CompanyId);
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);
            var isValidEndDate = this.dateTimeValidationService.IsValidDateTime(endDate);

            if (!this.ModelState.IsValid || !isValidStartDate || !isValidEndDate)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(inputModel.StartDate), inputModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                if (!isValidEndDate)
                {
                    this.ModelState.AddModelError(nameof(inputModel.EndDate), inputModel.EndDate + GlobalConstants.InvalidDateTime);
                }

                var model = new ShowAvailableStockByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetAvailableStock", "AvailableStock", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetAvailableStock(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var availableStocks = await this.availableStocksService.GetAvailableStocksForCurrentMonthByCompanyIdAsync(start_Date, end_Date, id);

            var model = new AvailableStockBindingViewModel
            {
                Name = name,
                AvailableStocks = availableStocks,
            };

            return this.View(model);
        }
    }
}
