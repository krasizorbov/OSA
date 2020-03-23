namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Invoices.Input_Models;

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
        public async Task<IActionResult> AddPartTwo(int id)
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
            var companyId = id;
            var invoiceExist = await this.invoicesService.InvoiceExistAsync(invoiceInputModelTwo.InvoiceNumber, companyId);

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
                companyId);
            this.TempData["message"] = GlobalConstants.SuccessfullyRegistered;
            return this.Redirect("/");
        }
    }
}
