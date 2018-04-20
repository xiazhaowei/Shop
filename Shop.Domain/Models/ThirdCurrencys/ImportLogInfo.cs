using System;

namespace Shop.Domain.Models.ThirdCurrencys
{
    public class ImportLogInfo
    {
        public Guid WalletId { get;private set; }
        public string Mobile { get; private set; }//导入账户
        public string Account { get; private set; }//第三方账户
        public decimal Amount { get; private set; }//导入的外币数量
        public decimal ShopCashAmount { get; private set; }//购物券数量
        public decimal Conversion { get; private set; }//导入的换算比例

        public ImportLogInfo(Guid walletId,
            string mobile,
            string account,
            decimal amount,
            decimal shopCashAmount,
            decimal conversion)
        {
            WalletId = walletId;
            Mobile = mobile;
            Account = account;
            Amount = amount;
            ShopCashAmount = shopCashAmount;
            Conversion = conversion;
        }
    }
}
