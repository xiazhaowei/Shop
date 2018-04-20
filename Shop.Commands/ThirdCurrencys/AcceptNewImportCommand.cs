using ENode.Commanding;
using System;

namespace Shop.Commands.ThirdCurrencys
{
    public class AcceptNewImportCommand:Command<Guid>
    {
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }
        public string Mobile { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }

        public AcceptNewImportCommand() { }
        public AcceptNewImportCommand(Guid walletId,
            Guid userId,
            string mobile,
            string account,
            decimal amount)
        {
            WalletId = walletId;
            UserId = userId;
            Mobile = mobile;
            Account = account;
            Amount = amount;
        }
    }
}
