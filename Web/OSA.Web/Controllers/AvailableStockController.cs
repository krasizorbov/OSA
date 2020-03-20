namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.AvailableStocks.Input_Models;

    public class AvailableStockController : BaseController
    {
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

            if (!this.ModelState.IsValid)
            {
                return this.View();
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