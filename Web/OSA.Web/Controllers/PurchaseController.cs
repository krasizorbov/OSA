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
    using OSA.Web.ViewModels.Purchases.Input_Models;
    using OSA.Web.ViewModels.Purchases.View_Models;

    public class PurchaseController : BaseController
    {
        private const string PurchaseErrorMessage = "There is no available stock! Please check your invoices and register some stocks.";
        private const string PurchaseExistMessage = "Purchases for the current month already done! Check your purchases for more details.";

        private readonly IPurchasesService purchasesService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public PurchaseController(IPurchasesService purchasesService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.purchasesService = purchasesService;
            this.companiesService = companiesService;
            this.dateTimeValidationService = dateTimeValidationService;
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
        public async Task<IActionResult> Add(CreatePurchaseInputModel purchaseInputModel, string startDate, string endDate)
        {
            var isValidStartDate = this.dateTimeValidationService.IsValidDateTime(startDate);
            var isValidEndDate = this.dateTimeValidationService.IsValidDateTime(endDate);
            if (!this.ModelState.IsValid || !isValidStartDate || !isValidEndDate)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                if (!isValidStartDate)
                {
                    this.ModelState.AddModelError(nameof(purchaseInputModel.StartDate), purchaseInputModel.StartDate + GlobalConstants.InvalidDateTime);
                }

                if (!isValidEndDate)
                {
                    this.ModelState.AddModelError(nameof(purchaseInputModel.EndDate), purchaseInputModel.EndDate + GlobalConstants.InvalidDateTime);
                }

                var model = new CreatePurchaseInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var stockNames = await this.purchasesService.GetStockNamesAsync(start_Date, end_Date, purchaseInputModel.CompanyId);
            var purchaseExist = await this.purchasesService.PurchaseExistAsync(start_Date, end_Date, purchaseInputModel.CompanyId);

            if (stockNames.Count == 0) // Check This!!!
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                var model = new CreatePurchaseInputModel
                {
                    CompanyNames = companyNames,
                };

                this.SetFlash(FlashMessageType.Error, PurchaseErrorMessage);
                return this.View(model);
            }

            if (purchaseExist.Count != 0) // Check This!!!
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
                purchaseInputModel.CompanyId);
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
        public async Task<IActionResult> GetCompany(ShowPurchaseByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowPurchaseByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetPurchase", "Purchase", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetPurchase(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var purchases = await this.purchasesService.GetPurchasesByCompanyIdAsync(start_Date, end_Date, id);

            var model = new PurchaseBindingViewModel
            {
                Name = name,
                Purchases = purchases,
            };

            return this.View(model);
        }
    }
}
