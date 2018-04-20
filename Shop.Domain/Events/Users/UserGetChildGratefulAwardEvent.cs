using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    public class UserGetChildGratefulAwardEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }

        public UserGetChildGratefulAwardEvent() { }
        public UserGetChildGratefulAwardEvent(Guid walletId,decimal amount,string remark)
        {
            WalletId = walletId;
            Amount = amount;
            Remark = remark;
        }
    }
}
