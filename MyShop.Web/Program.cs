using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyShop.DataAccess.Implementation;
using MyShop.DataAcess;
using MyShop.Entities.Repositiories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity.UI.Services;
using Myshop.Utilities;
using Stripe;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Entities.Models;
using MyShop.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection")));

builder.Services.Configure<Myshop.Utilities.Stripe>(builder.Configuration.GetSection("Stripe"));
//
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

//authorize
builder.Services.AddAuthorization(options =>
options.AddPolicy(SD.AdminRole, p=>p.RequireClaim("Admin","Admin"))
);

builder.Services.AddSingleton<IEmailSender,EmailSender>();

//add services  (unitofwork)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<ShoppingCart>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Read the Stripe API key from configuration
//StripeConfiguration.ApiKey = builder.Configuration["Stripe"];


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
StripeConfiguration.ApiKey= builder.Configuration.GetSection("Stripe:secretkey").Get<string>();
seeedDb();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "defaultgg",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "defaultgg",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 
app.Run();

void seeedDb()
{
    using (var scop = app.Services.CreateScope())
    {
        var dbInitalizer = scop.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitalizer.Initialize();
    }
}