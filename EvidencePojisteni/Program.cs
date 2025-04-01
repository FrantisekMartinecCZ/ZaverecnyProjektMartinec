using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// P�ipojen� k datab�zi

builder.Services.AddDbContext<DatabazovyKontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Konfigurace ASP.NET Identity

builder.Services.AddIdentity<UzivatelAplikace, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<DatabazovyKontext>() // zm�na zde
.AddDefaultTokenProviders();

// Konfigurace cookies pro autentizaci
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
    options.LoginPath = "/Uzivatelsky/Prihlaseni";
    options.LogoutPath = "/Uzivatelsky/Odhlaseni";
    options.AccessDeniedPath = "/Uzivatelsky/PristupZakazan";
});

// Nastaven� identity pro spr�vn� mapov�n� u�ivatelsk�ho jm�na a e-mailu
builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Email;
    options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
});
// Registace Sluzba Pojisteni
builder.Services.AddScoped<ISluzbaPojisteni, SluzbaPojisteni>();


//Registrace Sluzba pojisten� ud�losti
builder.Services.AddScoped<ISluzbaUdalosti, SluzbaPojistnychUdalosti>();


//
builder.Services.AddScoped<ISluzbaPojistovatelu, SluzbaPojistovatelu>();


builder.Services.AddScoped<SluzbaStatistik>();
// RegistracePojistitele slu�eb
builder.Services.AddScoped<ISluzbaPojistence, SluzbaPojistence>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Inicializace rol� a administr�tora
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UzivatelAplikace>>();
    await RoleInitializer.SeedRolesAndAdmin(roleManager, userManager);
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Mapa v�choz� trasy
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Uzivatelsky}/{action=Prihlaseni}/{id?}");

app.Run();
