using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Shop.Common;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Shop.AdminApi.Startup))]

namespace Shop.AdminApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888

            ConfigureAuth(app);
            InitializeENode();
            //RegisterControllers();
            RegisterApiControllers();
        }

        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = "AdminApiAuthCookie",
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromDays(30000),
                LoginPath = new PathString("/admin/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        }

        private void InitializeENode()
        {
            ConfigSettings.Initialize();
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
                .BuildContainer()
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();

            ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName).Info("ENode initialized.");
        }

        private void RegisterControllers()
        {
            var webAssembly = Assembly.GetExecutingAssembly();
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            var builder = new ContainerBuilder();
            builder.RegisterControllers(webAssembly);
            builder.Update(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void RegisterApiControllers()
        {
            // Mvc Register
            var webAssembly = Assembly.GetExecutingAssembly();
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            var builder = new ContainerBuilder();

            //WebApi Register
            builder.RegisterApiControllers(webAssembly);

            builder.Update(container);
            //Set the dependency resolver for Web API.
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}
