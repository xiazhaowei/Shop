using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class AddBankCardRequest
    {
        public string BankName { get; set; }
        public string OwnerName { get; set; }
        public string Number { get; set; }
    }
}