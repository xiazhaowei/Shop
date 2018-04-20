using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;

namespace Shop.WeiXin.Services.impl
{
    public class CreateMenuService:MpServiceBase,ICreateMenuService
    {
        /// <summary>
        /// 初始化创建菜单
        /// </summary>
        /// <param name="appId"></param>
        public void CreateMenu()
        {
            ButtonGroup buttonGroup = new ButtonGroup();

            buttonGroup.button.Add(new SingleViewButton()
            {
                name = "APP下载",
                url = "http://download.wftx666.com"
            });
            buttonGroup.button.Add(new SingleViewButton()
            {
                name = "进入商城",
                url = "http://m.wftx666.com"
            });

            //二级菜单
            var subButton = new SubButton()
            {
                name = "服务中心"
            };
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_Text_Complaint",
                name = "投诉建议"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "https://mp.weixin.qq.com/mp/homepage?__biz=MzUzOTE3OTMyNw==&hid=1&sn=398ed1e7070362f143920d8fcdbec971#wechat_redirect",
                name = "帮助中心"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://wx.wftx666.com/account",
                name = "你的信息"
            });
            buttonGroup.button.Add(subButton);

            var result = CommonApi.CreateMenu(AccessToken(),buttonGroup);
        }
    }
}