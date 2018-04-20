using ENode.Commanding;
using System;

namespace Shop.Commands.ThirdCurrencys
{
    public class ChargeThirdCurrencyCommand:Command<Guid>
    {
        public decimal Amount { get; set; }

        public ChargeThirdCurrencyCommand() { }
        public ChargeThirdCurrencyCommand(decimal amount)
        {
            Amount = amount;
        }
    }
}
