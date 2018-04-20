using System;

namespace Shop.Domain.Models.ThirdCurrencys
{
    public class ImportInfo
    {
        public Guid WalletId { get; private set; }
        public Guid UserId { get;private set; }
        public string Mobile { get; private set; }
        public string Account { get; private set; }//第三方账户名称
        public decimal Amount { get; private set; }//导入的外币数量

        public ImportInfo(Guid walletId,Guid userId,string mobile,string account,decimal amount)
        {
            WalletId = walletId;
            UserId = userId;
            Mobile = mobile;
            Account = account;
            Amount = amount;
        }
    }
}
