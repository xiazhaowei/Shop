using ENode.Eventing;
using Shop.Common;
using System;

namespace Shop.Domain.Events.Users.ExpressAddresses
{
    /// <summary>
    /// 添加快递地址
    /// </summary>
    [Serializable]
    public abstract  class ExpressAddressEvent : DomainEvent<Guid>
    {
        public Guid ExpressAddressId { get; private set; }
        public ExpressAddressInfo Info { get; private set; }

        public ExpressAddressEvent() { }
        public ExpressAddressEvent(Guid expressAddressId,ExpressAddressInfo info)
        {
            ExpressAddressId = expressAddressId;
            Info = info;
        }
    }
}
