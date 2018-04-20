using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class InvotedNewUserEvent:DomainEvent<Guid>
    {
        /// <summary>
        /// 被邀请人ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 邀请人的钱包ID
        /// </summary>
        public Guid WalletId { get; set; }

        public InvotedNewUserEvent() { }
        public InvotedNewUserEvent(Guid userId,Guid walletId)
        {
            UserId = userId;
            WalletId = walletId;
        }
    }
}
