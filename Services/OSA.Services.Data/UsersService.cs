﻿namespace OSA.Services.Data
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using OSA.Common;
    using OSA.Data;
    using OSA.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public UsersService(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
            this.context = context;
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
    }
}
