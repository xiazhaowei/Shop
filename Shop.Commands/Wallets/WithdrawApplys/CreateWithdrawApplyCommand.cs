using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.WithdrawApplys
{
    public class CreateWithdrawApplyCommand:Command<Guid>
    {
        public Guid WithdrawApplyId { get; private set; }
        public decimal Amount { get; private set; }
        public string BankName { get; private set; }
        public string BankNumber { get; private set; }
        public string BankOwner { get; private set; }

        public CreateWithdrawApplyCommand() { }
        public CreateWithdrawApplyCommand(
            Guid withdrawApplyId,
            Guid walletId,
            decimal amount,
            string bankName,
            string bankNumber,
            string bankOwner
            ):base(walletId)
        {
            WithdrawApplyId = withdrawApplyId;
            Amount = amount;
            BankName = bankName;
            BankNumber = bankNumber;
            BankOwner = bankOwner;
        }
    }
}
