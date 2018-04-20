using ENode.Domain;
using ENode.Eventing;
using Shop.Common;
using Shop.Domain.Events.OfflineStores;
using System;
using System.Collections.Generic;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.OfflineStores
{
    public class OfflineStore : AggregateRoot<Guid>
    {
        private Guid _userId;
        private OfflineStoreInfo _info;
        private StatisticInfo _statisticInfo;
        

        public OfflineStore(Guid id,Guid userId,OfflineStoreInfo info):base(id)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new OfflineStoreCreatedEvent(userId,info));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        public void Update(OfflineStoreInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new OfflineStoreUpdatedEvent(info));
        }
        /// <summary>
        /// 接受新的销售
        /// </summary>
        /// <param name="amount"></param>
        public void AcceptNewSale(Guid storeOwnerWalletId,Guid userId, Guid userWalletId, decimal amount)
        {
            //统计信息
            var statisticInfo = _statisticInfo;
            if (_statisticInfo.UpdatedOn.Date.Equals(DateTime.Now.Date))
            {
                //如果是今日
                statisticInfo.TodaySale += amount;
                statisticInfo.TotalSale += amount;
                statisticInfo.UpdatedOn = DateTime.Now;
            }
            else
            {
                //今日第一单
                statisticInfo.TodaySale = amount;
                statisticInfo.TotalSale += amount;
                statisticInfo.UpdatedOn = DateTime.Now;
            }
            //商家和消费者结算
            var profit =Math.Round( amount * _info.Persent / 100,2);//利润
            var benevolenceAmount =Math.Round( profit / ConfigSettings.BenevolenceCalculationFactor,4);
            var storeAmount = amount - profit;

            ApplyEvent(new NewSaleAcceptedEvent(
                _userId,
                storeOwnerWalletId, 
                userId,
                userWalletId,
                amount,
                storeAmount,
                benevolenceAmount,
                statisticInfo));
            ApplyEvent(new NewSaleLogEvent(new SaleLogInfo(userWalletId,
                storeOwnerWalletId,
                _info.Name,
                _info.Region,
                _info.Address,
                amount,
                storeAmount,
                benevolenceAmount)));
        }

        /// <summary>
        /// 重置今日销售
        /// </summary>
        public void ResetTodayStatistic()
        {
            var statisticInfo = _statisticInfo;
            statisticInfo.TodaySale = 0;
            statisticInfo.UpdatedOn = DateTime.Now;

            ApplyEvent(new OfflineStoreStatisticInfoChangedEvent(statisticInfo));
        }

        public void Delete()
        {
            ApplyEvent(new OfflineStoreDeletedEvent());
        }

        public Guid GetOwnerId()
        {
            return _userId;
        }

        #region Handle
        private void Handle(OfflineStoreCreatedEvent evnt)
        {
            _userId = evnt.UserId;
            _info = evnt.Info;
            _statisticInfo = new StatisticInfo(0, 0, DateTime.Now);
        }
        private void Handle(OfflineStoreUpdatedEvent evnt)
        {
            _info = evnt.Info;
        }
        private void Handle(NewSaleAcceptedEvent evnt)
        {
            _statisticInfo = evnt.Info;
        }
        private void Handle(OfflineStoreStatisticInfoChangedEvent evnt)
        {
            _statisticInfo = evnt.Info;
        }
        private void Handle(OfflineStoreDeletedEvent evnt)
        {
            _userId = Guid.Empty;
            _statisticInfo = null;
            _info = null;
        }
        private void Handle(NewSaleLogEvent evnt)
        {

        }
        #endregion

    }
}
