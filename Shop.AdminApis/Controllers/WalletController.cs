﻿using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request.Wallet;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Wallet;
using Shop.Commands.BenevolenceIndexs;
using Shop.Commands.Wallets;
using Shop.Commands.Wallets.BenevolenceTransfers;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Commands.Wallets.RechargeApplys;
using Shop.Commands.Wallets.ShopCashTransfers;
using Shop.Commands.Wallets.WithdrawApplys;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class WalletController : BaseApiController
    {
        private IWalletQueryService _walletQueryService;//钱包Q端
        /// <summary>
        /// IOC 构造函数注入
        /// </summary>
        /// <param name="commandService"></param>
        /// <param name="conferenceQueryService"></param>
        public WalletController(ICommandService commandService, IContextService contextService,
            IWalletQueryService walletQueryService) : base(commandService,contextService)
        {
            _walletQueryService = walletQueryService;
        }
        
        #region 后台管理
        /// <summary>
        /// 激励用户的善心
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("IncentiveBenevolence")]
        public async Task<BaseApiResponse> IncentiveBenevolence([FromBody]IncentiveBenevolenceRequest request)
        {
            request.CheckNotNull(nameof(request));
            if(request.BenevolenceIndex<=0 || request.BenevolenceIndex>=1)
            {
                return new BaseApiResponse { Code = 400, Message = "善心指数异常" };
            }
            //遍历所有的钱包发送激励指令
            var wallets = _walletQueryService.ListPage();
            if(wallets.Any())
            {
                var totalBenevolenceAmount = wallets.Where(x => x.Benevolence > 1).Sum(x => x.Benevolence);
                //创建激励记录
                await ExecuteCommandAsync(new CreateBenevolenceIndexCommand(
                    GuidUtil.NewSequentialId(),
                    request.BenevolenceIndex,
                    totalBenevolenceAmount
                    ));

                foreach (var wallet in wallets)
                {
                    if (wallet.Benevolence > 1)
                    {
                        var command = new IncentiveBenevolenceCommand(wallet.Id, request.BenevolenceIndex);
                        await ExecuteCommandAsync(command);
                    }
                     
                }
            }

            return new BaseApiResponse();
        }

        /// <summary>
        /// 激励某个钱包
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("IncentiveWallet")]
        public async Task< BaseApiResponse> IncentiveWallet([FromBody]IncentiveWalletRequest request)
        {
            request.CheckNotNull(nameof(request));
            if (request.BenevolenceIndex >= 1 || request.BenevolenceIndex<=0)
            {
                return new BaseApiResponse { Code = 400, Message = "指出值异常" };
            }
            var wallet = _walletQueryService.Info(request.WalletId);
            if(wallet==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有找到钱包" };
            }
            var command = new IncentiveBenevolenceCommand(request.WalletId, request.BenevolenceIndex);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 所有善心量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("TotalBenevolence")]
        public BaseApiResponse TotalBenevolence()
        {
            var totalBenevolence = _walletQueryService.TotalBenevolence();
            return new TotalBenevolenceResponse
            {
                TotalBenevolence = totalBenevolence
            };
        }
        /// <summary>
        /// 钱包列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public ListPageResponse ListPage([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var wallets = _walletQueryService.ListPage();
            var pageSize = 20;
            var total = wallets.Count();
            //筛选
            if (!request.Mobile.IsNullOrEmpty())
            {
                wallets = wallets.Where(x => x.OwnerMobile.Contains(request.Mobile));
            }
            total = wallets.Count();
            //分页
            wallets = wallets.Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new ListPageResponse
            {
                Total = total,
                Wallets = wallets.Select(x => new Wallet
                {
                    Id = x.Id,
                    OwnerMobile=x.OwnerMobile,
                    Cash=x.Cash,
                    ShopCash = x.ShopCash,
                    Benevolence=x.Benevolence,
                    BenevolenceTotal=x.BenevolenceTotal,
                    TodayBenevolenceAdded=x.TodayBenevolenceAdded,
                    YesterdayEarnings=x.YesterdayEarnings,
                    Earnings=x.Earnings,
                    AccessCode=x.AccessCode,
                    YesterdayIndex=x.YesterdayIndex
                }).ToList()
            };
        }

        /// <summary>
        /// 增减现金
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddCashTransfer")]
        public async Task<BaseApiResponse> AddCashTransfer([FromBody]AddCashTransferRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                request.Id,
                DateTime.Now.ToSerialNumber(),
                CashTransferType.SystemOp,
                CashTransferStatus.Placed,
                request.Amount,
                0,
                request.Direction,
                request.Remark);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 增减购物券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddShopCashTransfer")]
        public async Task<BaseApiResponse> AddShopCashTransfer([FromBody]AddShopCashTransferRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateShopCashTransferCommand(
                GuidUtil.NewSequentialId(),
                request.Id,
                DateTime.Now.ToSerialNumber(),
                ShopCashTransferType.SystemOp,
                ShopCashTransferStatus.Placed,
                request.Amount,
                0,
                request.Direction,
                request.Remark);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }


        /// <summary>
        /// 增减善心量
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddBenevolenceTransfer")]
        public async Task<BaseApiResponse> AddBenevolenceTransfer([FromBody]AddBenevolenceTransferRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateBenevolenceTransferCommand(
                GuidUtil.NewSequentialId(),
                request.Id,
                DateTime.Now.ToSerialNumber(),
                BenevolenceTransferType.SystemOp,
                BenevolenceTransferStatus.Placed,
                request.Amount,
                0,
                request.Direction,
                request.Remark);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 现金记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("CashTransfers")]
        public GetCashTransfersResponse CashTransfers([FromBody]CashTransfersRequest request)
        {
            request.CheckNotNull(nameof(request));

            //获取数据
            int pageSize = 10;
            var cashTransfers = _walletQueryService.GetCashTransfers(request.WalletId);
            var total = cashTransfers.Count();

            if (request.Type != CashTransferType.All)
            {
                cashTransfers = cashTransfers.Where(x => x.Type == request.Type);
            }
            total = cashTransfers.Count();
            //分页
            cashTransfers = cashTransfers.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new GetCashTransfersResponse
            {
                Total = total,
                CashTransfers = cashTransfers.Select(x => new CashTransfer
                {
                    Number = x.Number,
                    Amount = x.Amount,
                    Fee = x.Fee,
                    FinallyValue=x.FinallyValue,
                    Remark = x.Remark,
                    CreatedOn = x.CreatedOn.ToShortDateString(),
                    Type = x.Type.ToDescription(),
                    Direction = x.Direction.ToDescription(),
                    Status=x.Status.ToDescription()
                }).ToList()
            };
        }

        /// <summary>
        /// 现金记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ShopCashTransfers")]
        public GetShopCashTransfersResponse ShopCashTransfers([FromBody]ShopCashTransfersRequest request)
        {
            request.CheckNotNull(nameof(request));

            //获取数据
            int pageSize = 10;
            var cashTransfers = _walletQueryService.GetShopCashTransfers(request.WalletId);
            var total = cashTransfers.Count();

            if (request.Type != ShopCashTransferType.All)
            {
                cashTransfers = cashTransfers.Where(x => x.Type == request.Type);
            }
            total = cashTransfers.Count();
            //分页
            cashTransfers = cashTransfers.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new GetShopCashTransfersResponse
            {
                Total = total,
                ShopCashTransfers = cashTransfers.Select(x => new ShopCashTransfer
                {
                    Number = x.Number,
                    Amount = x.Amount,
                    Fee = x.Fee,
                    FinallyValue=x.FinallyValue,
                    Remark = x.Remark,
                    CreatedOn = x.CreatedOn.ToShortDateString(),
                    Type = x.Type.ToDescription(),
                    Direction = x.Direction.ToDescription(),
                    Status = x.Status.ToDescription()
                }).ToList()
            };
        }

        /// <summary>
        /// 善心记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("BenevolenceTransfers")]
        public GetBenevolenceTransfersResponse BenevolenceTransfers([FromBody]BenevolenceTransfersRequest request)
        {
            request.CheckNotNull(nameof(request));

            //获取数据
            int pageSize = 10;
            var benevolenceTransfers = _walletQueryService.GetBenevolenceTransfers(request.WalletId);
            var total = benevolenceTransfers.Count();
            //筛选数据
            if (request.Type != BenevolenceTransferType.All)
            {
                benevolenceTransfers = benevolenceTransfers.Where(x => x.Type == request.Type);
            }
            total = benevolenceTransfers.Count();
            //分页
            benevolenceTransfers = benevolenceTransfers.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new GetBenevolenceTransfersResponse
            {
                Total = total,
                BenevolenceTransfers = benevolenceTransfers.Select(x => new BenevolenceTransfer
                {
                    Number = x.Number,
                    Amount = x.Amount,
                    Fee=x.Fee,
                    FinallyValue=x.FinallyValue,
                    Remark = x.Remark,
                    CreatedOn = x.CreatedOn.ToShortDateString(),
                    Type = x.Type.ToDescription(),
                    Direction = x.Direction.ToDescription(),
                    Status=x.Status.ToDescription()
                }).ToList()
            };
        }
        /// <summary>
        /// 所有的提现申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("WithdrawApplysListPage")]
        public WithdrawApplysListPageResponse WithdrawApplysListPage([FromBody]WithdrawApplysListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var withdrawApplylogs = _walletQueryService.WithdrawApplyLogs();
            var total = withdrawApplylogs.Count();
            //筛选
            if (request.Status != WithdrawApplyStatus.All)
            {
                withdrawApplylogs = withdrawApplylogs.Where(x => x.Status == request.Status);
            }
            if (!request.Name.IsNullOrEmpty())
            {
                withdrawApplylogs = withdrawApplylogs.Where(x => x.BankOwner.Contains(request.Name));
                total = withdrawApplylogs.Count();
            }
            total = withdrawApplylogs.Count();
            //分页
            withdrawApplylogs = withdrawApplylogs.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new WithdrawApplysListPageResponse
            {
                Total = total,
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
                    WalletId = x.WalletId
                }).ToList()
            };
        }

        /// <summary>
        /// 更改申请状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ChangeWithdrawApplyStatus")]
        public async Task<BaseApiResponse> ChangeWithdrawApplyStatus([FromBody]ChangeWithdrawApplyStatusRequest request)
        {
            request.CheckNotNull(nameof(request));
            request.WalletId.CheckNotEmpty(nameof(request.WalletId));

            var command = new ChangeWithdrawStatusCommand(
                request.WithdrawApplyId,
                request.Status,
                request.Remark)
            {
                AggregateRootId = request.WalletId
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 审核线下充值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ChangeRechargeApplyStatus")]
        public async Task<BaseApiResponse> ChangeRechargeApplyStatus([FromBody]ChangeRechargeApplyStatusRequest request)
        {
            request.CheckNotNull(nameof(request));
            request.WalletId.CheckNotEmpty(nameof(request.WalletId));

            var command = new ChangeRechargeStatusCommand(
                request.RechargeApplyId,
                request.Status,
                request.Remark)
            {
                AggregateRootId = request.WalletId
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 线下充值申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("RechargeApplysListPage")]
        public RechargeApplysListPageResponse RechargeApplysListPage([FromBody]RechargeApplysListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var rechargeApplylogs = _walletQueryService.RechargeApplyLogs();
            var total = rechargeApplylogs.Count();
            //筛选
            if (request.Status != RechargeApplyStatus.All)
            {
                rechargeApplylogs = rechargeApplylogs.Where(x => x.Status == request.Status);
            }
            if (!request.Name.IsNullOrEmpty())
            {
                rechargeApplylogs = rechargeApplylogs.Where(x => x.BankOwner.Contains(request.Name));
            }
            total = rechargeApplylogs.Count();
            //分页
            rechargeApplylogs = rechargeApplylogs.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new RechargeApplysListPageResponse
            {
                Total = total,
                RechargeApplys = rechargeApplylogs.Select(x => new RechargeApply
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Pic = x.Pic,
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
