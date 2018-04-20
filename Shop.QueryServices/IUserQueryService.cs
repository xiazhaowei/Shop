using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IUserQueryService
    {
        
        User FindUser(Guid userId);
        User FindUser(string mobile);
        User FindUserByUnionId(string unionId);

        bool CheckMobileIsAvliable(string mobile);

        IEnumerable<ExpressAddress> GetExpressAddresses(Guid userId);
        IEnumerable<UserGift> UserGifts(Guid userId);

        IEnumerable<UserAlis> UserChildrens(Guid userId);

        #region 管理
        IEnumerable<User> UserList();
        IEnumerable<UserAlis> Users();
        #endregion
    }
}
