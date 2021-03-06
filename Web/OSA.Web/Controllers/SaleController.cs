﻿namespace OSA.Web.Controllers
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
    using OSA.Web.ViewModels.Sales.Input_Models;
    using OSA.Web.ViewModels.Sales.View_Models;

    public class SaleController : BaseController
    {
        private const string SaleErrorMessage = "Monthly sale for the current stock is already done!";
        private const string SaleQuantityErrorMessage = "Registration failed! The quantity of the sale is bigger than the quantity of the purchase!";
        private const string DateTimeToStringFormaat = "{0:dd/MM/yyyy}";
        private readonly ISalesService salesService;
        private readonly ICompaniesService companiesService;
        private readonly IStocksService stocksService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public SaleController(ISalesService salesService, ICompaniesService companiesService, IStocksService stocksService, IDateTimeValidationService dateTimeValidationService)
        {
            this.salesService = salesService;
            this.companiesService = companiesService;
            this.stocksService = stocksService;
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
            var companyIdExists = this.salesService.CompanyIdExists(id);
            if (!companyIdExists)
            {
                this.Response.StatusCode = 404;
                return this.View("Error", id);
            }

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
        public async Task<IActionResult> AddPartTwo(CreateSaleInputModelTwo saleInputModelTwo, string startDate, string stockName, int id)
        {
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);

            if (!this.ModelState.IsValid || !isValidStartDate)
            {
                var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(saleInputModelTwo.StartDate), saleInputModelTwo.StartDate + GlobalConstants.InvalidDateTime);
                }

                var model = new CreateSaleInputModelTwo
                {
                    StockNames = stockNames,
                };
                return this.View(model);
            }

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var saleExist = await this.salesService.SaleExistAsync(start_Date, stockName, id);
            if (saleExist != null)
            {
                if (saleExist != null)
                {
                    this.SetFlash(FlashMessageType.Error, SaleErrorMessage);
                }

                var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);
                var model = new CreateSaleInputModelTwo
                {
                    StockNames = stockNames,
                };
                return this.View(model);
            }

            var isTrue = await this.salesService.IsBigger(saleInputModelTwo.TotalPrice, saleInputModelTwo.ProfitPercent, start_Date, stockName, id);
            if (isTrue == true)
            {
                var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);
                this.SetFlash(FlashMessageType.Error, SaleQuantityErrorMessage);

                var model = new CreateSaleInputModelTwo
                {
                    StockNames = stockNames,
                };
                return this.View(model);
            }
            else
            {
                await this.salesService.AddAsync(
                saleInputModelTwo.StockName,
                saleInputModelTwo.TotalPrice,
                saleInputModelTwo.ProfitPercent,
                saleInputModelTwo.StartDate,
                id);
                this.TempData["message"] = GlobalConstants.SuccessfullyRegistered;
                return this.Redirect("/");
            }
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
        public async Task<IActionResult> GetCompany(ShowSaleByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowSaleByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            var salesToDelete = await this.salesService.DeleteAsync(ids);

            if (salesToDelete.Count == 0)
            {
                return this.NotFound();
            }

            this.TempData["message"] = GlobalConstants.SuccessfullyDeleted;
            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var sale = await this.salesService.GetSaleByIdAsync(id);

            var model = new EditSaleViewModel
            {
                Id = sale.Id,
                TotalPrice = sale.TotalPrice,
                ProfitPercent = sale.ProfitPercent,
                StartDate = string.Format(DateTimeToStringFormaat, sale.Date),
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditSaleViewModel editModel)
        {
            var isValidDateTime = this.dateTimeValidationService.IsValidDateTime(editModel.StartDate);

            if (!this.ModelState.IsValid || !isValidDateTime)
            {
                if (!isValidDateTime)
                {
                    this.ModelState.AddModelError(nameof(editModel.StartDate), editModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                return this.View();
            }

            var date = DateTime.ParseExact(editModel.StartDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            await this.salesService.UpdateSaleAsync(editModel.Id, editModel.TotalPrice, editModel.ProfitPercent, date);
            this.TempData["message"] = GlobalConstants.SuccessfullyUpdated;
            return this.Redirect("/");
        }
    }
}
