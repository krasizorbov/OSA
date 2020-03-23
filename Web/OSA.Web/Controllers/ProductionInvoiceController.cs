namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.ProductionInvoices.Input_Models;

    public class ProductionInvoiceController : BaseController
    {
        private const string InvoiceAlreadyExist = " already exists! Please enter a new invoice number.";
        private readonly IProductionInvoicesService productionInvoicesService;
        private readonly ICompaniesService companiesService;

        public ProductionInvoiceController(IProductionInvoicesService productionInvoicesService, ICompaniesService companiesService)
        {
            this.productionInvoicesService = productionInvoicesService;
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

            var model = new CreateProductionInvoiceInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateProductionInvoiceInputModel productionInvoiceInputModel, int id)
        {
            var companyId = productionInvoiceInputModel.CompanyId;
            var invoiceExist = await this.productionInvoicesService.InvoiceExistAsync(productionInvoiceInputModel.InvoiceNumber, companyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (invoiceExist != null)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                this.ModelState.AddModelError(nameof(productionInvoiceInputModel.InvoiceNumber), productionInvoiceInputModel.InvoiceNumber + InvoiceAlreadyExist);

                var model = new CreateProductionInvoiceInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            await this.productionInvoicesService.AddAsync(
                productionInvoiceInputModel.InvoiceNumber,
                productionInvoiceInputModel.Date,
                productionInvoiceInputModel.StockCost,
                productionInvoiceInputModel.ExternalCost,
                companyId);
            return this.Redirect("/");
        }
    }
}
