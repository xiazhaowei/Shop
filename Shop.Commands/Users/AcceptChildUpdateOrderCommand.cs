using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Users
{
    public class AcceptChildUpdateOrderCommand:Command<Guid>
    {
        public Guid NewVipId { get; private set; }
        public UserRole NewVipRole { get; private set; }
        public int GoodsCount { get; private set; }
        public decimal LeftAwardAmount { get; private set; }
        public int Level { get; set; }
        public UpdateOrderType UpdateOrderType { get; set; }

        public AcceptChildUpdateOrderCommand() { }
        public AcceptChildUpdateOrderCommand(Guid newVipId,UserRole newVipRole,int goodsCount,decimal leftAwardAmount,int level,UpdateOrderType updateOrderType)
        {
            NewVipId = newVipId;
            NewVipRole = newVipRole;
            GoodsCount = goodsCount;
            LeftAwardAmount = leftAwardAmount;
            Level = level;
            UpdateOrderType = updateOrderType;
        }
    }
}
