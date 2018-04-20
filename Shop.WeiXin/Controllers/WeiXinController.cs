using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using Shop.WeiXin.MessageHandlers.CustomMessageHandler;
using System;
using System.IO;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Shop.WeiXin.Controllers
{
    /// <summary>
    /// 用于处理微信回调的信息，用于启用服务器配置
    /// </summary>
    public class WeiXinController : Controller
    {
        readonly Func<string> _getRandomFileName = () => DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);

        private string appId= WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret= WebConfigurationManager.AppSettings["WeixinAppSecret"];
        private string token= WebConfigurationManager.AppSettings["WeixinToken"];
        private string encodingAESKey= WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];
        

        public ActionResult Index(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        public ActionResult Index(PostModel postModel/*,[FromBody]string requestXml*/)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                return Content("参数错误！");
            }


            postModel.Token = token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = encodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = appId;//根据自己后台的设置保持一致


            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);

            try
            {

                #region 记录 Request 日志

                var logPath = Utilities.Server.GetMapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。

                var requestDocumentFileName = Path.Combine(logPath, string.Format("{0}_Request_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName));
                var ecryptRequestDocumentFileName = Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName));
                messageHandler.RequestDocument.Save(requestDocumentFileName);
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(ecryptRequestDocumentFileName);
                }


                #endregion

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                messageHandler.Execute();

                #region 记录 Response 日志

                //测试时可开启，帮助跟踪数据

                //if (messageHandler.ResponseDocument == null)
                //{
                //    throw new Exception(messageHandler.RequestDocument.ToString());
                //}

                var responseDocumentFileName = Path.Combine(logPath, string.Format("{0}_Response_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName));
                var ecryptResponseDocumentFileName = Path.Combine(logPath, string.Format("{0}_Response_Final_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName));

                if (messageHandler.ResponseDocument != null)
                {
                    using (FileStream fs = new FileStream(responseDocumentFileName, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        messageHandler.ResponseDocument.Save(fs);
                    }
                }

                if (messageHandler.UsingEcryptMessage && messageHandler.FinalResponseDocument != null)
                {
                    using (FileStream fs = new FileStream(ecryptResponseDocumentFileName, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        //记录加密后的响应信息
                        messageHandler.FinalResponseDocument.Save(fs);
                    }
                }

                #endregion`

                //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new WeixinResult(messageHandler);//v0.8+
                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                #region 异常处理
                WeixinTrace.Log("MessageHandler错误：{0}", ex.Message);

                using (var fs = new FileStream(Utilities.Server.GetMapPath("~/App_Data/Error_" + _getRandomFileName() + ".txt"), FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    using (TextWriter tw = new StreamWriter(fs))
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
                    }
                }
                return Content("");
                #endregion
            }
        }
    }
}