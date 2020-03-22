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
            var purchaseExist = await this.purchasesService.PurchaseExist(start_Date, end_Date, companyId);

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
                purchaseInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}
