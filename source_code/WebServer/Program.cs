using Armpit.Library.MetricsManagers.Managers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NReco.Logging.File;
using WebServer.DatabaseManagement.Auth;
using WebServer.DatabaseManagement.Repositories;
using WebServer.Logging;

namespace WebServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("http://*:5000");

            await ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 404)
                {
                    context.HttpContext.Response.Redirect("/NotFound");
                }
            });

            app.MapRazorPages();

            app.Run();
        }

        public static async Task ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddRazorPages();

            // Logging
            var appLogger = new FileLoggerProvider("application.log", true).CreateLogger("ApplicationLogger");
            var loggersContainer = new LoggersContainer()
            {
                ApplicationLogger = appLogger
            };
            services.AddSingleton(loggersContainer);

            // Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                });

            // Authentication database
            var authDbConnectionString = configurationManager.GetConnectionString("MSSQL_Auth");
            services.AddDbContext<ArmpitAuthDbContext>(options => options.UseSqlServer(authDbConnectionString, o=>o.UseCompatibilityLevel(110)));
            // ensure database is created
            var authDbContext = services.BuildServiceProvider().GetRequiredService<ArmpitAuthDbContext>();
            await authDbContext.Database.EnsureCreatedAsync();
            
            services.AddScoped<AccountRepository>();

            if (authDbContext.Accounts.Count() == 0)
            {
                //create default account
                var accountRepo = services.BuildServiceProvider().GetRequiredService<AccountRepository>();
                await accountRepo.CreateAccount(new AccountInformation()
                {
                    Username = "admin",
                    Password = "admin",
                    Type = DataModels.Account.AccountType.Admin
                });
            }

            services.AddSingleton<MemoryMetricsManager>();
        }
    }
}
