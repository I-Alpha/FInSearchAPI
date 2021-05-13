﻿ 
using FInSearchAPI.Models;
using FInSearchAPI.Services;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Hosting;  
namespace FInSearchAPI
{
    public class Program
    {


        public static void  Main(string[] args)
        {
            ApiHelper.initializeClient(); 
            /*CreateHostBuilder(args).Build().Run();*/ 
        }
    



        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}