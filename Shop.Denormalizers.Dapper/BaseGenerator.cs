using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ECommon.IO;
using Shop.Common;
using System.Collections.Generic;

namespace Shop.Denormalizers.Dapper
{
    public abstract class BaseGenerator
    {
        #region 私有方法
        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task<AsyncTaskResult> TryInsertRecordAsync(Func<IDbConnection, Task<long>> action)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    await action(connection);
                    return AsyncTaskResult.Success;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)  //主键冲突，忽略即可；出现这种情况，是因为同一个消息的重复处理
                {
                    return AsyncTaskResult.Success;
                }
                throw;
            }
        }
       

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task<AsyncTaskResult> TryUpdateRecordAsync(Func<IDbConnection, Task<int>> action)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    await action(connection);
                    return AsyncTaskResult.Success;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)  //主键冲突，忽略即可；出现这种情况，是因为同一个消息的重复处理
                {
                    return AsyncTaskResult.Success;
                }
                throw;
            }
        }
        /// <summary>
        /// 异步事务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task<AsyncTaskResult> TryTransactionAsync(Func<IDbConnection, IDbTransaction, Task> action)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var transaction = await Task.Run<SqlTransaction>(() => connection.BeginTransaction()).ConfigureAwait(false);
                try
                {
                    await action(connection, transaction).ConfigureAwait(false);
                    await Task.Run(() => transaction.Commit()).ConfigureAwait(false);
                    return AsyncTaskResult.Success;
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 异步事务
        /// </summary>
        /// <param name="actions"></param>
        /// <returns></returns>
        protected async Task<AsyncTaskResult> TryTransactionAsync(Func<IDbConnection, IDbTransaction, IEnumerable<Task>> actions)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var transaction = await Task.Run<SqlTransaction>(() => connection.BeginTransaction()).ConfigureAwait(false);
                try
                {
                    await Task.WhenAll(actions(connection, transaction)).ConfigureAwait(false);
                    await Task.Run(() => transaction.Commit()).ConfigureAwait(false);
                    return AsyncTaskResult.Success;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }

        #endregion
    }
}
