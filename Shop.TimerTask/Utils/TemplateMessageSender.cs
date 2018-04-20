using ECommon.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Shop.TimerTask.Utils
{
    /// <summary>
    /// 商城第三方平台的发送模板消息服务
    /// </summary>
    [Component]
    public class TemplateMessageSender:ITemplateMessageSender
    {
        private static string serviceHost = "http://wx.wftx666.com/weixinservice";//接口地址
        private static string incantation = "helloxiatian!@#";//接口访问咒语

        //发送服务器报错消息
        public void SendExTemplateMessage(string openId,string title,string content,string url)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("openId", openId);
            parameters.Add("title", HttpUtility.UrlEncode(title));
            parameters.Add("content", HttpUtility.UrlEncode(content));
            parameters.Add("url", HttpUtility.UrlEncode(url));

            var method = "SendExTemplateMessage";
            Send(method, parameters);
        }
        
        //发送支付密码修改的通知
        public void SendChangePayPasswordTemplateMessage(string openId, string userName, string payPassword,string shopName, string url)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("openId", openId);
            parameters.Add("userName", HttpUtility.UrlEncode(userName));
            parameters.Add("payPassword", HttpUtility.UrlEncode(payPassword));
            parameters.Add("shopName", HttpUtility.UrlEncode(shopName));
            parameters.Add("url", HttpUtility.UrlEncode(url));

            var method = "SendChangePayPasswordTemplateMessage";
            Send(method, parameters);
        }
        //资金到账提醒
        public void SendMoneyToWalletTemplateMessage(string openId, string dateTime, string amount, string finallyValue,string remark, string url)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("openId", openId);
            parameters.Add("dateTime", HttpUtility.UrlEncode(dateTime));
            parameters.Add("amount", HttpUtility.UrlEncode(amount));
            parameters.Add("finallyValue", HttpUtility.UrlEncode(finallyValue));
            parameters.Add("remark", HttpUtility.UrlEncode(remark));
            parameters.Add("url", HttpUtility.UrlEncode(url));

            var method = "SendMoneyToWalletTemplateMessage";
            Send(method, parameters);
        }


        //发送新订单消息
        public void SendNewOrderTemplateMessage(string openId,
            string number, string goodsName, string customerName, string customerMobile, string expressAddress, string url)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("openId", openId);
            parameters.Add("number", number);
            parameters.Add("goodsName", HttpUtility.UrlEncode(goodsName));
            parameters.Add("customerName", HttpUtility.UrlEncode(customerName));
            parameters.Add("customerMobile", HttpUtility.UrlEncode(customerMobile));
            parameters.Add("expressAddress", HttpUtility.UrlEncode(expressAddress));
            parameters.Add("url", HttpUtility.UrlEncode(url));

            var method = "SendNewOrderTemplateMessage";
            Send(method, parameters);
        }

        /// <summary>
        /// 请求发送服务，发送消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private TemplateMessageResult Send(string method, IDictionary<string, string> parameters)
        {
            try
            {
                var host = $"{serviceHost}/{method}/";

                HttpWebRequest request = null;
                if (host.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    request = WebRequest.Create(host) as HttpWebRequest;
                }
                else
                {
                    request = WebRequest.Create(host) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                //发送POST数据  
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    //添加访问咒语
                    parameters.Add("token", incantation);
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                            i++;
                        }
                    }
                    byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                string[] values = request.Headers.GetValues("Content-Type");
                var httpResponse= request.GetResponse() as HttpWebResponse;

                using (Stream s = httpResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    var jsonResultText= reader.ReadToEnd();
                    //将相应序列化未对象
                    return JsonConvert.DeserializeObject<TemplateMessageResult>(jsonResultText);
                }
            }
            catch{}
            return new TemplateMessageResult { Code=400,Message="发送失败"};
        }
    }

    //接口请求结果
    public class TemplateMessageResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
