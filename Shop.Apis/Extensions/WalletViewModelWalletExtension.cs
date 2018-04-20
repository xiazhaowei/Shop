using Shop.Apis.ViewModels;
using Shop.Commands.Wallets.BankCards;
using Dtos = Shop.QueryServices.Dtos;

namespace Shop.Apis.Extensions
{
    public static class WalletViewModelWalletExtension
    {
        public static WalletViewModel ToWalletModel(this Dtos.Wallet value)
        {
            return new WalletViewModel()
            {
                Id=value.Id,
                UserId=value.UserId,
                AccessCode=value.AccessCode,
                Cash=value.Cash,
                Benevolence=value.Benevolence,
                BenevolenceTotal=value.BenevolenceTotal,
                Earnings=value.Earnings,
                TodayBenevolenceAdded=value.TodayBenevolenceAdded,
                YesterdayEarnings=value.YesterdayEarnings,
                YesterdayIndex=value.YesterdayIndex
            };
        }

        
        
    }
}