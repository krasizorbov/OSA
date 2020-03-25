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
    using OSA.Web.ViewModels.Purchases.Input_Models;
    using OSA.Web.ViewModels.Purchases.View_Models;

    public class PurchaseController : BaseController
    {
        private const string PurchaseErrorMessage = "There is no available stock! Please check your invoices and register some stocks.";
        private const string PurchaseExistMessage = "Purchases for the current month already done! Check your purchases for more details.";

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

            if (companyNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.CompanyErrorMessage);
            }

            var model = new CreatePurchaseInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreatePurchaseInputModel purchaseInputModel, string startDate, string endDate, int id)
        {
            var companyId = purchaseInputModel.CompanyId;
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var stockNames = await this.purchasesService.GetStockNamesAsync(start_Date, end_Date, companyId);
            var purchaseExist = await this.purchasesService.PurchaseExistAsync(start_Date, end_Date, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (stockNames.Count == 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                var model = new CreatePurchaseInputModel
                {
                    CompanyNames = companyNames,
                };

                this.SetFlash(FlashMessageType.Error, PurchaseErrorMessage);
                return this.View(model);
            }

            if (purchaseExist.Count != 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                var model = new CreatePurchaseInputModel
                {
                    CompanyNames = companyNames,
                };

                this.SetFlash(FlashMessageType.Warning, PurchaseExistMessage);
                return this.View(model);
            }

            await this.purchasesService.AddAsync(
                purchaseInputModel.StartDate,
                purchaseInputModel.EndDate,
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

            var model = new ShowPurchaseByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowPurchaseByCompanyInputModel inputModel)
        {
            var companyId = inputModel.CompanyId;
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(companyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetPurchase", "Purchase", new { id = companyId, name = companyName });
        }

        [Authorize]
        public async Task<IActionResult> GetPurchase(int id, string name)
        {
            var purchases = await this.purchasesService.GetPurchasesByCompanyIdAsync(id);

            var model = new PurchaseBindingViewModel
            {
                Name = name,
                Purchases = purchases,
            };

            return this.View(model);
        }
    }
}
