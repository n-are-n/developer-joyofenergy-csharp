using System;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JOIEnergy.Repositories;
using JOIEnergy.Data;

namespace JOIEnergy
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IMeterReadingService, MeterReadingService>();
            services.AddTransient<IMeterReadingRepository, MeterReadingRepository>();
            services.AddTransient<IPricePlanService, PricePlanService>();
            services.AddTransient<IPricePlanRepository, PricePlanRepository>();
            services.AddSingleton<InMemoryContext>();
            services.AddSwaggerGen();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMvc();
        }
    }
}