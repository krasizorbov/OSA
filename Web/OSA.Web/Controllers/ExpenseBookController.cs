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
    using OSA.Web.ViewModels.ExpenseBooks.Input_Models;
    using OSA.Web.ViewModels.ExpenseBooks.View_Models;

    public class ExpenseBookController : BaseController
    {
        private const string ExpenseBookErrorMessage = "Expense book for the month already done!";
        private const string ProductionInvoiceErrorMessage = "Please register a production invoice before proceeding!";
        private const string AvailableStockErrorMessage = "There is no Monthly Available Stock! Please register!";

        private readonly IExpenseBooksService expenseBooksService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public ExpenseBookController(IExpenseBooksService expenseBooksService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.expenseBooksService = expenseBooksService;
            this.companiesService = companiesService;
            this.dateTimeValidationService = dateTimeValidationService;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreateExpenseBookInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateExpenseBookInputModel expenseBookInputModel, string startDate, string endDate)
        {
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);
            var isValidEndDate = this.dateTimeValidationService.IsValidDateTime(endDate);

            if (!this.ModelState.IsValid || !isValidStartDate || !isValidEndDate)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(expenseBookInputModel.StartDate), expenseBookInputModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                if (!isValidEndDate)
                {
                    this.ModelState.AddModelError(nameof(expenseBookInputModel.EndDate), expenseBookInputModel.EndDate + GlobalConstants.InvalidDateTime);
                }

                var model = new CreateExpenseBookInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var expenseBook = await this.expenseBooksService.ExpenseBookExistAsync(start_Date, end_Date, expenseBookInputModel.CompanyId);
            var productionInvoices = await this.expenseBooksService.GetAllProductionInvoicesByMonthAsync(start_Date, end_Date, expenseBookInputModel.CompanyId);
            var availableStock = await this.expenseBooksService.GetMonthlyAvailableStockByCompanyIdAsync(start_Date, end_Date, expenseBookInputModel.CompanyId);
            if (expenseBook != null || productionInvoices.Count == 0 || availableStock == null)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                if (expenseBook != null)
                {
                    this.SetFlash(FlashMessageType.Error, ExpenseBookErrorMessage);
                }

                if (productionInvoices.Count == 0)
                {
                    this.SetFlash(FlashMessageType.Error, ProductionInvoiceErrorMessage);
                }

                if (availableStock == null)
                {
                    this.SetFlash(FlashMessageType.Error, AvailableStockErrorMessage);
                }

                var model = new CreateExpenseBookInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            await this.expenseBooksService.AddAsync(
                expenseBookInputModel.StartDate,
                expenseBookInputModel.EndDate,
                expenseBookInputModel.CompanyId);
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

            var model = new ShowExpenseBookByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowExpenseBookByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowExpenseBookByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetExpenseBook", "ExpenseBook", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetExpenseBook(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var expenseBooks = await this.expenseBooksService.GetExpenseBooksByCompanyIdAsync(start_Date, end_Date, id);

            var model = new ExpenseBookBindingViewModel
            {
                Name = name,
                ExpenseBooks = expenseBooks,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var expenseBook = await this.expenseBooksService.GetExpenseBookByIdAsync(id);

            if (expenseBook == null)
            {
                return this.NotFound();
            }

            var expenseBookToDelete = await this.expenseBooksService.DeleteAsync(id);

            if (expenseBookToDelete == null)
            {
                return this.NotFound();
            }

            this.TempData["message"] = GlobalConstants.SuccessfullyDeleted;
            return this.Redirect("/");
        }
    }
}
