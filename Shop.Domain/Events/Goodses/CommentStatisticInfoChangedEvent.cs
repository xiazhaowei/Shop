using ENode.Eventing;
using Shop.Domain.Models.Goodses;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class CommentStatisticInfoChangedEvent:DomainEvent<Guid>
    {
        public CommentStatisticInfo StatisticInfo { get; private set; }

        public CommentStatisticInfoChangedEvent() { }
        public CommentStatisticInfoChangedEvent(CommentStatisticInfo statisticInfo)
        {
            StatisticInfo = statisticInfo;
        }
    }
}
