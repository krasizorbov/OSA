namespace OSA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Purchases.Input_Models;

    public class PurchaseController : BaseController
    {
        private const string DateFormat = "dd/MM/yyyy";
        private const string PurchaseErrorMessage = "There is no available stock! Please check your invoices.";

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
        public async Task<IActionResult> Add(CreatePurchaseInputModel purchaseInputModel, string startDate, string endDate, int id)
        {
            var companyId = purchaseInputModel.CompanyId;
            var start_Date = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);
            List<string> stockNamesCurrentMonth = new List<string>(await this.purchasesService.GetStockNamesForCurrentMonthByCompanyIdAsync(start_Date, end_Date, companyId));
            List<string> stockNamesPreviousMonth = new List<string>(await this.purchasesService.GetStockNamesForPrevoiusMonthByCompanyIdAsync(start_Date, end_Date, companyId));

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else if (stockNamesCurrentMonth.Count == 0 && stockNamesPreviousMonth.Count == 0)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                var model = new CreatePurchaseInputModel
                {
                    CompanyNames = companyNames,
                };

                this.SetFlash(FlashMessageType.Error, PurchaseErrorMessage);
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
