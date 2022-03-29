global using Microsoft.EntityFrameworkCore;
global using ReversiMvc.Data;
global using ReversiMvc.Models;
global using ReversiMvc.Models.Entities;
global using ReversiMvc.Repository;
global using ReversiMvc.Repository.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using ReversiMvc.Authorization;
using ReversiMvc.Middleware;
using ReversiMvc.Security;
using ReversiMvc.Services;
using ReversiMvc.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddDbContext<ReversiDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ReversiDatabase"))
);

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("canManagePlayer",
        policyBuilder => policyBuilder.AddRequirements(
            new IsAllowedToManagePlayerRequirement()
        )
    );

    options.AddPolicy("canManageUsers",
        policyBuilder => policyBuilder.AddRequirements(
            new IsAllowedToManageUsersRequirement()
        )
    );
});

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(builder.Configuration.GetSection("Recaptcha").Get<RecaptchaConfiguration>());
builder.Services.AddSingleton<IRecaptcha, Recaptcha>();
builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsMediatorHandler>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPlayersRepository, PlayersRepository>();
builder.Services.AddScoped<IRepository<IdentityUser>, UsersRepository>();
builder.Services.AddScoped<IRepository<GameScoreEntity>, GameScoreRepository>();
builder.Services.AddScoped<ICurrentPlayerService, CurrentPlayerService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCookiePolicy(options =>
{
    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});

builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.MaxAge = TimeSpan.FromDays(7);
});

var app = builder.Build();

// Add important headers to response.
app.UseMiddleware<ResponseHeaders>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseMiddleware<UserIsRemovedCheck>();

app.Run();
