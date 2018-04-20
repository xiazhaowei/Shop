using Shop.Common.Enums;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IWalletQueryService
    {
        Wallet Info(Guid Id);

        Wallet InfoByUserId(Guid userId);

        decimal TotalBenevolence();

        IEnumerable<Wallet> ListPage();
        IEnumerable<Wallet> AllWallets();

        IEnumerable<WalletAlis> BenevolenceRank(int count);

        IEnumerable<IncentiveInfo> GetIncentiveInfos(Guid walletId, int count);

        IEnumerable<WithdrawApply> WithdrawApplyLogs(Guid walletId);
        IEnumerable<WithdrawApply> WithdrawApplyLogs();

        IEnumerable<RechargeApply> RechargeApplyLogs(Guid walletId);
        IEnumerable<RechargeApply> RechargeApplyLogs();


        IEnumerable<BankCard> GetBankCards(Guid walletId);

        IEnumerable<CashTransfer> GetCashTransfers(Guid walletId);
        IEnumerable<CashTransfer> GetCashTransfers(Guid walletId, CashTransferType type);

        IEnumerable<ShopCashTransfer> GetShopCashTransfers(Guid walletId);
        IEnumerable<ShopCashTransfer> GetShopCashTransfers(Guid walletId, ShopCashTransferType type);

        IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers(Guid walletId);
        IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers();
        IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers(Guid walletId, BenevolenceTransferType type);
        IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers(BenevolenceTransferType type);
    }
}
