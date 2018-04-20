using Senparc.Weixin.Open.ComponentAPIs;
using Senparc.Weixin.Open.Containers;
using Shop.WeiXin.Utilities;
using System.IO;
using System.Web.Configuration;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 微信服务的基类
    /// </summary>
    public class MpServiceBase
    {
        protected string component_AppId = WebConfigurationManager.AppSettings["Component_Appid"];
        protected string component_Secret = WebConfigurationManager.AppSettings["Component_Secret"];

        /// <summary>
        /// 公众号授权给第三方平台的accesstoken
        /// </summary>
        /// <returns></returns>
        public virtual string AccessToken()
        {
            //判断是否存在授权文件
            var authorizerPath = Path.Combine(Server.AppDomainAppPath, "App_Data\\AuthorizerInfo\\" + component_AppId);
            if (!Directory.Exists(authorizerPath))
            {
                return null;
            }
            var authorizedFiles = Directory.GetFiles(authorizerPath, "*.bin");
            if (authorizedFiles.Length == 0)
            {
                return null;
            }
            return GetAuthorizerInfoResult(Path.GetFileNameWithoutExtension(authorizedFiles[0])).authorization_info.authorizer_access_token;
        }

        /// <summary>
        /// 授权的公众号appid
        /// </summary>
        /// <param name="authorizerAppId"></param>
        /// <returns></returns>
        private GetAuthorizerInfoResult GetAuthorizerInfoResult(string authorizerAppId)
        {
            var getAuthorizerInfoResult = AuthorizerContainer.GetAuthorizerInfoResult(component_AppId, authorizerAppId);
            getAuthorizerInfoResult.authorization_info.authorizer_appid = authorizerAppId;
            return getAuthorizerInfoResult;
        }
    }
}