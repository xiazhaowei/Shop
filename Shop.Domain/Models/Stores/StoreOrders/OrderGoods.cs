using ENode.Domain;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores.StoreOrders;
using Shop.Domain.Events.Stores.StoreOrders.GoodsServices;
using Shop.Domain.Models.Stores.StoreOrders.GoodsServices;
using System;

namespace Shop.Domain.Models.Stores
{
    /// <summary>
    /// 订单商品  聚合跟
    /// </summary>
    public class OrderGoods:AggregateRoot<Guid>
    {
        private Guid _orderId;//商家订单ID
        private OrderGoodsInfo _info;
        private ServiceApplyInfo _serviceApplyInfo;// 商品服务申请信息
        private ServiceExpressInfo _serviceExpressInfo;// 用户退货发货
        private ServiceFinishExpressInfo _serviceFinishExpressInfo;//商家维修完成发货信息
        private OrderGoodsStatus _status;// 商品服务状态
        private DateTime _serviceExpirationDate;//售后服务自动过期日期

        public OrderGoods(Guid id,Guid orderId,OrderGoodsInfo info):base(id)
        {
            ApplyEvent(new OrderGoodsCreatedEvent(orderId,
                info,
                DateTime.Now.Add(ConfigSettings.OrderGoodsServiceAutoExpiration)));
        }

        /// <summary>
        /// 服务申请
        /// </summary>
        /// <param name="servicesApplyInfo"></param>
        public void ApplyServices(ServiceApplyInfo serviceApplyInfo)
        {
            if(_serviceApplyInfo!=null)
            {
                throw new Exception("您已经申请过服务，请等待处理");
            }
            ApplyEvent(new ServiceApplyedEvent(serviceApplyInfo));
        }
        /// <summary>
        /// 同意服务申请
        /// </summary>
        /// <param name="serviceNumber"></param>
        public void AgreeService(string serviceNumber)
        {
            if(_serviceApplyInfo.ServiceType==GoodsServiceType.SalesReturn 
                || _serviceApplyInfo.ServiceType==GoodsServiceType.Service 
                || _serviceApplyInfo.ServiceType==GoodsServiceType.Change)
            {
                ApplyEvent(new ServiceAgreedEvent(serviceNumber, OrderGoodsStatus.TobeSent));
            }
        }
        /// <summary>
        /// 客户 提交退货物流信息
        /// </summary>
        /// <param name="servicesExpressInfo"></param>
        public void AddServicesExpressInfo(ServiceExpressInfo serviceExpressInfo)
        {
            if(_serviceExpressInfo!=null)
            {
                throw new Exception("您已经填写了退货物流信息，无需再填写");
            }
            ApplyEvent(new ServiceExpressedEvent(serviceExpressInfo));
        }

        /// <summary>
        /// 同意退款
        /// </summary>
        public void AgreeRefund(string serviceNumber)
        {
            ApplyEvent(new AgreedRefundEvent(serviceNumber));
        }
        /// <summary>
        /// 不同意退款
        /// </summary>
        public void DisAgreeRefund(string serviceNumber)
        {
            ApplyEvent(new DisAgreedRefundEvent(serviceNumber));
        }
        /// <summary>
        /// 服务结束并发货
        /// </summary>
        public void ServiceFinish(ServiceFinishExpressInfo serviceFinishExpressInfo)
        {
            ApplyEvent(new ServiceFinishExpressedEvent(serviceFinishExpressInfo));
        }

        /// <summary>
        /// 服务结束
        /// </summary>
        /// <param name="serviceNumber"></param>
        public void ServiceFinish(string serviceNumber)
        {
            ApplyEvent(new ServiceFinishedEvent(_info.WalletId, _info.StoreOwnerWalletId, _info.PayDetailInfo.Total,_info.Benevolence));
        }
        /// <summary>
        /// 设置过期 订单服务自动过期
        /// </summary>
        public void MarkAsExpire()
        {
            if (_status ==OrderGoodsStatus.Normal)
            {
                ApplyEvent(new ServiceExpiredEvent(_info.WalletId,_info.StoreOwnerWalletId,_info.PayDetailInfo.Total,_info.Benevolence));
            }
        }

        #region Handle
        private void Handle(OrderGoodsCreatedEvent evnt)
        {
            _orderId = evnt.OrderId;
            _info = evnt.Info;
            _status = OrderGoodsStatus.Normal;
            _serviceExpirationDate = evnt.ServiceExpirationDate;
        }
        private void Handle(ServiceApplyedEvent evnt)
        {
            _serviceApplyInfo = evnt.Info;
            switch( evnt.Info.ServiceType)
            {
                case GoodsServiceType.Refund:
                    _status = OrderGoodsStatus.Refund;
                    break;
                case GoodsServiceType.SalesReturn:
                    _status = OrderGoodsStatus.SalesReturn;
                    break;
                case GoodsServiceType.Service:
                    _status = OrderGoodsStatus.Service;
                    break;
                case GoodsServiceType.ToDoorService:
                    _status = OrderGoodsStatus.ToDoorService;
                    break;
                case GoodsServiceType.Change:
                    _status = OrderGoodsStatus.Change;
                    break;
            }
        }
        private void Handle(ServiceAgreedEvent evnt)
        {
            _status = evnt.Status;
        }
        private void Handle(ServiceExpressedEvent evnt)
        {
            _serviceExpressInfo = evnt.Info;
        }
        private void Handle(ServiceFinishExpressedEvent evnt)
        {
            _status = OrderGoodsStatus.Closed;
            _serviceFinishExpressInfo = evnt.Info;
        }
        private void Handle(AgreedRefundEvent evnt)
        {
            _status = OrderGoodsStatus.Closed;
        }
        private void Handle(DisAgreedRefundEvent evnt)
        {
            _status = OrderGoodsStatus.Closed;
        }
        private void Handle(ServiceFinishedEvent evnt)
        {
            _status = OrderGoodsStatus.Closed;
        }
        private void Handle(ServiceExpiredEvent evnt)
        {
            _status = OrderGoodsStatus.Expire;
        }
        #endregion
    }

    
}
