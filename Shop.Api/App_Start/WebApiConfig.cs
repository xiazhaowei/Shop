using System.Web.Http;
using System.Web.Http.Cors;

namespace Shop.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // 允许接口跨域访问
            config.EnableCors(new EnableCorsAttribute("*", "*", "*") { SupportsCredentials=true});
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
