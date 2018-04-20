using ENode.Domain;
using Shop.Domain.Events.Wallets.BankCards;
using Shop.Domain.Events.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xia.Common.Extensions;
using Shop.Domain.Models.Wallets.CashTransfers;
using Shop.Domain.Models.Wallets.BankCards;
using Shop.Domain.Models.Wallets.BenevolenceTransfers;
using Shop.Domain.Models.Wallets.WithdrawApplys;
using Shop.Domain.Events.Wallets.WithdrawApplys;
using Shop.Common.Enums;
using Shop.Domain.Models.Wallets.RechargeApplys;
using Shop.Domain.Events.Wallets.RechargeApplys;
using Shop.Common;
using Shop.Domain.Models.Wallets.ShopCashTransfers;

namespace Shop.Domain.Models.Wallets
{
    /// <summary>
    /// 用户钱包 聚合跟
    /// </summary>
    public class Wallet:AggregateRoot<Guid>
    {
        private Guid _userId;//所有人
        private decimal _cash;//现金 量
        private decimal _shopCash;//购物金
        private decimal _lockedCash;//锁定的现金量，提现使用
        private decimal _benevolence;//善心量

        private string _accessCode;//访问密码 可以理解为交易密码

        private ISet<Guid> _cashTransfers;//现金账记录
        private ISet<Guid> _shopCashTransfers;//购物券记录
        private ISet<Guid> _benevolenceTransfers;//善心账记录
        private IList<BankCard> _bankCards;//银行卡
        private IList<WithdrawApply> _withdrawApplys;//提现记录
        private IList<RechargeApply> _rechargeApplys;//线下充值记录

        private WalletStatisticInfo _walletStatisticInfo;//钱包统计信息
        private WithdrawStatisticInfo _withdrawStatisticInfo;//提现统计信息

        private bool _isFreeze;//是否冻结账号 怀疑账号被盗可以冻结账号

        public Wallet(Guid id,Guid userId):base(id)
        {
            ApplyEvent(new WalletCreatedEvent(userId));
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="accessCode"></param>
        public void SetAccessCode(string accessCode)
        {
            if(!accessCode.IsPayPassword())
            {
                throw new Exception("支付密码为6为纯数字");
            }
            ApplyEvent(new WalletAccessCodeUpdatedEvent(accessCode));
        }

        public Guid GetOwnerId()
        {
            return _userId;
        }

        /// <summary>
        /// 冻结用户 限制交易
        /// </summary>
        public void Freeze()
        {
            ApplyEvent(new WalletFreezeEvent());
        }

        public void UnFreeze()
        {
            ApplyEvent(new WalletUnFreezeEvent());
        }

        #region 银行卡
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="bankCardInfo"></param>
        public void AddBankCard(BankCardInfo bankCardInfo)
        {
            ApplyEvent(new BankCardAddedEvent(Guid.NewGuid(), bankCardInfo));
        }

        /// <summary>
        /// 更新 
        /// </summary>
        /// <param name="bankCardId"></param>
        /// <param name="bankCardInfo"></param>
        public void UpdateBankCard(Guid bankCardId, BankCardInfo bankCardInfo)
        {
            var bankcard = _bankCards.SingleOrDefault(x => x.Id == bankCardId);
            if (bankcard == null)
            {
                throw new Exception("不存在该银行卡.");
            }
            ApplyEvent(new BankCardUpdatedEvent(bankCardId, bankCardInfo));
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="bankCardId"></param>
        public void RemoveBankCard(Guid bankCardId)
        {
            ApplyEvent(new BankCardRemovedEvent(bankCardId));
        }

        #endregion

        #region 处理流水信息

        
        /// <summary>
        /// 接受新的现金记录   更新统计信息
        /// </summary>
        /// <param name="reply"></param>
        public void AcceptNewCashTransfer(CashTransfer cashTransfer)
        {
            var finallyValue = _cash;
            //业务逻辑判断
            CashTransferInfo cashTransferInfo = cashTransfer.GetInfo();
            //统计信息
            var walletStatisticInfo = _walletStatisticInfo;

            if(cashTransferInfo.Direction==WalletDirection.Out)
            {
                if (_isFreeze)
                {
                    throw new Exception("钱包冻结，无法出账");
                }
                //如果是出账 判断账号余额是否够
                if(_cash<cashTransferInfo.Amount)
                {
                    throw new Exception("账户余额不足");
                }
                finallyValue -= cashTransferInfo.Amount;
            }
            else
            {
                //进账 判断收益
                if(cashTransfer.GetTransferType() == CashTransferType.Incentive)
                {
                    //善心激励
                    if (_walletStatisticInfo.UpdatedOn.Date.Equals(DateTime.Now.Date))
                    {
                        //非第一个
                        walletStatisticInfo.YesterdayEarnings += cashTransferInfo.Amount;
                    }
                    else
                    {
                        //今日第一个
                        walletStatisticInfo.YesterdayEarnings = cashTransferInfo.Amount;
                    }
                    walletStatisticInfo.Earnings += cashTransferInfo.Amount;
                    walletStatisticInfo.UpdatedOn = DateTime.Now;
                }
                finallyValue += cashTransferInfo.Amount;
            }
            finallyValue = Math.Round(finallyValue, 2);
            //新的记录接受后，发出新记录接受的事件
            ApplyEvent(new NewCashTransferAcceptedEvent(_userId,cashTransfer.Id, finallyValue,walletStatisticInfo));
        }

        public void AcceptNewShopCashTransfer(ShopCashTransfer shopCashTransfer)
        {
            var finallyValue = _shopCash;
            //业务逻辑判断
            ShopCashTransferInfo shopCashTransferInfo = shopCashTransfer.GetInfo();
            //统计信息
            var walletStatisticInfo = _walletStatisticInfo;

            if (shopCashTransferInfo.Direction == WalletDirection.Out)
            {
                if (_isFreeze)
                {
                    throw new Exception("钱包冻结，无法出账");
                }
                //如果是出账 判断账号余额是否够
                if (_shopCash < shopCashTransferInfo.Amount)
                {
                    throw new Exception("账户余额不足");
                }
                finallyValue -= shopCashTransferInfo.Amount;
            }
            else
            {
                //进账 
                finallyValue += shopCashTransferInfo.Amount;
            }
            finallyValue = Math.Round(finallyValue, 2);
            //新的记录接受后，发出新记录接受的事件
            ApplyEvent(new NewShopCashTransferAcceptedEvent(_userId, shopCashTransfer.Id, finallyValue, walletStatisticInfo));
        }
        /// <summary>
        /// 接受新的善心记录 更新统计信息
        /// </summary>
        /// <param name="benevolenceTransfer"></param>
        public void AcceptNewBenevolenceTransfer(BenevolenceTransfer benevolenceTransfer)
        {
            var finallyValue = _benevolence;
            //业务逻辑判断
            BenevolenceTransferInfo benevolenceTransferInfo = benevolenceTransfer.GetInfo();
            //统计信息
            var walletStatisticInfo = _walletStatisticInfo;

            if (benevolenceTransferInfo.Direction == WalletDirection.Out)
            {
                if (_isFreeze)
                {
                    throw new Exception("钱包冻结，无法出账");
                }
                //如果是出账 判断账号余额是否够
                if (_benevolence < benevolenceTransferInfo.Amount)
                {
                    throw new Exception("账户余额不足");
                }
                finallyValue -= benevolenceTransferInfo.Amount;
            }
            else
            {
                //如果是进账 更新统计信息
                if(_walletStatisticInfo.UpdatedOn.Date.Equals(DateTime.Now.Date))
                {
                    //今日非第一个记录
                    walletStatisticInfo.TodayBenevolenceAdded += benevolenceTransferInfo.Amount;
                }
               else
                {
                    //今日第一个收入
                    walletStatisticInfo.TodayBenevolenceAdded = benevolenceTransferInfo.Amount;
                }
                walletStatisticInfo.BenevolenceTotal += benevolenceTransferInfo.Amount;
                walletStatisticInfo.UpdatedOn = DateTime.Now;
                finallyValue += benevolenceTransferInfo.Amount;
            }
            finallyValue = Math.Round(finallyValue, 4);
            ApplyEvent(new NewBenevolenceTransferAcceptedEvent(_userId, benevolenceTransfer.Id,finallyValue,walletStatisticInfo));
        }

        public void ResetTodayStatistic()
        {
            var walletStatisticInfo = _walletStatisticInfo;
            walletStatisticInfo.TodayBenevolenceAdded = 0;
            walletStatisticInfo.YesterdayEarnings = 0;
            walletStatisticInfo.UpdatedOn = DateTime.Now;

            ApplyEvent(new StatisticInfoChangedEvent(walletStatisticInfo));
        }
        #endregion

        /// <summary>
        /// 激励善心 计算收益 并减扣善心
        /// </summary>
        /// <param name="benevolenceIndex"></param>
        public void IncentiveBenevolence(decimal benevolenceIndex)
        {
            if (_isFreeze)
            {
                throw new Exception("钱包冻结，无法激励");
            }
            if(benevolenceIndex<=0 || benevolenceIndex>1)
            {
                throw new Exception("善心指数异常");
            }
            if(_benevolence>=1)
            {
                //只有善心值大于1时才参与激励

                //激励善心就是善心值转换为现金而已，创建两个激励类型的记录即可 这个工作交给ProcessManager来做
                //MessagePublisher 将用户激励信息发布出去 ProcessManager接受消息处理
                
                var benevolenceValue = _benevolence * 100; //我的善心价值
                var incentiveValue = benevolenceValue * benevolenceIndex;//本次激励收益
                var benevolenceDeduct = _benevolence * benevolenceIndex;//善心扣除数量
                ApplyEvent(new IncentiveUserBenevolenceEvent(_userId,benevolenceIndex,incentiveValue,benevolenceDeduct));
            }
        }

        #region 提现申请

        
        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <param name="withdrawApplyId"></param>
        /// <param name="info"></param>
        public void ApplyWithdraw(Guid withdrawApplyId,WithdrawApplyInfo info)
        {
            info.CheckNotNull(nameof(info));
            
            var withdrawStatisticInfo = _withdrawStatisticInfo;
            
            if(_withdrawStatisticInfo.LastWithdrawTime.Date.Equals(DateTime.Now.Date))
            {
                //今日提现
                withdrawStatisticInfo.TodayWithdrawAmount += info.Amount;
            }
            else
            {
                withdrawStatisticInfo.TodayWithdrawAmount = info.Amount;
            }
            withdrawStatisticInfo.WithdrawTotalAmount += info.Amount;
            withdrawStatisticInfo.LastWithdrawTime = DateTime.Now;

            //业务逻辑判断
            if (withdrawStatisticInfo.TodayWithdrawAmount > ConfigSettings.OneDayWithdrawLimit)
            {
                throw new Exception("今日提现超出限额");
            }
            if (withdrawStatisticInfo.WeekWithdrawAmount > ConfigSettings.OneWeekWithdrawLimit)
            {
                throw new Exception("本周提现超出限额");
            }
            ApplyEvent(new WithdrawStatisticInfoChangedEvent(withdrawStatisticInfo));

            //判断提现金额
            if (_cash < info.Amount)
            {
                throw new Exception("余额不足无法提现");
            }
            var finalCash = _cash;
            finalCash -= info.Amount;
            var finalLockedCash = _lockedCash;
            finalLockedCash += info.Amount;

            ApplyEvent(new WithdrawApplyCreatedEvent(withdrawApplyId,finalCash,finalLockedCash,info));
        }

        /// <summary>
        /// 清空用户本周提现金额
        /// </summary>
        public void ClearWeekWithdrawAmount()
        {
            var withdrawStatisticInfo = _withdrawStatisticInfo;
            withdrawStatisticInfo.WeekWithdrawAmount = 0;

            ApplyEvent(new WithdrawStatisticInfoChangedEvent(withdrawStatisticInfo));
        }

        /// <summary>
        /// 审核提现状态
        /// </summary>
        /// <param name="withdrawApplyId"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        public void ChangeWithdrawApplyStatus(Guid withdrawApplyId,WithdrawApplyStatus status,string remark)
        {
            var withdrawApply = _withdrawApplys.SingleOrDefault(x => x.Id == withdrawApplyId);
            if (withdrawApply == null)
            {
                throw new Exception("不存在该提现申请.");
            }

            if (status==WithdrawApplyStatus.Success)
            {
                if(_lockedCash<withdrawApply.Info.Amount)
                {
                    throw new Exception("错误的锁定金额");
                }
                var finalLockedCash = _lockedCash;
                finalLockedCash -= withdrawApply.Info.Amount;

                ApplyEvent(new WithdrawApplySuccessEvent(withdrawApplyId,withdrawApply.Info.Amount, finalLockedCash));
            }
            if(status==WithdrawApplyStatus.Rejected)
            {
                //拒绝提现申请
                var finalLockedCash = _lockedCash;
                finalLockedCash -= withdrawApply.Info.Amount;
                ApplyEvent(new WithdrawApplyRejectedEvent(withdrawApplyId,withdrawApply.Info.Amount, finalLockedCash, remark));
            }
            
        }
        #endregion

        #region 线下充值
        /// <summary>
        /// 提交充值申请
        /// </summary>
        /// <param name="rechargeApplyId"></param>
        /// <param name="info"></param>
        public void ApplyRecharge(Guid rechargeApplyId, RechargeApplyInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new RechargeApplyCreatedEvent(rechargeApplyId, info, RechargeApplyStatus.Placed));
        }

        public void ChangeRechargeApplyStatus(Guid rechargeApplyId, RechargeApplyStatus status, string remark)
        {
            if (status == RechargeApplyStatus.Success)
            {

                var rechargeApply = _rechargeApplys.SingleOrDefault(x => x.Id == rechargeApplyId);
                if (rechargeApply == null)
                {
                    throw new Exception("不存在该充值申请.");
                }
               
                ApplyEvent(new RechargeApplySuccessEvent(rechargeApply.Info.Amount));
            }
            ApplyEvent(new RechargeApplyStatusChangedEvent(rechargeApplyId, status, remark));
        }


        #endregion

        #region Handler
        private void Handle(WalletCreatedEvent evnt)
        {
            _cash = 0;
            _shopCash = 0;
            _lockedCash = 0;
            _benevolence = 0;
            _isFreeze = false;
            _bankCards = new List<BankCard>();
            _withdrawApplys = new List<WithdrawApply>();
            _rechargeApplys = new List<RechargeApply>();
            _cashTransfers = new HashSet<Guid>();
            _shopCashTransfers = new HashSet<Guid>();
            _benevolenceTransfers = new HashSet<Guid>();
            _userId = evnt.UserId;
            _walletStatisticInfo = new WalletStatisticInfo(0, 0, 0, 0, 0, DateTime.Now);
            _withdrawStatisticInfo = new WithdrawStatisticInfo(0, 0, 0, DateTime.Now);
        }
        private void Handle(WalletFreezeEvent evnt)
        {
            _isFreeze = true;
        }

        private void Handle(WalletUnFreezeEvent evnt)
        {
            _isFreeze = false;
        }

        //提现申请
        private void Handle(WithdrawApplyCreatedEvent evnt)
        {
            _withdrawApplys.Add(new WithdrawApply(evnt.WithdrawApplyId,evnt.Info,WithdrawApplyStatus.Placed));
            _cash = evnt.FinalCash;
            _lockedCash = evnt.FinalLockedCash;
        }
        private void Handle(WithdrawApplyRejectedEvent evnt)
        {
            _withdrawApplys.Single(x => x.Id == evnt.WithdrawApplyId).Info.Remark = evnt.Remark;
            _lockedCash = evnt.FinalLockedCash;
            
        }
        private void Handle(WithdrawApplySuccessEvent evnt)
        {
            _lockedCash = evnt.FinalLockedCash;
        }
        private void Handle(IncentiveUserBenevolenceEvent evnt) { }
        //充值申请
        private void Handle(RechargeApplyCreatedEvent evnt)
        {
            _rechargeApplys.Add(new RechargeApply(evnt.RechargeApplyId, evnt.Info, evnt.Status));
        }
        private void Handle(RechargeApplyStatusChangedEvent evnt)
        {
            _rechargeApplys.Single(x => x.Id == evnt.RechargeApplyId).Status = evnt.Status;
            _rechargeApplys.Single(x => x.Id == evnt.RechargeApplyId).Info.Remark = evnt.Remark;
        }

        private void Handle(RechargeApplySuccessEvent evnt) { }

        private void Handle(WithdrawStatisticInfoChangedEvent evnt)
        {
            _withdrawStatisticInfo = evnt.Info;
        }

        private void Handle(StatisticInfoChangedEvent evnt)
        {
            _walletStatisticInfo = evnt.Info;
        }



        private void Handle(WalletAccessCodeUpdatedEvent evnt)
        {
            _accessCode = evnt.AccessCode;
        }
        private void Handle(BankCardAddedEvent evnt)
        {
            _bankCards.Add(new BankCard(evnt.BankCardId,evnt.Info));
        }
        private void Handle(BankCardUpdatedEvent evnt)
        {
            _bankCards.Single(x => x.Id == evnt.BankCardId).Info = evnt.Info;
        }
        private void Handle(BankCardRemovedEvent evnt)
        {
            _bankCards.Remove(_bankCards.Single(x => x.Id == evnt.BankCardId));
        }

        private void Handle(NewCashTransferAcceptedEvent evnt)
        {
            _cashTransfers.Add(evnt.TransferId);
            _cash = evnt.FinallyValue;
            _walletStatisticInfo = evnt.StatisticInfo;
        }
        private void Handle(NewShopCashTransferAcceptedEvent evnt)
        {
            _shopCashTransfers.Add(evnt.TransferId);
            _shopCash = evnt.FinallyValue;
            _walletStatisticInfo = evnt.StatisticInfo;
        }

        private void Handle(NewBenevolenceTransferAcceptedEvent evnt)
        {
            _benevolenceTransfers.Add(evnt.TransferId);
            _benevolence = evnt.FinallyValue;
            _walletStatisticInfo = evnt.StatisticInfo;
        }

       
        #endregion
    }
}
