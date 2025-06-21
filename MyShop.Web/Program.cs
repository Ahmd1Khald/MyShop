using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess;
using MyShop.DataAccess.Implementaions;
using MyShop.Entities.Repositories;
using System;
using Microsoft.AspNetCore.Identity;
using MyShop.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MyShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // For Runtime Compilation
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // For Connection String
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Defualt"))
            );

            builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDBContext>();

            // Add services for EmailSender
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            // Add services for UnitOFWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Customer",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            // Route for Areas (Admin, Customer, ...)
            //app.MapControllerRoute(
            //    name: "areas",
            //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //// Default route (if area not specified)
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
