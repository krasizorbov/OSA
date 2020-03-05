namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Suppliers.Input_Models;

    public class SupplierController : BaseController
    {
        private readonly ISupplierService supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateSupplierInputModel supplierInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.supplierService.AddAsync(supplierInputModel.Name, supplierInputModel.Bulstat);
            return this.Redirect("/");
        }
    }
}
