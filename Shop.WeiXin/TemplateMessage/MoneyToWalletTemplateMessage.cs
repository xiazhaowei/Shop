using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Shop.WeiXin.TemplateMessage
{
    public class MoneyToWalletTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//变得时间
        public TemplateDataItem keyword2 { get; set; }//变得金额
        public TemplateDataItem keyword3 { get; set; }//商城名称
        public TemplateDataItem remark { get; set; }
        
        public MoneyToWalletTemplateMessage(
            string dateTime, string amount, string finallyValue,string txtRemark,
            string url,
            string templateId = "tYj9BI5GLDooJwojo1wTmCUd8gZfHzFAyavaHZUD29M")
            : base(templateId, url, "您好，您有一笔资金到账~")
        {
            keyword1 = new TemplateDataItem(dateTime);
            keyword2 = new TemplateDataItem(amount);
            keyword3 = new TemplateDataItem(finallyValue);
            remark = new TemplateDataItem(txtRemark+"，登录商城查看详细资金记录");
        }
    }

}
