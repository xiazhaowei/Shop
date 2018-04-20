using ENode.Eventing;
using Shop.Domain.Models.Grantees;
using System;

namespace Shop.Domain.Events.Grantees
{
    /// <summary>
    /// 统计信息改变事件
    /// </summary>
    [Serializable]
    public class GranteeStatisticsInfoChangedEvent:DomainEvent<Guid>
    {
        public GranteeStatisticsInfo StatisticsInfo { get; private set; }

        public GranteeStatisticsInfoChangedEvent() { }
        public GranteeStatisticsInfoChangedEvent(GranteeStatisticsInfo statisticsInfo)
        {
            StatisticsInfo = statisticsInfo;
        }
    }
}
