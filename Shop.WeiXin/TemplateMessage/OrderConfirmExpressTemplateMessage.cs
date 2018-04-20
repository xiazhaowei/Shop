using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;

namespace Shop.WeiXin.TemplateMessage
{
    public class OrderConfirmExpressTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//订单编号
        public TemplateDataItem keyword2 { get; set; }//订单金额
        public TemplateDataItem keyword3 { get; set; }//确认时间
        public TemplateDataItem remark { get; set; }
        
        public OrderConfirmExpressTemplateMessage(
            string number, string total, 
            string url,
            string templateId = "czIYBSagNfmvl2-tFMl-LvGOXYKJJbc06JKe7Kirmnk")
            : base(templateId, url, "您好，您的一个订单已经确认收货了")
        {
            keyword1 = new TemplateDataItem(number);
            keyword2 = new TemplateDataItem(total);
            keyword3 = new TemplateDataItem(DateTime.Now.ToString());
            remark = new TemplateDataItem("感谢您在此购物成功，同时希望您的再次光临！");
        }
    }

}
