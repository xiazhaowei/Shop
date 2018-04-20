using ENode.Domain;
using Shop.Domain.Events.BenevolenceIndexs;
using System;

namespace Shop.Domain.Models.BenevolenceIndexs
{
    public class BenevolenceIndex : AggregateRoot<Guid>
    {
        private decimal _lastIndex = 0;
        private decimal _lastBenevolenceAmount = 0;
        private decimal _lastIncentivedAmount = 0;

        public BenevolenceIndex(Guid id,decimal benevolenceIndex,decimal benevolenceAmount):base(id)
        {
            var incentivedAmount = benevolenceAmount * benevolenceIndex;
            ApplyEvent(new BenevolenceIndexCreatedEvent(benevolenceIndex, benevolenceAmount, incentivedAmount));
        }

        private void Handle(BenevolenceIndexCreatedEvent evnt)
        {
            _lastIndex = evnt.BenevolenceIndex;
            _lastBenevolenceAmount = evnt.BenevolenceAmount;
            _lastIncentivedAmount = evnt.IncentivedAmount;
        }
    }
}
