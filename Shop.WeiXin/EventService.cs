using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using System;

namespace Shop.WeiXin
{
    /// <summary>
    /// 全局微信事件有关的处理程序
    /// </summary>
    public class EventService
    {
        public void ConfigOnWeixinExceptionFunc(WeixinException ex)
        {
            try
            {
                string desc = "发生错误"+ex.GetType().Name;
                string message = ex.Message;
                WeixinTrace.SendCustomLog(desc, desc);
            }
            catch (Exception e)
            {
                WeixinTrace.SendCustomLog("OnWeixinExceptionFunc过程错误", e.Message);
            }
        }
    }
}