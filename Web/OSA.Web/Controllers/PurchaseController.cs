namespace OSA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Purchases.Input_Models;

    public class PurchaseController : BaseController
    {
        private readonly IPurchasesService purchasesService;
        private readonly ICompaniesService companiesService;
        private readonly IStocksService stocksService;

        public PurchaseController(IPurchasesService purchasesService, ICompaniesService companiesService, IStocksService stocksService)
        {
            this.purchasesService = purchasesService;
            this.companiesService = companiesService;
            this.stocksService = stocksService;
        }

        [Authorize]
        public async Task<IActionResult> AddPartOne()
        {
            var companyNames = await this.companiesService.GetAllCompaniesByUserIdAsync();

            var model = new CreatePurchaseInputModelOne
            {
                CompanyNames = companyNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPartOne(CreatePurchaseInputModelOne purchaseInputModelOne)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var companyId = purchaseInputModelOne.CompanyId;
            return this.RedirectToAction("AddPartTwo", "Purchase", new { id = companyId });
        }

        [Authorize]
        public async Task<IActionResult> AddPartTwo(int id)
        {
            var stockNames = await this.stocksService.GetStockNamesByCompanyIdAsync(id);

            var model = new CreatePurchaseInputModelTwo
            {
                StockNames = stockNames,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartTwo(CreatePurchaseInputModelTwo purchaseInputModel, int id)
        {
            var companyId = id;
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.purchasesService.AddAsync(
                purchaseInputModel.StockName,
                purchaseInputModel.TotalQuantity,
                purchaseInputModel.TotalPrice,
                purchaseInputModel.Date,
                companyId);
            return this.Redirect("/");
        }
    }
}