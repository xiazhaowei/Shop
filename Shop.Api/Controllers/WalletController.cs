using ENode.Commanding;
using Shop.Api.Extensions;
using Shop.Api.Models;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Wallet;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Wallet;
using Shop.Api.Services;
using Shop.Commands.Wallets;
using Shop.Commands.Wallets.BankCards;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Commands.Wallets.RechargeApplys;
using Shop.Commands.Wallets.ShopCashTransfers;
using Shop.Commands.Wallets.WithdrawApplys;
using Shop.Common;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    public class WalletController : BaseApiController
    {
        private IWalletQueryService _walletQueryService;//钱包Q端
        private IUserQueryService _userQueryService;
        /// <summary>
        /// IOC 构造函数注入
        /// </summary>
        /// <param name="commandService"></param>
        /// <param name="conferenceQueryService"></param>
        public WalletController(ICommandService commandService, IContextService contextService,
            IWalletQueryService walletQueryService,
            IUserQueryService userQueryService) : base(commandService,contextService)
        {
            _walletQueryService = walletQueryService;
            _userQueryService = userQueryService;
        }

        #region 基本信息


        /// <summary>
        /// 获取钱包信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public WalletInfoResponse Info()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var walletinfo = _walletQueryService.Info(currentAccount.WalletId.ToGuid());

            return new WalletInfoResponse
            {
                WalletInfo =new WalletInfo
                {
                    Id=walletinfo.Id,
                    AccessCode=walletinfo.AccessCode,
                    Cash=walletinfo.Cash,
                    ShopCash=walletinfo.ShopCash,
                    Benevolence=walletinfo.Benevolence,
                    Earnings=walletinfo.Earnings,
                    YesterdayEarnings=walletinfo.YesterdayEarnings
                }
            };
        }

        /// <summary>
        /// 获取用户的最近5天的激励信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse IncentiveLogs()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var incentiveInfos = _walletQueryService.GetIncentiveInfos(currentAccount.WalletId.ToGuid() ,5);
            return new IncentiveLogsResponse
            {
                IncentiveLogs = incentiveInfos.Select(x => new IncentiveInfo
                {
                    Id=Guid.NewGuid(),
                    BenevolenceAmount = x.BenevolenceAmount,
                    Amount = x.Amount,
                    CreatedOn = x.CreatedOn.GetTimeSpan(),
                    Fee = x.Fee,
                    Remark = x.Remark
                }).ToList()
            };
        }

        /// <summary>
        /// 重置 支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetAccessCode(SetAccessCodeRequest request)
        {
            request.CheckNotNull(nameof(request));
            if(!request.AccessCode.IsPayPassword())
            {
                return new BaseApiResponse { Code = 400, Message = "支付密码为6为数字" };
            }

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var command = new SetAccessCodeCommand(currentAccount.WalletId.ToGuid(), request.AccessCode);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 用户的现金记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public GetCashTransfersResponse CashTransfers(CashTransfersRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            //获取数据
            int pageSize = 10;

            var cashTransfers = _walletQueryService.GetCashTransfers(currentAccount.WalletId.ToGuid()).Where(x=>x.Status==CashTransferStatus.Success);
            var total = cashTransfers.Count();

            //通过以上方法 已经获取_wallet实例了
            if (request.Type!=CashTransferType.All)
            {
                cashTransfers = cashTransfers.Where(x => x.Type == request.Type);
            }
            total = cashTransfers.Count();
            //分页
            cashTransfers = cashTransfers.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page-1)).Take(pageSize);

            return new GetCashTransfersResponse
            {
                Total=total,
                CashTransfers = cashTransfers.Select(x => new CashTransfer
                {
                    Number = x.Number,
                    Amount=x.Amount,
                    Fee=x.Fee,
                    FinallyValue=x.FinallyValue,
                    Remark=x.Remark,
                    CreatedOn = x.CreatedOn.ToShortDateString(),
                    Type = x.Type.ToDescription(),
                    Direction = x.Direction.ToDescription()
                }).ToList()
            };
        }


        /// <summary>
        /// 用户的购物券记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public GetShopCashTransfersResponse ShopCashTransfers(ShopCashTransfersRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            //获取数据
            int pageSize = 10;

            var shopCashTransfers = _walletQueryService.GetShopCashTransfers(currentAccount.WalletId.ToGuid()).Where(x => x.Status == ShopCashTransferStatus.Success);
            var total = shopCashTransfers.Count();

            //通过以上方法 已经获取_wallet实例了
            if (request.Type != ShopCashTransferType.All)
            {
                shopCashTransfers = shopCashTransfers.Where(x => x.Type == request.Type);
            }
            total = shopCashTransfers.Count();
            //分页
            shopCashTransfers = shopCashTransfers.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new GetShopCashTransfersResponse
            {
                Total = total,
                ShopCashTransfers = shopCashTransfers.Select(x => new ShopCashTransfer
                {
                    Number = x.Number,
                    Amount = x.Amount,
                    Fee = x.Fee,
                    FinallyValue = x.FinallyValue,
                    Remark = x.Remark,
                    CreatedOn = x.CreatedOn.ToShortDateString(),
                    Type = x.Type.ToDescription(),
                    Direction = x.Direction.ToDescription()
                }).ToList()
            };
        }
        /// <summary>
        /// 善心记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public GetBenevolenceTransfersResponse BenevolenceTransfers(BenevolenceTransfersRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            //获取数据
            int pageSize = 10;
            var benevolenceTransfers = _walletQueryService.GetBenevolenceTransfers(currentAccount.WalletId.ToGuid()).Where(x=>x.Status==BenevolenceTransferStatus.Success);
            var total = benevolenceTransfers.Count();
            //筛选数据
            if(request.Type!=BenevolenceTransferType.All)
            {
                benevolenceTransfers = benevolenceTransfers.Where(x=>x.Type==request.Type);
            }
            total = benevolenceTransfers.Count();
            //分页
            benevolenceTransfers = benevolenceTransfers.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page-1)).Take(pageSize);

            return new GetBenevolenceTransfersResponse
            {
                Total=total,
                BenevolenceTransfers = benevolenceTransfers.Select(x => new BenevolenceTransfer
                {
                    Number = x.Number,
                    Amount=x.Amount,
                    Fee=x.Fee,
                    FinallyValue=x.FinallyValue,
                    Remark=x.Remark,
                    CreatedOn = x.CreatedOn.ToShortDateString(),
                    Type = x.Type.ToDescription(),
                    Direction = x.Direction.ToDescription()
                }).ToList()
            };
        }

        #endregion

        #region 支付

        /// <summary>
        /// 钱包支付
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> WalletPay(WalletPayRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var walletinfo = _walletQueryService.Info(currentAccount.WalletId.ToGuid());

            if(request.CashPayAmount==0 && request.ShopCashPayAmount==0)
            {
                return new BaseApiResponse { Code = 401, Message = "付款金额为零" };
            }
            if (walletinfo.IsFreeze == Freeze.Freeze)
            {
                return new BaseApiResponse { Code = 402, Message = "钱包冻结，请用其他方式支付" };
            }
            if (walletinfo.Cash < request.CashPayAmount || walletinfo.ShopCash < request.ShopCashPayAmount)
            {
                return new BaseApiResponse { Code = 403, Message = "错误，余额不足" };
            }
            if (!request.IsNotVerifyAccessCode)
            {
                //验证支付密码
                if (!walletinfo.AccessCode.Equals(request.AccessCode))
                {
                    return new BaseApiResponse { Code = 404, Message = "支付密码错误" };
                }
            }
            
            string number = DateTime.Now.ToSerialNumber();

            var cashTransferType = CashTransferType.Shopping;
            var shopCashTransferType = ShopCashTransferType.Shopping;
            if(request.Type== "Transfer")
            {
                cashTransferType = CashTransferType.Transfer;
            }
            if (request.Type == "Recharge")
            {
                cashTransferType = CashTransferType.Charge;
            }

            //余额付款
            if (request.CashPayAmount > 0)
            {
                var command = new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                walletinfo.Id,
                number,//流水号
                cashTransferType,
                CashTransferStatus.Placed,//这里只是提交，只有钱包接受改记录后，才更新为成功
                request.CashPayAmount,
                0,
                WalletDirection.Out,
                request.Remark);

                var result = await ExecuteCommandAsync(command);
                if (!result.IsSuccess())
                {
                    return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
                }
            }
            //购物券付款
            if (request.ShopCashPayAmount > 0)
            {
                var command = new CreateShopCashTransferCommand(
                GuidUtil.NewSequentialId(),
                walletinfo.Id,
                number,//流水号
                shopCashTransferType,
                ShopCashTransferStatus.Placed,//这里只是提交，只有钱包接受改记录后，才更新为成功
                request.ShopCashPayAmount,
                0,
                WalletDirection.Out,
                request.Remark);

                var result = await ExecuteCommandAsync(command);
                if (!result.IsSuccess())
                {
                    return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
                }
            }

            return new BaseApiResponse();
        }

        /// <summary>
        /// 接受转账
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> AcceptTransfer(AcceptTransferRequest request)
        {
            request.CheckNotNull(nameof(request));

            var wallet = _walletQueryService.InfoByUserId(request.UserId);
            if(wallet==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有该收款人"};
            }

            string number = DateTime.Now.ToSerialNumber();
            var command = new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                wallet.Id,
                number,//流水号
                CashTransferType.Transfer,
                CashTransferStatus.Placed,//这里只是提交，只有钱包接受改记录后，才更新为成功
                request.Amount,
                0,
                WalletDirection.In,
                request.Remark);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Recharge(RechargeRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var command = new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                currentAccount.WalletId.ToGuid(),
                DateTime.Now.ToSerialNumber(),
                CashTransferType.Charge,
                CashTransferStatus.Placed,
                request.Amount,
                0,
                WalletDirection.In,
                "充值");

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            return new BaseApiResponse();
        }
        #endregion

        #region 银行卡


        /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public GetBankCardsResponse BankCards()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var bankCards = _walletQueryService.GetBankCards(currentAccount.WalletId.ToGuid());

            return new GetBankCardsResponse
            {
                BankCards = bankCards.Select(x => new BankCard
                {
                    Id = x.Id,
                    WalletId=x.WalletId,
                    BankName = x.BankName,
                    OwnerName=x.OwnerName,
                    Number = x.Number
                }).ToList()
            };
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> AddBankCard(AddBankCardRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var command = new AddBankCardCommand(
                currentAccount.WalletId.ToGuid(),
                request.BankName,
                request.OwnerName,
                request.Number);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expressAddressId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> DelBankCard(DelRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var command = new RemoveBankCardCommand(currentAccount.WalletId.ToGuid(), request.Id);
            var result = await ExecuteCommandAsync(command);

            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        #endregion

        #region 提现

        /// <summary>
        /// 提现申请记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public WithdrawApplyLogsResponse WithdrawApplyLogs()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            //通过以上方法 已经获取_wallet实例了
            var withdrawApplylogs = _walletQueryService.WithdrawApplyLogs(currentAccount.WalletId.ToGuid());

            return new WithdrawApplyLogsResponse
            {
                WithdrawApplys = withdrawApplylogs.Select(x => new WithdrawApply
                {
                    Id = x.Id,
                    Mobile=x.Mobile,
                    NickName=x.NickName,
                    Amount = x.Amount,
                    BankName = x.BankName,
                    BankNumber = x.BankNumber,
                    BankOwner = x.BankOwner,
                    CreatedOn = x.CreatedOn,
                    Remark = x.Remark,
                    Status = x.Status.ToDescription(),
                    WalletId=x.WalletId
                }).ToList()
            };
        }
        
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ApplyWithdraw(ApplyWithdrawRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            //判断提现限额
            var wallet = _walletQueryService.Info(currentAccount.WalletId.ToGuid());
            if (wallet.IsFreeze==Freeze.Freeze)
            {
                return new BaseApiResponse { Code = 400, Message = "钱包冻结，无法提现"};
            }
            if (wallet.TodayWithdrawAmount + request.Amount > ConfigSettings.OneDayWithdrawLimit)
            {
                return new BaseApiResponse { Code = 400, Message = "单日提现不得超过{0}元".FormatWith(ConfigSettings.OneDayWithdrawLimit) };
            }
            if (wallet.WeekWithdrawAmount + request.Amount > ConfigSettings.OneWeekWithdrawLimit)
            {
                return new BaseApiResponse { Code = 400, Message = "每周提现不得超过{0}元".FormatWith(ConfigSettings.OneWeekWithdrawLimit) };
            }

            var command = new CreateWithdrawApplyCommand(
                GuidUtil.NewSequentialId(),
                currentAccount.WalletId.ToGuid(),
                request.Amount,
                request.BankCard.BankName,
                request.BankCard.Number,
                request.BankCard.OwnerName);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

       
        

        #endregion

        #region 线下充值
        /// <summary>
        /// 线下充值申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ApplyRecharge(ApplyRechargeRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var command = new CreateRechargeApplyCommand(
                GuidUtil.NewSequentialId(),
                currentAccount.WalletId.ToGuid(),
                request.Amount,
                request.Pic,
                request.BankCard.BankName,
                request.BankCard.Number,
                request.BankCard.OwnerName);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 获取充值记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public RechargeApplyLogsResponse RechargeApplyLogs()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            //通过以上方法 已经获取_wallet实例了
            var rechargeApplylogs = _walletQueryService.RechargeApplyLogs(currentAccount.WalletId.ToGuid());

            return new RechargeApplyLogsResponse
            {
                RechargeApplys = rechargeApplylogs.Select(x => new RechargeApply
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Pic=x.Pic,
                    BankName = x.BankName,
                    BankNumber = x.BankNumber,
                    BankOwner = x.BankOwner,
                    CreatedOn = x.CreatedOn,
                    Remark = x.Remark,
                    Status = x.Status.ToDescription(),
                    WalletId = x.WalletId
                }).ToList()
            };
        }




        #endregion
        
    }
}
