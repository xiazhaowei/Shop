using Senparc.Weixin.Cache;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Shop.WeiXin.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            Func<string, FileVersionInfo> getFileVersionInfo = dllFileName =>
            {
                return FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/" + dllFileName));
            };

            Func<FileVersionInfo, string> getDisplayVersion = fileVersionInfo =>
            {
                return Regex.Match(fileVersionInfo.FileVersion, @"\d+\.\d+\.\d+").Value;
            };

            TempData["WeixinVersion"] = getDisplayVersion(getFileVersionInfo("Senparc.Weixin.dll"));

            //缓存
            var containerCacheStrategy = CacheStrategyFactory.GetObjectCacheStrategyInstance().ContainerCacheStrategy;
            TempData["CacheStrategy"] = containerCacheStrategy.GetType().Name.Replace("ContainerCacheStrategy", "");
            
            return View();
        }

        public ActionResult GoOpenAuth()
        {
            return View();
        }
    }
}