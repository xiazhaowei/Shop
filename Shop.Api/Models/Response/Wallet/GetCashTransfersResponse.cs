using System.Collections.Generic;

namespace Shop.Api.Models.Response.Wallet
{
    public class GetCashTransfersResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<CashTransfer> CashTransfers { get; set; }
    }

    public class CashTransfer
    {
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal FinallyValue { get; set; }
        public string Type { get; set; }
        public string CreatedOn { get; set; }
        public string Direction { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }
}