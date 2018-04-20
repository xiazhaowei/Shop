using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;

namespace Shop.WeiXin.TemplateMessage
{
    public class OrderDeliveredTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//商品名称
        public TemplateDataItem keyword2 { get; set; }//快递公司
        public TemplateDataItem keyword3 { get; set; }//快递单号
        public TemplateDataItem keyword4 { get; set; }//收货地址
        public TemplateDataItem remark { get; set; }
        
        public OrderDeliveredTemplateMessage(
            string goodsName, string expressName, string expressNumber,string expressAddress,
            string url,
            string templateId = "ughapJvdVlHSLoO1kM-BuCgq37IPLIs0_nf256X-n4I")
            : base(templateId, url, "您的订单发货啦~~")
        {
            keyword1 = new TemplateDataItem(goodsName);
            keyword2 = new TemplateDataItem(expressName);
            keyword3 = new TemplateDataItem(expressNumber);
            keyword4 = new TemplateDataItem(expressAddress);
            remark = new TemplateDataItem("感谢您在此购物成功哟");
        }
    }

}
