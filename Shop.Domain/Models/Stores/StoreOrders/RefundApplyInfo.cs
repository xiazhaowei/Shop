namespace Shop.Domain.Models.Stores.StoreOrders
{
    public class RefundApplyInfo
    {
        public string Reason { get;private set; }
        public decimal RefundAmount { get;private  set; }

        public RefundApplyInfo(string reason,decimal refundAmount)
        {
            Reason = reason;
            RefundAmount = refundAmount;
        }
    }
}
