using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores;
using System;
using System.Collections.Generic;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Stores
{
    /// <summary>
    /// 店铺聚合跟
    /// </summary>
    public class Store : AggregateRoot<Guid>
    {
        private StoreInfo _info;//基本信息
        private SubjectInfo _subjectInfo;//主体信息
        private ReturnAddressInfo _returnAddressInfo;//退货地址
        private Guid _userId;//所有人 必须是交年费的传递大使 身份失效店铺也失效？
        private Guid _sectionId;//所属行业
        private bool _isLocked;//店铺可单独锁定
        private ISet<Guid> _orderIds;
        private StoreStatisticInfo _storeStatisticInfo;//店铺统计信息
        private StoreStatus _status;//店铺状态
        private StoreType _type;//店铺类型
        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="sectionId"></param>
        /// <param name="info"></param>
        /// <param name="subjectInfo"></param>
        public Store(Guid id, Guid userId,StoreInfo info,SubjectInfo subjectInfo) : base(id)
        {
            userId.CheckNotEmpty(nameof(userId));
            ApplyEvent(new StoreCreatedEvent(userId,info,subjectInfo));
        }

        /// <summary>
        /// 设置管理密码
        /// </summary>
        /// <param name="accessCode"></param>
        public void SetAccessCode(string accessCode)
        {
            ApplyEvent(new StoreAccessCodeUpdatedEvent(accessCode));
        }

        /// <summary>
        /// 更新店铺状态
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(StoreStatus status)
        {
            ApplyEvent(new StoreStatusUpdatedEvent(status));
        }

        /// <summary>
        /// 更新主体信息
        /// </summary>
        /// <param name="subjectInfo"></param>
        public void UpdateSubjectInfo(SubjectInfo subjectInfo)
        {
            subjectInfo.CheckNotNull(nameof(subjectInfo));
            ApplyEvent(new SubjectInfoUpdatedEvent(subjectInfo));
            //更新主体信息接着更新店铺状态为待审核
            UpdateStatus(StoreStatus.Apply);
        }

        /// <summary>
        /// 更新店铺-更新基本信息
        /// </summary>
        /// <param name="info"></param>
        public void CustomerUpdate(StoreCustomerEditableInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new StoreCustomerUpdatedEvent(info));
        }

        public void ReturnAddressUpdate(ReturnAddressInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new StoreReturnAddressUpdatedEvent(info));
        }

        public void Update(StoreEditableInfo info,bool isLocked)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new StoreUpdatedEvent(info));
            if(isLocked)
            {
                ApplyEvent(new StoreLockedEvent());
            }
            else
            {
                ApplyEvent(new StoreUnLockedEvent());
            }
        }




        /// <summary>
        /// 接受新订单用户下单时 更新统计信息
        /// </summary>
        /// <param name="sale"></param>
        public void AcceptNewOrder(StoreOrder storeOrder)
        {
            if (!_orderIds.Add(storeOrder.Id)) return;

            var storeStatisticInfo = _storeStatisticInfo;
             if(_storeStatisticInfo.UpdatedOn.Date.Equals(DateTime.Now.Date))
            {
                //如果是今日
                storeStatisticInfo.TodaySale += storeOrder.GetTotal();
                storeStatisticInfo.TotalSale += storeOrder.GetTotal();
                storeStatisticInfo.TodayOrder += 1;
                storeStatisticInfo.TotalOrder += 1;
                storeStatisticInfo.UpdatedOn = DateTime.Now;
            }
            else
            {
                //今日第一单
                storeStatisticInfo.TodaySale = storeOrder.GetTotal();
                storeStatisticInfo.TotalSale += storeOrder.GetTotal();
                storeStatisticInfo.TodayOrder = 1;
                storeStatisticInfo.TotalOrder += 1;
                storeStatisticInfo.UpdatedOn = DateTime.Now;
            }
            ApplyEvent(new StoreStatisticInfoChangedEvent(storeStatisticInfo));
        }

        /// <summary>
        /// 重置今日销售额 该操作最好在0点-2点操作
        /// </summary>
        public void ResetTodayStatistic()
        {
            var storeStatisticInfo = _storeStatisticInfo;

            storeStatisticInfo.TodaySale = 0;
            storeStatisticInfo.TodayOrder = 0;
            storeStatisticInfo.UpdatedOn = DateTime.Now;

            ApplyEvent(new StoreStatisticInfoChangedEvent(storeStatisticInfo));
        }

        /// <summary>
        /// 接受商品的上下架信息，更新商家统计
        /// </summary>
        /// <param name="isOnSale"></param>
        public void AcceptGoodsOnOffSale(bool isOnSale)
        {
            var storeStatisticInfo = _storeStatisticInfo;
            if (isOnSale)
            {
                storeStatisticInfo.OnSaleGoodsCount += 1;
            }
            else
            {
                storeStatisticInfo.OnSaleGoodsCount -= 1;
            }
            ApplyEvent(new StoreStatisticInfoChangedEvent(storeStatisticInfo));
        }


        /// <summary>
        /// 锁定
        /// </summary>
        public void Lock()
        {
            ApplyEvent(new StoreLockedEvent());
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void UnLock()
        {
            ApplyEvent(new StoreUnLockedEvent());
        }

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <returns></returns>
        public StoreInfo GetInfo()
        {
            return _info;
        }
        public Guid GetUserId()
        {
            return _userId;
        }

        public SubjectInfo GetSubjectInfo()
        {
            return _subjectInfo;
        }

        #region Event Handler
        private void Handle(StoreCreatedEvent evnt)
        {
            _info = evnt.Info;
            _subjectInfo = evnt.SubjectInfo;
            _returnAddressInfo = null;
            _userId = evnt.UserId;
            _sectionId = Guid.Empty;
            _orderIds = new HashSet<Guid>();
            _isLocked = false;
            _storeStatisticInfo = new StoreStatisticInfo(
                    0,
                    0,
                    0,
                    0,
                    0,
                    DateTime.Now);
            _status = StoreStatus.Apply;
            _type = StoreType.ThirdParty;//默认为第三方店铺
        }
        private void Handle(StoreStatusUpdatedEvent evnt)
        {
            _status = evnt.Status;
        }
        private void Handle(StoreAccessCodeUpdatedEvent evnt)
        {
            _info.AccessCode = evnt.AccessCode;
        }
        private void Handle(StoreCustomerUpdatedEvent evnt)
        {
            var editableInfo = evnt.Info;
            _info = new StoreInfo(
                _info.AccessCode,
                editableInfo.Name, 
                editableInfo.Description,
                _info.Region,
                editableInfo.Address);
        }
        private void Handle(StoreReturnAddressUpdatedEvent evnt)
        {
            _returnAddressInfo = evnt.Info;
        }
        private void Handle(StoreUpdatedEvent evnt)
        {
            var editableInfo = evnt.Info;
            _info = new StoreInfo(
                _info.AccessCode,
                editableInfo.Name, 
                editableInfo.Description,
                _info.Region,
                editableInfo.Address);
            _type = editableInfo.Type;
        }

        private void Handle(SubjectInfoUpdatedEvent evnt)
        {
            _subjectInfo = evnt.SubjectInfo;
        }
        private void Handle(StoreLockedEvent evnt)
        {
            _isLocked = true;
        }
        private void Handle(StoreUnLockedEvent evnt)
        {
            _isLocked = false;
        }
        private void Handle(StoreStatisticInfoChangedEvent evnt)
        {
            _storeStatisticInfo = evnt.StatisticInfo;
        }
        #endregion
    }
}
