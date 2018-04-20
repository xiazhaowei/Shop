using Dapper;
using Shop.WeiXin.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 用户的查询服务
    /// </summary>
    public class UserQueryService:IUserQueryService
    {

        public UserInfo Find(string openId)
        {
            using (var connection = GetConnection())
            {
                var sql = @"select top 1 * from Users where OpenId=@OpenId";
                return connection.QueryFirstOrDefault<UserInfo>(sql, new { OpenId=openId});
            }
        }

        public UserInfo FindByUnionId(string unionId)
        {
            using (var connection = GetConnection())
            {
                var sql = @"select top 1 * from Users where UnionId=@UnionId";
                return connection.QueryFirstOrDefault<UserInfo>(sql, new { UnionId = unionId });
            }
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["shop_weixin"].ConnectionString);
        }
    }
}