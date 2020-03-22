namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICompaniesService
    {
        Task AddAsync(string name, string bulstat, string userId);

        Task<ICollection<SelectListItem>> GetAllCompaniesByUserIdAsync();

        Task<string> CompanyExistAsync(string companyName, string userId);
    }
}
