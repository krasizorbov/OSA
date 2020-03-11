namespace OSA.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Companies.Input_Models;

    public class CompanyController : BaseController
    {
        private readonly ICompaniesService companyService;
        private readonly IUsersService userService;

        public CompanyController(ICompaniesService companyService, IUsersService userService)
        {
            this.companyService = companyService;
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
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.companyService.AddAsync(companyInputModel.Name, companyInputModel.Bulstat, userId);
            return this.Redirect("/");
        }
    }
}
