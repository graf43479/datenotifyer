using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DBInitializer
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";         

            string[] roles = { "admin", "user" };
            foreach (string role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    if ((await roleManager.CreateAsync(new IdentityRole(role))).Succeeded != true)
                    {
                        throw new Exception($"Ошибка при создании роли \'{role}\'");
                    }
                }
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                AppUser admin = new AppUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
