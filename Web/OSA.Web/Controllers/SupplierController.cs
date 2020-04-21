namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Suppliers.Input_Models;
    using OSA.Web.ViewModels.Suppliers.View_Models;

    public class SupplierController : BaseController
    {
        private const string SupplierAlreadyExist = " already exists! Please enter a new name.";

        private readonly ISuppliersService suppliersService;
        private readonly ICompaniesService companiesService;

        public SupplierController(ISuppliersService suppliersService, ICompaniesService companiesService)
        {
            this.suppliersService = suppliersService;
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

            var model = new CreateSupplierInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateSupplierInputModel supplierInputModel)
        {
            var supplierExist = await this.suppliersService.SupplierExistAsync(supplierInputModel.Name, supplierInputModel.CompanyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (supplierExist != null)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

                var model = new CreateSupplierInputModel
                {
                    CompanyNames = companyNames,
                };

                this.ModelState.AddModelError(nameof(supplierInputModel.Name), supplierInputModel.Name + SupplierAlreadyExist);
                return this.View(model);
            }

            await this.suppliersService.AddAsync(
                supplierInputModel.Name,
                supplierInputModel.Bulstat,
                supplierInputModel.CompanyId);
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

            var model = new ShowSupplierByCompanyInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetCompany(ShowSupplierByCompanyInputModel inputModel)
        {
            var companyName = await this.companiesService.GetCompanyNameByIdAsync(inputModel.CompanyId);
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("GetSupplier", "Supplier", new { id = inputModel.CompanyId, name = companyName });
        }

        [Authorize]
        public async Task<IActionResult> GetSupplier(int id, string name)
        {
            var suppliers = await this.suppliersService.GetSuppliersByCompanyIdAsync(id);

            var model = new SupplierBindingViewModel
            {
                Name = name,
                Suppliers = suppliers,
            };

            return this.View(model);
        }
    }
}
