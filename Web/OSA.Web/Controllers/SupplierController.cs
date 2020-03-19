namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ValidationEnum;
    using OSA.Web.ViewModels.Suppliers.Input_Models;

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
            var supplierExist = this.suppliersService.SupplierExist(supplierInputModel.Name, supplierInputModel.CompanyId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else if (supplierExist)
            {
                var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();
                var model = new CreateSupplierInputModel
                {
                    CompanyNames = companyNames,
                };
                // this.SetFlash(FlashMessageType.Error, supplierInputModel.Name + SupplierAlreadyExist);
                this.ModelState.AddModelError(
                    nameof(supplierInputModel.Name),
                    supplierInputModel.Name + SupplierAlreadyExist);
                return this.View(model);
            }

            await this.suppliersService.AddAsync(
                supplierInputModel.Name,
                supplierInputModel.Bulstat,
                supplierInputModel.CompanyId);
            return this.Redirect("/");
        }
    }
}
