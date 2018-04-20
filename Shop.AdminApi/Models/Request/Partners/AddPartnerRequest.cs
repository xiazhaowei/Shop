using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Partners
{
    public class AddPartnerRequest
    {
        public string Mobile { get; set; }
        public string Region { get; set; }
        public PartnerLevel Level { get; set; }
        public decimal Persent { get; set; }
        public decimal CashPersent { get; set; }
        public int BalanceInterval { get; set; }
        public string Remark { get; set; }
        public bool IsLocked { get; set; }
    }
}