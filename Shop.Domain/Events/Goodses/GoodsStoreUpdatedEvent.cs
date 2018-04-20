using ENode.Eventing;
using Shop.Domain.Models.Goodses;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsStoreUpdatedEvent:DomainEvent<Guid>
    {
        public GoodsStoreEditableInfo Info { get; private set; }
        public IList<Guid> CategoryIds { get; private set; }

        public GoodsStoreUpdatedEvent() { }
        public GoodsStoreUpdatedEvent(IList<Guid> categoryIds,GoodsStoreEditableInfo info)
        {
            CategoryIds = categoryIds;
            Info = info;
        }
    }
}
