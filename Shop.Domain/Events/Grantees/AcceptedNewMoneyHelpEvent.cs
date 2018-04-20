using ENode.Eventing;
using Shop.Domain.Models.Grantees;
using System;

namespace Shop.Domain.Events.Grantees
{
    /// <summary>
    /// 接受新的帮助
    /// </summary>
    [Serializable]
    public class AcceptedNewMoneyHelpEvent:DomainEvent<Guid>
    {
        public MoneyHelp MoneyHelp { get; private set; }

        public AcceptedNewMoneyHelpEvent() { }
        public AcceptedNewMoneyHelpEvent(MoneyHelp moneyHelp)
        {
            MoneyHelp = moneyHelp;
        }
    }
}
