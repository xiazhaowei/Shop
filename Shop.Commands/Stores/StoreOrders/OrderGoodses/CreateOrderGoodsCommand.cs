using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    /// <summary>
    /// 创建订单商品 
    /// </summary>
    public class CreateOrderGoodsCommand:Command<Guid>
    {
        public Guid StoreId { get; private set; }
        public Guid OrderId { get; private set; }
        public OrderGoods OrderGoods { get; private set; }

        public CreateOrderGoodsCommand() { }
        public CreateOrderGoodsCommand(Guid id,Guid storeId,Guid orderId,OrderGoods orderGoods):base(id)
        {
            StoreId = storeId;
            OrderId = orderId;
            OrderGoods = orderGoods;
        }
    }

    
}
