namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Receipts.Input_Models;
    using OSA.Web.ViewModels.Receipts.View_Models;

    public class ReceiptController : BaseController
    {
        private const string ReceiptAlreadyExist = " already exists! Please enter a new receipt number.";

        private readonly IReceiptsService receiptsService;
        private readonly ICompaniesService companiesService;

        public ReceiptController(IReceiptsService receiptsService, ICompaniesService companiesService)
        {
            this.receiptsService = receiptsService;
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

            var model = new CreateReceiptInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateReceiptInputModel receiptInputModel, int id)
        {
            var companyId = receiptInputModel.CompanyId;
            var receiptExist = await this.receiptsService.ReceiptExistAsync(receiptInputModel.ReceiptNumber, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

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

            var model = new ShowReceiptByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowReceiptByCompanyInputModel inputModel)
        {
            var companyId = inputModel.CompanyId;
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(companyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetReceipt", "Receipt", new { id = companyId, name = companyName });
        }

        [Authorize]
        public async Task<IActionResult> GetReceipt(int id, string name)
        {
            var receipts = await this.receiptsService.GetReceiptsByCompanyIdAsync(id);

            var model = new ReceiptBindingViewModel
            {
                Name = name,
                Receipts = receipts,
            };

            return this.View(model);
        }
    }
}
