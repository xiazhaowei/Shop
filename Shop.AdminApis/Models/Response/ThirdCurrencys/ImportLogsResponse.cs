using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.ThirdCurrencys
{
    public class ImportLogsResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<ThirdCurrencyImportLog> ImportLogs { get; set; }
    }

    public class ThirdCurrencyImportLog
    {
        public Guid Id { get; set; }
        public Guid ThirdCurrencyId { get; set; }
        public Guid WalletId { get; set; }
        public string Mobile { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public decimal ShopCashAmount { get; set; }
        public decimal Conversion { get; set; }
        public string CreatedOn { get; set; }
    }
}