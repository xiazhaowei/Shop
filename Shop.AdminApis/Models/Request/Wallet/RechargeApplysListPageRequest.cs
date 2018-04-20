using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Wallet
{
    public class RechargeApplysListPageRequest
    {
        public string Name { get; set; }
        public int Page { get; set; }
        public RechargeApplyStatus Status { get; set; }
    }
}