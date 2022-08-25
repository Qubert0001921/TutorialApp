using EmptyTest.AppSettings;
using EmptyTest.Data;
using EmptyTest.Entities;
using EmptyTest.Repositories;
using EmptyTest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication("Cookie").AddCookie("Cookie", cfg =>
        {
            var authenticationSettings = new AuthenticationSettings();
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

            cfg.ClaimsIssuer = authenticationSettings.Issuer;
            cfg.ExpireTimeSpan = TimeSpan.FromDays(authenticationSettings.ExpireDays);
            cfg.LoginPath = "/Auth/SignIn";
            cfg.LogoutPath = "/Auth/Logout";
        });
        builder.Services.AddAuthorization();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
        });
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
        builder.Services.AddScoped<ITutorialService, TutorialService>();
        builder.Services.AddScoped<ITutorialRepository, TutorialRepository>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddScoped<ISectionRepository, SectionRepository>();
        builder.Services.AddScoped<ISectionService, SectionService>();

        var app = builder.Build();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run();
    }
}
