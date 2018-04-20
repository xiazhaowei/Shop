using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Open;
using Senparc.Weixin.Open.Containers;
using Senparc.Weixin.Open.OAuthAPIs;
using Shop.WeiXin.Services;
using Shop.WeiXin.Services.impl;
using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Shop.WeiXin.Controllers
{
    public class AccountController : BaseController
    {
        private IUserGenerator _userGenerator;
        private IUserQueryService _userQueryService;
        private IQrCodeService _qrCodeService;
        private IUserOAuthService _userOAuthService;
        public static readonly string appID = WebConfigurationManager.AppSettings["WeixinAppId"];
        public static readonly string appsecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];

        public AccountController(IUserGenerator userGenerator,
            IUserQueryService userQueryService,
            IQrCodeService qrCodeService,
            IUserOAuthService userOAuthService)
        {
            _userGenerator = userGenerator;
            _userQueryService = userQueryService;
            _qrCodeService = qrCodeService;
            _userOAuthService = userOAuthService;
        }

        #region 用户授权信息
        /// <summary>
        /// 获取用户授权
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //获取授权公众号信息
            var authInfo = TryGetAuthorizerInfoResult();
            var appId =authInfo.authorization_info.authorizer_appid;//这里为授权的公众号appid

            ViewData["UrlUserInfo"] = OAuthApi.GetAuthorizeUrl(appId, component_AppId,
                "http://wx.wftx666.com/Account/UserInfoCallback",
                "wofweixin", new[] { OAuthScope.snsapi_userinfo,OAuthScope.snsapi_base});
            return View();
        }

        /// <summary>
        /// 用户授权回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state, string appId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            if (state != "wofweixin")
            {
                return Content("验证失败！请从正规途径进入！");
            }
            OAuthAccessTokenResult result = null;
            //通过，用code换取access_token
            try
            {
                var componentAccessToken = ComponentContainer.GetComponentAccessToken(component_AppId);
                result = OAuthApi.GetAccessToken(appId, component_AppId, componentAccessToken, code);
            }
            catch (Exception ex)
            {
                return Content("用code换取accesstoken"+ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //存储或更新用户的访问令牌
            _userGenerator.UpdateOAuthAccessToken(new Models.UserOAuthAccessToken
            {
                OpenId = result.openid,
                StartTime = DateTime.Now,
                AccessToken = result.access_token,
            });

            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                //存储用户的微信资料
                _userGenerator.CreateUser(new Models.UserInfo
                {
                    OpenId = userInfo.openid,
                    UnionId = userInfo.unionid,
                    NickName = userInfo.nickname,
                    Gender = userInfo.sex.ToString(),
                    Province = userInfo.province,
                    City= userInfo.city,
                    County=userInfo.country,
                    Portrait = userInfo.headimgurl,
                });
                return View(userInfo);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content("获取用户信息"+ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 用户推广二维码页面
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult UserQrCode(string openId)
        {
            //用户关注了公众号，就可以获取用户的信息
            var userInfo = _userOAuthService.UserInfo(openId);
            if (userInfo == null)
            {
                return Content("获取用户信息失败");
            }
            //创建二维码
            var qrcodeUrl = _qrCodeService.CreateShareQrCode(openId);
            ViewData["UserInfo"] = userInfo;
            ViewData["QrCodeUrl"] = qrcodeUrl;
            return View();
        }

        /// <summary>
        /// 进入商城 垫片页
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult EnterShop(string code,string state)
        {
            //用code 获取access_token 和用户信息
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("index");
            }
            var accessToken = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(appID, appsecret, code);
            if (accessToken.errcode != ReturnCode.请求成功)
            {
                //如果令牌的错误信息不等于请求成功，则需要重新返回授权界面
                return RedirectToAction("index");
            }
            try
            {
                var userInfo = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(accessToken.access_token, accessToken.openid);
                //传递参数到移动页
                var headimgurl = HttpUtility.UrlEncode(userInfo.headimgurl);
                var nickname = HttpUtility.UrlEncode(userInfo.nickname);
                var province = HttpUtility.UrlEncode(userInfo.province);
                var city = HttpUtility.UrlEncode(userInfo.city);
                string shopUrl = $"http://m.wftx666.com/#/weixinlogin/{nickname}/{province}/{city}/{userInfo.openid}/{userInfo.unionid}/{headimgurl}";
                ViewData["ShopUrl"] = shopUrl;
                return View();
            }
            catch{}
            return View();
        }


        /// <summary>
        /// 针对早已关注的用户，重新创建用户数据库信息
        /// </summary>
        /// <returns></returns>
        public ActionResult InitUsersInfo()
        {
            var accessToken = new MpServiceBase().AccessToken();
            
            if(accessToken != null)
            {
                //获取所有已关注的用户
                var openids = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Get(accessToken, null);
                //遍历用户
                foreach (var openid in openids.data.openid)
                {
                    var userInfo = _userOAuthService.UserInfo(accessToken, openid);
                    //保存到数据库中
                    _userGenerator.CreateUser(new Models.UserInfo
                    {
                        OpenId = openid,
                        UnionId = userInfo.unionid,
                        NickName = userInfo.nickname,
                        Province = userInfo.province,
                        City = userInfo.city,
                        County = userInfo.country,
                        Gender = userInfo.sex.ToString(),
                        Portrait = userInfo.headimgurl,
                        ParentOpenId = ""
                    });
                }
            }

            return Content("初始化用户完成");
        }
        
    }
}