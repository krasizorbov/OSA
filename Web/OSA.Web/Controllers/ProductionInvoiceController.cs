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
    using OSA.Web.ViewModels.ProductionInvoices.Input_Models;
    using OSA.Web.ViewModels.ProductionInvoices.View_Models;

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

            var model = new ShowProductionInvoiceByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowProductionInvoiceByCompanyInputModel inputModel)
        {
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(inputModel.CompanyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetProductionInvoice", "ProductionInvoice", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetProductionInvoice(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var productionInvoices = await this.productionInvoicesService.GetProductionInvoicesByCompanyIdAsync(start_Date, end_Date, id);

            var model = new ProductionInvoiceBindingViewModel
            {
                Name = name,
                ProductionInvoices = productionInvoices,
            };

            return this.View(model);
        }
    }
}
