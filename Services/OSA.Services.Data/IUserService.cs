namespace OSA.Services.Data
{
    using System.Threading.Tasks;

    using OSA.Data.Models;

    public interface IUserService
    {
        string GetCurrentUserId();

        Task<ApplicationUser> GetCurrentUserAsync();

        //Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
