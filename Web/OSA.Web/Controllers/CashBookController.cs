namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.CashBooks.Input_Models;

    public class CashBookController : Controller
    {
        private readonly ICashBooksService cashBooksService;
        private readonly ICompaniesService companiesService;

        public CashBookController(ICashBooksService cashBooksService, ICompaniesService companiesService)
        {
            this.cashBooksService = cashBooksService;
            this.companiesService = companiesService;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreateCashBookInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateCashBookInputModel cashBookInputModel, string startDate, string endDate, int id)
        {
            var companyId = cashBookInputModel.CompanyId;

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.cashBooksService.AddAsync(
                cashBookInputModel.StartDate,
                cashBookInputModel.EndDate,
                cashBookInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}