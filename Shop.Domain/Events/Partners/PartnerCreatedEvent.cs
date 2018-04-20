using ENode.Eventing;
using Shop.Domain.Models.Partners;
using System;

namespace Shop.Domain.Events.Partners
{
    /// <summary>
    /// 联盟创建
    /// </summary>
    [Serializable]
    public class PartnerCreatedEvent:DomainEvent<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid WalletId { get; private set; }
        public PartnerInfo Info { get; private set; }

        public PartnerCreatedEvent() { }
        public PartnerCreatedEvent(Guid userId,Guid walletId,PartnerInfo info)
        {
            UserId = userId;
            WalletId = walletId;
            Info = info;
        }
    }
}
