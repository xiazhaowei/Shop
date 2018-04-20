using Senparc.Weixin;
using Senparc.Weixin.Entities.Request;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using Shop.WeiXin.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Shop.WeiXin.MessageHandlers.CustomMessageHandler
{
    /// <summary>
    /// 微信公众号自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private string agentUrl = WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
        private string agentToken = WebConfigurationManager.AppSettings["WeixinAgentToken"];
        private string wiweihiKey = WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51

        private string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];

        // 模板消息集合（Key：checkCode，Value：OpenId）
        public static Dictionary<string, string> TemplateMessageCollection = new Dictionary<string, string>();

        public CustomMessageHandler(RequestMessageBase requestMessage) : base(requestMessage) { }

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;

            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                appId = postModel.AppId;//通过第三方开放平台发送过来的请求appid=第三方平台的appID
            }

            //在指定条件下，不使用消息去重
            OmitRepeatedMessageFunc = requestMessage =>
            {
                var textRequestMessage = requestMessage as RequestMessageText;
                if (textRequestMessage != null && textRequestMessage.Content == "容错")
                {
                    return false;
                }
                return true;
            };
        }
        

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }
        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 文字处理
        /// 说明：实际项目中这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var defaultResponseMessage = CreateResponseMessage<ResponseMessageText>();

            var requestHandler =requestMessage.StartHandler()
                .Keyword("MUTE", () => //不回复任何消息
                {
                    return new SuccessResponseMessage();
                })
                .Keyword("OPENID", () =>
                {
                    var openId = requestMessage.FromUserName;//获取OpenId
                    var userInfo = new Services.impl.UserOAuthService().UserInfo(openId);

                    defaultResponseMessage.Content = string.Format(
                        @"您的OpenID为：{0}
UnionId为：{1}
昵称：{2}
性别：{3}
地区（国家/省/市）：{4}/{5}/{6}
关注时间：{7}
关注状态：{8}",
                        requestMessage.FromUserName,userInfo.unionid, userInfo.nickname, (Sex)userInfo.sex, userInfo.country, userInfo.province, userInfo.city, DateTimeHelper.GetDateTimeFromXml(userInfo.subscribe_time), userInfo.subscribe);
                    return defaultResponseMessage;
                })
                .Keyword("二维码",()=> {
                    defaultResponseMessage.Content = "点击<a href=\"http://wx.wftx666.com/account/UserQrCode?openid="+requestMessage.FromUserName+"\">我的推广二维码</a>";
                    return defaultResponseMessage;
                })
                .Keyword("进入商城", () => {
                    defaultResponseMessage.Content = "点击<a href=\"http://wx.wftx666.com/account/EnterShop?openid=" + requestMessage.FromUserName + "\">进入商城</a>";
                    return defaultResponseMessage;
                })
                .Keyword("EX", () =>
                {
                    //发送模板消息
                    var templateMessageService = new Services.impl.TemplateMessageService();
                    templateMessageService.SendExTemplateMessage(requestMessage.FromUserName, "发生错误了", "错误内容", "http://wx.wftx666.com");
                    return new SuccessResponseMessage();
                })
                .Default(() =>
                {
                    //非关键字消息直接转发给客服
                    var transferCustomerServiceResponseMessage = CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
                    return transferCustomerServiceResponseMessage;
                });

            return requestHandler.GetResponseMessage() as IResponseMessageBase;
        }
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }
        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您刚才发送的是小视频";
            return responseMessage;
        }
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            //一隔一返回News或Image格式
            if (base.WeixinContext.GetMessageContext(requestMessage).RequestMessages.Count() % 2 == 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageNews>();

                responseMessage.Articles.Add(new Article()
                {
                    Title = "您刚才发送了图片信息",
                    Description = "您发送的图片将会显示在边上",
                    PicUrl = requestMessage.PicUrl,
                    Url = "http://wx.wftx666.com"
                });
                responseMessage.Articles.Add(new Article()
                {
                    Title = "第二条",
                    Description = "第二条带连接的内容",
                    PicUrl = requestMessage.PicUrl,
                    Url = "http://wx.wftx666.com"
                });

                return responseMessage;
            }
            else
            {
                var responseMessage = CreateResponseMessage<ResponseMessageImage>();
                responseMessage.Image.MediaId = requestMessage.MediaId;
                return responseMessage;
            }
        }
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            //上传缩略图
            var uploadResult = MediaApi.UploadTemporaryMedia(appId, UploadMediaFileType.image,
                                                         Server.GetMapPath("~/Images/Logo.jpg"));

            //设置音乐信息
            responseMessage.Music.Title = "天籁之音";
            responseMessage.Music.Description = "播放您上传的语音";
            responseMessage.Music.MusicUrl = "http://wx.wftx666.com/Media/GetVoice?mediaId=" + requestMessage.MediaId;
            responseMessage.Music.HQMusicUrl = "http://wx.wftx666.com/Media/GetVoice?mediaId=" + requestMessage.MediaId;
            responseMessage.Music.ThumbMediaId = uploadResult.media_id;

            //推送一条客服消息
            try
            {
                CustomApi.SendText(appId, WeixinOpenId, "本次上传的音频MediaId：" + requestMessage.MediaId);

            }
            catch {
            }

            return responseMessage;
        }
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;

            #region 上传素材并推送到客户端

            Task.Factory.StartNew(async () =>
             {
                 //上传素材
                 var dir = Server.GetMapPath("~/App_Data/TempVideo/");
                 var file = await MediaApi.GetAsync(appId, requestMessage.MediaId, dir);
                 var uploadResult = await MediaApi.UploadTemporaryMediaAsync(appId, UploadMediaFileType.video, file, 50000);
                 await CustomApi.SendVideoAsync(appId, base.WeixinOpenId, uploadResult.media_id, "这是您刚才发送的视频", "这是一条视频消息");
             }).ContinueWith(async task =>
             {
                 if (task.Exception != null)
                 {
                     WeixinTrace.Log("OnVideoRequest()储存Video过程发生错误：", task.Exception.Message);

                     var msg = string.Format("上传素材出错：{0}\r\n{1}",
                                task.Exception.Message,
                                task.Exception.InnerException != null
                                    ? task.Exception.InnerException.Message
                                    : null);
                     await CustomApi.SendTextAsync(appId, base.WeixinOpenId, msg);
                 }
             });

            #endregion

            return responseMessage;
        }
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
                    Title：{0}
                    Description:{1}
                    Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
            * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
            * 只需要在这里统一发出委托请求，如：
            * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            * return responseMessage;
            */

            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }
        public override IResponseMessageBase OnUnknownTypeRequest(RequestMessageUnknownType requestMessage)
        {
            /*
             * 此方法用于应急处理SDK没有提供的消息类型，
             * 原始XML可以通过requestMessage.RequestDocument（或this.RequestDocument）获取到。
             * 如果不重写此方法，遇到未知的请求类型将会抛出异常（v14.8.3 之前的版本就是这么做的）
             */
            var msgType = MsgTypeHelper.GetRequestMsgTypeString(requestMessage.RequestDocument);
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "未知消息类型：" + msgType;

            WeixinTrace.SendCustomLog("未知请求消息类型", requestMessage.RequestDocument.ToString());//记录到日志中

            return responseMessage;
        }
    }
}
