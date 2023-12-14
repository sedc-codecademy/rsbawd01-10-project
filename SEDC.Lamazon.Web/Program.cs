using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

using SEDC.Lamazon.DataAccess.Context;
using SEDC.Lamazon.DataAccess.Implementations;
using SEDC.Lamazon.DataAccess.Interfaces;

using SEDC.Lamazon.Services.Implementations;
using SEDC.Lamazon.Services.Interfaces;

namespace SEDC.Lamazon.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add DB Context
        builder.Services.AddDbContext<LamazonDbContext>(options => 
        {
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LamazonStoreDB;Trusted_Connection=True;");
        });

        // https://www.c-sharpcorner.com/article/understanding-addtransient-vs-addscoped-vs-addsingleton-in-asp-net-core/

        // Add DB Repository
        builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

        // Add Services
        builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderItemService, OrderItemService>();

        // Set authentification cookie type and settings
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt => 
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                opt.LoginPath = "/User/Login";
                opt.AccessDeniedPath = "/Home/AccessDenied";
            });

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

        // Use auth
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}