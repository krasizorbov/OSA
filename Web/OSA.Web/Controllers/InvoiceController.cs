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
    using OSA.Web.ViewModels.Invoices.Input_Models;
    using OSA.Web.ViewModels.Invoices.View_Models;

    public class InvoiceController : BaseController
    {
        private const string InvoiceAlreadyExist = " already exists! Please enter a new invoice number.";

        private readonly IInvoicesService invoicesService;
        private readonly ICompaniesService companiesService;
        private readonly ISuppliersService suppliersService;
        private readonly IDateTimeValidationService dateTimeValidationService;

        public InvoiceController(IInvoicesService invoicesService, ICompaniesService companiesService, ISuppliersService suppliersService, IDateTimeValidationService dateTimeValidationService)
        {
            this.invoicesService = invoicesService;
            this.companiesService = companiesService;
            this.suppliersService = suppliersService;
            this.dateTimeValidationService = dateTimeValidationService;
        }

        [Authorize]
        public async Task<IActionResult> AddPartOne()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            if (companyNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.CompanyErrorMessage);
            }

            var model = new CreateInvoiceInputModelOne
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPartOne(CreateInvoiceInputModelOne invoiceInputModelOne)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("AddPartTwo", "Invoice", new { id = invoiceInputModelOne.CompanyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var companyIdExists = this.invoicesService.CompanyIdExists(id);
            if (!companyIdExists)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound", id);
            }

            var supplierNames = await this.suppliersService.GetAllSuppliersByCompanyIdAsync(id);

            if (supplierNames.Count == 0)
            {
                this.SetFlash(FlashMessageType.Error, GlobalConstants.SupplierErrorMessage);
            }

            var model = new CreateInvoiceInputModelTwo
            {
                SupplierNames = supplierNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartTwo(CreateInvoiceInputModelTwo invoiceInputModelTwo, int id, string date)
        {
            var invoiceExist = await this.invoicesService.InvoiceExistAsync(invoiceInputModelTwo.InvoiceNumber, id);
            var isValidDateTime = this.dateTimeValidationService.IsValidDateTime(date);
            var supplierNames = await this.suppliersService.GetAllSuppliersByCompanyIdAsync(id);

            if (!this.ModelState.IsValid || !isValidDateTime)
            {
                var model = new CreateInvoiceInputModelTwo
                {
                    SupplierNames = supplierNames,
                };

                if (!isValidDateTime)
                {
                    this.ModelState.AddModelError(nameof(invoiceInputModelTwo.Date), invoiceInputModelTwo.Date + GlobalConstants.InvalidDateTime);
                }

                return this.View(model);
            }

            if (invoiceExist != null)
            {
                this.ModelState.AddModelError(nameof(invoiceInputModelTwo.InvoiceNumber), invoiceInputModelTwo.InvoiceNumber + InvoiceAlreadyExist);

                var model = new CreateInvoiceInputModelTwo
                {
                    SupplierNames = supplierNames,
                };
                return this.View(model);
            }

            await this.invoicesService.AddAsync(
                invoiceInputModelTwo.InvoiceNumber,
                invoiceInputModelTwo.TotalAmount,
                invoiceInputModelTwo.Date,
                invoiceInputModelTwo.SupplierId,
                id);
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

            var model = new ShowInvoiceByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowInvoiceByCompanyInputModel inputModel, string startDate, string endDate)
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

                var model = new ShowInvoiceByCompanyInputModel
                {
                    CompanyNames = companyNames,
                };
                return this.View(model);
            }

            return this.RedirectToAction("GetInvoice", "Invoice", new
            {
                id = inputModel.CompanyId,
                name = companyName,
                startDate = inputModel.StartDate,
                endDate = inputModel.EndDate,
            });
        }

        [Authorize]
        public async Task<IActionResult> GetInvoice(int id, string name, string startDate, string endDate)
        {
            var start_Date = DateTime.ParseExact(startDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);
            var end_Date = DateTime.ParseExact(endDate, GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

            var invoices = await this.invoicesService.GetInvoicesByCompanyIdAsync(start_Date, end_Date, id);

            var model = new InvoiceBindingViewModel
            {
                Name = name,
                Invoices = invoices,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await this.invoicesService.GetInvoiceByIdAsync(id);

            if (invoice == null)
            {
                return this.NotFound();
            }

            var invoiceToDelete = await this.invoicesService.DeleteAsync(id);

            if (invoiceToDelete == null)
            {
                return this.NotFound();
            }

            this.TempData["message"] = GlobalConstants.SuccessfullyDeleted;
            return this.Redirect("/");
        }
    }
}
