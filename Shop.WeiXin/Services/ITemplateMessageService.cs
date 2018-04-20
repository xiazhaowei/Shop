namespace Shop.WeiXin.Services
{
    public interface ITemplateMessageService
    {
        void SendExTemplateMessage(string openId,string err, string errmessage, string url);
        void SendChangePayPasswordTemplateMessage(string openId,string userName, string payPassword, string shopName,string url);
        void SendMoneyToWalletTemplateMessage(string openId, string dateTime, string amount, string finallyValue, string remark, string url);
        void SendNewOrderTemplateMessage(string openId,string number, string goodsName, string customerName, string customerMobile, string expressAddress,string url);

    }
}
