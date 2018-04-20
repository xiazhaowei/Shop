using System.Collections.Generic;

namespace Shop.Api.Models.Response.Wallet
{
    public class WithdrawApplysListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }

        public IList<WithdrawApply> WithdrawApplys { get; set; }
    }
}