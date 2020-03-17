namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.ExpenseBooks.Input_Models;

    public class ExpenseBookController : BaseController
    {
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
            var companyId = expenseBookInputModel.CompanyId;

            if (!this.ModelState.IsValid)
            {
                return this.View();
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