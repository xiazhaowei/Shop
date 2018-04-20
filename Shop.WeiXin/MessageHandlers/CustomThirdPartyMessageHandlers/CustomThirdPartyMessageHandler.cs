using Senparc.Weixin.Open;
using Senparc.Weixin.Open.Entities.Request;
using Senparc.Weixin.Open.MessageHandlers;
using Shop.WeiXin.Utilities;
using System.IO;

namespace Shop.WeiXin.MessageHandlers.ThirdPartyMessageHandlers
{
    public class CustomThirdPartyMessageHandler : ThirdPartyMessageHandler
    {
        public CustomThirdPartyMessageHandler(Stream inputStream, PostModel encryptPostModel)
            : base(inputStream, encryptPostModel)
        { }

        /// <summary>
        /// 第三方平台ticket验证请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override string OnComponentVerifyTicketRequest(RequestMessageComponentVerifyTicket requestMessage)
        {
            var openTicketPath = Server.GetMapPath("~/App_Data/OpenTicket");
            if (!Directory.Exists(openTicketPath))
            {
                Directory.CreateDirectory(openTicketPath);
            }

            //RequestDocument.Save(Path.Combine(openTicketPath, string.Format("{0}_Doc.txt", DateTime.Now.Ticks)));

            //记录ComponentVerifyTicket（也可以存入数据库或其他可以持久化的地方）
            using (FileStream fs = new FileStream(Path.Combine(openTicketPath, 
                string.Format("{0}.txt",RequestMessage.AppId//该ID为第三方平台ID
                )),
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    tw.Write(requestMessage.ComponentVerifyTicket);
                    tw.Flush();
                }
            }
            return base.OnComponentVerifyTicketRequest(requestMessage);
        }

        /// <summary>
        /// 当取消授权的时候
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override string OnUnauthorizedRequest(RequestMessageUnauthorized requestMessage)
        {
            //取消授权
            return base.OnUnauthorizedRequest(requestMessage);
        }
    }
}
