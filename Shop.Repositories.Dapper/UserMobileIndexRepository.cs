using System;
using System.Linq;
using Shop.Domain.Repositories;
using ECommon.Components;
using ECommon.Dapper;
using Shop.Domain.Models.Users;
using Shop.Common;
using System.Data;
using System.Data.SqlClient;

namespace Shop.Repositories.Dapper
{
    [Component]
    public class UserMobileIndexRepository:IUserMobileIndexRepository
    {
        public void Add(UserMobileIndex index)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(new
                {
                    IndexId = index.IndexId,
                    UserId = index.UserId,
                    Mobile = index.Mobile
                }, ConfigSettings.UserMobileIndexTable);
            }
        }


        public UserMobileIndex FindMobileIndex(string mobile)
        {
            using (var connection = GetConnection())
            {
                var record = connection.QueryList(new { Mobile = mobile }, ConfigSettings.UserMobileIndexTable).SingleOrDefault();
                if (record != null)
                {
                    var indexId = record.IndexId as string;
                    var userId = (Guid)record.UserId;
                    return new UserMobileIndex(indexId, userId, mobile);
                }
                return null;
            }
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
