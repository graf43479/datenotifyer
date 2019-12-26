using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.HttpsPolicy;
using UserStore.WEB.Filters;
using Domain.Repositories;

namespace UserStore.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer();

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });


            //services.AddSqlServerDbContextFactory<ShopDbContext>();
            //services.AddScoped<IShopEFRepository, EFRepository<ShopDbContext>>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            //.AddIdentity<AppUser, AppRole>(config =>
            // {
            //     config.User.RequireUniqueEmail = true;
            //     config.Password.RequireNonAlphanumeric = false;
            //     config.Cookies.ApplicationCookie.AutomaticChallenge = false;
            // })

            //  services.AddIdentity<AppUser, AppRole>()                
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()                      
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //options.Password
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                //options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<UserManager<AppUser>>();
            services.AddTransient<ApplicationContext>();
            

            
            //services.AddMvcCore(options => options.Filters.Add(typeof(ExceptionLoggerFilter)));
            // services.AddMvc(options => options.Filters.Add(typeof(ExceptionLoggerFilter)));
            //services.AddControllersWithViews(options => options.Filters.Add(new ExceptionLoggerFilter(new IdentityUnitOfWork(new DbContextOptions<ApplicationContext>()))));


            //services.AddControllersWithViews(options => options.Filters.Add(new ExceptionLoggerFilter(new IdentityUnitOfWork(services.Where(x=>x.))));
            services.AddControllersWithViews(options => options.Filters.Add(typeof(ExceptionLoggerFilter)));
            //services.AddControllersWithViews();
            
            // services.AddMvcCore(options => options.Filters.Add(new ExceptionLoggerFilter(new IdentityUnitOfWork(new DbContextOptions<ApplicationContext>()))));
            services.AddRazorPages();
          
            services.AddTransient<DBInitializer>();
            //services.AddScoped<ExceptionLoggerFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // app.UseDatabaseErrorPage();                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
           app.UseAuthorization();

           

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
