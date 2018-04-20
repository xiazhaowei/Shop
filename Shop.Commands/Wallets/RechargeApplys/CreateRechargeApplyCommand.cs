using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.RechargeApplys
{
    public class CreateRechargeApplyCommand : Command<Guid>
    {
        public Guid RechargeApplyId { get; private set; }
        public decimal Amount { get; private set; }
        public string Pic { get; private set; }
        public string BankName { get; private set; }
        public string BankNumber { get; private set; }
        public string BankOwner { get; private set; }

        public CreateRechargeApplyCommand() { }
        public CreateRechargeApplyCommand(
            Guid rechargeApplyId,
            Guid walletId,
            decimal amount,
            string pic,
            string bankName,
            string bankNumber,
            string bankOwner
            ):base(walletId)
        {
            RechargeApplyId = rechargeApplyId;
            Amount = amount;
            Pic = pic;
            BankName = bankName;
            BankNumber = bankNumber;
            BankOwner = bankOwner;
        }
    }
}
