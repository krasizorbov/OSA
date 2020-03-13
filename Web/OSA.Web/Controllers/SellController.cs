namespace OSA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Sells.Input_Models;

    public class SellController : BaseController
    {
        private readonly ISellsService sellsService;
        private readonly ICompaniesService companiesService;

        public SellController(ISellsService sellsService, ICompaniesService companiesService)
        {
            this.sellsService = sellsService;
            this.companiesService = companiesService;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreateSellInputModel
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateSellInputModel sellInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.sellsService.AddAsync(
                sellInputModel.StockName,
                sellInputModel.TotalPrice,
                sellInputModel.ProfitPercent,
                sellInputModel.Date,
                sellInputModel.CompanyId);
            return this.Redirect("/");
        }
    }
}