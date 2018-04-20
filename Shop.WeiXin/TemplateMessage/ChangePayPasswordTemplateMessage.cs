using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Shop.WeiXin.TemplateMessage
{
    public class ChangePayPasswordTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//用户名
        public TemplateDataItem keyword2 { get; set; }//交易密码
        public TemplateDataItem keyword3 { get; set; }//商城名称
        public TemplateDataItem remark { get; set; }
        
        public ChangePayPasswordTemplateMessage(
            string userName, string payPassword, string shopName,
            string url,
            string templateId = "GheLOReUnyhPL4lcrTYVP-ErzDLXcd2PmjhNpJOOEkc")
            : base(templateId, url, "您的交易密码已经修改")
        {
            keyword1 = new TemplateDataItem(userName);
            keyword2 = new TemplateDataItem(payPassword);
            keyword3 = new TemplateDataItem(shopName);
            remark = new TemplateDataItem("登录商城了解详情");
        }
    }

}
