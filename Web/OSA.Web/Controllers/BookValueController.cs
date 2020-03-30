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
    using OSA.Web.ViewModels.BookValues.Input_Models;
    using OSA.Web.ViewModels.BookValues.View_Models;

    public class BookValueController : BaseController
    {
        private const string BookValueErrorMessage = "There is no monthly sale! Please register a sale before proceeding with your book values.";
        private const string BookValueAlreadyExistMessage = "Book values already done for the month and company selected!";

        private readonly IBookValuesService bookValuesService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public BookValueController(IBookValuesService bookValuesService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.bookValuesService = bookValuesService;
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

            var model = new CreateBookValueInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateBookValueInputModel bookValueInputModel, string startDate, string endDate)
        {
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);
            var isValidEndDate = this.dateTimeValidationService.IsValidDateTime(endDate);

            if (!this.ModelState.IsValid || !isValidStartDate || !isValidEndDate)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(bookValueInputModel.StartDate), bookValueInputModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                if (!isValidEndDate)
                {
                    this.ModelState.AddModelError(nameof(bookValueInputModel.EndDate), bookValueInputModel.EndDate + GlobalConstants.InvalidDateTime);
                }

                var model = new CreateBookValueInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var monthlySales = await this.bookValuesService.GetMonthlySalesAsync(start_Date, end_Date, bookValueInputModel.CompanyId);
            var stockNames = await this.bookValuesService.BookValueExistAsync(start_Date, end_Date, bookValueInputModel.CompanyId);

            if (monthlySales.Count == 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateBookValueInputModel
                {
                    CompanyNames = companyNames,
                };
                this.SetFlash(FlashMessageType.Error, BookValueErrorMessage);
                return this.View(model);
            }

            if (stockNames.Count != 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateBookValueInputModel
                {
                    CompanyNames = companyNames,
                };
                this.SetFlash(FlashMessageType.Error, BookValueAlreadyExistMessage);
                return this.View(model);
            }

            await this.bookValuesService.AddAsync(
                bookValueInputModel.StartDate,
                bookValueInputModel.EndDate,
                bookValueInputModel.CompanyId);
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

            var model = new ShowBookValueByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowBookValueByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowBookValueByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetBookValue", "BookValue", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetBookValue(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var bookValues = await this.bookValuesService.GetBookValuesByCompanyIdAsync(start_Date, end_Date, id);

            var model = new BookValueBindingViewModel
            {
                Name = name,
                BookValues = bookValues,
            };

            return this.View(model);
        }
    }
}