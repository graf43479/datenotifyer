using DateNotifier.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DateNotifier
{
    public class DBInitializer
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {


            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";

            //IdentityResult res0 =  await roleManager.CreateAsync(new AppRole("SuperAdmin"));

            //  if (res0.Succeeded)
            //  {
            //      string s = "";
            //  }
            //  else
            //  {
            //      string s = res0.Errors.First().Description;
            //  }
            //AppRole role = await roleManager.FindByNameAsync("admin");
            //if (role == null)
            //{ 
            //    IdentityResult res = await roleManager.CreateAsync(new AppRole("admin"));
            //    if (res.Succeeded)
            //    {
            //        string message = "Good";
            //    }
            //    else
            //    {
            //         string  message = res.Errors.First().Description;
            //    }

            //}

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




            //if (roleManager.FindByNameAsync("admin") == null)
            //{

            //    await roleManager.CreateAsync(new AppRole("admin"));
            //}


            //if (roleManager.FindByNameAsync("user") == null)
            //{
            //    await roleManager.CreateAsync(new AppRole("user"));
            //}


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
