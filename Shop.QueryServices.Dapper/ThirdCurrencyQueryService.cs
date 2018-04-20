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
    /// 查询服务 实现
    /// </summary>
    [Component]
    public class ThirdCurrencyQueryService : BaseQueryService,IThirdCurrencyQueryService
    {
        public ThirdCurrency Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ThirdCurrency>(new { Id = id }, ConfigSettings.ThirdCurrencyTable).SingleOrDefault();
            }
        }

        public IEnumerable<ThirdCurrency> ThirdCurrencys()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ThirdCurrency>(null, ConfigSettings.ThirdCurrencyTable);
            }
        }

        public IEnumerable<ThirdCurrencyImportLog> ThirdCurrencyImportLogs(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ThirdCurrencyImportLog>(new {ThirdCurrencyId=id }, ConfigSettings.ThirdCurrencyImportLogTable);
            }
        }

        public IEnumerable<ThirdCurrencyImportLog> ThirdCurrencyImportLogs(Guid id,Guid walletId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ThirdCurrencyImportLog>(new { ThirdCurrencyId = id,WalletId=walletId }, ConfigSettings.ThirdCurrencyImportLogTable);
            }
        }

    }
}