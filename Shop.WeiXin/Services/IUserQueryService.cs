using Shop.WeiXin.Models;

namespace Shop.WeiXin.Services
{
    public interface IUserQueryService
    {
        UserInfo Find(string openId);
        UserInfo FindByUnionId(string unionId);
    }
}