namespace OSA.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using OSA.Data.Models;

    public interface ICompaniesService
    {
        Task AddAsync(string name, string bulstat, string userId);

        Task<ICollection<SelectListItem>> GetAllCompaniesByUserIdAsync();

        Task<string> CompanyExistAsync(string companyName, string userId);

        Task<ICollection<Company>> GetCompaniesByUserIdAsync();

        Task<string> GetCompanyNameByIdAsync(int companyId);

        Task<Company> GetCompanyById(int id);

        Task UpdateCompany(int id, string name, string bulstat);
    }
}
