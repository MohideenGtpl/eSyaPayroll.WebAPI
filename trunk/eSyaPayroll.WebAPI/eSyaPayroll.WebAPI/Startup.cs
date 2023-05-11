using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaPayroll.WebAPI.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using DL_eSyaPayroll = eSyaPayroll.DL.Entities;
using eSyaPayroll.IF;
using eSyaPayroll.DL.Repository;
using eSyaPayroll.WebAPI.Extention;
using eSyaPayroll.DL.Entities;

namespace eSyaPayroll.WebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICommonDataRepository, CommonDataRepository>();
            services.AddScoped<IERCodesRepository, ERCodesRepository>();
            services.AddScoped<IPayPeriodRepository, PayPeriodRepository>();
            services.AddScoped<IVariableEntryRepository, VariableEntryRepository>();
            services.AddScoped<IAttendanceProcessRepository, AttendanceProcessRepository>();
            eSyaEnterprise._connString = Configuration.GetConnectionString("dbConn_eSyaEnterprise");

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpAuthAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //for cross origin support
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            //For displying response same as model property case avoid camel case
            services
           .AddMvc()
           .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes
                    .MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}")
                    .MapRoute(name: "api", template: "api/{controller}/{action}/{id?}");
            });

            app.UseMvc();
        }
    }
}

