﻿using Microsoft.AspNetCore.Identity;
using QuizREST.Auth.Model;
using System.Threading.Tasks;

namespace QuizREST.Data
{
    public class AuthDbSeeder
    {
        private readonly UserManager<QuizRestUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthDbSeeder(UserManager<QuizRestUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
           _userManager = userManager;
           _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }

        private async Task AddAdminUser()
        {
            var newAdminUser = new QuizRestUser
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);

            if (existingAdminUser != null) 
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafePassword1!");
                if (createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser, QuizRoles.all);
                }
            }
        }

        private async Task AddDefaultRoles()
        {
            foreach (var role in QuizRoles.all)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
