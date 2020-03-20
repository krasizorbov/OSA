﻿namespace OSA.Web.Controllers
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

    public class AvailableStockController : BaseController
    {
        private const string AvailableStockErrorMessage = "There is no purchases or sells for the current month! Please check your invoices and register some stocks.";

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

            var purchasedStockNamesForCurrentMonth = await this.availableStocksService.GetPurchasedStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);
            var soldStockNamesForCurrentMonth = await this.availableStocksService.GetSoldStockNamesByCompanyIdAsync(start_Date, end_Date, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else if (purchasedStockNamesForCurrentMonth.Count == 0 || soldStockNamesForCurrentMonth.Count == 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateAvailableStockInputModel
                {
                    CompanyNames = companyNames,
                };
                this.SetFlash(FlashMessageType.Error, AvailableStockErrorMessage);
                return this.View(model);
            }

            await this.availableStocksService.AddAsync(
                availableStockInputModel.StartDate,
                availableStockInputModel.EndDate,
                availableStockInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}