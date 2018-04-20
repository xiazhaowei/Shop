using Shop.WeiXin.Models;

namespace Shop.WeiXin.Services
{
    public interface IUserGenerator
    {
        int UpdateOAuthAccessToken(UserOAuthAccessToken userOAuthAccessToken);
        int CreateUser(UserInfo userInfo);
    }
}
