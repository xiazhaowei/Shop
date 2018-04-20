using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Shop.WeiXin.TemplateMessage
{
    public class NewOrderTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//订单号
        public TemplateDataItem keyword2 { get; set; }//订单概要
        public TemplateDataItem keyword3 { get; set; }//所属会员
        public TemplateDataItem keyword4 { get; set; }//会员手机
        public TemplateDataItem keyword5 { get; set; }//配送地址
        public TemplateDataItem remark { get; set; }
        
        public NewOrderTemplateMessage(
            string number, string goodsName, string customerName,
            string customerMobile, string expressAddress,
            string url,
            string templateId = "vOlHKIGqEfecRt5upcvLoe3HCf-tKRaYU7zhB5P8NWc")
            : base(templateId, url, "您的店铺收到一个新订单")
        {
            keyword1 = new TemplateDataItem(number);
            keyword2 = new TemplateDataItem(goodsName);
            keyword3 = new TemplateDataItem(customerName);
            keyword4 = new TemplateDataItem(customerMobile);
            keyword5 = new TemplateDataItem(expressAddress);
            remark = new TemplateDataItem("点击这里查看处理详细订单");
        }
    }

}
