using ENode.Domain;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores.StoreOrders;
using Shop.Domain.Models.Stores.StoreOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Stores
{
    /// <summary>
    /// 商家订单 聚合跟
    /// </summary>
    public class StoreOrder:AggregateRoot<Guid>
    {
        private Guid _walletId;//付款的钱包信息
        private Guid _storeOwnerWalletId;//店主钱包ID
        private StoreOrderInfo _info;//订单信息
        private ExpressAddressInfo _expressAddressInfo;//收货地址
        private PayInfo _payInfo;
        private PayDetailInfo _payDetailInfo;
        private RefundApplyInfo _refundApplyInfo;//申请退款信息

        private IList<OrderGoodsInfo> _orderGoodses;//订单商品
        private StoreOrderStatus _status;//订单状态

        public StoreOrder(
            Guid id,
            Guid walletId,
            Guid storeOwnerWalletId,
            StoreOrderInfo info,
            ExpressAddressInfo expressAddressInfo,
            PayInfo payInfo,
            PayDetailInfo payDetailInfo,
            IList<OrderGoodsInfo> orderGoodses):base(id)
        {
            ApplyEvent(new StoreOrderCreatedEvent(
                walletId,
                storeOwnerWalletId,
                info,
                expressAddressInfo,
                payInfo,
                payDetailInfo,
                orderGoodses));
        }
        
        public StoreOrderInfo GetInfo()
        {
            return _info;
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="expressInfo"></param>
        public void Deliver(ExpressInfo expressInfo)
        {
            if(_status!=StoreOrderStatus.Placed && _status!=StoreOrderStatus.Expressing)
            {
                throw new Exception("不正确的包裹状态");
            }
            ApplyEvent(new StoreOrderExpressedEvent(expressInfo));
        }

        /// <summary>
        /// 退货发货
        /// </summary>
        /// <param name="expressInfo"></param>
        public void ReturnDeliver(ExpressInfo expressInfo)
        {
            if (_status != StoreOrderStatus.AgreeReturn)
            {
                throw new Exception("不正确的包裹状态");
            }
            ApplyEvent(new StoreOrderReturnExpressedEvent(expressInfo));
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        public void ConfirmExpress()
        {
            if(_status!=StoreOrderStatus.Expressing)
            {
                throw new Exception("不正确的包裹状态");
            }
            ApplyEvent(new StoreOrderConfirmExpressedEvent(
                _walletId,
                _storeOwnerWalletId,
                _orderGoodses.Sum(x=>x.PayDetailInfo.StoreTotal),
                _orderGoodses));
        }

        /// <summary>
        /// 申请仅退款
        /// </summary>
        /// <param name="refoundApplyInfo"></param>
        public void ApplyRefund(RefundApplyInfo refoundApplyInfo)
        {
            refoundApplyInfo.CheckNotNull(nameof(refoundApplyInfo));

            ApplyEvent(new ApplyRefundedEvent(refoundApplyInfo));
        }

        /// <summary>
        /// 申请退货退款
        /// </summary>
        /// <param name="refoundApplyInfo"></param>
        public void ApplyReturnAndRefund(RefundApplyInfo refoundApplyInfo)
        {
            refoundApplyInfo.CheckNotNull(nameof(refoundApplyInfo));

            ApplyEvent(new ApplyReturnAndRefundedEvent(refoundApplyInfo));
        }

        /// <summary>
        /// 同意退款
        /// </summary>
        public void AgreeRefund()
        {
            if (_status != StoreOrderStatus.OnlyRefund && _status != StoreOrderStatus.ReturnExpressing)
            {
                throw new Exception("订单状态错误");
            }
            //计算退款金额
            var cashPayAmount = _payDetailInfo.Total - _payDetailInfo.ShopCash;
            var cashRefundAmount =Math.Round( (cashPayAmount/_payDetailInfo.Total)* _refundApplyInfo.RefundAmount, 2);
            var shopCashRefundAmount = Math.Round((_payDetailInfo.ShopCash / _payDetailInfo.Total) * _refundApplyInfo.RefundAmount, 2);

            ApplyEvent(new AgreeRefundedEvent(_walletId,_refundApplyInfo.RefundAmount, cashRefundAmount, shopCashRefundAmount));
        }
        /// <summary>
        /// 同意退货，请买家返回包裹
        /// </summary>
        public void AgreeReturn(string remark)
        {
            if (_status != StoreOrderStatus.ReturnAndRefund)
            {
                throw new Exception("订单状态错误");
            }
            ApplyEvent(new AgreeReturnEvent(remark));
        }
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            if (_status!=StoreOrderStatus.Closed)
            {
                throw new Exception("无法删除未完成的订单");
            }
            ApplyEvent(new StoreOrderDeletedEvent());
        }

        /// <summary>
        /// 获取订单商品总额
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            return _payInfo.Total;
        }

        #region Handle
        private void Handle(StoreOrderCreatedEvent evnt)
        {
            _walletId = evnt.WalletId;
            _storeOwnerWalletId = evnt.StoreOwnerWalletId;
            _info = evnt.Info;
            _expressAddressInfo = evnt.ExpressAddressInfo;
            _payInfo = evnt.PayInfo;
            _payDetailInfo = evnt.PayDetailInfo;
            _orderGoodses = evnt.OrderGoodses;
            _status = StoreOrderStatus.Placed;
        }
        private void Handle(StoreOrderDeletedEvent evnt)
        {
            _expressAddressInfo = null;
            _payInfo = null;
            _payDetailInfo = null;
            _orderGoodses = null;
            _info = null;
        }
        private void Handle(StoreOrderExpressedEvent evnt)
        {
            _status = StoreOrderStatus.Expressing;
        }
        private void Handle(StoreOrderReturnExpressedEvent evnt)
        {
            _status = StoreOrderStatus.ReturnExpressing;
        }
        private void Handle(StoreOrderConfirmExpressedEvent evnt)
        {
            _status = StoreOrderStatus.Success;
        }
        private void Handle(ApplyRefundedEvent evnt)
        {
            _refundApplyInfo = evnt.RefundApplyInfo;
            _status = StoreOrderStatus.OnlyRefund;
        }
        private void Handle(ApplyReturnAndRefundedEvent evnt)
        {
            _refundApplyInfo = evnt.RefundApplyInfo;
            _status = StoreOrderStatus.ReturnAndRefund;
        }

        private void Handle(AgreeRefundedEvent evnt)
        {
            _status = StoreOrderStatus.Closed;
        }
        private void Handle(AgreeReturnEvent evnt)
        {
            _status = StoreOrderStatus.AgreeReturn;
        }
        #endregion
    }
}
