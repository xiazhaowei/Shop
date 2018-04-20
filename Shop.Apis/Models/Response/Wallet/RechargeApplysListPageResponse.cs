using System.Collections.Generic;

namespace Shop.Api.Models.Response.Wallet
{
    public class RechargeApplysListPageResponse : BaseApiResponse
    {
        public int Total { get; set; }

        public IList<RechargeApply> RechargeApplys { get; set; }
    }
}