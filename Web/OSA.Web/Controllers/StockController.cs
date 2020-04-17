namespace OSA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Services.Data.Interfaces;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Stocks.Input_Models;
    using OSA.Web.ViewModels.Stocks.View_Models;

    public class StockController : BaseController
    {
        private readonly IStocksService stocksService;
        private readonly ICompaniesService companiesService;
        private readonly IInvoicesService invoicesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public StockController(IStocksService stocksService, ICompaniesService companiesService, IInvoicesService invoicesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.stocksService = stocksService;
            this.invoicesService = invoicesService;
            this.companiesService = companiesService;
            this.dateTimeValidationService = dateTimeValidationService;
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
        public IActionResult AddPartOne(CreateStockInputModelOne inputModelOne)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("AddPartTwo", "Stock", new { id = inputModelOne.CompanyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var companyIdExists = this.stocksService.CompanyIdExists(id);
            if (!companyIdExists)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound", id);
            }

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
        public async Task<IActionResult> AddPartTwo(CreateStockInputModelTwo inputModelTwo, int id, string date)
        {
            var isValidDateTime = this.dateTimeValidationService.IsValidDateTime(date);

            if (!this.ModelState.IsValid || !isValidDateTime)
            {
                var invoices = await this.invoicesService.GetAllInvoicesByCompanyIdAsync(id);

                var model = new CreateStockInputModelTwo
                {
                    Invoices = invoices,
                };

                if (!isValidDateTime)
                {
                    this.ModelState.AddModelError(nameof(inputModelTwo.Date), inputModelTwo.Date + GlobalConstants.InvalidDateTime);
                }

                return this.View(model);
            }

            await this.stocksService.AddAsync(
                inputModelTwo.Name,
                inputModelTwo.Quantity,
                inputModelTwo.Price,
                inputModelTwo.Date,
                inputModelTwo.InvoiceId,
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

            var model = new ShowStockByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowStockByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowStockByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetStock", "Stock", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetStock(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var stocks = await this.stocksService.GetStocksByCompanyIdAsync(start_Date, end_Date, id);

            var model = new StockBindingViewModel
            {
                Name = name,
                Stocks = stocks,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            var stocksToDelete = await this.stocksService.DeleteAsync(ids);

            if (stocksToDelete.Count == 0)
            {
                return this.NotFound();
            }

            this.TempData["message"] = GlobalConstants.SuccessfullyDeleted;
            return this.Redirect("/");
        }
    }
}
