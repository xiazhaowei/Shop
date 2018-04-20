using System;

namespace Shop.Domain.Models.Wallets.BankCards
{
    public class BankCard
    {
        public Guid Id { get; set; }
        public BankCardInfo Info { get; set; }

        public BankCard(Guid id,BankCardInfo info)
        {
            Id = id;
            Info = info;
        }
    }
}
