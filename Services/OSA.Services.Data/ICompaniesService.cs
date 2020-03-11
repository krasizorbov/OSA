namespace OSA.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICompaniesService
    {
        Task AddAsync(string name, string bulstat, string userId);

        Task<List<SelectListItem>> GetAllCompaniesByUserIdAsync();
    }
}
