using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// Q端查询服务 Dapper
    /// </summary>
    [Component]
    public class UserQueryService: BaseQueryService,IUserQueryService,IDisposable
    {
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User FindUser(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<User>(new { Id = userId }, ConfigSettings.UserTable).SingleOrDefault();
            }
        }
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public User FindUser(string mobile)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<User>(new { Mobile = mobile }, ConfigSettings.UserTable).SingleOrDefault();
            }
        }

        public User FindUserByUnionId(string unionId)
        {
            using(var connection = GetConnection())
            {
                return connection.QueryList<User>(new { UnionId = unionId }, ConfigSettings.UserTable).SingleOrDefault();
            }
        }

        /// <summary>
        /// 判断手机号是否可用
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns>true 可用</returns>
        public bool CheckMobileIsAvliable(string mobile)
        {
            bool result = true;
            using (var connection = GetConnection())
            {
                var userDto = connection.QueryList<User>(new { Mobile = mobile }, ConfigSettings.UserTable).SingleOrDefault();
                if(userDto!=null)
                {
                    result = false;
                }
            }
            return result;
        }


        public IEnumerable<ExpressAddress> GetExpressAddresses(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ExpressAddress>(new { UserId = userId }, ConfigSettings.ExpressAddressTable);
            }
        }

        public IEnumerable<UserGift> UserGifts(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<UserGift>(new { UserId = userId }, ConfigSettings.UserGiftTable);
            }
        }

        public IEnumerable<UserAlis> UserChildrens(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<UserAlis>(new { ParentId=userId}, ConfigSettings.UserTable);
            }
        }

        #region 管理
        public IEnumerable<User> UserList()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<User>(new {  }, ConfigSettings.UserTable);
            }
        }
        public IEnumerable<UserAlis> Users()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<UserAlis>(new { }, ConfigSettings.UserTable);
            }
        }
        #endregion

        public  void Dispose()
        {
            
        }
    }
}
