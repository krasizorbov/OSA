namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Receipts.Input_Models;

    public class ReceiptController : BaseController
    {
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

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.receiptsService.AddAsync(
                receiptInputModel.ReceiptNumber,
                receiptInputModel.Date,
                receiptInputModel.Salary,
                companyId);
            return this.Redirect("/");
        }
    }
}
