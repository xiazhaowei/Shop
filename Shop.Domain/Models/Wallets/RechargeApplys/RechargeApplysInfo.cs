namespace Shop.Domain.Models.Wallets.RechargeApplys
{
    public class RechargeApplyInfo
    {
        public decimal Amount { get;private set; }
        public string Pic { get; private set; }
        public string BankName { get;private set; }
        public string BankNumber { get;private set; }
        public string BankOwner { get;private set; }
        public string Remark { get;  set; }

        public RechargeApplyInfo(decimal amount,string pic,string bankName,string bankNumber,string bankOwner,string remark)
        {
            Amount = amount;
            Pic = pic;
            BankName = bankName;
            BankNumber = bankNumber;
            BankOwner = bankOwner;
            Remark = remark;
        }
    }
}
