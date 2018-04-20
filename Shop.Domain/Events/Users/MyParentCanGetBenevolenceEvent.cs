using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 我的父亲可以获得用户善心分成
    /// </summary>
    [Serializable]
    public class MyParentCanGetBenevolenceEvent:DomainEvent<Guid>
    {
        public Guid ParentId { get; private set; }
        public decimal Amount { get; private set; }//购物者获得的福豆
        public decimal ProfitAmount { get;private set; }//商品利润的福豆
        public decimal HighProfitAmount { get;private set; }
        public int Level { get; private set; }

        public MyParentCanGetBenevolenceEvent() { }
        public MyParentCanGetBenevolenceEvent(Guid parentId,decimal amount,decimal profitAmount,decimal highProfitAmount,int level)
        {
            ParentId = parentId;
            Amount = amount;
            ProfitAmount = profitAmount;
            HighProfitAmount = highProfitAmount;
            Level = level;
        }
    }
}
