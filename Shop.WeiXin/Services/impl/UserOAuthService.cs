using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 微信用户授权服务
    /// </summary>
    public class UserOAuthService:MpServiceBase,IUserOAuthService
    {
        public UserInfoJson UserInfo(string openId)
        {
            var accessToken = AccessToken();
            return UserApi.Info(accessToken, openId, Language.zh_CN);
        }

        public UserInfoJson UserInfo(string accessToken,string openId)
        {
            return UserApi.Info(accessToken, openId, Language.zh_CN);
        }
    }
}