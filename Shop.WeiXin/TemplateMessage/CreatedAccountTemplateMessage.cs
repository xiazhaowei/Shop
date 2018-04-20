using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Shop.WeiXin.TemplateMessage
{
    public class CreatedAccountTemplateMessage : TemplateMessageBase
    {
        public TemplateDataItem keyword1 { get; set; }//昵称
        public TemplateDataItem keyword2 { get; set; }//绑定手机号
        public TemplateDataItem remark { get; set; }
        
        public CreatedAccountTemplateMessage(
            string nickName, string mobile,
            string url,
            string templateId = "-RzDvnXg-a2gS_CgDvIHnDcbqEATwbm1gnOM4uksE-c")
            : base(templateId, url, "您已成功注册为会员，奖励您5元购物券 ！")
        {
            keyword1 = new TemplateDataItem(nickName);
            keyword2 = new TemplateDataItem(mobile);
            remark = new TemplateDataItem("购物得福豆，赚收益，天天赚不停，快来JOIN US");
        }
    }

}
