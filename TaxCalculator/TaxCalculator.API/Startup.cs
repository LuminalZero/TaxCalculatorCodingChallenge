using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.API.Calculators;
using TaxCalculator.API.Configuration;
using TaxCalculator.API.Interfaces;
using TaxCalculator.API.Services;

namespace TaxCalculator.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaxCalculator.API", Version = "v1" });
            });

            ConfigureIoc(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaxCalculator.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureIoc(IServiceCollection services)
        {
            var integrations = Configuration.GetSection("Integrations").Get<IntegrationsSection>();

            services.AddHttpClient<ITaxCalculator, TaxJarTaxCalculator>("TaxJarTaxCalculator",
                client =>
                {
                    client.BaseAddress = new Uri(integrations.TaxJar.Url);
                    client.DefaultRequestHeaders.Add("Authorization", $@"Bearer { integrations.TaxJar.Key}");
                });

            services.AddTransient<ITaxService, TaxService>();
        }
    }
}
