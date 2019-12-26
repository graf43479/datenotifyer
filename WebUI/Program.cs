using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Domain;
using Domain.Entities;
using UserStore.WEB.Filters;

namespace UserStore.WEB
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {                
                Task t;
                var services = scope.ServiceProvider;

                try
                {                    
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    //  var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    //await DBInitializer.InitializeAsync(userManager, roleManager);
                    //await DBInitializer.InitializeAsync(userManager, roleManager);
                    //await DBInitializer.InitializeAsync();
                    //await (new DBInitializer()).InitializeAsync();

                    //DBInitializer.Initialize(userManager, roleManager);
                    t = DBInitializer.InitializeAsync(userManager, roleManager);
                    t.Wait();

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
