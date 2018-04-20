using System;

namespace Shop.Api.Models.Request.ThirdCurrencys
{
    public class ImportCurrencyRequest
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public decimal Amount { get; set; }
    }
}