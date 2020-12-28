using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Repository.Role;
using ApplicationCore.Service;
using ApplicationCore.Services.PromotionStoreMappings;
using ApplicationCore.Services.Stores;
using ApplicationCore.Services.VoucherGroups;
using Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PromotionEngineAPI
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
            services.AddControllers();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "1" });
            });
            services.AddTransient<PromotionEngineContext, PromotionEngineContext>();
            //Account

            //Action

            //Brand

            //Channel

            //ConditionRule

            //Holiday

            //Membership

            //MembershipCondition

            //OrderCondition

            //ProductCondition

            //MembershipAction

            //Promotion

            //PromotionStoreMapping
            services.AddScoped<IPromotionStoreMappingService, PromotionStoreMappingService>();
            //PromotionTier

            //RoleEntity
            services.AddScoped<IRoleService, RoleService>();
            //Store
            services.AddScoped<IStoreService, StoreService>();
            //Voucher

            //VoucherChannel

            //VoucherGroup
            services.AddScoped<IVoucherGroupService, VoucherGroupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
