using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;

namespace Shop.WeiXin.TemplateMessage
{
    public class BindWeixinTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//会员号手机
        public TemplateDataItem keyword2 { get; set; }//时间
        public TemplateDataItem remark { get; set; }
        
        public BindWeixinTemplateMessage(
            string mobile, 
            string url,
            string templateId = "w5K0sOTpJzR_qVpie4GhHNjs1YtWiPeOHxsaB-qe62Q")
            : base(templateId, url, "您已成功绑定账号")
        {
            keyword1 = new TemplateDataItem(mobile);
            keyword2 = new TemplateDataItem(DateTime.Now.ToString());
            remark = new TemplateDataItem("感谢您在此购物成功哟");
        }
    }

}
