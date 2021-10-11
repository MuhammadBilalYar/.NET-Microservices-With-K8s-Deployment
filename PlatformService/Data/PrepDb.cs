using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Linq;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrePopulation(IApplicationBuilder app, bool isProduction)
        {

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                System.Console.WriteLine("--> Seeding data .....");

                context.Platforms.AddRange(
                    new Platform { Name = ".Net", Publisher = "Microsoft", Cost = "free" },
                    new Platform { Name = "SQL Server", Publisher = "Microsoft", Cost = "free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud  Native Computing Foundation", Cost = "free" }
                    );

                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("--> We already have data");
            }
        }
    }
}
