using System;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using Shop.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Shop.Apis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            //json输出命名规则
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            // 跨域设置
            var urls = Configuration["AppConfig:Cores"].Split(",", StringSplitOptions.RemoveEmptyEntries);
            services.AddCors(options =>
                options.AddPolicy("AllowSameDomain",
                builder => builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
            );

            ConfigSettings.Initialize();
            ConfigSettings.ConnectionString = Configuration.GetConnectionString("shop");
            ConfigSettings.ENodeConnectionString = Configuration.GetConnectionString("enode");
            InitializeENode(services);
            return new AutofacServiceProvider(((AutofacObjectContainer)ObjectContainer.Current).Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors("AllowSameDomain");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void InitializeENode(IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.Load("Shop.Commands"),
                Assembly.Load("Shop.QueryServices"),
                Assembly.Load("Shop.QueryServices.Dapper"),
                Assembly.GetExecutingAssembly()
            };

            ECommon.Configurations.Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .RegisterMvcServices(services)
                .BuildContainer()
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();

            ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName).Info("ENode initialized.");
        }
    }
}
