using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户推荐了VIP
    /// </summary>
    [Serializable]
    public class MyParentRecommandAPasserEvent:DomainEvent<Guid>
    {
        public Guid ParentId { get;private set; }

        public Guid UserId { get; private set; }
        public UserRole UserRole { get; private set; }

        public Guid NewVipId { get; private set; }
        public UserRole NewVipRole { get; private set; }
        

        public int GoodsCount { get; private set; }
        public UpdateOrderType UpdateOrderType { get; set; }
        public decimal LeftAwardAmount { get;private set; }
        public int Level { get; set; }

        public MyParentRecommandAPasserEvent() { }
        public MyParentRecommandAPasserEvent(Guid parentId,
            Guid userId,UserRole userRole,
            Guid newVipId ,UserRole newVipRole,int goodsCount,decimal leftAwardAmount,int level,UpdateOrderType updateOrderType=UpdateOrderType.VipOrder)
        {
            ParentId = parentId;
            UserId=userId;
            UserRole = userRole;
            NewVipId = newVipId;
            NewVipRole = newVipRole;
            GoodsCount = goodsCount;
            LeftAwardAmount = leftAwardAmount;
            Level = level;
            UpdateOrderType = updateOrderType;
        }
    }
}
