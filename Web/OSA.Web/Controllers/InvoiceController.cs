namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Invoices.Input_Models;

    public class InvoiceController : Controller
    {
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
        public async Task<IActionResult> Add(int companyId)
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
            var supplierNames = await this.suppliersService.GetAllSuppliersByCompanyIdAsync(companyId);
            var model = new CreateInvoiceInputModelTwo
            {
                CompanyNames = companyNames,
                SupplierNames = supplierNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateInvoiceInputModelTwo invoiceInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.invoicesService.AddAsync(invoiceInputModel.InvoiceNumber, invoiceInputModel.Date, invoiceInputModel.CompanyId, invoiceInputModel.SupplierId);
            return this.Redirect("/");
        }
    }
}