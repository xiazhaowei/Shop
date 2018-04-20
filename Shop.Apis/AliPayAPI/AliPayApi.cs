using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Xia.Common.Extensions;

namespace Shop.Apis.AliPayAPI
{
    public class AliPayApi
    {
        /// <summary>
        /// 生成APP支付订单信息
        /// </summary>
        /// <returns></returns>
        public static string GetAlipayOrderInfo(decimal amount,string orderNumber)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", 
                Config.APPID, 
                Config.APP_PRIVATE_KEY,
                "json", "1.0", "RSA2", 
                Config.ALIPAY_PUBLIC_KEY, 
                Config.CHARSET, false);
            
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = "订单付款{0}".FormatWith(orderNumber);
            model.Subject = "五福天下商城订单付款";
            model.TotalAmount = amount.ToString();
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = orderNumber;
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl("http://m.wftx666.com");
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //return HttpUtility.HtmlEncode(response.Body);
            return response.Body;
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
        }
    }
}