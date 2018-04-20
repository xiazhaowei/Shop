namespace Shop.TimerTask.Utils
{
    public interface ITemplateMessageSender
    {
        void SendExTemplateMessage(string openId, string title, string content, string url);
        void SendChangePayPasswordTemplateMessage(string openId, string userName, string payPassword, string shopName, string url);
        void SendMoneyToWalletTemplateMessage(string openId, string dateTime, string amount, string finallyValue, string remark, string url);
        void SendNewOrderTemplateMessage(string openId,
            string number, string goodsName, string customerName, string customerMobile, string expressAddress, string url);
    }
}
