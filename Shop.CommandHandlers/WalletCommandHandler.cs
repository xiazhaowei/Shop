using ENode.Commanding;
using Shop.Commands.Wallets;
using Shop.Commands.Wallets.BankCards;
using Shop.Commands.Wallets.RechargeApplys;
using Shop.Commands.Wallets.WithdrawApplys;
using Shop.Domain.Models.Wallets;
using Shop.Domain.Models.Wallets.BankCards;
using Shop.Domain.Models.Wallets.BenevolenceTransfers;
using Shop.Domain.Models.Wallets.CashTransfers;
using Shop.Domain.Models.Wallets.RechargeApplys;
using Shop.Domain.Models.Wallets.ShopCashTransfers;
using Shop.Domain.Models.Wallets.WithdrawApplys;

namespace Shop.CommandHandlers
{
    public class WalletCommandHandler:
        ICommandHandler<CreateWalletCommand>,
        ICommandHandler<FreezeWalletCommand>,
        ICommandHandler<UnFreezeWalletCommand>,
        ICommandHandler<AcceptNewCashTransferCommand>,
        ICommandHandler<AcceptNewShopCashTransferCommand>,
        ICommandHandler<AcceptNewBenevolenceTransferCommand>,

        ICommandHandler<AddBankCardCommand>,
        ICommandHandler<UpdateBankCardCommand>,
        ICommandHandler<RemoveBankCardCommand>,

        ICommandHandler<SetAccessCodeCommand>,

        ICommandHandler<CreateWithdrawApplyCommand>,
        ICommandHandler<ChangeWithdrawStatusCommand>,

        ICommandHandler<CreateRechargeApplyCommand>,
        ICommandHandler<ChangeRechargeStatusCommand>,

        ICommandHandler<IncentiveBenevolenceCommand>,
        ICommandHandler<ClearWeekWithdrawAmountCommand>,
        ICommandHandler<ResetTodayStatisticCommand>
    {
        public void Handle(ICommandContext context, CreateWalletCommand command)
        {
            var wallet = new Wallet(command.AggregateRootId, command.UserId);
            context.Add(wallet);
        }
        public void Handle(ICommandContext context, AcceptNewCashTransferCommand command)
        {
            var cashTransfer = context.Get<CashTransfer>(command.TransferId);
            context.Get<Wallet>(command.AggregateRootId).AcceptNewCashTransfer(cashTransfer);
        }
        public void Handle(ICommandContext context, AcceptNewShopCashTransferCommand command)
        {
            var shopCashTransfer = context.Get<ShopCashTransfer>(command.TransferId);
            context.Get<Wallet>(command.AggregateRootId).AcceptNewShopCashTransfer(shopCashTransfer);
        }
        public void Handle(ICommandContext context, AcceptNewBenevolenceTransferCommand command)
        {
            var benevolenceTransfer = context.Get<BenevolenceTransfer>(command.TransferId);
            context.Get<Wallet>(command.AggregateRootId).AcceptNewBenevolenceTransfer(benevolenceTransfer);
        }

        public void Handle(ICommandContext context, AddBankCardCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).AddBankCard(new BankCardInfo(
                command.BankName,
                command.OwnerName,
                command.Number));
        }

        public void Handle(ICommandContext context, UpdateBankCardCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).UpdateBankCard(command.BankCardId, new BankCardInfo(
                command.BankName,
                command.OwnerName,
                command.Number));
        }

        public void Handle(ICommandContext context, RemoveBankCardCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).RemoveBankCard(command.BankCardId);
        }

        public void Handle(ICommandContext context, SetAccessCodeCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).SetAccessCode(command.AccessCode);
        }

        public void Handle(ICommandContext context, CreateWithdrawApplyCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).ApplyWithdraw(command.WithdrawApplyId, new WithdrawApplyInfo(
                command.Amount,
                command.BankName,
                command.BankNumber,
                command.BankOwner,
                "等待审核"));
        }

        public void Handle(ICommandContext context, ChangeWithdrawStatusCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).ChangeWithdrawApplyStatus(
                command.WithdrawApplyId,
                command.Status,
                command.Remark);
        }

        public void Handle(ICommandContext context, CreateRechargeApplyCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).ApplyRecharge(command.RechargeApplyId, new RechargeApplyInfo(
                command.Amount,
                command.Pic,
                command.BankName,
                command.BankNumber,
                command.BankOwner,
                "等待审核"));
        }

        public void Handle(ICommandContext context, ChangeRechargeStatusCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).ChangeRechargeApplyStatus(
                command.RechargeApplyId,
                command.Status,
                command.Remark);
        }

        public void Handle(ICommandContext context, IncentiveBenevolenceCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).IncentiveBenevolence(command.BenevolenceIndex);
        }

        public void Handle(ICommandContext context, ClearWeekWithdrawAmountCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).ClearWeekWithdrawAmount();
        }

        public void Handle(ICommandContext context, ResetTodayStatisticCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).ResetTodayStatistic();
        }

        public void Handle(ICommandContext context, FreezeWalletCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).Freeze();
        }

        public void Handle(ICommandContext context, UnFreezeWalletCommand command)
        {
            context.Get<Wallet>(command.AggregateRootId).UnFreeze();
        }
    }
}
