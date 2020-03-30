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
    using OSA.Web.ViewModels.Receipts.Input_Models;
    using OSA.Web.ViewModels.Receipts.View_Models;

    public class ReceiptController : BaseController
    {
        private const string ReceiptAlreadyExist = " already exists! Please enter a new receipt number.";

        private readonly IReceiptsService receiptsService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public ReceiptController(IReceiptsService receiptsService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.receiptsService = receiptsService;
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

            var model = new CreateReceiptInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateReceiptInputModel receiptInputModel, string date)
        {
            var isValidDateTime = this.dateTimeValidationService.IsValidDateTime(date);

            if (!this.ModelState.IsValid || !isValidDateTime)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                var model = new CreateReceiptInputModel
                {
                    CompanyNames = companyNames,
                };

                if (!isValidDateTime)
                {
                    this.ModelState.AddModelError(nameof(receiptInputModel.Date), receiptInputModel.Date + GlobalConstants.InvalidDateTime);
                }

                return this.View(model);
            }

            var receiptExist = await this.receiptsService.ReceiptExistAsync(receiptInputModel.ReceiptNumber, receiptInputModel.CompanyId);
            if (receiptExist != null)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                this.ModelState.AddModelError(nameof(receiptInputModel.ReceiptNumber), receiptInputModel.ReceiptNumber + ReceiptAlreadyExist);

                var model = new CreateReceiptInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            await this.receiptsService.AddAsync(
                receiptInputModel.ReceiptNumber,
                receiptInputModel.Date,
                receiptInputModel.Salary,
                receiptInputModel.CompanyId);
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

            var model = new ShowReceiptByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowReceiptByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowReceiptByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetReceipt", "Receipt", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetReceipt(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var receipts = await this.receiptsService.GetReceiptsByCompanyIdAsync(start_Date, end_Date, id);

            var model = new ReceiptBindingViewModel
            {
                Name = name,
                Receipts = receipts,
            };

            return this.View(model);
        }
    }
}
