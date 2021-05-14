
using FInSearchAPI.Interfaces;
using FInSearchAPI.Models;
using FInSearchAPI.Services;
using FinSearchDataAcessLibrary.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Microsoft.OpenApi.Models;

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
            services.AddDbContext<FinSearchDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FinDBContext")));
            

            services.AddSingleton(new ApiHelper());
            services.AddScoped<IService, PermIDService>();
            services.AddScoped<IService, OpenFigiService>();
            services.AddScoped<IService, MappingService>();

            services.AddControllers();
            services.AddSwaggerGen();
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });

            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
          
            app.UseHttpsRedirection(); 
        }
    }
}
