namespace OSA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateSupplierInputModel supplierInputModel)
        {
            await this.supplierService.AddAsync(supplierInputModel.Name, supplierInputModel.Bulstat);
            return this.Redirect("/");
        }
    }
}