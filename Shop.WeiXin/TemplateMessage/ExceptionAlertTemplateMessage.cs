using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;

namespace Shop.WeiXin.TemplateMessage
{
    /// <summary>
    /// 服务器报错提醒
    /// </summary>
    public class ExceptionAlertTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//错误描述
        public TemplateDataItem keyword2 { get; set; }//错误详情
        public TemplateDataItem keyword3 { get; set; }//时间
        public TemplateDataItem remark { get; set; }

        public ExceptionAlertTemplateMessage(
            string err, string errMessage, string url,
            string templateId= "aCHDKa7btdiZHf10a_CC5u5w96WE38dHb5_MjDQO9e0") :
            base(templateId, url, "服务器报错提醒")
        {
            keyword1 = new TemplateDataItem(err);
            keyword2 = new TemplateDataItem(errMessage);
            keyword3 = new TemplateDataItem(DateTime.Now.ToString());
            remark = new TemplateDataItem("点击这里查看详情");
        }
    }
}
