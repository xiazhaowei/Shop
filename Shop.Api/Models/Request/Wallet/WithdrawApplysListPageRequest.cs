using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Wallet
{
    public class WithdrawApplysListPageRequest
    {
        public string Name { get; set; }
        public int Page { get; set; }
        public WithdrawApplyStatus Status { get; set; }
    }
}