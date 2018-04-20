using ENode.Commanding;
using Microsoft.AspNet.Identity;
using Shop.Api.Extensions;
using Shop.Api.Models;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Users;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.User;
using Shop.Api.Services;
using Shop.Api.Utils;
using Shop.Commands.Users;
using Shop.Commands.Users.ExpressAddresses;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xia.Common;
using Xia.Common.Extensions;
using Xia.Common.Secutiry;
using Xia.Common.Utils;
using ApiResponse = Shop.Api.Models.Response.User;

namespace Shop.Api.Controllers
{
    public class UserController : BaseApiController
    {
        private static readonly TimeSpan RegUserWaitTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan RegUserPollInterval = TimeSpan.FromMilliseconds(750);

        private IUserQueryService _userQueryService;//用户Q端
        private IWalletQueryService _walletQueryService;//钱包Q端
        private ICartQueryService _cartQueryService;//购物车Q端
        private IStoreQueryService _storeQueryService;//商家
        private IStoreOrderQueryService _storeOrderQueryService;//商家
        private IPartnerQueryService _partnerQueryService;
        private ISMSender _smSender;
        /// <summary>
        /// IOC 构造函数注入
        /// </summary>
        /// <param name="commandService"></param>
        /// <param name="conferenceQueryService"></param>
        public UserController(ICommandService commandService,ISMSender smSender,IContextService contentService,
            IUserQueryService userQueryService,
            IWalletQueryService walletQueryService,
            ICartQueryService cartQueryService,
            IStoreQueryService storeQueryService,
            IPartnerQueryService partnerQueryService,
            IStoreOrderQueryService storeOrderQueryService) : base(commandService,contentService)
        {
            _smSender = smSender;
            _userQueryService = userQueryService;
            _walletQueryService = walletQueryService;
            _cartQueryService = cartQueryService;
            _storeQueryService = storeQueryService;
            _storeOrderQueryService = storeOrderQueryService;
            _partnerQueryService = partnerQueryService;
        }


        #region 登陆 注册

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse SendMsgCode(SendMsgCodeRequest request)
        {
            request.CheckNotNull(nameof(request));
            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "错误的手机号码" };
            }
            //创建验证码
            var code = new Random().GetRandomNumberString(6);
            var mobile = request.Mobile;
            //发送验证码短信
            _smSender.SendMsgCode(mobile, code);

            //验证码缓存 设置过期期限缓存策略默认已设置好
            _apiSession.SetMsgCode(mobile, code);

            return new SendMsgCodeResponse() {
                Token = mobile,
                MsgCode = code
            };
        }
        /// <summary>
        /// 检查手机号是否可以注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse CheckPhoneAvailable(CheckPhoneAvailableRequest request)
        {
            request.CheckNotNull(nameof(request));

            if (!request.Phone.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "错误的手机号码" };
            }

            if (_userQueryService.CheckMobileIsAvliable(request.Phone))
            {
                return new CheckPhoneAvailableResponse { Result = true };
            }
            else
            {
                return new CheckPhoneAvailableResponse { Result = false, Message = "该手机号已经注册." };
            }
        }

        [HttpPost]
        public BaseApiResponse VerifyMsgCode(VerifyMsgCodeRequest request)
        {
            request.CheckNotNull(nameof(request));
            //验证码验证
            if (request.Token.IsNullOrEmpty() || _apiSession.GetMsgCode(request.Token) == null)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码过期" };
            }
            if (_apiSession.GetMsgCode(request.Token) != request.MsgCode)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码错误" };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 注册新用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseApiResponse> Register(RegisterRequest request)
        {
            request.CheckNotNull(nameof(request));
            //验证码验证
            if (request.Token.IsNullOrEmpty() || _apiSession.GetMsgCode(request.Token) == null)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码过期" };
            }
            if (_apiSession.GetMsgCode(request.Token) != request.MsgCode)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码错误" };
            }

            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "手机号不正确" };
            }
            if (request.Password.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "密码长度不能大于20字符" };
            }
            if (request.Password.Contains(" "))
            {
                return new BaseApiResponse { Code = 400, Message = "密码不能包含空格." };
            }

            //检查手机号是否可用
            if (!_userQueryService.CheckMobileIsAvliable(request.Mobile))
            {
                return new BaseApiResponse { Code = 400, Message = "该手机号已注册." };
            }
            //验证推荐人
            var parentId = Guid.Empty;

            if (request.ParentId != Guid.Empty)
            {
                var parent = _userQueryService.FindUser(request.ParentId);
                if (parent == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "没有找到该推荐人." };
                }
                parentId = request.ParentId;
            }
            if (!request.ParentMobile.IsNullOrEmpty())
            {
                if (!request.ParentMobile.IsMobileNumber())
                {
                    return new BaseApiResponse { Code = 400, Message = "推荐人手机号不正确" };
                }
                if (request.ParentMobile == request.Mobile)
                {
                    return new BaseApiResponse { Code = 400, Message = "推荐人不能是自己" };
                }
                var parent = _userQueryService.FindUser(request.ParentMobile);
                if (parent == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "没有找到该推荐人." };
                }
                parentId = parent.Id;
            }
            if (request.NickName.IsNullOrEmpty())
            {
                request.NickName = "用户" + StringGenerator.Generate(4);
            }
            if (request.Portrait.IsNullOrEmpty())
            {
                request.Portrait = "http://wftx-goods-img-details.oss-cn-shanghai.aliyuncs.com/default-userpic/userpic.png";
            }
            if (request.Region.IsNullOrEmpty())
            {
                request.Region = "北京";
            }
            var command = new CreateUserCommand(
                GuidUtil.NewSequentialId(),
                parentId,
                request.NickName,
                request.Portrait,
                "保密",
                request.Mobile,
                request.Region,
                PasswordHash.CreateHash(request.Password),
                request.WeixinId,//微信ID
                request.UnionId//unionid
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            return new RegisterResponse
            {
                Result = new RegisterResult
                {
                    Id = command.AggregateRootId.ToString()
                }
            };
        }

        /// <summary>
        /// 绑定微信账号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseApiResponse> BindWeixin(BindWeixinRequest request)
        {
            #region 验证
            request.CheckNotNull(nameof(request));
            //验证码验证
            if (request.Token.IsNullOrEmpty() || _apiSession.GetMsgCode(request.Token) == null)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码过期" };
            }
            if (_apiSession.GetMsgCode(request.Token) != request.MsgCode)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码错误" };
            }

            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "手机号不正确" };
            }
            if (request.Password.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "密码长度不能大于20字符" };
            }
            if (request.Password.Contains(" "))
            {
                return new BaseApiResponse { Code = 400, Message = "密码不能包含空格." };
            }
            #endregion
            var user = _userQueryService.FindUserByUnionId(request.UnionId);
            if (user != null)
            {
                return new BaseApiResponse { Code = 401, Message = "该微信已经绑定，可以直接登录" };
            }
            var userInfo = _userQueryService.FindUser(request.Mobile);
            if (userInfo == null)
            {
                //没有该用户注册用户
                var parentId = Guid.Empty;
                var regcommand = new CreateUserCommand(
                GuidUtil.NewSequentialId(),
                parentId,
                request.NickName,
                request.Portrait,
                "保密",
                request.Mobile,
                request.Region,
                PasswordHash.CreateHash(request.Password),
                request.WeixinId,//微信ID
                request.UnionId//unionid
                );
                var regresult = await ExecuteCommandAsync(regcommand);
                if (!regresult.IsSuccess())
                {
                    return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(regresult.GetErrorMessage()) };
                }
                //等待5秒，完成用户注册
                userInfo = WaitUntilUserRegCompleted(request.Mobile).Result;
                if (userInfo == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "注册失败" };
                }
            }
            else
            {
                //验证密码
                if (!PasswordHash.ValidatePassword(request.Password, userInfo.Password))
                {
                    return new BaseApiResponse { Code = 400, Message = "密码错误" };
                }
                //设置绑定信息
                var command = new BindWeixinCommand(request.WeixinId, request.UnionId)
                {
                    AggregateRootId = userInfo.Id
                };
                var result = await ExecuteCommandAsync(command);
                if (!result.IsSuccess())
                {
                    return new BaseApiResponse { Code = 400, Message = "设置微信信息失败：{0}".FormatWith(result.GetErrorMessage()) };
                }
            }
            //直接登录,到了这一步手机肯定已经注册了,返回登录信息即可
            try
            {
                //获取钱包信息
                var walletinfo = _walletQueryService.Info(userInfo.WalletId);
                if (walletinfo == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "获取钱包信息失败" };
                }
                //购物车信息
                var cart = _cartQueryService.Info(userInfo.CartId);
                if (cart == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "获取购物车信息失败" };
                }
                //店铺信息
                var storeId = "";
                var storeinfo = _storeQueryService.InfoByUserId(userInfo.Id);
                if (storeinfo != null)
                {
                    storeId = storeinfo.Id.ToString();
                }
                SignIn(userInfo.Id.ToString(), userInfo.WalletId.ToString(), userInfo.Mobile, true);
                return new LoginResponse
                {
                    UserInfo = new UserInfo
                    {
                        Id = userInfo.Id,
                        ParentId = userInfo.ParentId,
                        NickName = userInfo.NickName,
                        Portrait = userInfo.Portrait.ToOssStyleUrl(OssImageStyles.UserPortrait.ToDescription()),
                        Mobile = userInfo.Mobile,
                        Gender = userInfo.Gender,
                        Region = userInfo.Region,
                        Role = userInfo.Role.ToDescription(),
                        StoreId = storeId,
                        CartId = userInfo.CartId.ToString(),
                        CartGoodsCount = cart.GoodsCount,
                        IsLocked = userInfo.IsLocked,
                        Token = userInfo.Id.ToString(),
                        WeixinId=userInfo.WeixinId,
                        UnionId=userInfo.UnionId,
                    },
                    WalletInfo = new WalletInfo
                    {
                        Id = walletinfo.Id,
                        AccessCode = walletinfo.AccessCode,
                        Cash = walletinfo.Cash,
                        Benevolence = walletinfo.Benevolence,
                        Earnings = walletinfo.Earnings,
                        YesterdayEarnings = walletinfo.YesterdayEarnings,
                        IsFreeze = walletinfo.IsFreeze
                    }
                };
            }
            catch (Exception e)
            {
                return new BaseApiResponse { Code = 400, Message = e.Message };
            }
        }
        
        /// <summary>
        /// 用户登录。返回用户基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public  BaseApiResponse Login(LoginRequest request)
        {
            request.CheckNotNull(nameof(request));
            if (!request.Mobile.IsMobileNumber())
            {//是否手机号
                return new BaseApiResponse { Code = 400, Message = "手机号格式不正确" };
            }
            var userinfo = _userQueryService.FindUser(request.Mobile);
            //验证用户
            if (userinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该账号" };
            }
            //验证账号
            if (userinfo.IsLocked == UserLock.Locked)
            {
                return new BaseApiResponse { Code = 400, Message = "账号锁定" };
            }
            //验证密码
            if (!PasswordHash.ValidatePassword(request.Password, userinfo.Password))
            {
                return new BaseApiResponse { Code = 400, Message = "登录密码错误" };
            }
            try
            {
                //获取钱包信息
                var walletinfo = _walletQueryService.Info(userinfo.WalletId);
                if (walletinfo == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "获取钱包信息失败" };
                }

                //购物车信息
                var cart = _cartQueryService.Info(userinfo.CartId);
                if (cart == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "获取购物车信息失败" };
                }
                //店铺信息
                var storeId = "";
                var storeinfo = _storeQueryService.InfoByUserId(userinfo.Id);
                if (storeinfo != null)
                {
                    storeId = storeinfo.Id.ToString();
                }

                SignIn(userinfo.Id.ToString(),userinfo.WalletId.ToString(), userinfo.Mobile, true);

                return new LoginResponse
                {
                    UserInfo = new UserInfo
                    {
                        Id = userinfo.Id,
                        ParentId = userinfo.ParentId,
                        NickName = userinfo.NickName,
                        Portrait = userinfo.Portrait.ToOssStyleUrl(OssImageStyles.UserPortrait.ToDescription()),
                        Mobile = userinfo.Mobile,
                        Gender = userinfo.Gender,
                        Region = userinfo.Region,
                        Role = userinfo.Role.ToDescription(),
                        StoreId = storeId,
                        CartId = userinfo.CartId.ToString(),
                        CartGoodsCount = cart.GoodsCount,
                        IsLocked = userinfo.IsLocked,
                        Token = userinfo.Id.ToString(),
                        WeixinId=userinfo.WeixinId,
                        UnionId=userinfo.UnionId,
                    },
                    WalletInfo = new WalletInfo
                    {
                        Id = walletinfo.Id,
                        AccessCode = walletinfo.AccessCode,
                        Cash = walletinfo.Cash,
                        Benevolence = walletinfo.Benevolence,
                        Earnings = walletinfo.Earnings,
                        YesterdayEarnings = walletinfo.YesterdayEarnings,
                        IsFreeze = walletinfo.IsFreeze
                    }
                };
            }
            catch (Exception e)
            {
                return new BaseApiResponse { Code = 400, Message = e.Message };
            }
        }

        /// <summary>
        /// 微信登录用openid 和unionid登录，如果已经绑定微信即可直接登录，否则返回未绑定提示
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseApiResponse WeixinLogin(WeixinLoginRequest request)
        {
            try
            {
                var userinfo = _userQueryService.FindUserByUnionId(request.UnionId);
                if (userinfo == null)
                {
                    return new BaseApiResponse { Code = 401, Message = "用户没有绑定该微信" };
                }
                //获取钱包信息
                var walletinfo = _walletQueryService.Info(userinfo.WalletId);
                if (walletinfo == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "获取钱包信息失败" };
                }

                //购物车信息
                var cart = _cartQueryService.Info(userinfo.CartId);
                if (cart == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "获取购物车信息失败" };
                }
                //店铺信息
                var storeId = "";
                var storeinfo = _storeQueryService.InfoByUserId(userinfo.Id);
                if (storeinfo != null)
                {
                    storeId = storeinfo.Id.ToString();
                }

                //登录系统
                SignIn(userinfo.Id.ToString(), userinfo.WalletId.ToString(), userinfo.Mobile, true);

                return new LoginResponse
                {
                    UserInfo = new UserInfo
                    {
                        Id = userinfo.Id,
                        ParentId = userinfo.ParentId,
                        NickName = userinfo.NickName,
                        Portrait = userinfo.Portrait.ToOssStyleUrl(OssImageStyles.UserPortrait.ToDescription()),
                        Mobile = userinfo.Mobile,
                        Gender = userinfo.Gender,
                        Region = userinfo.Region,
                        Role = userinfo.Role.ToDescription(),
                        StoreId = storeId,
                        CartId = userinfo.CartId.ToString(),
                        CartGoodsCount = cart.GoodsCount,
                        IsLocked = userinfo.IsLocked,
                        Token = userinfo.Id.ToString(),
                        WeixinId=userinfo.WeixinId,
                        UnionId=userinfo.UnionId,
                    },
                    WalletInfo = new WalletInfo
                    {
                        Id = walletinfo.Id,
                        AccessCode = walletinfo.AccessCode,
                        Cash = walletinfo.Cash,
                        Benevolence = walletinfo.Benevolence,
                        Earnings = walletinfo.Earnings,
                        YesterdayEarnings = walletinfo.YesterdayEarnings,
                        IsFreeze = walletinfo.IsFreeze
                    }
                };
            }
            catch (Exception e)
            {
                return new BaseApiResponse { Code = 400, Message = e.Message };
            }
        }

        /// <summary>
        /// 同步微信资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> UnifyWeixinInfo(UnifyWeixinInfoRequest request)
        {
            request.CheckNotNull(nameof(request));

            var user = _userQueryService.FindUser(request.Id);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到用户" };
            }
            //更新资料
            var command = new UpdateInfoCommand(request.NickName,request.Region,request.Portrait)
            {
                AggregateRootId = request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse MsgLogin(MsgLoginRequest request)
        {
            if (!request.Mobile.IsMobileNumber())
            {//是否手机号
                return new BaseApiResponse { Code = 400, Message = "手机号格式不正确" };
            }
            //验证码验证
            if (request.Token.IsNullOrEmpty() || _apiSession.GetMsgCode(request.Token) == null)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码过期" };
            }
            if (_apiSession.GetMsgCode(request.Token) != request.MsgCode)
            {
                return new BaseApiResponse { Code = 400, Message = "验证码错误" };
            }

            var userinfo = _userQueryService.FindUser(request.Mobile);
            //验证用户
            if (userinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该账号" };
            }

            //获取钱包信息
            var walletinfo = _walletQueryService.Info(userinfo.WalletId);
            if (walletinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "获取钱包信息失败" };
            }
            //购物车信息
            var cart = _cartQueryService.Info(userinfo.CartId);
            if (cart == null)
            {
                return new BaseApiResponse { Code = 400, Message = "获取购物车信息失败" };
            }
            //店铺信息
            var storeId = "";
            var storeinfo = _storeQueryService.InfoByUserId(userinfo.Id);
            if (storeinfo != null)
            {
                storeId = storeinfo.Id.ToString();
            }

            SignIn(userinfo.Id.ToString(),userinfo.WalletId.ToString(), userinfo.Mobile, true);

            return new LoginResponse
            {
                UserInfo = new UserInfo
                {
                    Id = userinfo.Id,
                    ParentId = userinfo.ParentId,
                    NickName = userinfo.NickName,
                    Portrait = userinfo.Portrait,
                    Mobile = userinfo.Mobile,
                    Gender = userinfo.Gender,
                    Region = userinfo.Region,
                    Role = userinfo.Role.ToDescription(),
                    StoreId = storeId,
                    CartId = userinfo.CartId.ToString(),
                    CartGoodsCount = cart.GoodsCount,
                    Token = userinfo.Id.ToString()
                },
                WalletInfo = new WalletInfo
                {
                    Id = walletinfo.Id,
                    AccessCode = walletinfo.AccessCode,
                    Cash = walletinfo.Cash,
                    Benevolence = walletinfo.Benevolence,
                    Earnings = walletinfo.Earnings,
                    YesterdayEarnings = walletinfo.YesterdayEarnings
                }
            };
        }
        

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse Info(InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.Id);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到用户" };
            }
            return new UserInfoResponse
            {
                UserInfo = new UserInfo
                {
                    Id = user.Id,
                    NickName = user.NickName,
                    Portrait = user.Portrait,
                    Region = user.Region
                }
            };
        }

        /// <summary>
        /// 设置我的推荐人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetMyParent(SetMyParentRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "请输入正确的手机号" };
            }
            var parent = _userQueryService.FindUser(request.Mobile);
            if (parent == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该推荐人" };
            }
            var user = _userQueryService.FindUser(currentAccount.UserId.ToGuid());
            if (user.CreatedOn < parent.CreatedOn)
            {
                return new BaseApiResponse { Code = 400, Message = "推荐人的注册日期似乎比您晚" };
            }

            var command = new SetMyParentCommand(parent.Id)
            {
                AggregateRootId = currentAccount.UserId.ToGuid()
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        #endregion

        #region 收货地址

        /// <summary>
        /// 获取当前用户的所以收货地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ExpressAddressesResponse ExpressAddresses()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            //用户登录成功就可用通过token 初始化_user 对象 ，代表当前登录用户

            //应该从缓存中获取用户的地址信息
            var addresses = _userQueryService.GetExpressAddresses(currentAccount.UserId.ToGuid());
            return new ExpressAddressesResponse
            {
                ExpressAddresses = addresses.Select(x => new ApiResponse.ExpressAddress
                {
                    Id = x.Id,
                    Name = x.Name,
                    Mobile = x.Mobile,
                    Address = x.Address,
                    Region = x.Region,
                    Zip = x.Zip
                }).ToList()
            };
        }


        /// <summary>
        /// 添加收货地址 只有登录才可访问
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> AddExpressAddress(AddExpressAddressRequest request)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            request.CheckNotNull(nameof(request));
            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "请输入正确的手机号" };
            }
            
            var command = new AddExpressAddressCommand(currentAccount.UserId.ToGuid(),
                request.Name,
                request.Mobile,
                request.Region,
                request.Address,
                request.Zip);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> DelExpressAddress(DelRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var command = new RemoveExpressAddressCommand(currentAccount.UserId.ToGuid(), request.Id);
            var result = await ExecuteCommandAsync(command);

            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        #endregion

        /// <summary>
        /// 用户登录。返回用户基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public   BaseApiResponse ShopLogin(LoginRequest request)
        {
            request.CheckNotNull(nameof(request));
            if (!request.Mobile.IsMobileNumber())
            {//是否手机号
                return new BaseApiResponse { Code = 400, Message = "手机号格式不正确" };
            }
            var userinfo = _userQueryService.FindUser(request.Mobile);
            //验证用户
            if (userinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该账号" };
            }
            //验证密码
            if (!PasswordHash.ValidatePassword(request.Password, userinfo.Password))
            {
                return new BaseApiResponse { Code = 400, Message = "登录密码错误" };
            }


            //店铺信息
            var storeinfo = _storeQueryService.InfoByUserId(userinfo.Id);
            if (storeinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "您没有店铺" };
            }
            if (storeinfo.IsLocked)
            {
                return new BaseApiResponse { Code = 400, Message = "店铺被锁定，无法登陆" };
            }
            if(storeinfo.Status!=StoreStatus.Normal)
            {
                return new BaseApiResponse { Code = 400, Message = "您的店铺状态异常，无法登录" };
            }
             SignIn(userinfo.Id.ToString(), userinfo.WalletId.ToString(),userinfo.Mobile, true);

            return new ShopLoginResponse
            {
                UserInfo = new UserInfo
                {
                    Id = userinfo.Id,
                    NickName = userinfo.NickName,
                    Portrait = userinfo.Portrait.ToOssStyleUrl(OssImageStyles.UserPortrait.ToDescription()),
                    Mobile = userinfo.Mobile,
                    Gender = userinfo.Gender,
                    Region = userinfo.Region,
                    Role = userinfo.Role.ToDescription(),
                    StoreId = storeinfo.Id.ToString(),
                    CartId = userinfo.CartId.ToString(),
                    Token = userinfo.Id.ToString()
                },
                StoreInfo = new StoreInfo
                {
                    Id = storeinfo.Id,
                    Name = storeinfo.Name,
                    Description = storeinfo.Description,
                    Region = storeinfo.Region,
                    Address = storeinfo.Address,
                    TodayOrder = storeinfo.TodayOrder,
                    TodaySale = storeinfo.TodaySale,
                    TotalOrder = storeinfo.TotalOrder,
                    TotalSale = storeinfo.TotalSale
                },
                ReturnAddressInfo=new ReturnAddressInfo
                {
                    StoreId=storeinfo.Id,
                    Name=storeinfo.ReturnAddressName,
                    Mobile=storeinfo.ReturnAddressMobile,
                    Address=storeinfo.ReturnAddress
                }
            };
        }

        /// <summary>
        /// 所有的VIP用户都报一个升级单
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<BaseApiResponse> DoRoleUpdate()
        //{
        //    var userInfos = _userQueryService.Users().Where(x => x.Role == UserRole.Passer);
        //    foreach(var userInfo in userInfos)
        //    {
        //        var command = new AcceptNewUpdateOrderCommand(1,UpdateOrderType.VipOrder)
        //        {
        //            AggregateRootId = userInfo.Id
        //        };
        //        await ExecuteCommandAsync(command);
        //    }
        //    return new BaseApiResponse();
        //}

        /// <summary>
        /// 获取用户信息 包含一些统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse MeInfo()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            //应该设置缓存


            var userinfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());
            //验证用户
            if (userinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该账号" };
            }
            //钱包信息
            var walletinfo = _walletQueryService.Info(userinfo.WalletId);
            if (walletinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到钱包信息" };
            }
            //购物车信息
            var cart = _cartQueryService.Info(userinfo.CartId);
            if(cart==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到购物车信息" };
            }

            //店铺信息
            var storeId = "";
            int todayOrder = 0;
            var todaySale = 0M;
            var storeinfo = _storeQueryService.InfoByUserId(userinfo.Id);
            if (storeinfo != null)
            {
                storeId = storeinfo.Id.ToString();
                todayOrder = storeinfo.TodayOrder;
                todaySale = storeinfo.TodaySale;
            }
            //代理信息
            var partnerId = "";
            int regionTodayOrder = 0;
            var regionTodaySale = 0M;
            var partnerInfo = _partnerQueryService.FindByUserId(userinfo.Id);
            if (partnerInfo != null)
            {
                var storeOrders = _storeOrderQueryService.StoreOrders().Where(x=>x.ExpressRegion.Contains(partnerInfo.Region) && x.CreatedOn.Date.Equals(DateTime.Now.Date));
                partnerId = partnerInfo.Id.ToString();
                regionTodayOrder = storeOrders.Count();
                regionTodaySale = storeOrders.Sum(x => x.Total);
            }


            return new MeInfoResponse
            {
                UserInfo = new UserInfo
                {
                    Id = userinfo.Id,
                    ParentId=userinfo.ParentId,
                    NickName= userinfo.NickName,
                    Gender= userinfo.Gender,
                    Portrait= userinfo.Portrait.ToOssStyleUrl(OssImageStyles.UserPortrait.ToDescription()),
                    Region= userinfo.Region,
                    Mobile= userinfo.Mobile,
                    Role= userinfo.Role.ToDescription(),
                    StoreId = storeId,
                    CartId = userinfo.CartId.ToString(),
                    CartGoodsCount=cart.GoodsCount,
                    IsLocked=userinfo.IsLocked,
                    Token = userinfo.Id.ToString(),
                    WeixinId = userinfo.WeixinId,
                    UnionId=userinfo.UnionId
                },
                WalletInfo =new WalletInfo
                {
                    Id= walletinfo.Id,
                    AccessCode= walletinfo.AccessCode,
                    Cash= walletinfo.Cash,
                    ShopCash=walletinfo.ShopCash,
                    Benevolence= walletinfo.Benevolence,
                    Earnings= walletinfo.Earnings,
                    YesterdayEarnings= walletinfo.YesterdayEarnings,
                    IsFreeze=walletinfo.IsFreeze
                },
                StatisticsInfo=new StatisticsInfo
                {
                    StoreId=storeId,
                    TodayOrder=todayOrder,
                    TodaySale=todaySale,
                    PartnerId=partnerId,
                    RegionTodayOrder=regionTodayOrder,
                    RegionTodaySale=regionTodaySale
                }
            };
        }

        /// <summary>
        /// 我的直推
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse MyInvotes()
        {
            //递归获取分类包含子类
            Func<QueryServices.Dtos.UserAlis,int, object> getNodeData = null;
            getNodeData = (user,level) => {
                dynamic node = new ExpandoObject();
                node.Id = user.Id;
                node.NickName = user.NickName;
                node.Mobile = user.Mobile;
                node.CreatedOn = user.CreatedOn.GetTimeSpan();
                node.Portrait = user.Portrait;
                node.Role = user.Role.ToDescription();
                node.Invotes = new List<dynamic>();
                if (level <= 1)
                {
                    level++;
                    var invotes = _userQueryService.UserChildrens(user.Id).OrderByDescending(x => x.CreatedOn);
                    foreach (var invote in invotes)
                    {
                        node.Invotes.Add(getNodeData(invote, level));
                    }
                }
                return node;
            };
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var myInvotes = _userQueryService.UserChildrens(currentAccount.UserId.ToGuid()).OrderByDescending(x => x.CreatedOn);
            List<object> nodes = myInvotes.Select(x=>getNodeData(x,0)).ToList();

            return new MyInvotesResponse
            {
                MyInvotes = nodes
            };

        }

        /// <summary>
        /// 获取我的团队信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse MyTeamInfo()
        {
            int totalUserCount = 0;
            int directPasserCount = 0;
            int totalPasserCount = 0;
            int directVipPasserCount = 0;
            int totalVipPasserCount = 0;

            #region 获取团队信息

            
            Func<QueryServices.Dtos.UserAlis, int, object> getNodeData = null;
            getNodeData = (user, level) => {
                dynamic node = new ExpandoObject();
                node.Id = user.Id;
                node.Role = user.Role.ToDescription();
                node.Invotes = new List<dynamic>();
                totalUserCount++;
                if (user.Role == UserRole.Passer)
                {
                    totalPasserCount++;
                }
                if (user.Role == UserRole.VipPasser)
                {
                    totalVipPasserCount++;
                }
                
                var invotes = _userQueryService.UserChildrens(user.Id).OrderByDescending(x => x.CreatedOn);
                foreach (var invote in invotes)
                {
                    node.Invotes.Add(getNodeData(invote, level));
                }
                return node;
            };
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var myInvotes = _userQueryService.UserChildrens(currentAccount.UserId.ToGuid()).OrderByDescending(x => x.CreatedOn);
            List<object> nodes = myInvotes.Select(x => getNodeData(x, 0)).ToList();
            directPasserCount = myInvotes.Where(x => x.Role == UserRole.Passer).Count();
            directVipPasserCount = myInvotes.Where(x => x.Role == UserRole.VipPasser).Count();
            #endregion

            return new MyTeamResponse
            {
                TotalVipPasserCount=totalVipPasserCount,
                TotalPasserCount=totalPasserCount,
                TotalUserCount=totalUserCount,
                DirectPasserCount=directPasserCount,
                DirectVipPasserCount=directVipPasserCount,
            };

        }

        #region 基本信息

        /// <summary>
        /// 通过手机验证码设置新密码 需要登录才能访问
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ResetPassword(ResetPasswordRequest request)
        {
            if (request.Password.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "密码长度不能大于20字符" };
            }
            if (request.Password.Contains(" "))
            {
                return new BaseApiResponse { Code = 400, Message = "密码不能包含空格." };
            }
            var passwordHash = PasswordHash.CreateHash(request.Password);

            var userinfo = _userQueryService.FindUser(request.Mobile);
            if (userinfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到该用户" };
            }

            var command = new UpdatePasswordCommand(passwordHash) { AggregateRootId = userinfo.Id };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 当前登录用户通过旧密码设置新密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ChangePassword(ChangePasswordRequest request)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());
            //验证密码
            if (!PasswordHash.ValidatePassword(request.OldPassword, userInfo.Password))
            {
                return new BaseApiResponse { Code = 400, Message = "原密码错误" };
            }
            if (request.OldPassword.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "密码长度不能大于20字符" };
            }
            if (request.OldPassword.Contains(" "))
            {
                return new BaseApiResponse { Code = 400, Message = "密码不能包含空格." };
            }
            var passwordHash = PasswordHash.CreateHash(request.NewPassword);
            var command = new UpdatePasswordCommand(passwordHash) { AggregateRootId = userInfo.Id };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            

            return new BaseApiResponse();
        }

        /// <summary>
        /// 设置当前用户的昵称 需要登录才能访问
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetNickName(SetNickNameRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            if(request.NickName.Length>20)
            {
                return new BaseApiResponse { Code = 400, Message = "昵称长度不得超过20字符." };
            }
            //更新
            var command = new UpdateNickNameCommand(request.NickName) { AggregateRootId = currentAccount.UserId.ToGuid() };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            return new BaseApiResponse();
        }

        /// <summary>
        /// 设置当前用户头像地址 需要登录才能访问
        /// </summary>
        /// <param name="portraitUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetPortrait(SetPortraitRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            
            //更新
            var command = new UpdatePortraitCommand(request.Portrait) { AggregateRootId = currentAccount.UserId.ToGuid()  };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            return new BaseApiResponse();
        }


        /// <summary>
        /// 设置当前用户的性别 需要登陆才能访问
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetGender(SetGenderRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            if (!"男,女,保密".IsIncludeItem(request.Gender))
            {
                return new BaseApiResponse { Code = 400, Message = "性别参数错误，非： 男/女/保密" };
            }
            //更新
            var command = new UpdateGenderCommand(request.Gender) { AggregateRootId = currentAccount.UserId.ToGuid() };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            return new BaseApiResponse();
        }
        /// <summary>
        /// 设置当前用户的地区 需要登陆才能访问
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetRegion(SetRegionRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
           
            //更新
            var command = new UpdateRegionCommand(request.Region) { AggregateRootId = currentAccount.UserId.ToGuid()};
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }


        #endregion
        
        /// <summary>
        /// 当前用户注销。需要登录才能访问。
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public  void Logout()
        {
            var authManager = HttpContext.Current.GetOwinContext().Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        #region 私有方法
        /// <summary>
        /// 等待用户注册完成
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private Task<QueryServices.Dtos.User> WaitUntilUserRegCompleted(string mobile)
        {
            return TimerTaskFactory.StartNew<QueryServices.Dtos.User>(
                    () => _userQueryService.FindUser(mobile),
                    x => x != null,
                    RegUserPollInterval,
                    RegUserWaitTimeout);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <param name="persistentCookie"></param>
        /// <returns></returns>
        private void SignIn(string userId,string walletId, string mobile, bool persistentCookie)
        {
            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,userId),
                    new Claim(ClaimTypes.UserData,walletId),
                    new Claim(ClaimTypes.MobilePhone,mobile)
                }, DefaultAuthenticationTypes.ApplicationCookie);

            //specify the context
            HttpContext.Current.GetOwinContext().Authentication.SignIn(identity);
        }
        /// <summary>
        /// 计算用户的消费额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private decimal UserOrdersTotal(Guid userId)
        {
            var storeOrders = _storeOrderQueryService.UserStoreOrderAlises(userId).Where(x => x.Status == StoreOrderStatus.Success);
            return storeOrders.Sum(x => x.Total);
        }
        
        #endregion
    }
}
