using System.Web.Optimization;

namespace Shop.WeiXin.App_Start
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/csses").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/site.css"
                ));
            bundles.Add(new ScriptBundle("~/jses").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js"
                         ));
        }
    }
}