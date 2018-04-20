using System;

namespace Shop.Domain.Models.Payments
{
    /// <summary>
    /// 支付项目
    /// </summary>
    public class PaymentItem
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public PaymentItem(string description, decimal amount)
        {
            Id = Guid.NewGuid();
            Description = description;
            Amount = amount;
        }
    }
}
