using Senparc.Weixin.MP.AdvancedAPIs.User;

namespace Shop.WeiXin.Services
{
    public interface IUserOAuthService
    {
        UserInfoJson UserInfo(string openId);
        UserInfoJson UserInfo(string accessToken, string openId);
    }
}
