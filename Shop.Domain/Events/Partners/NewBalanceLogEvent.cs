using ENode.Eventing;
using Shop.Domain.Models.Partners;
using System;

namespace Shop.Domain.Events.Partners
{
    [Serializable]
    public class NewBalanceLogEvent:DomainEvent<Guid>
    {
        public BalanceLogInfo LogInfo { get; private set; }

        public NewBalanceLogEvent() { }
        public NewBalanceLogEvent(BalanceLogInfo logInfo)
        {
            LogInfo = logInfo;
        }

    }
}
