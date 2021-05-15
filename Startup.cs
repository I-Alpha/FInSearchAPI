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
            services.AddSingleton(new LoggerFactory());
            services.AddLogging();
            services.AddMvc();

            services.AddDbContext<FinSearchDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FinDBContext")));
             
            services.AddScoped<ApiHelper>();
            services.AddScoped<IService,PermIDService>();
            services.AddScoped<IService,OpenFigiService>();
            services.AddScoped<MappingService>();
/*
            services.AddScoped<IRequest<IEnumerable<Company>>, GetCompanyLevelInfoCommand>();
            services.AddScoped<IRequest<IEnumerable<Figi>>, GetSecurityLevelInfoCommand>();*/


            services.AddScoped<IValidator<GetCompanyLevelInfoCommand>, PermIDValidator>();
            services.AddScoped<IValidator<GetSecurityLevelInfoCommand>, OpenFigiIDValidator>();


            services.AddScoped < IRequestHandler < GetCompanyLevelInfoCommand, IEnumerable < Company >>, GetCompanyLevelInfoCommandHandler > ();
            services.AddScoped < IRequestHandler < PostCompanyLevelInfoCommand, IEnumerable < Company >> ,PostCompanyLevelInfoCommandHandler >();
            services.AddScoped < IRequestHandler < GetSecurityLevelInfoCommand, IEnumerable < Figi >> ,GetSecurityLevelInfoCommandHandler >();




            /*

                        services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());*/

            services.AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinSearch API", Version = "v1" });

               });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinSearch API v1");
                });

            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }  
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
