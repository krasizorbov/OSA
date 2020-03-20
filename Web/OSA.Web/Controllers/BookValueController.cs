namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.BookValues.Input_Models;

    public class BookValueController : BaseController
    {
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

            if (!this.ModelState.IsValid)
            {
                return this.View();
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