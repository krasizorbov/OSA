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
    using OSA.Web.ViewModels.Invoices.Input_Models;
    using OSA.Web.ViewModels.Invoices.View_Models;

    public class InvoiceController : BaseController
    {
        private const string InvoiceAlreadyExist = " already exists! Please enter a new invoice number.";

        private readonly IInvoicesService invoicesService;
        private readonly ICompaniesService companiesService;
        private readonly ISuppliersService suppliersService;

        public InvoiceController(IInvoicesService invoicesService, ICompaniesService companiesService, ISuppliersService suppliersService)
        {
            this.invoicesService = invoicesService;
            this.companiesService = companiesService;
            this.suppliersService = suppliersService;
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

            var companyId = invoiceInputModelOne.CompanyId;
            return this.RedirectToAction("AddPartTwo", "Invoice", new { id = companyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id, string date)
        {
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
        public async Task<IActionResult> AddPartTwo(CreateInvoiceInputModelTwo invoiceInputModelTwo, int id)
        {
            var invoiceExist = await this.invoicesService.InvoiceExistAsync(invoiceInputModelTwo.InvoiceNumber, id);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (invoiceExist != null)
            {
                var supplierNames = await this.suppliersService.GetAllSuppliersByCompanyIdAsync(id);

                this.ModelState.AddModelError(nameof(invoiceInputModelTwo.InvoiceNumber), invoiceInputModelTwo.InvoiceNumber + InvoiceAlreadyExist);

                var model = new CreateInvoiceInputModelTwo
                {
                    SupplierNames = supplierNames,
                };
                return this.View(model);
            }

            await this.invoicesService.AddAsync(
                invoiceInputModelTwo.InvoiceNumber,
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
        public async Task<IActionResult> GetCompany(ShowInvoiceByCompanyInputModel inputModel)
        {
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(inputModel.CompanyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
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
    }
}
