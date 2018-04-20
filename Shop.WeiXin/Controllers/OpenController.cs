using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.Open.ComponentAPIs;
using Senparc.Weixin.Open.Containers;
using Senparc.Weixin.Open.Entities.Request;
using Shop.WeiXin.MessageHandlers.CustomMessageHandler;
using Shop.WeiXin.MessageHandlers.OpenMessageHandler;
using Shop.WeiXin.MessageHandlers.ThirdPartyMessageHandlers;
using System;
using System.IO;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Shop.WeiXin.Controllers
{
    /// <summary>
    /// 微信第三方平台
    /// </summary>
    public class OpenController : BaseController
    {
        /// <summary>
        /// 发起到微信公众号授权
        /// </summary>
        /// <returns></returns>
        public ActionResult OAuth()
        {
            //获取预授权码
            var preAuthCode = ComponentContainer.TryGetPreAuthCode(component_AppId, component_Secret, true);

            var callbackUrl = "http://wx.wftx666.com/Open/OAuthCallback";//成功回调地址
            var url = ComponentApi.GetComponentLoginPageUrl(component_AppId, preAuthCode, callbackUrl);
            return Redirect(url);
        }

        /// <summary>
        /// 微信服务器会不间断推送最新的Ticket（10分钟一次），需要在此方法中更新缓存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Notice(PostModel postModel)
        {
            var logPath = Server.MapPath(string.Format("~/App_Data/Open/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            
            try
            {
                postModel.Token = component_Token;
                postModel.EncodingAESKey = component_EncodingAESKey;//根据自己后台的设置保持一致
                postModel.AppId = component_AppId;//根据自己后台的设置保持一致

                var messageHandler = new CustomThirdPartyMessageHandler(Request.InputStream, postModel);//初始化
                //注意：再进行“全网发布”时使用上面的CustomThirdPartyMessageHandler，发布完成之后使用正常的自定义的MessageHandler，例如下面一行。
                //var messageHandler = new CustomMessageHandler(Request.InputStream,postModel, 10);

                //记录RequestMessage日志（可选）
                messageHandler.RequestDocument.Save(Path.Combine(logPath,
                    string.Format("{0}_NoticeRequest_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.AppId)
                    ));

                messageHandler.Execute();//执行

                //记录ResponseMessage日志（可选）
                using (TextWriter tw = new StreamWriter(Path.Combine(logPath, string.Format("{0}_NoticeResponse_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.AppId))))
                {
                    tw.WriteLine(messageHandler.ResponseMessageText);
                    tw.Flush();
                    tw.Close();
                }

                return Content(messageHandler.ResponseMessageText);
            }
            catch (Exception ex)
            {
                return Content("error：" + ex.Message);
            }
        }

        /// <summary>
        /// 公众号消息和事件接收
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Callback(Senparc.Weixin.MP.Entities.Request.PostModel postModel)
        {
            //此处的URL格式类型为：http://wx.wftx666.com/Open/Callback/$APPID$， 在RouteConfig中进行了配置，你也可以用自己的格式，只要和开放平台设置的一致。

            //处理微信普通消息，可以直接使用公众号的MessageHandler。此处的URL也可以直接填写公众号普通的URL，如本Demo中的/Weixin访问地址。

            var logPath = Server.MapPath(string.Format("~/App_Data/Open/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            WeixinTrace.SendCustomLog("记录APPID","CallbackAppId:"+postModel.AppId);


            postModel.Token = component_Token;
            postModel.EncodingAESKey = component_EncodingAESKey;
            postModel.AppId = component_AppId; 

            var maxRecordCount = 10;
            MessageHandler<CustomMessageContext> messageHandler = null;

            try
            {
                //是否在“全网发布”阶段
                var checkPublish = false; 
                if (checkPublish)
                {
                    //全网发布测试处理
                    messageHandler = new OpenCheckMessageHandler(Request.InputStream, postModel, maxRecordCount);
                }
                else
                {
                    //处理公众号消息
                    messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);
                }

                messageHandler.RequestDocument.Save(Path.Combine(logPath,
                    string.Format("{0}_CallbackRequest_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));

                messageHandler.Execute(); //执行

                if (messageHandler.ResponseDocument != null)
                {
                    var ticks = DateTime.Now.Ticks;
                    messageHandler.ResponseDocument.Save(Path.Combine(logPath,
                        string.Format("{0}_CallbackResponse_{1}.txt", ticks,
                            messageHandler.RequestMessage.FromUserName)));
                    
                }
                return new FixWeixinBugWeixinResult(messageHandler);
            }
            catch (Exception ex)
            {
                using (
                    TextWriter tw =
                        new StreamWriter(Server.MapPath("~/App_Data/Open/CallbackError_" + DateTime.Now.Ticks + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);

                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }

                    if (ex.InnerException != null)
                    {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }

                    tw.Flush();
                    tw.Close();
                    return Content("");
                }
            }
        }

        /// <summary>
        /// 用于接收取消授权通知、授权成功通知、授权更新通知
        /// </summary>
        /// <param name="auth_code"></param>
        /// <param name="expires_in"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult OAuthCallback(string auth_code, int expires_in, string appId)
        {
            try
            {
                //获取公众号授权结果
                QueryAuthResult queryAuthResult;
                try
                {
                    queryAuthResult = ComponentContainer.GetQueryAuthResult(component_AppId, auth_code);
                }
                catch (Exception ex)
                {
                    throw new Exception("QueryAuthResult：" + ex.Message);
                }

                var authorizerInfoResult = AuthorizerContainer.GetAuthorizerInfoResult(component_AppId,
                    queryAuthResult.authorization_info.authorizer_appid);

                ViewData["AuthorizationInfo"] = queryAuthResult.authorization_info;
                ViewData["AuthorizerInfo"] = authorizerInfoResult.authorizer_info;

                //sdk已经本地存储授权信息，是否要将授权信息存储到数据库？

                return View();
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

    }
}