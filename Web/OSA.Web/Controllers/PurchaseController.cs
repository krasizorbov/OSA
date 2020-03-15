namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Purchases.Input_Models;

    public class PurchaseController : BaseController
    {
        private readonly IPurchasesService purchasesService;
        private readonly ICompaniesService companiesService;

        public PurchaseController(IPurchasesService purchasesService, ICompaniesService companiesService)
        {
            this.purchasesService = purchasesService;
            this.companiesService = companiesService;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreatePurchaseInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreatePurchaseInputModel purchaseInputModel, string startDate, string endDate, int id, string stockName)
        {
            var companyId = purchaseInputModel.CompanyId;

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.purchasesService.AddAsync(
                purchaseInputModel.StockName,
                purchaseInputModel.StartDate,
                purchaseInputModel.EndDate,
                purchaseInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}