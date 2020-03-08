namespace OSA.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IUserService
    {
        string GetCurrentUserId();

        Task<ApplicationUser> GetCurrentUserAsync();

        //Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
