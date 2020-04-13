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
    using OSA.Web.ViewModels.ProductionInvoices.Input_Models;
    using OSA.Web.ViewModels.ProductionInvoices.View_Models;

    public class ProductionInvoiceController : BaseController
    {
        private const string InvoiceAlreadyExist = " already exists! Please enter a new invoice number.";
        private readonly IProductionInvoicesService productionInvoicesService;
        private readonly ICompaniesService companiesService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public ProductionInvoiceController(IProductionInvoicesService productionInvoicesService, ICompaniesService companiesService, IDateTimeValidationService dateTimeValidationService)
        {
            this.productionInvoicesService = productionInvoicesService;
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

            var model = new CreateProductionInvoiceInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateProductionInvoiceInputModel productionInvoiceInputModel, string date)
        {
            var isValidDateTime = this.dateTimeValidationService.IsValidDateTime(date);

            if (!this.ModelState.IsValid || !isValidDateTime)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateProductionInvoiceInputModel
                {
                    CompanyNames = companyNames,
                };

                if (!isValidDateTime)
                {
                    this.ModelState.AddModelError(nameof(productionInvoiceInputModel.Date), productionInvoiceInputModel.Date + GlobalConstants.InvalidDateTime);
                }

                return this.View(model);
            }

            var invoiceExist = await this.productionInvoicesService.InvoiceExistAsync(productionInvoiceInputModel.InvoiceNumber, productionInvoiceInputModel.CompanyId);
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
                productionInvoiceInputModel.Salary,
                productionInvoiceInputModel.ExternalCost,
                productionInvoiceInputModel.CompanyId);
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
        public async Task<IActionResult> GetCompany(ShowProductionInvoiceByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowProductionInvoiceByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productionInvoice = await this.productionInvoicesService.GetProductionInvoiceByIdAsync(id);

            if (productionInvoice == null)
            {
                return this.NotFound();
            }

            var productionInvoiceToDelete = await this.productionInvoicesService.DeleteAsync(id);

            if (productionInvoiceToDelete == null)
            {
                return this.NotFound();
            }

            this.TempData["message"] = GlobalConstants.SuccessfullyDeleted;
            return this.Redirect("/");
        }
    }
}
