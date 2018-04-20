using Senparc.Weixin;
using Senparc.Weixin.Open.ComponentAPIs;
using Senparc.Weixin.Open.Containers;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Shop.WeiXin.Controllers
{
    public class BaseController : Controller
    {
        protected string component_AppId = WebConfigurationManager.AppSettings["Component_Appid"];
        protected string component_Secret = WebConfigurationManager.AppSettings["Component_Secret"];
        protected string component_Token = WebConfigurationManager.AppSettings["Component_Token"];
        protected string component_EncodingAESKey = WebConfigurationManager.AppSettings["Component_EncodingAESKey"];

        /// <summary>
        /// 尝试获取授权公众号，未授权返回NULL
        /// </summary>
        /// <returns></returns>
        public GetAuthorizerInfoResult TryGetAuthorizerInfoResult()
        {
            //判断是否存在授权文件
            var authorizerPath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\AuthorizerInfo\\" + component_AppId);
            if (!Directory.Exists(authorizerPath))
            {
                return null;
            }
            var authorizedFiles = Directory.GetFiles(authorizerPath, "*.bin");
            if (authorizedFiles.Length == 0)
            {
                return null;
            }
            return GetAuthorizerInfoResult(Path.GetFileNameWithoutExtension(authorizedFiles[0]));
        }
        /// <summary>
        /// 授权的公众号appid
        /// </summary>
        /// <param name="authorizerAppId"></param>
        /// <returns></returns>
        public GetAuthorizerInfoResult GetAuthorizerInfoResult(string authorizerAppId)
        {
            var getAuthorizerInfoResult = AuthorizerContainer.GetAuthorizerInfoResult(component_AppId, authorizerAppId);
            getAuthorizerInfoResult.authorization_info.authorizer_appid = authorizerAppId;
            return getAuthorizerInfoResult;
        }
        public ActionResult GetAuthorizerInfoResultPage(string authorizerAppId)
        {
            WeixinTrace.SendCustomLog("查询授权信息json", authorizerAppId);//记录到日志中
            var getAuthorizerInfoResult = AuthorizerContainer.GetAuthorizerInfoResult(component_AppId, authorizerAppId);
            getAuthorizerInfoResult.authorization_info.authorizer_appid = authorizerAppId;
            return Json(getAuthorizerInfoResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RefreshAuthorizerAccessToken(string authorizerAppId)
        {
            var componentAccessToken = ComponentContainer.GetComponentAccessToken(component_AppId);
            var authorizationInfo = AuthorizerContainer.GetAuthorizationInfo(component_AppId, authorizerAppId);
            if (authorizationInfo == null)
            {
                return Content("授权信息读取失败！");
            }

            var refreshToken = authorizationInfo.authorizer_refresh_token;
            var result = AuthorizerContainer.RefreshAuthorizerToken(componentAccessToken, component_AppId, authorizerAppId,
                refreshToken);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}