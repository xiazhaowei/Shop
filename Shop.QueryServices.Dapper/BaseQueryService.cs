using Shop.Common;
using System.Data;
using System.Data.SqlClient;

namespace Shop.QueryServices.Dapper
{
    public abstract class BaseQueryService
    {
        #region 私有方法
        /// <summary>
        /// 获取MSSQL数据库链接
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
        #endregion
    }
}
