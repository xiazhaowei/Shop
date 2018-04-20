using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Wallet
{
    public class GetBankCardsResponse:BaseApiResponse
    {
        public IList<BankCard> BankCards { get; set; }
    }

    public class BankCard
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string BankName { get; set; }
        public string OwnerName { get; set; }
        public string Number { get; set; }
    }
}