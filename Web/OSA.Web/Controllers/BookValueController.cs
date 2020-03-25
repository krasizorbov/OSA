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
    using OSA.Web.ViewModels.BookValues.Input_Models;
    using OSA.Web.ViewModels.BookValues.View_Models;

    public class BookValueController : BaseController
    {
        private const string BookValueErrorMessage = "There is no monthly sale! Please register a sale before proceeding with your book values.";
        private const string BookValueAlreadyExistMessage = "Book values already done for the company selected!";

        private readonly IBookValuesService bookValuesService;
        private readonly ICompaniesService companiesService;

        public BookValueController(IBookValuesService bookValuesService, ICompaniesService companiesService)
        {
            this.bookValuesService = bookValuesService;
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

            var model = new CreateBookValueInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateBookValueInputModel bookValueInputModel, string startDate, string endDate, int id)
        {
            var companyId = bookValueInputModel.CompanyId;

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var monthlySales = await this.bookValuesService.GetMonthlySalesAsync(start_Date, end_Date, companyId);
            var stockNames = await this.bookValuesService.BookValueExistAsync(start_Date, end_Date, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (monthlySales.Count == 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateBookValueInputModel
                {
                    CompanyNames = companyNames,
                };
                this.SetFlash(FlashMessageType.Warning, BookValueErrorMessage);
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

            var model = new ShowBookValueByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowBookValueByCompanyInputModel inputModel)
        {
            var companyId = inputModel.CompanyId;
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(companyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetBookValue", "BookValue", new { id = companyId, name = companyName });
        }

        [Authorize]
        public async Task<IActionResult> GetBookValue(int id, string name)
        {
            var bookValues = await this.bookValuesService.GetBookValuesByCompanyIdAsync(id);

            var model = new BookValueBindingViewModel
            {
                Name = name,
                BookValues = bookValues,
            };

            return this.View(model);
        }
    }
}