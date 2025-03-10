﻿using System;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace KooliProjekt.IntegrationTests.Helpers
{
    public class FakeStartup //: Startup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var dbGuid = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TestConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();



            services.AddControllersWithViews()
                    .AddApplicationPart(typeof(HomeController).Assembly);

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUserBetsService, UserBetsService>();
            services.AddScoped<ITournamentsService, TournamentsService>();
            services.AddScoped<ITeamsService, TeamsService>();
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<ILeaderboardsService, LeaderboardsService>();
              
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{pathStr?}");
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dbContext == null)
                {
                    throw new NullReferenceException("Cannot get instance of dbContext");
                }

                var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
                if (!connectionString.Contains("aspnet-KooliProjekt-Integration"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}