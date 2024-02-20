using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication_Online_Shopping.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProjectContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ProjectCon")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(Option => {
    Option.ExpireTimeSpan = TimeSpan.FromSeconds(360);
    Option.Cookie.HttpOnly = true;
    Option.LoginPath = "/Admin/Login";
    Option.AccessDeniedPath = "/Admin/Login";
});
builder.Services.AddSession(Option =>
{
    Option.IdleTimeout = TimeSpan.FromMinutes(5);
    Option.Cookie.HttpOnly = true;
    Option.Cookie.IsEssential = true;
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
app.UseSession();
app.UseAuthentication();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
