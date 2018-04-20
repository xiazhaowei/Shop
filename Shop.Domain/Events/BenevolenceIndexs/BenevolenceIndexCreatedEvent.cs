using ENode.Eventing;
using System;

namespace Shop.Domain.Events.BenevolenceIndexs
{
    [Serializable]
    public class BenevolenceIndexCreatedEvent:DomainEvent<Guid>
    {
        public decimal BenevolenceIndex { get; private set; }
        public decimal BenevolenceAmount { get; private set; }
        public decimal IncentivedAmount { get; set; }

        public BenevolenceIndexCreatedEvent() { }
        public BenevolenceIndexCreatedEvent(
            decimal benevolenceIndex,
            decimal benevolenceAmount,
            decimal incentivedAmount)
        {
            BenevolenceIndex = benevolenceIndex;
            BenevolenceAmount = benevolenceAmount;
            IncentivedAmount = incentivedAmount;
        }

    }
}
