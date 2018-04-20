using ENode.Domain;
using ENode.Eventing;
using Shop.Common;
using Shop.Domain.Events.Partners;
using System;
using System.Collections.Generic;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Partners
{
    /// <summary>
    /// 区域代理 聚合跟
    /// </summary>
    public class Partner:AggregateRoot<Guid>
    {
        private Guid _userId;//关联用户
        private Guid _walletId;//用户的钱包
        private PartnerInfo _info;//信息
        private PartnerStatisticInfo _statisticInfo;//统计信息

        public Partner(Guid id,Guid userId,Guid walletId,PartnerInfo info):base(id)
        {
            ApplyEvent(new PartnerCreatedEvent(userId,walletId,info));
        }

        public Guid GetUserId()
        {
            return _userId;
        }
        public PartnerInfo GetInfo()
        {
            return _info;
        }

        public void Update(PartnerInfo info)
        {
            info.CheckNotNull(nameof(info));

            if(_info.IsLocked && !info.IsLocked)
            {
                //如果原来是锁定状态 现在是解锁 更新结算日期到解锁日期
                var statisticInfo = _statisticInfo;
                statisticInfo.BalancedDate = DateTime.Now;
                ApplyEvent(new PartnerStatisticInfoChangedEvent(statisticInfo));
            }
            ApplyEvent(new PartnerUpdatedEvent(info));
        }


        public void Delete()
        {
            ApplyEvent(new PartnerDeletedEvent());
        }

        /// <summary>
        /// 接受新的结算
        /// </summary>
        /// <param name="amount">代理地区销售额</param>
        public void AcceptNewBalance(decimal amount)
        {
            if (_info.IsLocked)
            {
                throw new Exception("代理锁定，不参与分红");
            }
            //判断代理的分红级别
            var amountPersent = _info.Persent;
            if (amountPersent >= 1)
            {
                throw new Exception("代理分红比例错误");
            }
            //待分红金额
            var toBalanceAmount = Math.Round(amount * amountPersent,2);
            //现金分红金额
            var cashBalanceAmount = Math.Round( toBalanceAmount * (_info.CashPersent / 100M),2);
            //福豆分红金额
            var benevolenceBalanceAmount = Math.Round((toBalanceAmount - cashBalanceAmount) / ConfigSettings.BenevolenceValue, 4);
            
            //统计信息
            var statisticInfo = _statisticInfo;
            statisticInfo.LastBalancedAmount = toBalanceAmount;
            statisticInfo.LastCashBalancedAmount = cashBalanceAmount;
            statisticInfo.LastBenevolenceBalancedAmount = benevolenceBalanceAmount;

            statisticInfo.TotalCashBalancedAmount += cashBalanceAmount;
            statisticInfo.TotalBenevolenceBalancedAmount += benevolenceBalanceAmount;
            statisticInfo.TotalBalancedAmount += toBalanceAmount;
            statisticInfo.BalancedDate = DateTime.Now;

            
            ApplyEvent(new AcceptedNewBalanceEvent(_walletId,
                cashBalanceAmount,
                benevolenceBalanceAmount, 
                toBalanceAmount,
                statisticInfo));

            ApplyEvent(new NewBalanceLogEvent(new BalanceLogInfo(_walletId,
                _info.Region,
                amount,toBalanceAmount,
                cashBalanceAmount,
                benevolenceBalanceAmount)));

        }

        #region Handler
        private void Handle(PartnerCreatedEvent evnt)
        {
            _userId = evnt.UserId;
            _walletId = evnt.WalletId;
            _info = evnt.Info;
            _statisticInfo = new PartnerStatisticInfo(0M,0M,0M,0M,0M, 0M, DateTime.Now);
        }
        private void Handle(AcceptedNewBalanceEvent evnt)
        {
            _statisticInfo = evnt.StatisticInfo;
        }
        private void Handle(PartnerUpdatedEvent evnt)
        {
            _info = evnt.Info;
        }
        private void Handle(PartnerStatisticInfoChangedEvent evnt)
        {
            _statisticInfo = evnt.StatisticInfo;
        }
        private void Handle(PartnerDeletedEvent evnt)
        {
            _info = null;
            _statisticInfo = null;
        }
        private void Handle(NewBalanceLogEvent evnt)
        {
        }
        #endregion

    }
}
