using Dapper;
using Shop.WeiXin.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 用户数据库操作
    /// </summary>
    public class UserGenerator:IUserGenerator
    {
        /// <summary>
        /// 添加或更新用户OAuth访问令牌
        /// </summary>
        /// <param name="userOAuthAccessToken"></param>
        /// <returns></returns>
        public int UpdateOAuthAccessToken(UserOAuthAccessToken userOAuthAccessToken)
        {
            var sql = @"delete from UserOAuthAccessTokens where OpenId=@OpenId;
insert into UserOAuthAccessTokens (OpenId,StartTime,AccessToken) values (@OpenId,@StartTime,@AccessToken)";
            using (var connection = GetConnection())
            {
                return connection.Execute(sql, userOAuthAccessToken);
            }
        }

        /// <summary>
        /// 添加或更新用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public int CreateUser(UserInfo userInfo)
        {
            var sql = @"delete from Users where OpenId=@OpenId;
insert into Users (OpenId,UnionId,NickName,Province,City,County,Gender,Portrait,ParentOpenId) values (@OpenId,@UnionId,@NickName,@Province,@City,@County,@Gender,@Portrait,@ParentOpenId)";
            using (var connection = GetConnection())
            {
                return connection.Execute(sql, userInfo);
            }
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["shop_weixin"].ConnectionString);
        }
    }
}