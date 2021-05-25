using FInSearchAPI.Models;
using FInSearchAPI.Services;
using FinSearchDataAcessLibrary.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using FInSearchAPI.Commands; 
using FInSearchAPI.Validators; 
using FluentValidation;
using Microsoft.OpenApi.Models;
using FInSearchAPI.Interfaces;
using MediatR;
using System.Collections.Generic;
using FInSearchAPI.Handlers;
using FinSearchDataAccessLibrary.Models.Database;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;

namespace FInSearchAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            var server = Configuration["DBServer"] ?? "ms-sql-server";
            var port = Configuration["DBPort"] ?? "1433";
            var user = Configuration["DBUser"] ?? "SA";// use actual user logic for publishing
            var password = Configuration["DBPassword"] ?? "Pa55w0rd2021"; // erase for publishing
            var database = Configuration["Database"] ?? "FinSearchDb"; // erase for publishing



            services.AddDbContext<FinSearchDBContext>(options => 
                options.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID ={user};Password={password}"));  


            services.AddSingleton(new LoggerFactory());
            services.AddLogging();
            services.AddMvc();
            services.AddAutofac();
            services.AddMediatR(this.GetType().GetTypeInfo().Assembly);
            /*
                        services.AddDbContext<FinSearchDBContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("FinDBContext")));*/
  
            services.AddScoped<ApiHelper>();
            services.AddScoped<PermIDService>();
            services.AddScoped<OpenFigiService>();
            services.AddScoped<MappingService>();
            /*
                        services.AddScoped<IRequest<IEnumerable<Company>>, GetCompanyLevelInfoCommand>();
                        services.AddScoped<IRequest<IEnumerable<Figi>>, GetSecurityLevelInfoCommand>();*/

            services.AddValidatorsFromAssemblyContaining(this.GetType());

            services.AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinSearch API", Version = "v1" });

               });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            PrepDb.PreparePopulation(app); 

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinSearch API v1");
                });
             
          /*  else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }*/
             
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
