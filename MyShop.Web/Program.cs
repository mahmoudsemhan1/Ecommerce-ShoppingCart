using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyShop.DataAccess.Implementation;
using MyShop.DataAcess;
using MyShop.Entities.Repositiories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity.UI.Services;
using Myshop.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbConext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection")));
//
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbConext>();

builder.Services.AddSingleton<IEmailSender,EmailSender>();

//add services  (unitofwork)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();




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
