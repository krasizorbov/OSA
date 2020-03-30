﻿namespace OSA.Web.Controllers
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
    using OSA.Web.ViewModels.CashBooks.Input_Models;
    using OSA.Web.ViewModels.CashBooks.View_Models;

    public class CashBookController : BaseController
    {
        private const string CashBookErrorMessage = "Cash book for the month already done!";

        private readonly ICashBooksService cashBooksService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public CashBookController(ICashBooksService cashBooksService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.cashBooksService = cashBooksService;
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

            var model = new CreateCashBookInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateCashBookInputModel cashBookInputModel, string startDate, string endDate)
        {
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);
            var isValidEndDate = this.dateTimeValidationService.IsValidDateTime(endDate);

            if (!this.ModelState.IsValid || !isValidStartDate || !isValidEndDate)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(cashBookInputModel.StartDate), cashBookInputModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                if (!isValidEndDate)
                {
                    this.ModelState.AddModelError(nameof(cashBookInputModel.EndDate), cashBookInputModel.EndDate + GlobalConstants.InvalidDateTime);
                }

                var model = new CreateCashBookInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var cashBook = await this.cashBooksService.CashBookExistAsync(start_Date, end_Date, cashBookInputModel.CompanyId);
            if (cashBook != null)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                this.SetFlash(FlashMessageType.Error, CashBookErrorMessage);

                var model = new CreateCashBookInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            await this.cashBooksService.AddAsync(
                cashBookInputModel.StartDate,
                cashBookInputModel.EndDate,
                cashBookInputModel.CompanyId);
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

            var model = new ShowCashBookByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowCashBookByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowCashBookByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetCashBook", "CashBook", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetCashBook(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var cashBooks = await this.cashBooksService.GetCashBooksByCompanyIdAsync(start_Date, end_Date, id);

            var model = new CashBookBindingViewModel
            {
                Name = name,
                CashBooks = cashBooks,
            };

            return this.View(model);
        }
    }
}