using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EMarket.Models;
using Emarket_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;




namespace EMarket
{
    
    public class Startup
    {
        public static class Settings
        {
            public static IConfiguration? Configuration;
        }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings.Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddLogging();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
          

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            services.AddDbContext<EmarketContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultnConnection"))
            );
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
           



            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapControllerRoute(
                        name: "default",
                       pattern: "{controller=Home}/{action=Guest}/{id?}");

            });
        }
    }
}
