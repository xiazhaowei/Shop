using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IThirdCurrencyQueryService
    {
        ThirdCurrency Find(Guid id);
        IEnumerable<ThirdCurrency> ThirdCurrencys();
        IEnumerable<ThirdCurrencyImportLog> ThirdCurrencyImportLogs(Guid id);
        IEnumerable<ThirdCurrencyImportLog> ThirdCurrencyImportLogs(Guid id,Guid walletId);
    }
}
