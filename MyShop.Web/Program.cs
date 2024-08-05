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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbConext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection")));
//
builder.Services.AddIdentity<IdentityUser,IdentityRole>(options=>
options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromDays(1))
    .AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbConext>();

//authorize
builder.Services.AddAuthorization(options =>
options.AddPolicy("AdminRole", p=>p.RequireClaim("Admin","Admin"))
);

builder.Services.AddSingleton<IEmailSender,EmailSender>();

//add services  (unitofwork)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); 
builder.Services.AddScoped<ShoppingCart>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Read the Stripe API key from configuration
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


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
    name: "defaultgg",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "defaultgg",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 
app.Run();
