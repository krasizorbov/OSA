namespace OSA.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using OSA.Services.Data;
    using OSA.Web.ViewModels.Companies.Input_Models;

    public class CompanyController : BaseController
    {
        private readonly ICompanyService companyService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CompanyController(ICompanyService companyService, IHttpContextAccessor httpContextAccessor)
        {
            this.companyService = companyService;
            this.httpContextAccessor = httpContextAccessor;
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
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.companyService.AddAsync(companyInputModel.Name, companyInputModel.Bulstat, userId);
            return this.Redirect("/");
        }
    }
}
