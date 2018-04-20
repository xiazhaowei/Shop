namespace Shop.Domain.Models.Wallets.WithdrawApplys
{
    public class WithdrawApplyInfo
    {
        public decimal Amount { get;private set; }
        public string BankName { get;private set; }
        public string BankNumber { get;private set; }
        public string BankOwner { get;private set; }
        public string Remark { get;  set; }

        public WithdrawApplyInfo(decimal amount,string bankName,string bankNumber,string bankOwner,string remark)
        {
            Amount = amount;
            BankName = bankName;
            BankNumber = bankNumber;
            BankOwner = bankOwner;
            Remark = remark;
        }
    }
}
