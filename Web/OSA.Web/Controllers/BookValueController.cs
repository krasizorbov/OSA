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

    public class BookValueController : BaseController
    {
        private const string BookValueErrorMessage = "There is no monthly sale! Please register a sale before proceeding with your book values.";
        private const string BookValueAlreadyExistMessage = "Book values already done for the month selected!";

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
                bookValueInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}