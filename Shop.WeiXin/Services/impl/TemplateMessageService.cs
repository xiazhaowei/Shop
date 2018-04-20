using Senparc.Weixin.MP.AdvancedAPIs;
using System.Threading.Tasks;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 模板消息发送服务
    /// </summary>
    public class TemplateMessageService:MpServiceBase,ITemplateMessageService
    {
        /// <summary>
        /// 发生服务器错误模板消息
        /// </summary>
        public void SendExTemplateMessage(string openId,string err, string errMessage, string url)
        {
            Task.Factory.StartNew(async () =>
            {
                var templateMessage = new TemplateMessage.ExceptionAlertTemplateMessage(err, errMessage,url);
                var accessToken = AccessToken();
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(accessToken))
                {
                    var result = await TemplateApi.SendTemplateMessageAsync(accessToken, openId, templateMessage.TemplateId,
                      templateMessage.Url, templateMessage);
                }
            });
            return;
        }

        /// <summary>
        /// 修改支付密码通知
        /// </summary>
        public void SendChangePayPasswordTemplateMessage(string openId,string userName, string payPassword,string shopName,string url)
        {
            Task.Factory.StartNew(async () =>
            {
                var templateMessage = new TemplateMessage.ChangePayPasswordTemplateMessage(userName, payPassword, shopName,url);
                var accessToken = AccessToken();
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(accessToken))
                {
                    var result = await TemplateApi.SendTemplateMessageAsync(accessToken, openId, templateMessage.TemplateId,
                      templateMessage.Url, templateMessage);
                }
            });
            return;
        }

        /// <summary>
        /// 资金到账提醒
        /// </summary>
        public void SendMoneyToWalletTemplateMessage(string openId, string dateTime, string amount, string finallyValue,string remark, string url)
        {
            Task.Factory.StartNew(async () =>
            {
                var templateMessage = new TemplateMessage.MoneyToWalletTemplateMessage(dateTime, amount, finallyValue,remark, url);
                var accessToken = AccessToken();
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(accessToken))
                {
                    var result = await TemplateApi.SendTemplateMessageAsync(accessToken, openId, templateMessage.TemplateId,
                      templateMessage.Url, templateMessage);
                }
            });
            return;
        }

        /// <summary>
        /// 发送新订单提醒
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="number"></param>
        /// <param name="goodsName"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="expressAddress"></param>
        /// <param name="url"></param>
        public void SendNewOrderTemplateMessage(string openId,string number,string goodsName,string customerName,string customerMobile,string expressAddress, string url)
        {
            Task.Factory.StartNew(async () =>
            {
                var templateMessage = new TemplateMessage.NewOrderTemplateMessage(number,
                    goodsName, customerName,customerMobile,expressAddress,url);
                var accessToken = AccessToken();
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(accessToken))
                {
                    var result = await TemplateApi.SendTemplateMessageAsync(accessToken, openId, templateMessage.TemplateId,
                      templateMessage.Url, templateMessage);
                }
            });
            return;
        }
    }
}