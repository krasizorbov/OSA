﻿namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Companies.Input_Models;

    public class CompanyController : BaseController
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateCompanyInputModel companyInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.companyService.AddAsync(companyInputModel.Name, companyInputModel.Bulstat);
            return this.Redirect("/");
        }
    }
}
