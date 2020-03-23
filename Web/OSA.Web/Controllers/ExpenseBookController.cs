namespace OSA.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.ExpenseBooks.Input_Models;

    public class ExpenseBookController : BaseController
    {
        private const string DateFormat = "dd/MM/yyyy";
        private const string ExpenseBookErrorMessage = "Expense book for the month already done!";

        private readonly IExpenseBooksService expenseBooksService;
        private readonly ICompaniesService companiesService;

        public ExpenseBookController(IExpenseBooksService expenseBooksService, ICompaniesService companiesService)
        {
            this.expenseBooksService = expenseBooksService;
            this.companiesService = companiesService;
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
        public async Task<IActionResult> Add(CreateExpenseBookInputModel expenseBookInputModel, string startDate, string endDate, int id)
        {
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);

            var companyId = expenseBookInputModel.CompanyId;
            var expenseBook = await this.expenseBooksService.ExpenseBookExistAsync(start_Date, end_Date, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (expenseBook != null)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                this.SetFlash(FlashMessageType.Error, ExpenseBookErrorMessage);

                var model = new CreateExpenseBookInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            await this.expenseBooksService.AddAsync(
                expenseBookInputModel.StartDate,
                expenseBookInputModel.EndDate,
                expenseBookInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}