using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class DeleteBankCardRequest
    {
        public Guid BankCardId { get; set; }
    }
}