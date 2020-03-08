namespace OSA.Services.Data
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using OSA.Data.Models;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly ApplicationDbContext context;

        public UserService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
            //this.context = context;
        }

        public string GetCurrentUserId()
        {
            var currentUserId = this.contextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            return currentUserId;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);

            return currentUser;
        }

        //public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        //{
        //    if (!await this.context..AnyAsync(x => x.Id == userId))
        //    {
        //        throw new ArgumentException(GlobalConstants.InvalidUserIdErrorMessage);
        //    }

        //    var user = await this.userManager.FindByIdAsync(userId);

        //    return user;
        //}
    }
}
