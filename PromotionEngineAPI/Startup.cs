using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.Models;
using Infrastructure.UnitOrWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            services.AddCors(options =>
            {
                options.AddPolicy("VueCorsPolicy", builder =>
                {
                    builder
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins("http://localhost:8080")
                      .WithOrigins("https://promotion-engine.netlify.app")
                      .WithOrigins("https://blue-forest-070876000.azurestaticapps.net");
                });
            });
            services.AddControllers();
            // add config swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Promotion Engine API", Version = "1" });
            });
            services.AddTransient<PromotionEngineContext, PromotionEngineContext>();
            // add config auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // connect unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //DI for all service
            ServiceAddScoped(services);
            // configure controller
            services.AddControllers().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


        }

        private void ServiceAddScoped(IServiceCollection services)
        {

            //Account
            services.AddScoped<IAccountService, AccountService>();
            //Action
            services.AddScoped<IActionService, ActionService>();
            //Brand
            services.AddScoped<IBrandService, BrandService>();
            //Channel
            services.AddScoped<IChannelService, ChannelService>();
            //ConditionRule
            services.AddScoped<IConditionRuleService, ConditionRuleService>();
            //Holiday
            services.AddScoped<IHolidayService, HolidayService>();
            //Membership
            services.AddScoped<IMembershipService, MembershipService>();
            //MembershipCondition
            services.AddScoped<IMembershipConditionService, MembershipConditionService>();
            //OrderCondition
            services.AddScoped<IOrderConditionService, OrderConditionService>();
            //ProductCondition
            services.AddScoped<IProductConditionService, ProductConditionService>();
            //MembershipAction
            services.AddScoped<IMembershipActionService, MembershipActionService>();
            //Promotion
            services.AddScoped<IPromotionService, PromotionService>();
            //PromotionStoreMapping
            services.AddScoped<IPromotionStoreMappingService, PromotionStoreMappingService>();
            //PromotionTier
            services.AddScoped<IPromotionTierService, PromotionTierService>();
            //RoleEntity
            services.AddScoped<IRoleService, RoleService>();
            //Store
            services.AddScoped<IStoreService, StoreService>();
            //Voucher
            services.AddScoped<IVoucherService, VoucherService>();
            //VoucherChannel
            services.AddScoped<IVoucherChannelService, VoucherChannelService>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promotion Engine API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("VueCorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
