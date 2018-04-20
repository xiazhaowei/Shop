using Shop.WeiXin.Services;
using System.Web.Mvc;

namespace Shop.WeiXin.Controllers
{
    //对商城提供微信服务，如发送短信模板到公众号
    public class WeiXinServiceController : BaseController
    {
        private IUserQueryService _userQueryService;
        private ITemplateMessageService _templateMessageService;
        private static string _incantation = "helloxiatian!@#";//访问咒语

        public WeiXinServiceController(IUserQueryService userQueryService,
            ITemplateMessageService templateMessageService)
        {
            _userQueryService = userQueryService;
            _templateMessageService = templateMessageService;
        }

        //  发送服务错误模板消息
        [HttpPost]
        public ActionResult SendExTemplateMessage(string token,string openId,
            string title,string content,string url)
        {
            if(token!= _incantation)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //这里的openid为unionId 要转换为公众号的openid
            var userInfo = _userQueryService.FindByUnionId(openId);
            if(userInfo == null)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //发送模板消息
            _templateMessageService.SendExTemplateMessage(userInfo.OpenId,title, content, url);

            return Json(new { Code = 200, Message = "发送成功" });
        }

        //资金到账提醒
        [HttpPost]
        public ActionResult SendMoneyToWalletTemplateMessage(string token, string openId,
            string dateTime, string amount,string finallyValue,string remark, string url)
        {
            if (token != _incantation)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //这里的openid为unionId 要转换为公众号的openid
            var userInfo = _userQueryService.FindByUnionId(openId);
            if (userInfo == null)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //发送模板消息
            _templateMessageService.SendMoneyToWalletTemplateMessage(userInfo.OpenId, dateTime, amount,finallyValue,remark, url);

            return Json(new { Code = 200, Message = "发送成功" });
        }

        [HttpPost]
        public ActionResult SendChangePayPasswordTemplateMessage(string token, string openId,
            string userName, string payPassword, string shopName, string url)
        {
            if (token != _incantation)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //这里的openid为unionId 要转换为公众号的openid
            var userInfo = _userQueryService.FindByUnionId(openId);
            if (userInfo == null)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //发送模板消息
            _templateMessageService.SendChangePayPasswordTemplateMessage(userInfo.OpenId, userName, payPassword, shopName, url);

            return Json(new { Code = 200, Message = "发送成功" });
        }
        //发送新订单消息
        [HttpPost]
        public ActionResult SendNewOrderTemplateMessage(string token,string openId,
             string number, string goodsName, string customerName, string customerMobile, string expressAddress, string url)
        {
            if (token != _incantation)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //这里的openid为unionId 要转换为公众号的openid
            var userInfo = _userQueryService.FindByUnionId(openId);
            if (userInfo == null)
            {
                return Json(new { Code = 400, Message = "不被允许的访问" });
            }
            //发送模板消息
            _templateMessageService.SendNewOrderTemplateMessage(userInfo.OpenId, 
                number,goodsName,customerName,customerMobile,expressAddress, url);

            return Json(new { Code = 200, Message = "发送成功" });
        }
    }
}