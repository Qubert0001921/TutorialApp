using EmptyTest.AppSettings;
using EmptyTest.Data;
using EmptyTest.Entities;
using EmptyTest.Helpers;
using EmptyTest.Repositories;
using EmptyTest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EmptyTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, services, opts) =>
        {
            opts.Enrich.WithMachineName();
            opts.Enrich.WithProcessName();
            opts.Enrich.WithProcessId();
            opts.Enrich.FromLogContext();
            opts.Enrich.With(services.GetService<AppNameEnricher>());

            opts.WriteTo.Console();
            opts.WriteTo.Seq("http://localhost:8081");
            //opts.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticSearch:Uri"]))
            //{
            //    AutoRegisterTemplate = true,
            //    IndexFormat = $"{context.Configuration["AppName"]}-log-{DateTime.UtcNow.ToString("yyyy-MM-hh")}",
            //    NumberOfShards = 2,
            //    NumberOfReplicas = 2
            //});
            //opts.WriteTo.Seq(context.Configuration["Seq:Url"]);
        });

        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = long.MaxValue;
            options.Limits.MaxRequestBufferSize = int.MaxValue;
        });

        builder.Services.AddTransient<AppNameEnricher>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(AuthenticationSchemas.Default).AddCookie(AuthenticationSchemas.Default, cfg =>
        {
            var authenticationSettings = new AuthenticationSettings();
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

            cfg.ClaimsIssuer = authenticationSettings.Issuer;
            cfg.ExpireTimeSpan = TimeSpan.FromDays(authenticationSettings.ExpireDays);
            cfg.LoginPath = "/Auth/SignIn";
            cfg.LogoutPath = "/Auth/Logout";
            cfg.Cookie.HttpOnly = true;
            cfg.ReturnUrlParameter = "RedirectUrl";
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
        builder.Services.AddScoped<ITopicRepository, TopicRepository>();
        builder.Services.AddScoped<ITopicService, TopicService>();
        builder.Services.AddSingleton<IFileSaveSettingsService, FileSaveSettingsService>();

        try
        {
            var app = builder.Build();

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

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
        catch (Exception ex)
        {
            Log.Fatal("Application caught an exception: {ex}", ex);
        }
        finally
        {
            Log.Information("Application stopped");
        }
    }
}
