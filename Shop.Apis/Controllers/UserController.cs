using ENode.Commanding;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Users;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.User;
using Shop.Apis.Extensions;
using Shop.Apis.Services;
using Shop.Apis.Utils;
using Shop.Apis.ViewModels;
using Shop.Commands.Users;
using Shop.Commands.Users.ExpressAddresses;
using Shop.Commands.Users.UserGifts;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;
using Xia.Common.Secutiry;
using ApiResponse = Shop.Api.Models.Response.User;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseApiController
    {

        private IUserQueryService _userQueryService;//用户Q端
        private IWalletQueryService _walletQueryService;//钱包Q端
        private ICartQueryService _cartQueryService;//购物车Q端
        private IStoreQueryService _storeQueryService;//商家
        private IStoreOrderQueryService _storeOrderQueryService;//商家
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
            IStoreOrderQueryService storeOrderQueryService) : base(commandService,contentService)
        {
            _smSender = smSender;
            _userQueryService = userQueryService;
            _walletQueryService = walletQueryService;
            _cartQueryService = cartQueryService;
            _storeQueryService = storeQueryService;
            _storeOrderQueryService = storeOrderQueryService;
        }


        #region 登陆 注册

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendMsgCode")]
        public BaseApiResponse SendMsgCode([FromBody]SendMsgCodeRequest request)
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
        [Route("CheckPhoneAvailable")]
        public BaseApiResponse CheckPhoneAvailable([FromBody]CheckPhoneAvailableRequest request)
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
        [Route("VerifyMsgCode")]
        public BaseApiResponse VerifyMsgCode([FromBody]VerifyMsgCodeRequest request)
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
        [Route("Register")]
        public async Task<BaseApiResponse> Register([FromBody]RegisterRequest request)
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
            //创建用户command
            var userViewModel = new UserViewModel {
                Id = GuidUtil.NewSequentialId(),
                ParentId = parentId,
                Mobile = request.Mobile,
                NickName = "用户" + StringGenerator.Generate(4),//创建随机昵称
                Password = request.Password,
                Portrait = "http://wftx-goods-img-details.oss-cn-shanghai.aliyuncs.com/default-userpic/userpic.png",
                Region = "北京",
                Gender = "保密"
            };
            var command = userViewModel.ToCreateUserCommand();
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
        /// 用户登录。返回用户基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<BaseApiResponse> Login([FromBody]LoginRequest request)
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

                await SignInAsync(userinfo.Id.ToString(),userinfo.WalletId.ToString(), userinfo.Mobile, true);

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
                        Token = userinfo.Id.ToString()
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
        /// 验证码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MsgLogin")]
        public async Task<BaseApiResponse> MsgLogin([FromBody]MsgLoginRequest request)
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

            await SignInAsync(userinfo.Id.ToString(),userinfo.WalletId.ToString(), userinfo.Mobile, true);

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
        /// 当前用户注销。需要登录才能访问。
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async void Logout()
        {
            await HttpContext.SignOutAsync();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Info")]
        public BaseApiResponse Info([FromBody]InfoRequest request)
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
        [Route("SetMyParent")]
        public async Task<BaseApiResponse> SetMyParent([FromBody]SetMyParentRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

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
        [Route("ExpressAddresses")]
        public ExpressAddressesResponse ExpressAddresses()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
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
        [Route("AddExpressAddress")]
        public async Task<BaseApiResponse> AddExpressAddress([FromBody]AddExpressAddressRequest request)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            request.CheckNotNull(nameof(request));
            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "请输入正确的手机号" };
            }

            var expressAddressViewModel = new ExpressAddressViewModel
            {
                UserId = currentAccount.UserId.ToGuid(),
                Mobile = request.Mobile,
                Name = request.Name,
                Region = request.Region,
                Address = request.Address,
                Zip = request.Zip
            };
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
        [Route("DeleteExpressAddress")]
        public async Task<BaseApiResponse> DeleteExpressAddress([FromBody]DeleteRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

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
        [Route("ShopLogin")]
        public async Task< BaseApiResponse> ShopLogin([FromBody]LoginRequest request)
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
            await SignInAsync(userinfo.Id.ToString(), userinfo.WalletId.ToString(),userinfo.Mobile, true);

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
        /// 获取用户信息 包含一些统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("MeInfo")]
        public BaseApiResponse MeInfo()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

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

            //_apiSession.SetUserInfo(userinfo.Id.ToString(), userinfo.ToUserModel());
            //_apiSession.SetWalletInfo(walletinfo.Id.ToString(), walletinfo.ToWalletModel());

            //店铺信息
            var storeId = "";
            var storeinfo = _storeQueryService.InfoByUserId(userinfo.Id);
            if (storeinfo != null)
            {
                storeId = storeinfo.Id.ToString();
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
                    Token = userinfo.Id.ToString()
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
                }
            };
        }

        /// <summary>
        /// 我的直推
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("MyInvotes")]
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
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var myInvotes = _userQueryService.UserChildrens(currentAccount.UserId.ToGuid()).OrderByDescending(x => x.CreatedOn);
            List<object> nodes = myInvotes.Select(x=>getNodeData(x,0)).ToList();

            return new MyInvotesResponse
            {
                MyInvotes = nodes
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
        [Route("ResetPassword")]
        public async Task<BaseApiResponse> ResetPassword([FromBody]ResetPasswordRequest request)
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
        [Route("ChangePassword")]
        public async Task<BaseApiResponse> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
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
        [Route("SetNickName")]
        public async Task<BaseApiResponse> SetNickName([FromBody]SetNickNameRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

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
        [Route("SetPortrait")]
        public async Task<BaseApiResponse> SetPortrait([FromBody]SetPortraitRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            
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
        [Route("SetGender")]
        public async Task<BaseApiResponse> SetGender([FromBody]SetGenderRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

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
        [Route("SetRegion")]
        public async Task<BaseApiResponse> SetRegion([FromBody]SetRegionRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
           
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

        #region 开通大使流程

        
        /// <summary>
        /// 添加礼物和收货信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AdduserGift")]
        public async Task<BaseApiResponse> AddUserGift([FromBody]AddUserGiftRequest request)
        {
            request.CheckNotNull(nameof(request));
            request.GiftInfo.CheckNotNull(nameof(request.GiftInfo));
            request.ExpressAddressInfo.CheckNotNull(nameof(request.ExpressAddressInfo));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            //要将新的ID 返回给前端
            var userGiftId = GuidUtil.NewSequentialId();
            var command = new AddUserGiftCommand(
                userGiftId,
                currentAccount.UserId.ToGuid(),
                request.GiftInfo.Name,
                request.GiftInfo.Size,
                request.ExpressAddressInfo.Name,
                request.ExpressAddressInfo.Mobile,
                request.ExpressAddressInfo.Region,
                request.ExpressAddressInfo.Address,
                request.ExpressAddressInfo.Zip);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new AddUserGiftResponse {
                UserGiftId=userGiftId
            };

        }

        /// <summary>
        /// 获取用户未支付礼物
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("GetUserUnPayGift")]
        public BaseApiResponse GetUserUnPayGift()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var unPayUserGifts = _userQueryService.UserGifts(currentAccount.UserId.ToGuid()).Where(x => x.Remark == "未支付").OrderByDescending(x=>x.CreatedOn);
            if(!unPayUserGifts.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "没有未支付" };
            }
            var unPayUserGift = unPayUserGifts.First();
            return new GetUserUnPayGiftResponse
            {
                UserGift = new UserGift
                {
                    Id=unPayUserGift.Id,
                    GiftName = unPayUserGift.GiftName,
                    GiftSize = unPayUserGift.GiftSize,
                    Name = unPayUserGift.Name,
                    Mobile = unPayUserGift.Mobile,
                    Region = unPayUserGift.Region,
                    Address = unPayUserGift.Address,
                    Zip = unPayUserGift.Zip
                }
            };
            
        }

        /// <summary>
        /// 设置礼物已付款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("SetUserGiftPayed")]
        public async Task<BaseApiResponse> SetUserGiftPayed([FromBody]SetUserGiftPayedRequest request)
        {
            request.CheckNotNull(nameof(request));
            request.UserGiftId.CheckNotEmpty(nameof(request.UserGiftId));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            //设置支付成功
            var command = new SetUserGiftPayedCommand(currentAccount.UserId.ToGuid(), request.UserGiftId);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }



        #endregion
        

        #region 私有方法
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <param name="persistentCookie"></param>
        /// <returns></returns>
        private Task SignInAsync(string userId,string walletId, string mobile, bool persistentCookie)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.MobilePhone, mobile),
                new Claim(ClaimTypes.UserData,walletId)
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = persistentCookie
            };
            if (persistentCookie)
            {
                authenticationProperties.ExpiresUtc = DateTime.UtcNow.AddDays(365);
            }
            else
            {
                authenticationProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);
            }

            return HttpContext.SignInAsync(user, authenticationProperties);
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
