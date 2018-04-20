using Senparc.Weixin.MP.AdvancedAPIs;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 客服服务类
    /// </summary>
    public class CustomService:MpServiceBase,ICustomService
    {
        /// <summary>
        /// 向用户发送文本消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="message"></param>
        public void SendText(string openId,string message)
        {
            var accessToken = AccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return;
            }
            CustomApi.SendText(accessToken, openId, message);
        }
    }
}