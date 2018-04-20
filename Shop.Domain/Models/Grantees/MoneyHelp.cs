using System;

namespace Shop.Domain.Models.Grantees
{
    /// <summary>
    /// 现金帮助
    /// </summary>
    [Serializable]
    public class MoneyHelp
    {
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Says { get; private set; }

        public MoneyHelp(Guid userId,decimal amount,string says)
        {
            UserId = userId;
            Amount = amount;
            Says = says;
        }
    }
}
