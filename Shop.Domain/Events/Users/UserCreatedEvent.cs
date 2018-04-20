using System;
using Shop.Domain.Models.Users;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户创建事件
    /// </summary>
    [Serializable]
    public class UserCreatedEvent : UserEvent
    {
        public Guid ParentId { get; private set; }
        public Guid WalletId { get; private set; }
        public Guid CartId { get; set; }

        public UserCreatedEvent() { }
        public UserCreatedEvent(UserInfo info,Guid parentId,Guid walletId,Guid cartId): base(info)
        {
            ParentId = parentId;
            WalletId = walletId;
            CartId = cartId;
        }
    }
}
