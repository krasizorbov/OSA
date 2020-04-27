namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Common;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Companies.Input_Models;
    using OSA.Web.ViewModels.Companies.View_Models;

    public class CompanyController : BaseController
    {
        private const string CompanyAlreadyExist = " already exists! Please enter a new name.";

        private readonly ICompaniesService companiesService;
        private readonly IUsersService userService;

        public CompanyController(ICompaniesService companiesService, IUsersService userService)
        {
            this.companiesService = companiesService;
            this.userService = userService;
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
            var userId = this.userService.GetCurrentUserId();
            var companyExist = await this.companiesService.CompanyExistAsync(companyInputModel.Name, userId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (companyExist != null)
            {
                this.ModelState.AddModelError(nameof(companyInputModel.Name), companyInputModel.Name + CompanyAlreadyExist);
                return this.View();
            }

            await this.companiesService.AddAsync(
                companyInputModel.Name,
                companyInputModel.Bulstat,
                userId);
            this.TempData["message"] = GlobalConstants.SuccessfullyRegistered;
            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Get()
        {
            var companies = await this.companiesService.GetCompaniesByUserIdAsync();

            var model = new CompanyBindingViewModel
            {
                Companies = companies,
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var company = await this.companiesService.GetCompanyById(id);

            var model = new EditCompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                Bulstat = company.Bulstat,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCompanyViewModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.companiesService.UpdateCompany(editModel.Id, editModel.Name, editModel.Bulstat);
            this.TempData["message"] = GlobalConstants.SuccessfullyUpdated;
            return this.Redirect("/");
        }
    }
}
