namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Companies.Input_Models;

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
            var companyExist = this.companiesService.CompanyExist(companyInputModel.Name, userId);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else if (companyExist)
            {
                this.ModelState.AddModelError(nameof(companyInputModel.Name), companyInputModel.Name + CompanyAlreadyExist);
                return this.View();
            }

            await this.companiesService.AddAsync(companyInputModel.Name, companyInputModel.Bulstat, userId);
            return this.Redirect("/");
        }
    }
}
