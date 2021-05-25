using FinSearchDataAcessLibrary.DataAccess;
using FinSearchDataAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FInSearchAPI.Models
{
    public static class PrepDb
    {
        public static void PreparePopulation(IApplicationBuilder app)
        {
            var resourcePath = @"Resources/Bloomberg-Exchange-Code-to-MIC-Mapping.csv";
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<FinSearchDBContext>(), resourcePath);
            }
        }

        public static void SeedData(FinSearchDBContext context, string resourcePath)
        {
            System.Console.WriteLine("Applying Migrations...");
            context.Database.Migrate();
            if (!context.BloomBergLookUp.Any())
            {
                System.Console.WriteLine("Adding data - seeding ");
                var data = Utilities.GetExcelData(resourcePath);
                if (data.Count == 0 )
                    throw new ArgumentNullException(nameof(data));
                context.BloomBergLookUp.AddRange(data);
                context.SaveChanges(); 
            }
            else {
                System.Console.WriteLine("Already have data - not seeding.");

               }
        } 
                
    }
}
